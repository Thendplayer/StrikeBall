using UnityEngine;
using VContainer;

namespace StrikeBall.Core.Joystick
{
    public class JoystickView : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RectTransform _joystickHandle;
        
        private Canvas _parentCanvas;
        private Camera _camera;
        private RectTransform _parentCanvasRectTransform;
        
        public RectTransform RectTransform => _rectTransform;
        public RectTransform CanvasRectTransform => _parentCanvasRectTransform;
        public Camera Camera => _camera;
        
        [Inject]
        public void Construct(Canvas parentCanvas, Camera camera)
        {
            _parentCanvas = parentCanvas;
            _camera = camera;
            _parentCanvasRectTransform = _parentCanvas.transform as RectTransform;
            _joystickHandle.anchoredPosition = Vector2.zero;
        }
        
        public void SetPosition(Vector2 position) => _rectTransform.anchoredPosition = position;
        public void SetHandlePosition(Vector2 position) => _joystickHandle.anchoredPosition = position;
    }
}