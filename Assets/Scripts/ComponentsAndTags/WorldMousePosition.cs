using Unity.Entities;
using Unity.Mathematics;

namespace TMG.Shooter
{
    public struct WorldMousePosition : IComponentData
    {
        public float3 Value;
    }
}
