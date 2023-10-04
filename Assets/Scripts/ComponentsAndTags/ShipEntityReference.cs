using Unity.Entities;

namespace TMG.Shooter
{
    public struct ShipEntityReference : IComponentData
    {
        public Entity Value;
    }
}