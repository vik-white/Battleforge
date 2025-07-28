using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace vikwhite.View
{
    public class SquadWindowView : UIElement
    {
        public Action<int, ICharacterData> OnSetDeckCard;
        public Action<int> OnRemoveDeckCard;
        public Action<ICharacterData> OnCardSelected;
        
        [SerializeField] private Button _closeButton;
        [SerializeField] private UIDropContainer _cardsContainer;
        [SerializeField] private List<UIDropContainer> _deckContainers;
        [SerializeField] private CardView _cardPrefab;

        private IUIService _ui;
        private IViewFactory  _factory;
        
        [Inject]
        public void Constract(IUIService ui, IViewFactory  factory) {
            _ui = ui;
            _factory = factory;
        } 
        
        public void Initialize(List<ICharacterData> cards, ICharacterData[] deck) {
            _closeButton.onClick.AddListener(() => _ui.Close<SquadWindowView>());
            CreateCards(cards.ToList(), deck);
            CreateDeck(deck);
        }
        
        private void CreateCards(List<ICharacterData> cards, ICharacterData[] deck) {
            cards.Sort((a, b) => a.ID.CompareTo(b.ID));
            cards.RemoveAll(e => deck.Contains(e));
            foreach (Transform child in _cardsContainer.Container) Destroy(child.gameObject);
            foreach (var card in cards) CreateCard(card, _cardsContainer.Container);
            InitializeCardsContainer();
        }

        private void CreateCard(ICharacterData data, Transform parent) {
            var card = _factory.Create(_cardPrefab, parent);
            card.Initialize(data, OnCardSelected);
        }

        private void CreateDeck(ICharacterData[] deck) {
            for (int i = 0; i < deck.Length; ++i) {
                InitializeDeckContainer(_deckContainers[i]);
                if(deck[i] != null) CreateCard(deck[i], _deckContainers[i].Container);
            }
        }

        private void InitializeCardsContainer() {
            _cardsContainer.OnAddElement = (e) => {
                if (_deckContainers.Contains(e.SourceContainer)) {
                    int index = _deckContainers.IndexOf(e.SourceContainer);
                    OnRemoveDeckCard?.Invoke(index);
                }
                SortingCardsView();
                return true;
            };
        }

        private void InitializeDeckContainer(UIDropContainer container) {
            container.OnRemoveElement = (e) => {
                int index = _deckContainers.IndexOf(container);
                OnRemoveDeckCard?.Invoke(index);
            };
            container.OnAddElement = (e) => {
                int index = _deckContainers.IndexOf(container);
                var cardsView = container.Container.GetComponentsInChildren<CardView>();
                if (cardsView.Length > 1) {
                    var cardView = cardsView.FirstOrDefault(c => c != e);
                    cardView.transform.SetParent(_cardsContainer.Container);
                    OnRemoveDeckCard?.Invoke(index);
                }
                OnSetDeckCard?.Invoke(index, (e as CardView).Data);
                e.transform.localPosition = Vector3.zero;
                return true;
            };
        }
        
        private void SortingCardsView() {
            var cards = _cardsContainer.Container.Cast<Transform>().OrderBy(t => t.GetComponent<CardView>()?.Data.ID).ToList();
            for (int i = 0; i < cards.Count; i++) cards[i].SetSiblingIndex(i);
        }
    }
}