using UnityEngine;
using Zenject;

public interface IGameFactory
{
    BattleSide CreateBattleSide(BattlePlayer battlePlayer, Side side);
    BattleAI CreateBattleAI(BattleSide side);
    Board CreateBoard(Side side);
    BattlePlayer CreateBattlePlayer(string name);
    BattlePlayer CreateBattlePlayer();
    Character CreateCharacter(string id, Vector2Int position, Side side);
    Character CreateCharacter(ICharacterData data, Vector2Int position, Side side);
    Hero CreateHero(float health);
    AbilitiesHandler CreateAbilityHandler(Character character);
    CommandsHandler CreateCommandsHandler(Character character);
}

public class GameFactory : IGameFactory
{
    private readonly DiContainer _container;
    private readonly IConfigs _configs;
    private readonly IPlayerService _player;
    
    public GameFactory(DiContainer  container, IConfigs configs, IPlayerService player) {
        _container = container;
        _configs = configs;
        _player = player;
    }

    public BattleSide CreateBattleSide(BattlePlayer battlePlayer, Side side) {
        var battleSide = _container.Resolve<BattleSide>();
        battleSide.Initialize(battlePlayer, side);
        return battleSide;
    }

    public BattleAI CreateBattleAI(BattleSide side) {
        var ai = _container.Resolve<BattleAI>();
        ai.Initialize(side);
        return ai;
    }
    
    public Board CreateBoard(Side side) {
        var board = _container.Resolve<Board>();
        board.Initialize(side);
        return board;
    }
    
    public BattlePlayer CreateBattlePlayer(string name) {
        var player = _container.Resolve<BattlePlayer>();
        player.Initialize(name);
        return player;
    }
    
    public BattlePlayer CreateBattlePlayer() {
        var player = _container.Resolve<BattlePlayer>();
        player.Initialize(_player);
        return player;
    }

    public Character CreateCharacter(string id, Vector2Int position, Side side) {
        var character = _container.Resolve<Character>();
        character.Initialize(_configs.Characters.Get(id), position, side);
        return character;
    }
    
    public Character CreateCharacter(ICharacterData data, Vector2Int position, Side side) {
        var character = _container.Resolve<Character>();
        character.Initialize(data, position, side);
        return character;
    }

    public Hero CreateHero(float health) {
        var hero = _container.Resolve<Hero>();
        hero.Initialize(health);
        return hero;
    }
    
    public AbilitiesHandler CreateAbilityHandler(Character character) {
        var abilities = _container.Resolve<AbilitiesHandler>();
        abilities.Initialize(character);
        return abilities;
    }
    
    public CommandsHandler CreateCommandsHandler(Character character) {
        var commands = _container.Resolve<CommandsHandler>();
        commands.Initialize(character);
        return commands;
    }
}