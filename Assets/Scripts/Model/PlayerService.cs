using System.Collections.Generic;
using System.Linq;

public interface IPlayerService
{
    string Name { get; }
    void Initialize(string name);
    List<ICharacterData> GetCards();
    ICharacterData[] GetDeck();
    void SetDeckCard(int index, ICharacterData data);
    void RemoveDeckCard(int index);
}

public class PlayerService : IPlayerService
{
    private string _name;
    private List<ICharacterData> _cards;
    private ICharacterData[] _deck;
    private readonly IConfigs _configs;
    
    public string Name => _name;
    
    public PlayerService(IConfigs configs) {
        _configs = configs;
    }

    public void Initialize(string name) {
        _name = name;
        _cards = _configs.Characters.GetAll().ToList().FindAll(e => e.IsCollectable);
        _deck = new ICharacterData[6];
        _deck[1] = _cards[1];
    }

    public List<ICharacterData> GetCards() => _cards;
    
    public ICharacterData[] GetDeck() => _deck;
    
    public void SetDeckCard(int index, ICharacterData data) => _deck[index] = data;
    
    public void RemoveDeckCard(int index) => _deck[index] = null;
}