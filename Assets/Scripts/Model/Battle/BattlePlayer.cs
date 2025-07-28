using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattlePlayer
{
    private string _name;
    private List<ICharacterData> _deck;
    private readonly IConfigs _configs;
    
    public string Name => _name;
    
    public BattlePlayer(IConfigs configs) {
        _configs = configs;
    }

    public void Initialize(string name) {
        _name = name;
        _deck  = new List<ICharacterData>() {};
    }
    
    public void Initialize(IPlayerService player) {
        _name = player.Name;
        _deck = player.GetDeck().ToList();
        _deck.RemoveAll(e => e == null);
    }

    public List<ICharacterData> GetRandomCards(int count) => _deck
        .OrderBy(x => Random.value)
        .Take(_deck.Count < count ? _deck.Count : count )
        .ToList();
}