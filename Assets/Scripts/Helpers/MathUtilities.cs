using Unity.Mathematics;

namespace TMG.Shooter
{
    public static class MathUtilities
    {
        public static quaternion GetRotationToPoint(float3 currentPosition, float3 targetPosition)
        {
            var targetDirection = math.normalize(targetPosition.xz - currentPosition.xz);
            var targetDirection3 = new float3(targetDirection.x, 0f, targetDirection.y);
            return quaternion.LookRotation(targetDirection3, math.up());
        }
        
        public static void SetRotationTowards(this ref quaternion from, quaternion to, float maxDegreesDelta)
        {
            var num = Angle(from, to);
            from = math.slerp(from, to, math.min(1f, maxDegreesDelta / num));
        }
        
        private static float Angle(quaternion a, quaternion b)
        {
            var num = math.dot(a, b);
            return (float)((double)math.acos(math.min(math.abs(num), 1f)) * 2.0 * 57.2957801818848);
        }
    }
}