using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class BattleCardView : UIDrag
{
    public Func<ICharacterData, Vector3, bool> OnDragCardToBoardPlace;
    public Action OnPlaceCharacter;
    public Action<CharacterType> OnDrag;
    public Action OnDrop;
    
    [SerializeField] private CardView _cardView;
    [SerializeField] private GameObject _lock;
    private ICharacterData _data;
    
    public void Initialize(ICharacterData  data) {
        _data = data;
        _cardView.Initialize(data, null);
    }

    public override void OnBeginDrag(PointerEventData eventData) {
        base.OnBeginDrag(eventData);
        OnDrag?.Invoke(_data.Type);
    }

    public override void OnEndDrag(PointerEventData eventData) {
        var position = _mouse.GetWorldGroundPosition();
        if (OnDragCardToBoardPlace.Invoke(_data, position)){
            GameObject.Destroy(gameObject);
            OnPlaceCharacter?.Invoke();
        }
        else base.OnEndDrag(eventData);
        OnDrop?.Invoke();
    }
    
    public void Lock() {
        _isDraggable = false;
        _lock.SetActive(true);
    }
}