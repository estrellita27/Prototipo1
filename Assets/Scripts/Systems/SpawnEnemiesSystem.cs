using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace TMG.Shooter
{
    public partial class SpawnEnemiesSystem : SystemBase
    {
        private BeginInitializationEntityCommandBufferSystem _ecbSystem;

        protected override void OnStartRunning()
        {
            _ecbSystem = World.GetExistingSystem<BeginInitializationEntityCommandBufferSystem>();
        }
        protected override void OnUpdate()
        {
            new SpawnEnemiesJob
            {
                DeltaTime = Time.DeltaTime,
                ECB = _ecbSystem.CreateCommandBuffer()
            }.Run();
        }
    }

#pragma warning disable CS0282
    [BurstCompile]
    public partial struct SpawnEnemiesJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer ECB;

        private void Execute(ref SpawnTimer spawnTimer, ref EntityRandom random, in SpawnPointReference spawnPoints,
            in EnemyWalterPrefab enemyWalterPrefab)
        {
            spawnTimer.Value -= DeltaTime;
            if (spawnTimer.Value > 0f) return;
            spawnTimer.Value = spawnTimer.Interval;

            var newEnemyWalter = ECB.Instantiate(enemyWalterPrefab.Value);
        }
    }
}


