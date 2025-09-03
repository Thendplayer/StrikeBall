using UnityEngine;

namespace StrikeBall.Core.Entity
{
    public class EntityModel
    {
        private readonly float _maxSpeed;
        private readonly float _minAngleForRotation;
        private readonly float _rotationSmoothTime;
        private readonly float _hitRadius;
        private readonly float _hitForce;
        private readonly float _kickForce;
        
        private Vector3 _velocity;
        private float _rotationSpeed;
        private bool _isMoving;

        public Vector3 Velocity => _velocity;
        public float MaxSpeed => _maxSpeed;
        public bool IsMoving => _isMoving;
        public float RotationSpeed =>  _rotationSpeed;
        public float HitRadius => _hitRadius;
        public float HitForce => _hitForce;
        public float KickForce => _kickForce;

        public EntityModel(EntityData data)
        {
            _maxSpeed = data.MaxSpeed;
            _minAngleForRotation = data.MinAngleForRotation;
            _rotationSmoothTime = data.RotationSmoothTime;
            _hitRadius = data.HitRadius;
            _hitForce = data.HitForce;
            _kickForce = data.KickForce;
            
            _rotationSpeed = 0f;
            _velocity = Vector3.zero;
            _isMoving = false;
        }
        
        public void SetVelocity(Vector2 direction, float magnitude)
        {
            var input = direction * magnitude;
            var inputDirection = new Vector3(input.x, 0f, input.y);
            _velocity = inputDirection * _maxSpeed;
        }
        
        public void SetMoving(bool isMoving)
        {
            _isMoving = isMoving;
            if (!isMoving)
            {
                _velocity = Vector3.zero;
                _rotationSpeed = 0f;
            }
        }

        public float CalculateRotationAngle(float currentAngle, float targetAngle)
        {
            var angleDifference = Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle));
            if (!(angleDifference > _minAngleForRotation)) return 0;
            
            return Mathf.SmoothDampAngle(
                currentAngle, 
                targetAngle, 
                ref _rotationSpeed, 
                _rotationSmoothTime
            );
        }
    }
}