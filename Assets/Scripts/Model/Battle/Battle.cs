using System;

public interface IBattle
{
    Action<int, bool> OnNextRound {get; set;}
    Action OnVictory {get; set;}
    Action OnDefeat {get; set;}
    Action<Character> OnAddCharacter {get; set;}
    
    BattleSide LeftSide { get; }
    BattleSide RightSide { get; }
    BattleSide CurrentSide { get; }
    BattleSide WaitingSide { get; }
    
    void Initialize(BattlePlayer player1, BattlePlayer player2);
    void Start();
    BattleSide GetBattleSide(Side side);
}

public class Battle : IBattle
{
    public Action<int, bool> OnNextRound { get; set; }
    public Action OnVictory { get; set; }
    public Action OnDefeat { get; set; }
    public Action<Character> OnAddCharacter {get; set;}

    private BattleSide _leftSide;
    private BattleSide _rightSide;
    private BattleSide _currentSide;
    private BattleSide _waitingSide;
    private int _roundCount;
    private readonly IGameFactory _factory;

    public BattleSide LeftSide => _leftSide;
    public BattleSide RightSide => _rightSide;
    public BattleSide CurrentSide => _currentSide;
    public BattleSide WaitingSide => _waitingSide;

    public Battle(IGameFactory factory) {
        _factory = factory;
    }

    public void Initialize(BattlePlayer player1, BattlePlayer player2) {
        _leftSide = _factory.CreateBattleSide(player1, Side.Left);
        _leftSide.OnEndChoose = Attack;
        _leftSide.OnEndAttack = NextRound;
        _leftSide.Hero.OnDie = Defeat;
        _leftSide.Board.OnAddCharacter = OnAddCharacter;
        
        _rightSide = _factory.CreateBattleSide(player2 , Side.Right);
        _rightSide.OnEndChoose = Attack;
        _rightSide.OnEndAttack = NextRound;
        _rightSide.Hero.OnDie = Victory;
        _rightSide.Board.OnAddCharacter = OnAddCharacter;
        
        _currentSide = _leftSide;
        _waitingSide = _rightSide;
        _rightSide.EnableAI();
    }

    public void Start(){
        _currentSide.StartChoose();
    }

    private void Attack() {
        _currentSide.Attack();
    }

    private void NextRound() {
        SwitchCurrentSide();
        _currentSide.StartChoose();
        _roundCount++;
        OnNextRound?.Invoke(_roundCount, _currentSide == _rightSide);
    }

    private void SwitchCurrentSide() => (_currentSide, _waitingSide) = (_waitingSide, _currentSide);

    private void Victory() {
        _leftSide.EndBattle();
        _rightSide.EndBattle();
        OnVictory?.Invoke();
    }
    
    private void Defeat() {
        _leftSide.EndBattle();
        _rightSide.EndBattle();
        OnDefeat?.Invoke();
    }
    
    public BattleSide GetBattleSide(Side side) => side == Side.Left ? _leftSide : _rightSide;
}