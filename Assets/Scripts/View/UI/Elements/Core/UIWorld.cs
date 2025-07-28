using UnityEngine;
using Zenject;

namespace vikwhite.View
{
    public class UIWorld : UIElement {
        protected Transform _target;
        protected Vector3 _offset;
        protected IUIRoot _uIRoot;
        protected IUIService _uIService;
        protected ICameraService _camera;

        [Inject]
        public void Construct(IUIRoot uIRoot, IUIService uIService, ICameraService camera) {
            _uIRoot = uIRoot;
            _uIService = uIService;
            _camera = camera;
        }

        protected virtual Vector3 GetWorldPosition() {
            return (_target != null ? _target.position : Vector3.zero) + _offset;
        }

        public override void UpdatePosition() {
            Vector3 position = _camera.Current.WorldToViewportPoint(GetWorldPosition());
            RectTransform.anchoredPosition = position * _uIRoot.CanvasSize - _uIRoot.CanvasCenter;
        }
    }
}