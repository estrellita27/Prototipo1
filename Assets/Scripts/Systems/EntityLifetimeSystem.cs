using TMG.Shooter;
using Unity.Burst;
using Unity.Entities;

namespace TMG.Shooter
{
    public partial class EntityLifetimeSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _ecbSystem;

        protected override void OnStartRunning()
        {
            _ecbSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            new EntityLifetimeJob
            {
                DeltaTime = Time.DeltaTime,
                ECB = _ecbSystem.CreateCommandBuffer().AsParallelWriter()
            }.ScheduleParallel();

            _ecbSystem.AddJobHandleForProducer(Dependency);
        }
    }

#pragma warning disable CS0282
    [BurstCompile]
    public partial struct EntityLifetimeJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter ECB;

        private void Execute(Entity entity, [EntityInQueryIndex] int sortKey, ref EntityLifetime entityLifetime)
        {
            entityLifetime.Value -= DeltaTime;
            if (entityLifetime.Value > 0f) return;
            ECB.AddComponent<DestroyEntityTag>(sortKey, entity);
        }
    }
}