using UnityEngine;

namespace StrikeBall.Core.Ball
{
    [CreateAssetMenu(menuName = "Create Data/Ball", fileName = "BallData")]
    public class BallData : ScriptableObject
    {
        [SerializeField] private Vector3 _position;
        [SerializeField, Range(1f, 50f)] private float _maxSpeed = 20f;
        
        public float MaxSpeed => _maxSpeed;
        public Vector3 Position => _position;
    }
}