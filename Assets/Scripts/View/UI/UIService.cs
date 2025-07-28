using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace vikwhite.View
{
    public interface IUIService
    {
        T CreateElement<T>(T prefab) where T : Component;
        T CreateFlytext<T>(T prefab) where T : Component;
        T CreateWorldUI<T>(T prefab) where T : Component;
        T CreateWindow<T>(T prefab) where T : Component;
        T Get<T>() where T : IUIElement;
        void Close(IUIElement element);
        void Close<T>() where T : IUIElement;
        UIDrag GetDragElement();
        void SetDragElement(UIDrag drag);
    }

    public class UIService : IUIService, ITickable
    {
        protected readonly IUIRoot _uiRoot;
        protected readonly IMouseService _mouse;
        protected readonly IViewFactory _viewFactory;
        private readonly List<IUIElement> _elements;
        private UIDrag _dragElement;

        public UIService(IUIRoot uiRoot, IMouseService mouse, IViewFactory viewFactory) {
            _uiRoot = uiRoot;
            _mouse = mouse;
            _viewFactory = viewFactory;
            _elements = new();
        }

        public T CreateWindow<T>(T prefab) where T : Component => Create(prefab, UILayer.WINDOW);

        public T CreateElement<T>(T prefab) where T : Component => Create(prefab, UILayer.GUI);
        
        public T CreateFlytext<T>(T prefab) where T : Component => Create(prefab, UILayer.FLYTEXT);
        
        public T CreateWorldUI<T>(T prefab) where T : Component => Create(prefab, UILayer.WORLD);

        public T Get<T>() where T : IUIElement => (T)_elements.Find(e => e is T);

        public void Close(IUIElement element) {
            if(_elements.Contains(element))
                _elements.Remove(element);
            element.Destroy();
        }

        public void Close<T>() where T : IUIElement {
            IUIElement element = Get<T>();
            if(element != null) Close(element);
        }

        protected T Create<T>(T prefab, UILayer layer) where T : Component {
            T element = _viewFactory.Create(prefab, _uiRoot.GetLayer(layer));
            _elements.Add(element as IUIElement);
            return element;
        }

        public void Tick() {
            _elements.ToList().ForEach(e => e.UpdatePosition());
        }
        
        public UIDrag GetDragElement() => _dragElement;
        
        public void SetDragElement(UIDrag drag) => _dragElement = drag;
    }
}