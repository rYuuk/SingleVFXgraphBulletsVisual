using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace SingleVFXGraphBulletsVisual
{
    [BurstCompile]
    public partial struct ProjectileSpawnSystem : ISystem
    {
        private const float RATE_OF_FIRE = 50f;

        private EntityQuery projectileSpawnerQuery;

        private float cooldown;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            projectileSpawnerQuery = SystemAPI.QueryBuilder()
                .WithAll<ProjectileSpawner>()
                .Build();

            state.RequireForUpdate(projectileSpawnerQuery);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (cooldown > 0)
            {
                cooldown -= SystemAPI.Time.DeltaTime;
                return;
            }

            cooldown = 1 / RATE_OF_FIRE;

            var projectileSpawners = projectileSpawnerQuery.ToComponentDataArray<ProjectileSpawner>(Allocator.Temp);
            for (var i = 0; i < projectileSpawners.Length; i++)
            {
                var projectileSpawner = projectileSpawners[i];
                var localToWorld = SystemAPI.GetComponent<LocalToWorld>(projectileSpawner.SpawnPointEntity);

                var entity = state.EntityManager.CreateEntity();
                state.EntityManager.AddComponentData(entity, new Projectile
                {
                    Lifetime = projectileSpawner.ProjectileLifetime
                });

                var spawnPosition = localToWorld.Position;

                state.EntityManager.AddComponentData(entity, new LocalToWorld
                {
                    Value = float4x4.TRS(
                        spawnPosition,
                        quaternion.LookRotation(localToWorld.Forward, math.up()),
                        1)
                });

                state.EntityManager.AddComponentData(entity, new ProjectileDirection
                {
                    Direction = localToWorld.Forward
                });

                state.EntityManager.AddComponentData(entity, new ProjectileSpeed
                {
                    Speed = projectileSpawner.ProjectileSpeed
                });

            }
            projectileSpawners.Dispose();
        }
    }
}
