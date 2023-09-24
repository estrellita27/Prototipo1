
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace TMG.Shooter{
    public class SpawnerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity spawnerEntity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var spawnTransforms = transform.GetComponentsInChildren<Transform>();

            using var blobBuilder = new BlobBuilder(Allocator.Temp);
            ref var spawnPointArray = ref blobBuilder.ConstructRoot<SpawnPointArray>();
            var blobArray = blobBuilder.Allocate(ref spawnPointArray.Value, spawnTransforms.Length);

            for ( var i = 0; i < spawnTransforms.Length; i++)
            {
                blobArray[i] = new SpawnPoint { Value = spawnTransforms[i].position };
            }

            var spawnPointReference = blobBuilder.CreateBlobAssetReference<SpawnPointArray>(Allocator.Persistent);
            dstManager.AddComponentData(spawnerEntity, new SpawnPointReference { Value = spawnPointReference });
        }
    }

    public struct SpawnPoint
    {
        public float3 Value;
    }

    public struct SpawnPointArray
    {
        public BlobArray<SpawnPoint> Value;
    }

    public struct SpawnPointReference : IComponentData
    {
        public BlobAssetReference<SpawnPointArray> Value;

        public int Length => Value.Value.Value.Length;

        public float3 this[int i] => Value.Value.Value[i].Value;
    }

}
