using Unity.Entities;
using UnityEngine;

namespace TMG.Shooter
{

    public class WalterAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;
        
        public void Convert(Entity newWalterEntity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(newWalterEntity, new MoveSpeed {Value = _moveSpeed });
            dstManager.AddComponentData(newWalterEntity, new RotationSpeed {Value = _rotationSpeed });
        }

    }
}