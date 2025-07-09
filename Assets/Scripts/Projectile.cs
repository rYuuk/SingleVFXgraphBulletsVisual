using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;

namespace SingleVFXGraphBulletsVisual
{
    public struct Projectile : IComponentData
    {
        public float Lifetime;
    }

    public struct ProjectileDirection : IComponentData
    {
        public float3 Direction;
    }

    public struct ProjectileSpeed : IComponentData
    {
        public float Speed;
    }
    
    [VFXType(VFXTypeAttribute.Usage.GraphicsBuffer)]
    public struct ProjectilePositionData
    {
        public Vector3 Position;
        public int IsAlive;
    }
}
