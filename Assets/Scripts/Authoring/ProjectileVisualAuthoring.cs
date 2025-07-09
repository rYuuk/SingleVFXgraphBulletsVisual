using Unity.Entities;
using UnityEngine;
using UnityEngine.VFX;

namespace SingleVFXGraphBulletsVisual
{
    public class ProjectileVisualAuthoring : MonoBehaviour
    {
        [SerializeField] private VisualEffect visualEffect;
        public class ProjectileVisualBaker : Baker<ProjectileVisualAuthoring>
        {
            public override void Bake(ProjectileVisualAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<ProjectileVisual>(entity);
            }
        }
    }

    public struct ProjectileVisual : IComponentData
    {
    }
}
