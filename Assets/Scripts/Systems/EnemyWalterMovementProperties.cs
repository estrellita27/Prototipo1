using Unity.Entities;

namespace TMG.Shooter
{ 
    public struct EnemyWalterMovementProperties : IComponentData
    {
        public float LateralAmplitude;
        public float LateralFrequency;
        public float MinDistanceFromTargetSq;
    }

    public struct TimeInWorld : IComponentData
    {
        public float Value; 
    }

    public struct SetTargetTag : IComponentData
    {
       
    }
    public struct TargetEntity : IComponentData
    {
        public Entity Value;
    }

}

