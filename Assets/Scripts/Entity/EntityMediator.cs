using System;
using StrikeBall.Core.UseCases;
using UnityEngine;
using VContainer.Unity;

namespace StrikeBall.Core.Entity
{
    public abstract class EntityMediator : IInitializable, IDisposable, IFixedTickable
    {
        protected readonly EntityView _view;
        protected readonly EntityModel _model;
        
        protected readonly PredictBallPositionUseCase _predictBallPositionUseCase;
        protected readonly TriggerBallHitUseCase _triggerBallHitUseCase;

        protected EntityMediator(
            EntityView view, 
            EntityModel model, 
            PredictBallPositionUseCase predictBallPositionUseCase,
            TriggerBallHitUseCase triggerBallHitUseCase
        )
        {
            _view = view;
            _model = model;
            _predictBallPositionUseCase = predictBallPositionUseCase;
            _triggerBallHitUseCase = triggerBallHitUseCase;
        }

        public virtual void Initialize()
        {
            _view.OnCollisionTriggered += HandleCollision;
        }

        public virtual void Dispose()
        {
            _view.OnCollisionTriggered -= HandleCollision;
            
            if (_view != null && _view.gameObject != null)
            {
                UnityEngine.Object.DestroyImmediate(_view.gameObject);
            }
        }

        public virtual void FixedTick()
        {
            UpdateMovement();
            UpdateRotation();
        }

        protected void HandleMovementInput(Vector2 direction, float magnitude)
        {
            _model.SetVelocity(direction, magnitude);
        }

        protected void TriggerHit(float force)
        {
            var ballPosition = _predictBallPositionUseCase.GetCurrentBallPosition();
            var distanceToBall = Vector3.Distance(_view.Position, ballPosition);

            if (distanceToBall > _model.HitRadius) return;
            
            var directionToBall = (ballPosition - _view.Position).normalized;
            var targetAngle = Mathf.Atan2(directionToBall.x, directionToBall.z) * Mathf.Rad2Deg;
            
            _view.MoveRotation(new Vector3(0f, targetAngle, 0f));
            _triggerBallHitUseCase.ExecuteHit(directionToBall, force);
            _view.TriggerAnimationParameter(EntityData.AnimatorParameters.Hit);
        }

        private void HandleCollision() => TriggerHit(_model.HitForce);
        
        private void UpdateMovement()
        {
            _view.MovePosition(_model.Velocity * Time.fixedDeltaTime);
            _view.UpdateBoolAnimationParameter(EntityData.AnimatorParameters.Move, _model.IsMoving);
        }

        private void UpdateRotation()
        {
            if (_model.Velocity.sqrMagnitude <= 0) return;
            
            var targetAngle = Mathf.Atan2(_model.Velocity.x, _model.Velocity.z) * Mathf.Rad2Deg;
            var currentAngle = _view.EulerAngles.y;
            
            var newAngle = _model.CalculateRotationAngle(currentAngle, targetAngle);
            if (newAngle == 0) return;
            
            _view.MoveRotation(new Vector3(0f, newAngle, 0f));
        }
    }
}