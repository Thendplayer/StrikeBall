using System;
using UnityEngine;
using VContainer;

namespace StrikeBall.Core.Entity
{
    public class EntityView : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Animator _animator;
        
        private Transform _cameraTransform;

        public Vector3 Position => _rigidbody.position;
        public Vector3 EulerAngles => _rigidbody.transform.eulerAngles;
        public Action OnCollisionTriggered;

        [Inject]
        public void Construct(Camera camera)
        {
            _cameraTransform = camera.transform;
            
            _rigidbody.useGravity = false;
            _rigidbody.drag = 5f;
            _rigidbody.angularDrag = 5f;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | 
                                     RigidbodyConstraints.FreezeRotationZ |
                                     RigidbodyConstraints.FreezePositionY;
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            var collisionRigidbody = collision.rigidbody;
            if (collisionRigidbody == null || collisionRigidbody.isKinematic) return;
            OnCollisionTriggered();
        }
        
        public void MovePosition(Vector3 direction) => _rigidbody.MovePosition(_rigidbody.position + direction);
        public void MoveRotation(Vector3 rotation) => _rigidbody.MoveRotation(Quaternion.Euler(rotation));
        public void UpdateBoolAnimationParameter(int parameterHash, bool value) => _animator.SetBool(parameterHash, value);
        public void TriggerAnimationParameter(int parameterHash) => _animator.SetTrigger(parameterHash);
    }
}