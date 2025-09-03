using UnityEngine;

namespace StrikeBall.Core.Entity
{
    [CreateAssetMenu(menuName = "Create Data/Entity", fileName = "EntityData")]
    public class EntityData : ScriptableObject
    {
        public static class AnimatorParameters
        {
            public static readonly int Move = Animator.StringToHash("Move");
            public static readonly int Hit = Animator.StringToHash("Hit");
        }
        
        [Header("Movement")]
        [SerializeField] private float _maxSpeed = 2f;
        
        [Header("Rotation")]
        [SerializeField] private float _rotationSmoothTime = 0.06f;
        [SerializeField] private float _minAngleForRotation = 2f;
        
        [Header("Hit")]
        [SerializeField] private float _hitRadius = 2f;
        [SerializeField] private float _hitForce = 10f;
        [SerializeField] private float _kickForce = 20f;
        
        public float MaxSpeed => _maxSpeed;
        public float MinAngleForRotation => _minAngleForRotation;
        public float RotationSmoothTime  => _rotationSmoothTime;
        public float HitRadius => _hitRadius;
        public float HitForce => _hitForce;
        public float KickForce => _kickForce;
    }
}