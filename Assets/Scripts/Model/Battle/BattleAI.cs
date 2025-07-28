using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleAI
{
    private readonly IGameFactory _factory;
    private readonly IConfigs _configs;
    private BattleSide _battleSide;

    public BattleAI(IGameFactory factory, IConfigs configs) {
        _factory = factory;
        _configs = configs;
    }

    public void Initialize(BattleSide battleSide) => _battleSide = battleSide;

    public void StartChoose() {
        var cards = _configs.Characters.GetAll().ToList()
            .FindAll(e => e.IsCollectable); // e.IsCollectable Assassin Spy Sceleton Thorns
        var card = cards[Random.Range(0, cards.Count)];
        //var card = cards[0];
        if(_battleSide.Board.TryGetRandomEmptyPosition(card.Type, out var position)) {
            _battleSide.Board.Add(_factory.CreateCharacter(card, position, _battleSide.Side));
        }
        _battleSide.EndChoose();
    }
}
