using System;
using UnityEngine;
using UnityEngine.EventSystems;
using vikwhite.View;
using Zenject;

public class UIDropContainer : MonoBehaviour, IDropHandler
{
    public Transform Container;
    public Func<UIDrag, bool> OnAddElement;
    public Action<UIDrag> OnRemoveElement;
    private IUIService _ui;

    [Inject]
    public void Construct(IUIService ui) {
        _ui = ui;
    }

    public void OnDrop(PointerEventData eventData) {
        _ui.GetDragElement()?.SetTargetContainer(this);
    }

    public void Add(UIDrag element) {
        element.transform.SetParent(Container);
    }
}