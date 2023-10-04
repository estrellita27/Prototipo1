using TMG.Shooter;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

namespace TMG.Shooter
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(ExportPhysicsWorld))]
    [UpdateBefore(typeof(EndFramePhysicsSystem))]
    public partial class DetectShotCollisionSystem : SystemBase
    {
        private BuildPhysicsWorld _buildPhysicsWorld;
        private StepPhysicsWorld _stepPhysicsWorld;

        protected override void OnStartRunning()
        {
            _buildPhysicsWorld = World.GetExistingSystem<BuildPhysicsWorld>();
            _stepPhysicsWorld = World.GetExistingSystem<StepPhysicsWorld>();
            this.RegisterPhysicsRuntimeSystemReadWrite();
        }

        protected override void OnUpdate()
        {
            Dependency = new DetectShotCollisionJob
            {
                ShotCollisionLookup = GetComponentDataFromEntity<ShotCollision>(),
                TeamIDLookup = GetComponentDataFromEntity<TeamID>(true),
                PhysicsWorld = _buildPhysicsWorld.PhysicsWorld
            }.Schedule(_stepPhysicsWorld.Simulation, Dependency);
        }
    }

    public struct DetectShotCollisionJob : ICollisionEventsJob
    {
        public ComponentDataFromEntity<ShotCollision> ShotCollisionLookup;
        [ReadOnly] public ComponentDataFromEntity<TeamID> TeamIDLookup;
        public PhysicsWorld PhysicsWorld;

        public void Execute(CollisionEvent collisionEvent)
        {
            var entityA = collisionEvent.EntityA;
            var entityB = collisionEvent.EntityB;

            var isEntityAShot = ShotCollisionLookup.HasComponent(entityA);
            var isEntityBShot = ShotCollisionLookup.HasComponent(entityB);

            if (!(isEntityAShot ^ isEntityBShot)) return;

            if (!TeamIDLookup.HasComponent(entityA) || !TeamIDLookup.HasComponent(entityB)) return;

            var entityATeamID = TeamIDLookup[entityA].Value;
            var entityBTeamID = TeamIDLookup[entityB].Value;
            if (entityATeamID == entityBTeamID) { return; }

            var shotEntity = isEntityAShot ? entityA : entityB;
            var hitEntity = isEntityAShot ? entityB : entityA;

            var newShotCollision = new ShotCollision
            {
                HitEntity = hitEntity,
                Position = collisionEvent.CalculateDetails(ref PhysicsWorld).AverageContactPointPosition
            };

            ShotCollisionLookup[shotEntity] = newShotCollision;
        }
    }
}
