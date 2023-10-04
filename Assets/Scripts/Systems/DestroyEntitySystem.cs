using TMG.Shooter;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace TMG.Shooter
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    [UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]
    public partial class DestroyEntitySystem : SystemBase
    {
        private EndInitializationEntityCommandBufferSystem _ecbSystem;

        public delegate void GameEventDelegate();

        public event GameEventDelegate OnGameOver;

        protected override void OnStartRunning()
        {
            _ecbSystem = World.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = _ecbSystem.CreateCommandBuffer();
            var entityManager = EntityManager;
            var onGameOver = OnGameOver;

            Entities
                .WithAll<DestroyEntityTag>()
                .ForEach((Entity entityToDestroy) =>
                {
                    if (HasComponent<PlayerWalterTag>(entityToDestroy))
                    {
                        onGameOver?.Invoke();
                    }

                    if (entityManager.HasComponent<Child>(entityToDestroy))
                    {
                        var childBuffer = GetBuffer<Child>(entityToDestroy);
                        foreach (var child in childBuffer)
                        {
                            ecb.DestroyEntity(child.Value);
                        }
                    }

                    if (HasComponent<CopyTransformToGameObject>(entityToDestroy))
                    {
                        var companionGameObject =
                            entityManager.GetComponentObject<Transform>(entityToDestroy).gameObject;
                        Object.Destroy(companionGameObject);
                    }

                    ecb.DestroyEntity(entityToDestroy);
                }).WithoutBurst().Run();
        }
    }
}