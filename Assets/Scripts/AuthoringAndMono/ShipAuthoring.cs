using TMG.Shooter;
using Unity.Entities;
using UnityEngine;

namespace TMG.Shooter
{
    public class ShipAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private int _hitPoints;
        [SerializeField] private int _pointsGivenOnDestroy;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _cooldownTime;
        [SerializeField] private GameObject _firePointEntity;
        [SerializeField] private GameObject _shipRenderEntity;
        [SerializeField] private TeamIDs _teamID;

        public void Convert(Entity newShipEntity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(newShipEntity, new HitPoints { Value = _hitPoints });
            dstManager.AddComponentData(newShipEntity, new PointsGivenOnDestroy { Value = _pointsGivenOnDestroy });
            dstManager.AddComponentData(newShipEntity, new MoveSpeed { Value = _moveSpeed });
            dstManager.AddComponentData(newShipEntity, new RotationSpeed { Value = _rotationSpeed });
            dstManager.AddComponentData(newShipEntity, new ShotCooldownTimer
            {
                Value = 0f,
                CooldownTime = _cooldownTime
            });
            dstManager.AddComponentData(newShipEntity, new FiringPointEntity
            {
                Value = conversionSystem.GetPrimaryEntity(_firePointEntity)
            });
            //dstManager.AddComponentData(newShipEntity, new ShipRenderEntity
            //{
                //Value = conversionSystem.GetPrimaryEntity(_shipRenderEntity)
            // });
            dstManager.AddComponentData(newShipEntity, new TeamID { Value = _teamID });
            dstManager.AddComponent<ShipScorePoints>(newShipEntity);
            dstManager.AddBuffer<ScoreBufferElement>(newShipEntity);
        }
    }
}