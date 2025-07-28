using UnityEngine;
using Zenject;

namespace vikwhite.View
{
    public interface IMouseService
    {
        Vector2 MouseCanvasPosition { get; }
        void ShowCursor(bool show = true);
        Vector3 GetWorldGroundPosition();
    }

    public class MouseService : IMouseService, ITickable
    {
        private readonly IUIRoot _uIRoot;
        private Vector2 _mouseCanvasPosition;
        public Vector2 MouseCanvasPosition => _mouseCanvasPosition;

        public MouseService(IUIRoot uIRoot) {
            _uIRoot = uIRoot;
        }

        public void ShowCursor(bool show = true) {
            Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = show;
        }

        public void Tick() {
            if(Camera.current == null) return;
            _mouseCanvasPosition = Camera.current.ScreenToViewportPoint(Input.mousePosition) * _uIRoot.CanvasSize;
        }
        
        public Vector3 GetWorldGroundPosition() {
            var ray = Camera.current.ScreenPointToRay(Input.mousePosition);
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            if (groundPlane.Raycast(ray, out float enter)) return ray.GetPoint(enter);
            return default;
        }
    }
}