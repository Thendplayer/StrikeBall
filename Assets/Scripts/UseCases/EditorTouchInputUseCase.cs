using UnityEngine.InputSystem;

namespace StrikeBall.Core.UseCases
{
    public class EditorTouchInputUseCase : TouchInputHandlerUseCase
    {
        protected override InputState GetInputState()
        {
            var mouse = Mouse.current;
            return new InputState
            {
                Position = mouse.position.ReadValue(),
                IsDown = mouse.leftButton.wasPressedThisFrame,
                IsUp = mouse.leftButton.wasReleasedThisFrame,
                IsHeld = mouse.leftButton.isPressed
            };
        }
    }
}