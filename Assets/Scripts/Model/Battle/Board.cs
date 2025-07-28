using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Board
{
    public Action<Character> OnAddCharacter;

    private const int ROW_SIZE = 3;
    private const int COLUMN_SIZE = 4;
    
    private Character[,] _characters;
    private Side _side;
    private readonly IGameFactory _factory;
    
    public Side Side => _side;
    
    public Board(IGameFactory factory) {
        _factory = factory;
    }

    public void Initialize(Side side) {
        _side = side;
        _characters = new Character[ROW_SIZE, COLUMN_SIZE];
    }
    
    public Character Get(Vector2Int localPosition) => _characters[localPosition.x, localPosition.y];
    
    public void Add(Character character) {
        _characters[character.LocalPosition.x, character.LocalPosition.y] = character;
        character.OnDie += () => {
            if( _characters[character.LocalPosition.x, character.LocalPosition.y] == character)
                _characters[character.LocalPosition.x, character.LocalPosition.y] = null;
        };
        character.OnSpawn?.Invoke();
        OnAddCharacter?.Invoke(character);
    }
    
    public bool TryAdd(ICharacterData data, Vector2Int localPosition) {
        if (IsAvailable(data.Type, localPosition)) {
            Add(_factory.CreateCharacter(data, localPosition, _side));
            return true;
        } 
        return false;
    }

    public List<Character> GetCharacters() {
        var characters = new List<Character>();
        for (int y = COLUMN_SIZE - 1; y >= 0; y--) {
            for (int x = 0; x < ROW_SIZE; x++) {
                if(_characters[x, y] != null)
                    characters.Add(_characters[x, y]);
            }
        }
        return characters;
    }

    public Character GetFirstCharacterInRow(int y) {
        for (int x = 0; x < ROW_SIZE; x++) {
            if(_characters[x, y] != null) return _characters[x, y];
        }
        return null;
    }
    
    public List<Vector2Int> GetEmptyPositions(CharacterType type) {
        List<Vector2Int> positions = new List<Vector2Int>();
        for (int x = 0; x < ROW_SIZE; x++) {
            for (int y = 0; y < COLUMN_SIZE; y++) {
                if(_characters[x, y] == null && IsAvailableType(type, new Vector2Int(x,y))) 
                    positions.Add(new Vector2Int(x, y));
            }
        }
        return positions;
    }
    
    public bool TryGetRandomEmptyPosition(CharacterType type, out Vector2Int position) {
        position = default;
        var positions = GetEmptyPositions(type);
        if(positions.Count == 0) return false;
        position = positions[Random.Range(0,positions.Count)];
        return true;
    }
    
    public bool IsAvailable(CharacterType type, Vector2Int localPosition) {
        if(!(localPosition.x >= 0 && localPosition.x < ROW_SIZE && localPosition.y >= 0 && localPosition.y < COLUMN_SIZE)) return false;
        return Get(localPosition) == null && IsAvailableType(type, localPosition);
    }

    private bool IsAvailableType(CharacterType type, Vector2Int localPosition) {
        if(type == CharacterType.Wall) return true;
        if(type == CharacterType.Melee && localPosition.x == 0) return true;
        if(type == CharacterType.Range && (localPosition.x == 1 || localPosition.x == 2)) return true;
        return false;
    }
}