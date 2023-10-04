using TMG.Shooter;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.Shooter
{
    [UpdateBefore(typeof(TransformSystemGroup))]
    public partial class MoveShotSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;

            Entities
                .WithAll<ShotTag>()
                .ForEach((ref Translation translation, in MoveSpeed moveSpeed, in Rotation rotation) =>
                {
                    var currentMovement = math.forward(rotation.Value) * moveSpeed.Value * deltaTime;
                    translation.Value += currentMovement;
                }).ScheduleParallel();
        }
    }
}