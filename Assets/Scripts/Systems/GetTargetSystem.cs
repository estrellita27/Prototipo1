using Unity.Burst;
using Unity.Entities;
using Unity.Collections;

namespace TMG.Shooter
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    [UpdateAfter(typeof(BeginSimulationEntityCommandBufferSystem))]
    public partial class GetTargetSystem : SystemBase
    {
        private Entity _playerWalterEntity;
        private EntityQuery _enemyWalterQuery;

        protected override void OnCreate()
        {
            Enabled = false;
        }

        protected override void OnStartRunning()
        {
            _playerWalterEntity = GetSingletonEntity<PlayerWalterTag>();
            RequireSingletonForUpdate<PlayerWalterTag>();

            _enemyWalterQuery = EntityManager.CreateEntityQuery(typeof(SetTargetTag));
        }

        protected override void OnUpdate()
        {
            var ecb = new EntityCommandBuffer(Allocator.TempJob);

            new GetTargetJob
            {
                ECB = ecb,
                PlayerWalterEntity = _playerWalterEntity
            }.Run(_enemyWalterQuery);

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
        
    }

#pragma warning disable CS0282
    [BurstCompile]
    public partial struct GetTargetJob : IJobEntity
    {
        public Entity PlayerWalterEntity;
        public EntityCommandBuffer ECB;

        private void Execute(Entity enemyWalterEntity)
        {
            ECB.RemoveComponent<SetTargetTag>(enemyWalterEntity);
            ECB.AddComponent(enemyWalterEntity, new TargetEntity { Value = PlayerWalterEntity });
        }
    }
   
}
