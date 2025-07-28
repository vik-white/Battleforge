using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using vikwhite.View;
using Zenject;

public class BattleHUDView : UIElement
{
    public Func<ICharacterData, Vector3, bool> OnDragCardToBoardPlace;
    public Action OnPlaceCharacter;
    public Action<CharacterType> OnDrag;
    public Action OnDrop;
    
    [SerializeField] private TMP_Text _playerNameLeft;
    [SerializeField] private TMP_Text _playerNameRight;
    [SerializeField] private TMP_Text _roundCounter;
    [SerializeField] private TMP_Text _leftHeroHealth;
    [SerializeField] private TMP_Text _rightHeroHealth;
    [SerializeField] private GameObject _playerSelectorLeft;
    [SerializeField] private GameObject _playerSelectorRight;
    [SerializeField] private Transform _cardsContainer;
    [FormerlySerializedAs("_cardPrefab")] [SerializeField] private BattleCardView battleCardPrefab;
    private IViewFactory  _factory;
    private List<BattleCardView> _cards;
    
    [Inject]
    public void Constarct(IViewFactory factory) {
        _factory = factory;
    }
    
    public void Initialize(string player1Name, string player2Name) {
        _playerNameLeft.text = player1Name;
        _playerNameRight.text = player2Name;
        _playerSelectorRight.SetActive(false);
        _roundCounter.text = "1";
        _cards =  new List<BattleCardView>();
    }

    public void OnNextRound(int round, bool rightSide) {
        _roundCounter.text = round.ToString();
        _playerSelectorLeft.SetActive(!rightSide);
        _playerSelectorRight.SetActive(rightSide);
    }

    public void UpdateCards(List<ICharacterData> cards) {
        _cards.Clear();
        foreach (Transform child in _cardsContainer) Destroy(child.gameObject);
        foreach (var card in cards) CreateCard(card);
    }

    private void CreateCard(ICharacterData data) {
        var card = _factory.Create(battleCardPrefab, _cardsContainer);
        card.Initialize(data);
        card.OnDragCardToBoardPlace = OnDragCardToBoardPlace;
        card.OnPlaceCharacter += OnPlaceCharacter;
        card.OnPlaceCharacter += LockCards;
        card.OnDrag += OnDrag;
        card.OnDrop += OnDrop;
        _cards.Add(card);
    }

    public void LockCards() {
        foreach (var card in _cards) card.Lock();
    }

    public void SetLeftHeroHealth(float health) => _leftHeroHealth.text = Mathf.CeilToInt(health).ToString();
    
    public void SetRightHeroHealth(float health) => _rightHeroHealth.text = Mathf.CeilToInt(health).ToString();
}
