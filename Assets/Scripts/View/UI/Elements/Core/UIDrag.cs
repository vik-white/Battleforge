using UnityEngine;
using UnityEngine.EventSystems;
using vikwhite.View;
using Zenject;

public class UIDrag : UIElement, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public UIDropContainer SourceContainer;
    [HideInInspector] public UIDropContainer TargetContainer;
    private CanvasGroup _canvasGroup;
    private IUIRoot _uIRoot;
    protected IMouseService _mouse;
    protected IUIService _ui;
    protected bool _isDraggable = true;
    protected bool _isDragging = false;

    [Inject]
    public void Construct(IUIRoot uIRoot, IMouseService mouse, IUIService ui) {
        _uIRoot = uIRoot;
        _mouse = mouse;
        _ui = ui;
        _canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public virtual void OnBeginDrag(PointerEventData eventData) {
        if(!_isDraggable) return;
        _isDragging = true;
        _ui.SetDragElement(this);
        TargetContainer = null;
        _canvasGroup.blocksRaycasts = false;
        SourceContainer = transform.GetComponentInParent<UIDropContainer>();
        transform.SetParent(_uIRoot.GetLayer(UILayer.DRAG));
        SourceContainer.OnRemoveElement?.Invoke(this);
        RectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        RectTransform.anchorMax = new Vector2(0.5f, 0.5f);
    }

    public virtual void OnDrag(PointerEventData eventData) {
        RectTransform.anchoredPosition = _mouse.MouseCanvasPosition - _uIRoot.CanvasCenter;
    }

    public virtual void OnEndDrag(PointerEventData eventData) {
        if(!_isDraggable) return;
        _canvasGroup.blocksRaycasts = true;
        if(TargetContainer == null) {
            transform.SetParent(SourceContainer.Container);
            SourceContainer.OnAddElement?.Invoke(this);
        }
        else {
            if(SourceContainer == TargetContainer) {
                transform.SetParent(SourceContainer.Container);
                SourceContainer.OnAddElement?.Invoke(this);
            }
            else {
                transform.SetParent(TargetContainer.Container);
                if(TargetContainer.OnAddElement == null || !TargetContainer.OnAddElement.Invoke(this)) {
                    transform.SetParent(SourceContainer.Container);
                    SourceContainer.OnAddElement?.Invoke(this);
                }
            }
        }
        TargetContainer = null;
    }

    public void SetTargetContainer(UIDropContainer targetContainer) {
        TargetContainer = targetContainer;
    }

    public bool IsSource<T>() where T : class {
        return SourceContainer.GetComponentInParent<T>() != null;
    }
}