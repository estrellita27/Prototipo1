using TMG.Shooter;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace TMG.Shooter
{
    [UpdateAfter(typeof(TransformSystemGroup))]
    public partial class ShootWeaponSystem : SystemBase
    {
        private BeginInitializationEntityCommandBufferSystem _ecbSystem;
        private Entity _shotPrefab;
        private EntityQuery _shootingEntityQuery;

        protected override void OnStartRunning()
        {
            _ecbSystem = World.GetExistingSystem<BeginInitializationEntityCommandBufferSystem>();
            _shotPrefab = GetSingleton<ShotPrefab>().Value;
            var shootingDesc = new EntityQueryDesc
            {
                All = new ComponentType[] { typeof(ShotCooldownTimer), typeof(FiringPointEntity), typeof(TeamID) },
                Any = new ComponentType[] { typeof(PlayerWalterTag), typeof(TargetEntity) }
            };
            _shootingEntityQuery = GetEntityQuery(shootingDesc);
        }

        protected override void OnUpdate()
        {
            Dependency = new ShootWeaponJob
            {
                ShotCooldownTypeHandle = GetComponentTypeHandle<ShotCooldownTimer>(),
                PlayerWeaponKeyTypeHandle = GetComponentTypeHandle<PlayerWeaponKeyPressed>(true),
                TargetEntityTypeHandle = GetComponentTypeHandle<TargetEntity>(true),
                LocalToWorldLookup = GetComponentDataFromEntity<LocalToWorld>(true),
                FiringPointTypeHandle = GetComponentTypeHandle<FiringPointEntity>(true),
                TeamIDTypeHandle = GetComponentTypeHandle<TeamID>(true),
                DeltaTime = Time.DeltaTime,
                ECB = _ecbSystem.CreateCommandBuffer().AsParallelWriter(),
                ShotPrefab = _shotPrefab,
                EntityTypeHandle = GetEntityTypeHandle()
            }.ScheduleParallel(_shootingEntityQuery, Dependency);

            _ecbSystem.AddJobHandleForProducer(Dependency);
        }
    }

    [BurstCompile]
    public struct ShootWeaponJob : IJobEntityBatch
    {
        public ComponentTypeHandle<ShotCooldownTimer> ShotCooldownTypeHandle;
        [ReadOnly] public ComponentTypeHandle<PlayerWeaponKeyPressed> PlayerWeaponKeyTypeHandle;
        [ReadOnly] public ComponentTypeHandle<TargetEntity> TargetEntityTypeHandle;
        [ReadOnly] public ComponentDataFromEntity<LocalToWorld> LocalToWorldLookup;
        [ReadOnly] public ComponentTypeHandle<FiringPointEntity> FiringPointTypeHandle;
        [ReadOnly] public ComponentTypeHandle<TeamID> TeamIDTypeHandle;
        [ReadOnly] public EntityTypeHandle EntityTypeHandle;

        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter ECB;
        public Entity ShotPrefab;

        public void Execute(ArchetypeChunk batchInChunk, int batchIndex)
        {
            var shotTimers = batchInChunk.GetNativeArray(ShotCooldownTypeHandle);

            var isPlayer = false;
            var isPlayerWeaponKeyPressed = false;

            // if is player
            if (batchInChunk.Has(PlayerWeaponKeyTypeHandle))
            {
                isPlayer = true;
                isPlayerWeaponKeyPressed = batchInChunk.GetNativeArray(PlayerWeaponKeyTypeHandle)[0].Value;
            }

            // if is enemy
            if (batchInChunk.Has(TargetEntityTypeHandle))
            {
                var targetEntity = batchInChunk.GetNativeArray(TargetEntityTypeHandle)[0].Value;
                if (!LocalToWorldLookup.HasComponent(targetEntity)) return;
            }

            var firingPoints = batchInChunk.GetNativeArray(FiringPointTypeHandle);
            var teamIDs = batchInChunk.GetNativeArray(TeamIDTypeHandle);

            for (var i = 0; i < batchInChunk.Count; i++)
            {
                var shotTimer = shotTimers[i];
                shotTimer.Value -= DeltaTime;
                if (shotTimer.Value > 0f)
                {
                    shotTimers[i] = shotTimer;
                    continue;
                }

                if (isPlayer && !isPlayerWeaponKeyPressed) continue;

                shotTimer.Value = shotTimer.CooldownTime;
                shotTimers[i] = shotTimer;

                var newShot = ECB.Instantiate(batchIndex, ShotPrefab);

                var firingPointEntity = firingPoints[i].Value;
                var firePointLTW = LocalToWorldLookup[firingPointEntity];
                var firePosition = firePointLTW.Position;
                var fireOrientation = firePointLTW.Rotation;

                ECB.SetComponent(batchIndex, newShot, new Translation { Value = firePosition });
                ECB.SetComponent(batchIndex, newShot, new Rotation { Value = fireOrientation });

                var teamID = teamIDs[i];
                ECB.AddComponent(batchIndex, newShot, teamID);

                var shipEntity = batchInChunk.GetNativeArray(EntityTypeHandle)[i];
                ECB.AddComponent(batchIndex, newShot, new ShipEntityReference { Value = shipEntity });
            }
        }
    }
}