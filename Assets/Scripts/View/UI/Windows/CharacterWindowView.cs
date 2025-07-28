using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace vikwhite.View
{
    public class CharacterWindowView : UIElement
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _rarity;
        [SerializeField] private TMP_Text _race;
        [SerializeField] private TMP_Text _health;
        [SerializeField] private TMP_Text _damage;
        [SerializeField] private Image _bg;
        [SerializeField] private Image _character;
        [SerializeField] private Image _characterRim;
        [SerializeField] private Image _raceIcon;
        [SerializeField] private Image _typeIcon;
        [SerializeField] private GameObject _abilityContainer;
        [SerializeField] private Image _abilityIcon;
        [SerializeField] private TMP_Text _abilityDescription;
        
        private IUIService _ui;
        private IConfigs _configs;
        
        [Inject]
        public void Constract(IUIService ui, IConfigs configs) {
            _ui = ui;
            _configs = configs;
        } 
        
        public void Initialize(ICharacterData data) {
            _closeButton.onClick.AddListener(() => _ui.Close<CharacterWindowView>());
            _name.text = data.ID;
            _race.text = data.Race switch {
                CharacterRace.City => "Concord",
                CharacterRace.Hell => "Underground",
                CharacterRace.Nature => "Nature",
                _ => default
            };
            _race.color = data.Race switch {
                CharacterRace.City => ColorUtils.FromHex("D8CA8E"),
                CharacterRace.Hell => ColorUtils.FromHex("DD4A1E"),
                CharacterRace.Nature => ColorUtils.FromHex("3FA63B"),
                _ => default
            };
            _rarity.color = data.Rarity switch {
                CharacterRarity.Common => ColorUtils.FromHex("C7BA9E"),
                CharacterRarity.Rare => ColorUtils.FromHex("3D9DD6"),
                CharacterRarity.Epic => ColorUtils.FromHex("C367EA"),
                CharacterRarity.Legendary => ColorUtils.FromHex("DFBA42"),
                _ => default
            };
            _rarity.text = data.Rarity.ToString();
            _health.text = data.Health.ToString();
            _damage.text = data.Damage.ToString(); 
            _character.sprite = data.Image;
            _characterRim.sprite = data.Image;
            _bg.sprite = _configs.UI.GetBigRaceBG(data.Race);
            _raceIcon.sprite = _configs.UI.GetRaceIcon(data.Race);
            _typeIcon.sprite = _configs.UI.GetCharacterTypeIcons(data.Type);
            _abilityContainer.SetActive(data.AbilityDescription != "");
            _abilityDescription.text = data.AbilityDescription;
            _abilityIcon.sprite = data.AbilityImage;
        }
    }
}