using Unity.Entities;

namespace TMG.Shooter
{
    [GenerateAuthoringComponent]
    public struct ShotPrefab : IComponentData
    {
        public Entity Value;
    }
}