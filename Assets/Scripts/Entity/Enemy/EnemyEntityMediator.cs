using StrikeBall.Core.UseCases;
using UnityEngine;

namespace StrikeBall.Core.Entity.Enemy
{
    public class EnemyEntityMediator : EntityMediator
    {
        public EnemyEntityMediator(
            EntityView view, 
            EntityModel model,
            PredictBallPositionUseCase predictBallPositionUseCase,
            TriggerBallHitUseCase triggerBallHitUseCase) : base(view, model, predictBallPositionUseCase, triggerBallHitUseCase)
        {
        }

        public override void FixedTick()
        {
            CalculateAIMovement();
            TriggerHit(_model.KickForce);
            base.FixedTick();
        }

        private void CalculateAIMovement()
        {
            var enemyPosition = _view.Position;
            var targetPosition = _predictBallPositionUseCase.GetTargetPosition(enemyPosition, _model.MaxSpeed);
            var deltaX = targetPosition.x - enemyPosition.x;
            
            if (Mathf.Abs(deltaX) < 0.1f)
            {
                _model.SetMoving(false);
                return;
            }

            var direction = deltaX > 0 ? Vector2.right : Vector2.left;
            HandleMovementInput(direction, 1f);
            _model.SetMoving(true);
        }
    }
}