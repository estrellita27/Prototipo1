using Unity.Entities;
using UnityEngine;

namespace TMG.Shooter
{ 
    public class EnemyWalterAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private float _lateralAmplitude;
        [SerializeField] private float _lateralFrequency;
        [SerializeField] private float _minDistanceFromTarget;

        public void Convert(Entity newEnemy, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(newEnemy, new EnemyWalterMovementProperties
            {
                LateralAmplitude = _lateralAmplitude,
                LateralFrequency = _lateralFrequency,
                MinDistanceFromTargetSq = _minDistanceFromTarget * _minDistanceFromTarget

            });
            dstManager.AddComponent<TimeInWorld>(newEnemy);
            dstManager.AddComponent<SetTargetTag>(newEnemy); 
              
        }
    }
}

