using UnityEngine;

namespace StrikeBall.Core.Joystick
{
    [CreateAssetMenu(menuName = "Create Data/Joystick", fileName = "JoystickData")]
    public class JoystickData : ScriptableObject
    {
        [Header("Basic Settings")]
        [SerializeField, Range(50f, 200f)] private float _maxDistance = 100f;
        
        [Header("Dynamic Positioning")]
        [SerializeField, Range(80f, 250f)] private float _relocateThreshold = 120f;
        
        [Header("Input Sensitivity")]
        [SerializeField, Range(0.001f, 0.1f)] private float _inputDeadZone = 0.001f;
        
        public float MaxDistance => _maxDistance;
        public float RelocateThreshold => _relocateThreshold;
        public float InputDeadZone => _inputDeadZone;
        
        private void OnValidate()
        {
            _maxDistance = Mathf.Max(50f, _maxDistance);
            _relocateThreshold = Mathf.Max(_maxDistance, _relocateThreshold);
            _inputDeadZone = Mathf.Max(0.001f, _inputDeadZone);
        }
    }
}