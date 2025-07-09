using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

namespace SingleVFXGraphBulletsVisual
{
    public partial class ProjectileVisualSystem : SystemBase
    {
        private readonly int positionBufferId = Shader.PropertyToID("PositionBuffer");

        private EntityQuery projectileQuery;
        private GraphicsBuffer graphicsBuffer;

        private bool isPlaying;

        protected override void OnCreate()
        {
            projectileQuery = SystemAPI.QueryBuilder()
                .WithAll<Projectile>()
                .WithAll<LocalToWorld>()
                .Build();

            ReallocateBuffer(500);
        }

        protected override void OnDestroy()
        {
            ReleaseBuffer();
        }

        protected override void OnUpdate()
        {
            var projectileVisual = SystemAPI.GetSingletonEntity<ProjectileVisual>();
            var visualEffect = EntityManager.GetComponentObject<VisualEffect>(projectileVisual);

            var entityCount = projectileQuery.CalculateEntityCount();

            if (!isPlaying)
            {
                visualEffect.Play();
                isPlaying = true;
            }

            var transformDataArray = new NativeArray<ProjectilePositionData>(graphicsBuffer.count, Allocator.Temp);
            transformDataArray.FillArray(new ProjectilePositionData { Position = Vector3.zero, IsAlive = 0 });

            var projectileLocalToWorld = projectileQuery.ToComponentDataArray<LocalToWorld>(Allocator.Temp);

            if (entityCount > 0)
            {
                for (var i = 0; i < projectileLocalToWorld.Length; i++)
                {
                    var localToWorld = projectileLocalToWorld[i];
                    transformDataArray[i] = new ProjectilePositionData
                    {
                        Position = localToWorld.Position,
                        IsAlive = 1,
                    };
                }
            }
            graphicsBuffer.SetData(transformDataArray);

            projectileLocalToWorld.Dispose();
            visualEffect.SetGraphicsBuffer(positionBufferId, graphicsBuffer);

            transformDataArray.Dispose();
        }

        private void ReallocateBuffer(int size)
        {
            graphicsBuffer?.Release();

            graphicsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, size,
                Marshal.SizeOf(typeof(ProjectilePositionData)));

            graphicsBuffer.SetData(new ProjectilePositionData[size]);
        }

        private void ReleaseBuffer()
        {
            if (graphicsBuffer == null) return;

            graphicsBuffer.Release();
            graphicsBuffer.Dispose();
        }
    }
}
