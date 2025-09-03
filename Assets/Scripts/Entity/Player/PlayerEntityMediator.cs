using System;
using MessagePipe;
using StrikeBall.Core.Events;
using StrikeBall.Core.UseCases;

namespace StrikeBall.Core.Entity.Player
{
    public class PlayerEntityMediator : EntityMediator
    {
        private readonly ISubscriber<JoystickDragEvent> _onJoystickDrag;
        private readonly ISubscriber<JoystickPressedEvent> _onJoystickPressed;
        private readonly ISubscriber<JoystickReleasedEvent> _onJoystickReleased;
        
        private IDisposable _joystickDragSubscription;
        private IDisposable _joystickPressedSubscription;
        private IDisposable _joystickReleasedSubscription;
        
        public PlayerEntityMediator(EntityView view,
            EntityModel model,
            ISubscriber<JoystickDragEvent> onJoystickDrag,
            ISubscriber<JoystickPressedEvent> onJoystickPressed,
            ISubscriber<JoystickReleasedEvent> onJoystickReleased,
            PredictBallPositionUseCase predictBallPositionUseCase,
            TriggerBallHitUseCase triggerBallHitUseCase) 
            : base(view, model, predictBallPositionUseCase, triggerBallHitUseCase)
        {
            _onJoystickDrag = onJoystickDrag;
            _onJoystickPressed = onJoystickPressed;
            _onJoystickReleased = onJoystickReleased;
        }

        public override void Initialize()
        {
            _joystickDragSubscription = _onJoystickDrag.Subscribe(HandleJoystickDrag);
            _joystickPressedSubscription = _onJoystickPressed.Subscribe(HandleJoystickPressed);
            _joystickReleasedSubscription = _onJoystickReleased.Subscribe(HandleJoystickReleased);
            
            base.Initialize();
        }

        public override void Dispose()
        {
            _joystickDragSubscription?.Dispose();
            _joystickPressedSubscription?.Dispose();
            _joystickReleasedSubscription?.Dispose();
            
            base.Dispose();
        }
        
        private void HandleJoystickDrag(JoystickDragEvent dragEvent)
        {
            HandleMovementInput(dragEvent.Direction, dragEvent.Magnitude);
        }
        
        private void HandleJoystickPressed(JoystickPressedEvent pressedEvent)
        {
            _model.SetMoving(true);
        }
        
        private void HandleJoystickReleased(JoystickReleasedEvent releasedEvent)
        {
            _model.SetMoving(false);
            TriggerHit(_model.KickForce);
        }
    }
}