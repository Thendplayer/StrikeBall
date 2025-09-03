using UnityEngine;

namespace StrikeBall.Core.Joystick
{
    public class JoystickModel
    {
        private Vector2 _originalPosition;
        private float _maxDistance;
        private float _relocateThreshold;
        private float _inputDeadZone;
        
        private Vector2 _direction;
        private float _magnitude;
        private bool _isActive;
        private bool _isDragging;

        public Vector2 OriginalPosition => _originalPosition;
        public float MaxDistance => _maxDistance;
        public float RelocateThreshold => _relocateThreshold;
        public float InputDeadZone => _inputDeadZone;
        
        public Vector2 Direction => _direction;
        public float Magnitude => _magnitude;
        public bool IsActive => _isActive;
        public bool IsDragging => _isDragging;

        public JoystickModel(JoystickData data)
        {
            _maxDistance =  data.MaxDistance;
            _relocateThreshold =  data.RelocateThreshold;
            _inputDeadZone =  data.InputDeadZone;
        }
        
        public void SetOriginalPosition(Vector2 originalPosition) => _originalPosition = originalPosition;
        public void SetDragging(bool isDragging) => _isDragging = isDragging;

        public void UpdateInput(Vector2 handlePosition)
        {
            var inputVector = handlePosition / _maxDistance;
            
            _direction = inputVector.normalized;
            _magnitude = Mathf.Clamp01(inputVector.magnitude);
            _isActive = Magnitude > _inputDeadZone;
        }
        
        public void ResetInput()
        {
            _isDragging = false;
            _direction = Vector2.zero;
            _magnitude = 0f;
            _isActive = false;
        }
    }
}