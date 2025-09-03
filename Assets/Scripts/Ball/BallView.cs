using UnityEngine;
using VContainer;

namespace StrikeBall.Core.Ball
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
    public class BallView : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        
        public Vector3 Direction => _rigidbody ? _rigidbody.linearVelocity : Vector3.zero;
        public float Speed => _rigidbody ? _rigidbody.linearVelocity.magnitude : 0f;
        
        [Inject]
        public void Construct()
        {
            _rigidbody.useGravity = false;
            _rigidbody.linearDamping = 0f;
            _rigidbody.angularDamping = 0f;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | 
                                     RigidbodyConstraints.FreezeRotationZ |
                                     RigidbodyConstraints.FreezePositionY;
        }

        public void ApplyLinearVelocity(float maxSpeed)
        {
            _rigidbody.linearVelocity = _rigidbody.linearVelocity.normalized * maxSpeed;
        }
        
        public void ApplyForce(Vector3 force)
        {
            _rigidbody.AddForce(force, ForceMode.Impulse);
        }
        
        public void ResetPosition(Vector3 position)
        {
            transform.position = position;
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }
    }
}