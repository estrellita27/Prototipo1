using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


namespace TMG.Shooter
{
    public partial class MoveEnemyWalterSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            new MoveEnemyJob
            {
                TranslationLookup = GetComponentDataFromEntity<Translation>(),
                DeltaTime = Time.DeltaTime,
                Epsilon = 0.01f
            }.ScheduleParallel();
        }
    }
#pragma warning disable C50282
    [BurstCompile]
    public partial struct MoveEnemyJob : IJobEntity
    {
        [NativeDisableParallelForRestriction, NativeDisableContainerSafetyRestriction]
        public ComponentDataFromEntity<Translation> TranslationLookup;

        public float DeltaTime;
        public float Epsilon;

        private void Execute(ref Translation translation, ref Rotation rotation, ref TimeInWorld timeInWorld,
             in EnemyWalterMovementProperties movementProperties, in TargetEntity targetEntity, in MoveSpeed moveSpeed,
             in RotationSpeed rotationSpeed, in LocalToWorld localToWorld)
        {
            if (!TranslationLookup.HasComponent(targetEntity.Value)) return;
            var targetPosition = TranslationLookup[targetEntity.Value].Value;
            if (math.distancesq(translation.Value, targetPosition) > movementProperties.MinDistanceFromTargetSq)
            {
                var currentMovement = localToWorld.Forward * moveSpeed.Value * DeltaTime;
                translation.Value += currentMovement;
            }

            timeInWorld.Value += DeltaTime;

            var lateralMovement = localToWorld.Right * movementProperties.LateralAmplitude * DeltaTime *
                                  math.sin(timeInWorld.Value * movementProperties.LateralFrequency);
            translation.Value += lateralMovement;

            if (math.distancesq(translation.Value, targetPosition) <= Epsilon) return;

            var rotationTarget = MathUtilities.GetRotationToPoint(translation.Value, targetPosition);
            rotation.Value.SetRotationTowards(rotationTarget, rotationSpeed.Value * DeltaTime);
        }
    }
}