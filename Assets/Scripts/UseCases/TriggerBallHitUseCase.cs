using UnityEngine;
using StrikeBall.Core.Ball;

namespace StrikeBall.Core.UseCases
{
    public class TriggerBallHitUseCase
    {
        private readonly BallView _ballView;

        public TriggerBallHitUseCase(BallView ballView)
        {
            _ballView = ballView;
        }

        public void ExecuteHit(Vector3 hitDirection, float hitForce)
        {
            var force = hitDirection.normalized * hitForce;
            _ballView.ApplyForce(force);
        }
    }
}