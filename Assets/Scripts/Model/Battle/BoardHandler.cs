using UnityEngine;

public class BoardHandler
{
    public static Vector2Int WorldToBoardPosition(Vector3 position, Side side) {
        position = new(Mathf.CeilToInt(position.x - 0.5f), 0, Mathf.CeilToInt(position.z - 0.5f));
        var p = new Vector2Int((int)position.x, (int)position.z);
        if (side == Side.Right) p.x -= 3;
        else p.x = 2 - p.x;
        return p;
    }

    public static Vector3 GlobalToWorldPosition(Vector2Int position) => new(position.x, 0, position.y);

    public static Vector2Int BoardToGlobalPosition(Vector2Int position, Side side) {
        if (side == Side.Right) position.x += 3;
        else position.x = 2 - position.x;
        return position;
    }
}