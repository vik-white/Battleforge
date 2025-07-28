using System;
using UnityEngine;
using vikwhite.Data;

public interface IConfigs
{
    CharactersConfig Characters { get; }
    UIConfigs UI { get; }
}

[Serializable]
[CreateAssetMenu(fileName = "Configs", menuName = "VikWhite/Configs", order = 1)]
public class Configs : ScriptableObject, IConfigs
{
    [SerializeField] 
    private CharactersConfig characters; 
    [SerializeField] 
    private UIConfigs _ui; 
    
    public CharactersConfig Characters => characters;
    public UIConfigs UI => _ui;
}