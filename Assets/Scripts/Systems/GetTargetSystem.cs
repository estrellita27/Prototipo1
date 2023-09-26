using Unity.Burst;
using Unity.Entities; 

namespace TMG.Shooter
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    [UpdapeAfter(typeof(BeginSimulationEntityCommandBufferSystem))]
    public partial class GetTargetSystem : SystemBase
    {
        protecyed override void OnUpdate()
        {

        }
    }
#pragma warning disable CS0282
    [BurstCompile]
    public partial struct GetTargetJob : IJobEntity
    { 
    }

}
