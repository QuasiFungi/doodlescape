using UnityEngine;
using UnityEngine.Tilemaps;
public class GameNavigation
{
    public static int GetTileAtPosition(Vector2 position, out GameObject tile)
    {
        // 
        Collider2D hit = Physics2D.OverlapCircle(position, .1f);
        // allow actions over sensor tiles
        if (hit != null && hit.gameObject.layer != LayerMask.NameToLayer("sensor"))
        {
            tile = hit.gameObject;
            return tile.gameObject.layer - 1;
        }
        tile = null;
        return 0;
    }
}