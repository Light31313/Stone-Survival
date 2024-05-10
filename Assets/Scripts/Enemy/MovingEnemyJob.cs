using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public struct MovingEnemyJob : IJob
{
    public float3 moveDir;
    public float3 currentPos;
    public float deltaTime;
    public float3 speed;
    public NativeArray<float3> newMovePos;

    [BurstCompile]
    public void Execute()
    {
        newMovePos[0] = currentPos + deltaTime * speed * moveDir * 2;
    }
}