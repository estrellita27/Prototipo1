using Unity.Entities;

namespace TMG.Shooter
{
    public struct ShipScorePoints : IComponentData
    {
        public int Value;
    }

    public struct PointsGivenOnDestroy : IComponentData
    {
        public int Value;
    }

    [InternalBufferCapacity(1)]
    public struct ScoreBufferElement : IBufferElementData
    {
        public int Value;
    }
}
