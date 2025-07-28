using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using Zenject;

namespace vikwhite.View
{
    public enum UILayer { WORLD, GUI, WINDOW, FLYTEXT, DRAG, POPUP }

    public interface IUIRoot
    {
        Vector2 CanvasSize { get; }
        Vector2 CanvasCenter { get; }
        RectTransform GetLayer(UILayer layer);
        void Create();
    }

    public class UIRoot : MonoBehaviour, IUIRoot
    {
        private Dictionary<UILayer, RectTransform> _layers;
        private RectTransform _rectTransform;
        private ICameraService _camera;
        
        public Vector2 CanvasSize => _rectTransform.sizeDelta;
        public Vector2 CanvasCenter => CanvasSize * 0.5f;

        [Inject]
        public void Constract(ICameraService camera) {
            _camera = camera;
        }
        
        void Awake() {
            //CreateCanvas();
            _layers = new Dictionary<UILayer, RectTransform>();
            _layers[UILayer.WORLD] = CraeteLayer("WORLD");
            _layers[UILayer.GUI] = CraeteLayer("GUI");
            _layers[UILayer.WINDOW] = CraeteLayer("WINDOW");
            _layers[UILayer.FLYTEXT] = CraeteLayer("FLYTEXT");
            _layers[UILayer.DRAG] = CraeteLayer("DRAG");
            _layers[UILayer.POPUP] = CraeteLayer("POPUP");
            transform.SetParent(null);
            GameObject.DontDestroyOnLoad(this);
        }

        public RectTransform GetLayer(UILayer layer) {
            return _layers[layer];
        }

        public void Create() {
            Canvas canvas = gameObject.AddComponent<Canvas>();
            //canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = _camera.Current;
            canvas.planeDistance = 1;
            canvas.sortingOrder = 1;
            canvas.vertexColorAlwaysGammaSpace = true;
            CanvasScaler canvasScaler = gameObject.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2Int(1920, 1080);
            canvasScaler.matchWidthOrHeight = 1;
            gameObject.AddComponent<GraphicRaycaster>();
            gameObject.AddComponent<EventSystem>();
            gameObject.AddComponent<StandaloneInputModule>();
            _rectTransform = gameObject.GetComponent<RectTransform>();
        }

        private RectTransform CraeteLayer(string name) {
            GameObject gameObject = new GameObject(name);
            gameObject.transform.SetParent(transform);
            RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.localPosition = Vector3.zero;
            rectTransform.localScale = Vector3.one;
            return rectTransform;
        }
    }
}