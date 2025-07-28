using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ICharactersConfig
{
    ICharacterData Get(string id);
    IEnumerable<ICharacterData> GetAll();
}

[Serializable]
[CreateAssetMenu(fileName = "CharactersConfig", menuName = "VikWhite/CharactersConfig", order = 1)]
public class CharactersConfig : ScriptableObject, ICharactersConfig
{
    [SerializeField]
    private List<CharacterConfig> _characters;

    public ICharacterData Get(string id) => _characters.Find(e => e.Data.ID == id).Data;
    
    public IEnumerable<ICharacterData> GetAll() => _characters.Select(e => e.Data);
}