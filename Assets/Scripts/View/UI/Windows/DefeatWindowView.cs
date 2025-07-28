using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace vikwhite.View
{
    public class DefeatWindowView : UIElement
    {
        [SerializeField] private Button _button;
        
        public void Initialize(Action callback) {
            _button.onClick.AddListener(() => callback?.Invoke());
            
        }
    }
}
