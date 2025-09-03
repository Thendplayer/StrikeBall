using UnityEngine;
using System;

namespace StrikeBall.Core.UseCases
{
    public abstract class TouchInputHandlerUseCase
    {
        public event Action<Vector2> OnTouchStarted;
        public event Action<Vector2> OnTouchMoved;
        public event Action OnTouchEnded;

        protected bool _isDragging;

        public void HandleInput(ref bool isDragging)
        {
            _isDragging = isDragging;
            var inputState = GetInputState();

            if (!_isDragging)
            {
                if (inputState.IsDown)
                {
                    OnTouchStarted?.Invoke(inputState.Position);
                    _isDragging = true;
                }
            }
            else
            {
                if (inputState.IsUp)
                {
                    OnTouchEnded?.Invoke();
                    _isDragging = false;
                }
                else if (inputState.IsHeld)
                {
                    OnTouchMoved?.Invoke(inputState.Position);
                }
            }

            isDragging = _isDragging;
        }

        protected abstract InputState GetInputState();

        protected struct InputState
        {
            public Vector2 Position;
            public bool IsDown;
            public bool IsUp;
            public bool IsHeld;
        }
    }
}