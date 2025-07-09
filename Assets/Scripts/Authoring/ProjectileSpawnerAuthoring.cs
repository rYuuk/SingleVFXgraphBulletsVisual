using Unity.Entities;
using UnityEngine;

namespace SingleVFXGraphBulletsVisual
{
    public class ProjectileSpawnerAuthoring : MonoBehaviour
    {
        [SerializeField] private Transform spawnerTransform;
        [SerializeField] private float projectileSpeed;
        [SerializeField] private float projectileLifetime;
        [SerializeField] private bool rotate;
        [SerializeField] private float rotateSpeed;
        [SerializeField] private GameObject objectToRotate;

        public class ProjectileSpawnerBaker : Baker<ProjectileSpawnerAuthoring>
        {
            public override void Bake(ProjectileSpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ProjectileSpawner
                {
                    ProjectileSpeed = authoring.projectileSpeed,
                    ProjectileLifetime = authoring.projectileLifetime,
                    SpawnPointEntity = GetEntity(authoring.spawnerTransform, TransformUsageFlags.Dynamic),
                    Mallet = GetEntity(authoring.objectToRotate, TransformUsageFlags.Dynamic)
                });

                if (authoring.rotate)
                {
                    AddComponent(entity, new AddRotation
                    {
                        Speed = authoring.rotateSpeed,
                    });
                }
            }
        }
    }

    public struct ProjectileSpawner : IComponentData
    {
        public float ProjectileSpeed;
        public float ProjectileLifetime;
        public Entity SpawnPointEntity;
        public Entity Mallet;
    }

    public struct AddRotation : IComponentData
    {
        public float Speed;
    }
}
