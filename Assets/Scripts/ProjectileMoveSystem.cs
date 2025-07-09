using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace SingleVFXGraphBulletsVisual
{
    [BurstCompile]
    public partial struct ProjectileMoveSystem : ISystem
    {
        private EntityQuery projectileQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            projectileQuery = SystemAPI.QueryBuilder()
                .WithAll<ProjectileDirection, ProjectileSpeed, LocalToWorld>()
                .Build();

            state.RequireForUpdate(projectileQuery);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (projectileDirection, projectileSpeed, localToWorld) in
                     SystemAPI.Query<RefRO<ProjectileDirection>, RefRO<ProjectileSpeed>, RefRW<LocalToWorld>>())
            {
                var pos = localToWorld.ValueRO.Position + projectileSpeed.ValueRO.Speed *
                    projectileDirection.ValueRO.Direction * SystemAPI.Time.DeltaTime;

                var rot = localToWorld.ValueRO.Rotation;
                localToWorld.ValueRW = new LocalToWorld
                {
                    Value = float4x4.TRS(pos, rot, 1)
                };
            }
        }
    }
}
