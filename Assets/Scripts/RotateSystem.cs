using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace SingleVFXGraphBulletsVisual
{
    [BurstCompile]
    public partial struct RotateSystem : ISystem
    {
        private EntityQuery rotateQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            rotateQuery = SystemAPI.QueryBuilder()
                .WithAll<AddRotation, ProjectileSpawner>()
                .Build();

            state.RequireForUpdate(rotateQuery);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var addRotations = rotateQuery.ToComponentDataArray<AddRotation>(Allocator.Temp);
            var projectileSpawners = rotateQuery.ToComponentDataArray<ProjectileSpawner>(Allocator.Temp);
            for (int i = 0; i < projectileSpawners.Length; i++)
            {
                var rotationSpeed = addRotations[i].Speed;
                var projectileVisual = projectileSpawners[i];
                
                var localTransform = SystemAPI.GetComponentRW<LocalTransform>(projectileVisual.Mallet);
                var currentRotation = localTransform.ValueRW.Rotation;
                
                var delta   = SystemAPI.Time.DeltaTime * rotationSpeed;
                var dq = quaternion.RotateY(delta);

                localTransform.ValueRW.Rotation = math.mul(currentRotation, dq);
            }
        }
    }
}
