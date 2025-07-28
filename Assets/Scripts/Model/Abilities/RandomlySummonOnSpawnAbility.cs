using UnityEngine;

public class RandomlySummonOnSpawnAbility : Ability
{
    private readonly IBattle _battle;
    private readonly IGameFactory _factory;
    private readonly IConfigs _configs;
    
    public new RandomlySummonOnSpawnAbilityData Data => _data as RandomlySummonOnSpawnAbilityData;
    
    public RandomlySummonOnSpawnAbility(IBattle battle, IGameFactory factory, IConfigs configs) {
        _battle = battle;
        _factory = factory;
        _configs = configs;
    }
    
    public override void Initialize(Character character) {
        character.OnSpawn += () => {
            var board = _battle.GetBattleSide(character.Side).Board;
            for (int i = 0; i < Data.Count; i++) {
                if (board.TryGetRandomEmptyPosition(_configs.Characters.Get(Data.ID).Type, out var position)) {
                    board.Add(_factory.CreateCharacter(Data.ID, position, character.Side));
                }
            }
        };
    }
}