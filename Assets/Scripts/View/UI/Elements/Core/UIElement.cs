using System;
using UnityEngine;

namespace vikwhite.View
{
    public interface IUIElement : IView
    {
        RectTransform RectTransform { get; }
        void Destroy();
        void Hide();
        void Show();
        void UpdatePosition();
    }

    public class UIElement : MonoBehaviour, IUIElement
    {
        private RectTransform _rectTransform;
        public RectTransform RectTransform => _rectTransform ?? (_rectTransform = GetComponent<RectTransform>());
        public Action OnClose;

        public virtual void Destroy() => GameObject.Destroy(gameObject);

        public virtual void Show() => gameObject.SetActive(true);

        public virtual void Hide() => gameObject.SetActive(false);

        public virtual void UpdatePosition() { }

        private void OnDestroy() => OnClose?.Invoke();
    }
}