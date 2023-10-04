using Unity.Entities;

namespace TMG.Shooter
{
    public struct ShotCooldownTimer : IComponentData
    {
        public float Value;
        public float CooldownTime;
    }
}