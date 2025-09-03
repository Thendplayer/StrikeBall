using UnityEngine;
using UnityEngine.InputSystem;

namespace StrikeBall.Core.UseCases
{
    public class MobileTouchInputUseCase : TouchInputHandlerUseCase
    {
        protected override InputState GetInputState()
        {
            var position = Vector2.zero;
            var isDown = false;
            var isUp = false;
            var isHeld = false;
            
            var touchscreen = Touchscreen.current;
            if (touchscreen != null && touchscreen.touches.Count > 0)
            {
                var touch = touchscreen.touches[0];
                position = touch.position.ReadValue();
                
                var phase = touch.phase.ReadValue();
                isDown = phase == UnityEngine.InputSystem.TouchPhase.Began;
                isUp = phase == UnityEngine.InputSystem.TouchPhase.Ended || phase == UnityEngine.InputSystem.TouchPhase.Canceled;
                isHeld = phase == UnityEngine.InputSystem.TouchPhase.Moved || phase == UnityEngine.InputSystem.TouchPhase.Stationary;
            }
            else if (_isDragging)
            {
                isUp = true; //No touches while dragging means touch ended
            }

            return new InputState
            {
                Position = position,
                IsDown = isDown,
                IsUp = isUp,
                IsHeld = isHeld
            };
        }
    }
}