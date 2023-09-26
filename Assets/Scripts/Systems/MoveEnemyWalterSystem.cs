using Unity.Burst;
using Unity.Entities; 

namespace TMG.Shooter
{
    public partial class MoveEnemyWalterSystem : SystenBase
    {
        protected override void OnUpdate()
        {

        }
    }
#pragma warning disable C50282
    [BurstCompile]
    public partial struct MoveEnemyJob : IJobEntity
    {
        public ComponentDataFromEntity<Translation> TranslationLookup;

        private void Execute(ref Translation translation, ref Rotation rotation, ref TimeInWorld time)
    }
}