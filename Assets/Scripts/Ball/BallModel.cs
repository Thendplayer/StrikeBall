using UnityEngine;

namespace StrikeBall.Core.Ball
{
    public class BallModel
    {
        private readonly float _maxSpeed;
        private readonly Vector3 _position;

        private Vector3 _randomDirection;
        
        public float MaxSpeed => _maxSpeed;
        public Vector3 Position => _position;

        public BallModel(BallData data)
        {
            _maxSpeed = data.MaxSpeed;
            _position = data.Position;
        }

        public Vector3 GetRandomDirection()
        {
            var z = _randomDirection.z <= 0 ? 1f : -1f;
            var x = Random.Range(-1f, 1f);
            
            _randomDirection = new Vector3(x, 0, z).normalized;
            return _randomDirection;
        }
    }
}