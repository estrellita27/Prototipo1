using Unity.Entities;
using Unity.Mathematics;

namespace TMG.Shooter
{
    public struct PlayerMoveInput : IComponentData
    {
        public float3 Value;
    }
}
