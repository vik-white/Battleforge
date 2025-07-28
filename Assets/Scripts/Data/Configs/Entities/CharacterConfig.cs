using System;
using UnityEngine;

public interface ICharacterConfig
{
    CharacterData Data { get; }
}

[Serializable]
[CreateAssetMenu(fileName = "CharacterConfig", menuName = "VikWhite/CharacterConfig", order = 1)]
public class CharacterConfig : ScriptableObject, ICharacterConfig
{
    [SerializeField][SerializeReference]
    private CharacterData _data;

    public CharacterData Data => _data;
}