using UnityEngine;
using StrikeBall.Core.Ball;

namespace StrikeBall.Core.UseCases
{
    public class PredictBallPositionUseCase
    {
        private readonly BallView _ballView;

        public PredictBallPositionUseCase(BallView ballView)
        {
            _ballView = ballView;
        }

        public Vector3 GetCurrentBallPosition()
        {
            return _ballView.transform.position;
        }

        public Vector3 GetTargetPosition(Vector3 entityPosition, float entitySpeed)
        {
            var ballPosition = _ballView.transform.position;
            var ballVelocityX = _ballView.Direction.x;
            
            var relativeDistance = entityPosition.x - ballPosition.x;
            var timeToIntercept = CalculateInterceptionTime(
                relativeDistance, ballVelocityX, entitySpeed
            );

            var predictedX = ballPosition.x + ballVelocityX * timeToIntercept;
            return new Vector3(predictedX, entityPosition.y, entityPosition.z);
        }

        private float CalculateInterceptionTime(float relativeDistance, float relativeVelocity, float entitySpeed)
        {
            var discriminant = entitySpeed * entitySpeed - relativeVelocity * relativeVelocity;
            if (discriminant < 0) return Mathf.Abs(relativeDistance) / entitySpeed;

            var t = relativeDistance / relativeVelocity;
            return Mathf.Max(0, t);
        }
    }
}