using UnityEngine;
public class GameVariables
{
    public static bool IsPlayer(GameObject target)
    {
        return target.layer == LayerCreature && target.tag == TagPlayer;
    }
    private static string TagPlayer
    {
        get { return "player"; }
    }
    public static int Depth
    {
        get { return 2; }
    }
    // public static Color ColorDecal
    // {
    //     get { return new Color(.25f, .25f, .25f, 1f); }
    // }
    // public static Color ColorInteract
    // {
    //     get { return new Color(1f, 1f, 0f, 1f); }
    // }
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
    public static LayerMask ScanLayerObject
    {
        get { return LayerMask.GetMask("Default") | LayerMask.GetMask("creature") | LayerMask.GetMask("interact") | LayerMask.GetMask("item") | LayerMask.GetMask("sensor") | LayerMask.GetMask("breakable"); }
    }
    #endregion
}