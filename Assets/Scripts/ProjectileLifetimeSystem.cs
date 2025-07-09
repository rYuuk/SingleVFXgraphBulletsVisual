using Unity.Burst;
using Unity.Entities;

namespace SingleVFXGraphBulletsVisual
{
    [BurstCompile]
    public partial struct ProjectileLifetimeSystem : ISystem
    {
        private EntityQuery projectileQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            projectileQuery = SystemAPI.QueryBuilder()
                .WithAll<Projectile>()
                .Build();

            state.RequireForUpdate(projectileQuery);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (projectile, entity) in SystemAPI.Query<RefRW<Projectile>>().WithEntityAccess())
            {
                projectile.ValueRW.Lifetime -= SystemAPI.Time.DeltaTime;

                if (projectile.ValueRO.Lifetime <= 0)
                {
                    ecb.DestroyEntity(entity);
                }
            }
        }
    }
}
