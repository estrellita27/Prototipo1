using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.Shooter
{
    [UpdateAfter(typeof(GetPlayerInputSystem))]
    [UpdateBefore(typeof(TransformSystemGroup))]
    public partial class MovePlayerWalterSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            new MovePlayerWalterJob
            {
                DeltaTime = Time.DeltaTime,
                Epsilon = 0.01f,
                
            }.ScheduleParallel(); //Esta accion funciona paralelo a otras funciones.
        }
    }

#pragma warning disable CS0282
    [BurstCompile]
    public partial struct MovePlayerWalterJob : IJobEntity
    {
        public float DeltaTime;
        public float Epsilon;
        

        private void Execute(ref Translation translation, in PlayerMoveInput playerMoveInput, in MoveSpeed moveSpeed,
            ref Rotation rotation, in WorldMousePosition worldMousePosition, in RotationSpeed rotationSpeed )
        {
            var currentMovement = playerMoveInput.Value * moveSpeed.Value * DeltaTime;
            translation.Value += currentMovement;

            if (math.distancesq(translation.Value, worldMousePosition.Value) <= Epsilon) { return; }
            var rotationTarget = MathUtilities.GetRotationToPoint(translation.Value, worldMousePosition.Value);
            rotation.Value.SetRotationTowards(rotationTarget, rotationSpeed.Value * DeltaTime) ;

           
        }
    }
}
