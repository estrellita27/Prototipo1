using Unity.Entities;

namespace TMG.Shooter
{
    public struct TeamID : IComponentData
    {
        public TeamIDs Value;
    }

    public enum TeamIDs : byte
    {
        PlayerShips = 0,
        EnemyShips = 1
    }
}