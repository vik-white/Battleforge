using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using vikwhite.View;
using Zenject;

public class CardView : UIScrollAndDrag
{
    [SerializeField] private TMP_Text _health;
    [SerializeField] private TMP_Text _damage;
    [SerializeField] private Image _character;
    [SerializeField] private Image _bg;
    [SerializeField] private Image _rarity;
    [SerializeField] private Image _typeIcon;
    [SerializeField] private Button _button;
    private ICharacterData _data;
    private IConfigs _configs;
    
    public ICharacterData Data => _data;
    
    [Inject]
    public void Constarct(IConfigs configs) {
        _configs = configs;
    }

    public void Initialize(ICharacterData data, Action<ICharacterData> callback) {
        _data = data;
        _health.text = _data.Health.ToString();
        _damage.text = _data.Damage.ToString();
        _character.sprite = _data.Image;
        _bg.sprite = _configs.UI.GetRaceBG(data.Race);
        _rarity.color = data.Rarity switch {
            CharacterRarity.Common => ColorUtils.FromHex("C7BA9E"),
            CharacterRarity.Rare => ColorUtils.FromHex("3D9DD6"),
            CharacterRarity.Epic => ColorUtils.FromHex("C367EA"),
            CharacterRarity.Legendary => ColorUtils.FromHex("DFBA42"),
            _ => default  
        };
        _onClickWithoutDrag = () => callback?.Invoke(data);
        _typeIcon.sprite = _configs.UI.GetCharacterTypeIcons(data.Type);
    }
}