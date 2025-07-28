using System.Collections.Generic;
using UnityEngine;

namespace vikwhite.Data
{
    public enum UIKey { MainMenuWindow, BattleHUD, LobbyHUD, VictoryWindow, DefeatWindow, SquadWindow, CharacterWindow }

    public interface IUIConfigs
    {
        MonoBehaviour Get(UIKey key);
        T Get<T>(UIKey key) where T : MonoBehaviour;
        Sprite GetRaceBG(CharacterRace race);
        Sprite GetBigRaceBG(CharacterRace race);
        Sprite GetRaceIcon(CharacterRace race);
        Sprite GetCharacterTypeIcons(CharacterType type);
    }

    [CreateAssetMenu(fileName = "UIConfigs", menuName = "VikWhite/UIConfigs", order = 1)]
    public class UIConfigs : ScriptableObject, IUIConfigs
    {
        [SerializeField] private List<SerializableKeyValuePair<UIKey, MonoBehaviour>> _uiElements;
        private Dictionary<UIKey, MonoBehaviour> _uiElementMap;
        [SerializeField] private List<SerializableKeyValuePair<CharacterRace, Sprite>> _racesBG;
        [SerializeField] private List<SerializableKeyValuePair<CharacterRace, Sprite>> _bigRacesBG;
        [SerializeField] private List<SerializableKeyValuePair<CharacterRace, Sprite>> _racesIcons;
        [SerializeField] private List<SerializableKeyValuePair<CharacterType, Sprite>> _characterTypeIcons;

        public MonoBehaviour Get(UIKey key) => Get<MonoBehaviour>(key);

        public T Get<T>(UIKey key) where T : MonoBehaviour {
            if(_uiElementMap == null) {
                _uiElementMap = new Dictionary<UIKey, MonoBehaviour>();
                foreach(var pair in _uiElements)
                    _uiElementMap.Add(pair.Key, pair.Value);
            }
            return _uiElementMap[key] as T;
        }
        
        public Sprite GetRaceBG(CharacterRace race) => _racesBG.Find(x => x.Key == race).Value;
        
        public Sprite GetBigRaceBG(CharacterRace race) => _bigRacesBG.Find(x => x.Key == race).Value;
        
        public Sprite GetRaceIcon(CharacterRace race) => _racesIcons.Find(x => x.Key == race).Value;
        
        public Sprite GetCharacterTypeIcons(CharacterType type) => _characterTypeIcons.Find(x => x.Key == type).Value;
    }
}