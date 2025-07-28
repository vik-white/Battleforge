using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace vikwhite.View
{
    public class UIFlytext : UIElement
    {
        private float _moveDistance = 0.07f;
        private float _time = 1.5f;
        protected IUIRoot _uIRoot;
        protected IUIService _ui;

        [Inject]
        public void Construct(IUIRoot uIRoot, IUIService ui) {
            _uIRoot = uIRoot;
            _ui = ui;
        }

        public override void UpdatePosition() {
            RectTransform.anchoredPosition += new Vector2(0, _moveDistance * _uIRoot.CanvasSize.y) * Time.deltaTime;
            UpdateDelay();
        }

        private void UpdateDelay() {
            _time -= Time.deltaTime;
            if(_time <= 0) _ui.Close(this);
        }
    }
}