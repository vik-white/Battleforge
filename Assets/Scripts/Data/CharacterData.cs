using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public interface ICharacterData
{
    string ID { get; }
    GameObject Prefab { get; }
    Sprite Image { get; }
    float Damage { get; }
    float Health { get; }
    string AbilityDescription { get; }
    Sprite AbilityImage { get; }
    bool IsCollectable { get; }
    CharacterType Type { get; }
    CharacterRace Race { get; }
    CharacterRarity Rarity { get; }
    List<AbilityData> Abilities { get; }
}

[Serializable]
public class CharacterData : ICharacterData
{
    public string ID;
    public GameObject Prefab;
    [PreviewField(250, ObjectFieldAlignment.Right)]
    public Sprite Image;
    public float Damage;
    public float Health;
    [Multiline(4)]
    public string AbilityDescription;
    [PreviewField(50, ObjectFieldAlignment.Right)]
    public Sprite AbilityImage;
    public bool IsCollectable;
    public CharacterType Type;
    public CharacterRace Race;
    public CharacterRarity Rarity;
    [SerializeReference] public List<AbilityData> Abilities;

    string ICharacterData.ID => ID;
    GameObject ICharacterData.Prefab => Prefab;
    Sprite ICharacterData.Image => Image;
    float ICharacterData.Damage => Damage;
    float ICharacterData.Health => Health;
    string ICharacterData.AbilityDescription => AbilityDescription;
    Sprite ICharacterData.AbilityImage => AbilityImage;
    bool ICharacterData.IsCollectable => IsCollectable;
    CharacterType ICharacterData.Type => Type;
    CharacterRace ICharacterData.Race => Race;
    CharacterRarity ICharacterData.Rarity => Rarity;
    List<AbilityData> ICharacterData.Abilities => Abilities;
}