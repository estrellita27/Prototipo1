using TMG.Shooter;
using Unity.Burst;
using Unity.Entities;

namespace TMG.Shooter
{
    [UpdateAfter(typeof(DamageOnCollisionSystem))]
    public partial class AddScoreSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            new AddScoreJob().Schedule();
        }
    }

    [BurstCompile]
    [WithChangeFilter(typeof(ScoreBufferElement))]
    public partial struct AddScoreJob : IJobEntity
    {
        private void Execute(ref DynamicBuffer<ScoreBufferElement> scoreBuffer, ref ShipScorePoints shipScorePoints)
        {
            if (scoreBuffer.IsEmpty) return;

            foreach (var scoreBufferElement in scoreBuffer)
            {
                shipScorePoints.Value += scoreBufferElement.Value;
            }
            scoreBuffer.Clear();
        }
    }
}
