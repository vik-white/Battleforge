using System;
using System.Collections;
using UnityEngine;

public enum Side {Left, Right}

public class BattleSide
{
    public Action OnEndAttack;
    public Action OnStartChoose;
    public Action OnEndChoose;
    
    private BattlePlayer _battlePlayer;
    private Hero _hero;
    private Board _board;
    private BattleAI _ai;
    private Character _currentCharacter;
    private bool _isEndBattle;
    private readonly IGameFactory _factory;
    private Side _side;
    
    public BattlePlayer BattlePlayer => _battlePlayer;
    public Board Board => _board;
    public Hero Hero => _hero;
    public Side Side => _side;
    public Character CurrentCharacter => _currentCharacter;
    
    public BattleSide(IGameFactory factory) {
        _factory = factory;
    }

    public void Initialize(BattlePlayer battlePlayer, Side side) {
        _battlePlayer = battlePlayer;
        _side = side;
        _board = _factory.CreateBoard(side);
        _hero = _factory.CreateHero(50);
    }

    public void SetPlayer(BattlePlayer battlePlayer) {
        _battlePlayer = battlePlayer;
    }

    public void EnableAI() {
        _ai = _factory.CreateBattleAI(this);
    }
    
    public void StartChoose() {
        OnStartChoose?.Invoke();
        _ai?.StartChoose();
    }

    public void EndChoose() {
        OnEndChoose?.Invoke();
    }

    public void EndBattle() => _isEndBattle = true;

    public void Attack() {
        Coroutine.Run(AttackSequence());
    }
    
    private IEnumerator AttackSequence() {
        foreach (var character in _board.GetCharacters()) {
            _currentCharacter = character;
            character.StartAttack();
            while (character.Commands.Count > 0) {
                character.Commands.Execute();
                if (_isEndBattle) yield break;
                yield return new WaitUntil(() => character.Commands.Next);
            }
            yield return new WaitForSeconds(0.3f);
        }
        OnEndAttack?.Invoke();
        _currentCharacter = null;
    }
}