using Unity.Entities;
using Unity.Mathematics; 

namespace TMG.Shooter
{
    public struct SpawnTimer : IComponentData
    {
        public float Value;
        public float Interval;
    }
    public struct EnemyWalterPrefab : IComponentData
    {
        public Entity Value;  
    }
    public struct EntityRandom : IComponentData
    {
        public Random Value;
    }
}
   
