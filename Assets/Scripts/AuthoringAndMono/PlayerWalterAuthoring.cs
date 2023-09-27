using Unity.Entities;
using UnityEngine;

namespace TMG.Shooter
{
    public class PlayerWalterAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        
        public void Convert(Entity newPlayerWalter, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponent<PlayerMoveInput>(newPlayerWalter);
            dstManager.AddComponent<WorldMousePosition>(newPlayerWalter);
            dstManager.AddComponent<PlayerWalterTag>(newPlayerWalter);
        }

    }
}