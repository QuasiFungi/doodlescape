using UnityEngine;
public class GameVariables
{
    public static bool IsPlayer(GameObject target)
    {
        return target.layer == LayerCreature && target.tag == TagPlayer;
    }
    // public static bool IsItem(GameObject target)
    // {
    //     return target.layer == LayerItem;
    // }
    // only one use case
    public static bool IsMob(GameObject target)
    {
        return target.layer == LayerCreature && target.tag != TagPlayer;
    }
    public static Vector3 PositionDamage(Vector2 position)
    {
        // ? hard coded
        return new Vector3(position.x, position.y, 6f);
    }
    // * testing trail/vfx
    public static Vector3 PositionVFX(Vector2 position)
    {
        // ? hard coded
        return new Vector3(position.x, position.y, 1f);
    }
    private static string TagPlayer
    {
        get { return "player"; }
    }
    // // ? make public const
    // public static int Depth
    // {
    //     get { return 2; }
    // }
    public const float ROOM_SIZE = 9f;
    public static Color ColorDefault
    {
        get { return new Color(1f, 1f, 1f, 1f); }
    }
    public static Color ColorDecal
    {
        get { return new Color(.25f, .25f, .25f, 1f); }
    }
    // public static Color ColorTransparent
    // {
    //     get { return new Color(1f, 1f, 1f, 0f); }
    // }
    public static Color ColorInteract
    {
        get { return new Color(1f, 1f, 0f, 1f); }
    }
    public static Color ColorDamage
    {
        get { return new Color(1f, 0f, 0f, 1f); }
    }
    public static Color ColorSensor
    {
        get { return new Color(1f, 0f, 1f, 1f); }
    }
    #region Layer
    public static int LayerChunk
    {
        get { return LayerMask.NameToLayer("chunk"); }
    }
    public static int LayerBreakable
    {
        get { return LayerMask.NameToLayer("breakable"); }
    }
    public static int LayerDamage
    {
        get { return LayerMask.NameToLayer("damage"); }
    }
    public static int LayerSensor
    {
        get { return LayerMask.NameToLayer("sensor"); }
    }
    public static int LayerCreature
    {
        get { return LayerMask.NameToLayer("creature"); }
    }
    public static int LayerItem
    {
        get { return LayerMask.NameToLayer("item"); }
    }
    // // ? match layers with z depth
    // public static int LayerItem
    // {
    //     // get { return LayerMask.NameToLayer("item"); }
    //     get { return 2; }
    // }
    #endregion
    #region Scan Layer
    public static LayerMask ScanLayerCreature
    {
        get { return LayerMask.GetMask("creature"); }
    }
    // used by grid to mark unwalkable tiles
    public static LayerMask ScanLayerSolid
    {
        get { return LayerMask.GetMask("Default"); }
    }
    public static LayerMask ScanLayerSensor
    {
        get { return LayerMask.GetMask("sensor"); }
    }
    public static LayerMask ScanLayerItem
    {
        get { return LayerMask.GetMask("item"); }
    }
    // // 
    // public static LayerMask ScanLayerIndestructible
    // {
    //     get { return LayerMask.GetMask("Default") | LayerMask.GetMask("creature") | LayerMask.GetMask("item") | LayerMask.GetMask("interact"); }
    // }
    // used by game navigation for player actions ? creature
    public static LayerMask ScanLayerAction
    {
        get { return LayerMask.GetMask("Default") | LayerMask.GetMask("breakable") | LayerMask.GetMask("creature") | LayerMask.GetMask("item") | LayerMask.GetMask("interact"); }
    }
    // ? item block path but must also be detected by vision for monkey..?
    public static LayerMask ScanLayerObstruction
    {
        get { return LayerMask.GetMask("Default") | LayerMask.GetMask("interact") | LayerMask.GetMask("item") | LayerMask.GetMask("breakable"); }
    }
    public static LayerMask ScanLayerTarget
    {
        get { return LayerMask.GetMask("creature") | LayerMask.GetMask("item"); }
    }
    // ? special case for flyTrap
    public static LayerMask ScanLayerTrigger
    {
        get { return LayerMask.GetMask("breakable") |LayerMask.GetMask("creature") | LayerMask.GetMask("item"); }
    }
    public static LayerMask ScanLayerObject
    {
        get { return LayerMask.GetMask("Default") | LayerMask.GetMask("creature") | LayerMask.GetMask("interact") | LayerMask.GetMask("item") | LayerMask.GetMask("sensor") | LayerMask.GetMask("breakable"); }
    }
    #endregion
}