using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIScrollAndDrag : UIDrag, IPointerDownHandler, IPointerUpHandler
{
    private ScrollRect _scrollRect;
    private Vector2 _startDragPos;
    private bool _isDraggingObject = false;
    private bool _isScrolling = false;
    private bool _pointerDown = false;
    private const float _dragThreshold = 10f;
    protected Action _onClickWithoutDrag;
    
    public override void OnBeginDrag(PointerEventData eventData) {
        _scrollRect = GetComponentInParent<ScrollRect>();
        _startDragPos = eventData.position;
        _isDraggingObject = false;
        _isScrolling = false;
    }

    public override void OnDrag(PointerEventData eventData) {
        _isDragging = true;
        if (!_isDraggingObject && !_isScrolling) {
            Vector2 delta = eventData.position - _startDragPos; 
            if (_scrollRect == null) {
                _isDraggingObject = true;
                base.OnBeginDrag(eventData);
            }
            else if (Mathf.Abs(delta.y) > _dragThreshold && Mathf.Abs(delta.y) > Mathf.Abs(delta.x)) {
                _isDraggingObject = true;
                base.OnBeginDrag(eventData);
            }
            else if (Mathf.Abs(delta.x) > _dragThreshold && Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) {
                _isScrolling = true;
                _scrollRect?.OnBeginDrag(eventData);
            }
        }
        if (_isDraggingObject) {
            base.OnDrag(eventData);
        }
        else if (_isScrolling) {
            _scrollRect?.OnDrag(eventData);
        }
    }

    public override void OnEndDrag(PointerEventData eventData) {
        if (_isScrolling) {
            _scrollRect?.OnEndDrag(eventData);
        }
        else {
            base.OnEndDrag(eventData);
        }
        _isDraggingObject = false;
        _isScrolling = false;
    }
    
    public void OnPointerDown(PointerEventData eventData) {
        _pointerDown = true;
    }
    
    public void OnPointerUp(PointerEventData eventData) {
        if (_pointerDown && !_isDragging) {
            _onClickWithoutDrag?.Invoke();
        }
        _pointerDown = false;
        _isDragging = false;
    }
}