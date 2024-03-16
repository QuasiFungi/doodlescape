using UnityEngine;
using UnityEngine.Tilemaps;
public class GameNavigation
{
    public static int GetTileAtPosition(Vector2 position, out GameObject tile)
    {
        // 
        Collider2D hit = Physics2D.OverlapCircle(position, .1f);
        if (hit != null)
        {
            tile = hit.gameObject;
            return tile.gameObject.layer - 1;
        }
        tile = null;
        return 0;
    }
}