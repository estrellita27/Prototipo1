using TMG.Shooter;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace TMG.Shooter
{
    public partial class DamageOnCollisionSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _ecbSystem;

        protected override void OnStartRunning()
        {
            _ecbSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            new DamageOnCollisionJob
            {
                HitPointLookup = GetComponentDataFromEntity<HitPoints>(),
                ECB = _ecbSystem.CreateCommandBuffer().AsParallelWriter(),
                ScoreBufferLookup = GetBufferFromEntity<ScoreBufferElement>(true),
                PointsGivenLookup = GetComponentDataFromEntity<PointsGivenOnDestroy>(true)
            }.ScheduleParallel();

            _ecbSystem.AddJobHandleForProducer(Dependency);
        }
    }

#pragma warning disable CS0282
    [BurstCompile]
    [WithChangeFilter(typeof(ShotCollision))]
    public partial struct DamageOnCollisionJob : IJobEntity
    {
        [NativeDisableParallelForRestriction]
        public ComponentDataFromEntity<HitPoints> HitPointLookup;

        public EntityCommandBuffer.ParallelWriter ECB;

        [ReadOnly] public BufferFromEntity<ScoreBufferElement> ScoreBufferLookup;
        [ReadOnly] public ComponentDataFromEntity<PointsGivenOnDestroy> PointsGivenLookup;

        private void Execute([EntityInQueryIndex] int sortKey, in ShotCollision shotCollision,
            in ShipEntityReference shipEntityReference)
        {
            var hitEntity = shotCollision.HitEntity;

            if (!HitPointLookup.HasComponent(hitEntity)) return;

            var otherEntityHitPoints = HitPointLookup[hitEntity];
            otherEntityHitPoints.Value--;
            HitPointLookup[hitEntity] = otherEntityHitPoints;

            if (otherEntityHitPoints.Value <= 0)
            {
                ECB.AddComponent<DestroyEntityTag>(sortKey, hitEntity);
                if (!ScoreBufferLookup.HasComponent(shipEntityReference.Value)) return;
                var scoreToAdd = PointsGivenLookup[hitEntity].Value;
                var newScoreElement = new ScoreBufferElement { Value = scoreToAdd };
                ECB.AppendToBuffer(sortKey, shipEntityReference.Value, newScoreElement);
            }
        }
    }
}