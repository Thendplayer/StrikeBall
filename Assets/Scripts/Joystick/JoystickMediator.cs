using System;
using MessagePipe;
using StrikeBall.Core.Events;
using StrikeBall.Core.UseCases;
using UnityEngine;
using VContainer.Unity;

namespace StrikeBall.Core.Joystick
{
    public class JoystickMediator : IInitializable, ITickable, IDisposable
    {
        private readonly JoystickView _view;
        private readonly JoystickModel _model;
        private readonly TouchInputHandlerUseCase _touchInputHandler;
        private readonly IPublisher<JoystickDragEvent> _joystickInputPublisher;
        private readonly IPublisher<JoystickPressedEvent> _joystickPressedPublisher;
        private readonly IPublisher<JoystickReleasedEvent> _joystickReleasedPublisher;

        public JoystickMediator(JoystickView view, JoystickModel model, TouchInputHandlerUseCase touchInputHandler, 
            IPublisher<JoystickDragEvent> joystickInputPublisher, 
            IPublisher<JoystickPressedEvent> joystickPressedPublisher,
            IPublisher<JoystickReleasedEvent> joystickReleasedPublisher)
        {
            _view = view;
            _model = model;
            _touchInputHandler = touchInputHandler;
            _joystickInputPublisher = joystickInputPublisher;
            _joystickPressedPublisher = joystickPressedPublisher;
            _joystickReleasedPublisher = joystickReleasedPublisher;
        }

        public void Initialize()
        {
            _model.SetOriginalPosition(_view.RectTransform.anchoredPosition);
            
            _touchInputHandler.OnTouchStarted += HandleTouchStarted;
            _touchInputHandler.OnTouchEnded += HandleTouchEnded;
            _touchInputHandler.OnTouchMoved += HandleTouchMoved;
        }
        
        public void Tick()
        {
            var isDragging = _model.IsDragging;
            _touchInputHandler.HandleInput(ref isDragging);
            _model.SetDragging(isDragging);

            if (!_model.IsActive) return;
            var inputEvent = new JoystickDragEvent(_model.Direction, _model.Magnitude);
            _joystickInputPublisher.Publish(inputEvent);
        }

        public void Dispose()
        {
            _touchInputHandler.OnTouchStarted -= HandleTouchStarted;
            _touchInputHandler.OnTouchEnded -= HandleTouchEnded;
            _touchInputHandler.OnTouchMoved -= HandleTouchMoved;
        }
        
        private void HandleTouchStarted(Vector2 screenPosition)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_view.CanvasRectTransform, screenPosition, _view.Camera, out var canvasPosition))
            {
                _view.SetPosition(canvasPosition);
                _view.SetHandlePosition(Vector2.zero);
                _joystickPressedPublisher.Publish(new JoystickPressedEvent());
            }
        }
        
        private void HandleTouchEnded()
        {
            _model.ResetInput();
            _view.SetHandlePosition(Vector2.zero);
            _view.SetPosition(_model.OriginalPosition);
            _joystickReleasedPublisher.Publish(new JoystickReleasedEvent());
        }

        private void HandleTouchMoved(Vector2 screenPosition)
        {
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_view.RectTransform, screenPosition, _view.Camera, out var localPoint))
                return;
            
            if (localPoint.magnitude > _model.RelocateThreshold)
            {
                var relocateAmount = localPoint.normalized * (localPoint.magnitude - _model.MaxDistance);
                _view.RectTransform.anchoredPosition += relocateAmount;
                
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_view.RectTransform, screenPosition, _view.Camera, out localPoint);
            }
            
            var clampedOffset = Vector2.ClampMagnitude(localPoint, _model.MaxDistance);
            _model.UpdateInput(clampedOffset);
            _view.SetHandlePosition(clampedOffset);
        }
    }
}