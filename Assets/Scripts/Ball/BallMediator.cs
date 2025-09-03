using System;
using VContainer.Unity;

namespace StrikeBall.Core.Ball
{
    public class BallMediator : IInitializable, IFixedTickable
    {
        private readonly BallView _view;
        private readonly BallModel _model;

        public BallMediator(BallView view, BallModel model)
        {
            _view = view;
            _model = model;
        }

        public void Initialize()
        {
            _view.ResetPosition(_model.Position);
            _view.ApplyForce(_model.GetRandomDirection() * (_model.MaxSpeed / 2));
        }
        
        public void FixedTick()
        {
            if (_view?.Speed > _model?.MaxSpeed)
            {
                _view.ApplyLinearVelocity(_model.MaxSpeed);
            }
        }
    }
}