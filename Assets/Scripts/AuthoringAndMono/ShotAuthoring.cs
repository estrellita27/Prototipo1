using TMG.Shooter;
using Unity.Entities;
using UnityEngine;

namespace TMG.Shooter
{
    public class ShotAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _shotLifetime;

        public void Convert(Entity shotEntity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(shotEntity, new MoveSpeed { Value = _moveSpeed });
            dstManager.AddComponentData(shotEntity, new EntityLifetime { Value = _shotLifetime });
            dstManager.AddComponent<ShotTag>(shotEntity);
            dstManager.AddComponent<ShotCollision>(shotEntity);
                                    
        }
    }
}