using UnityEngine;
public class GameVariables
{
    // public static Color ColorDecal
    // {
    //     get { return new Color(.25f, .25f, .25f, 1f); }
    // }
    // public static Color ColorInteract
    // {
    //     get { return new Color(1f, 1f, 0f, 1f); }
    // }
    public static int LayerSensor
    {
        get { return LayerMask.NameToLayer("sensor"); }
    }
    public static int LayerCreature
    {
        get { return LayerMask.NameToLayer("creature"); }
    }
    // public static LayerMask ScanLayerSolid
    // {
    //     get { return LayerMask.GetMask("Default"); }
    // }
    public static LayerMask ScanLayerSensor
    {
        get { return LayerMask.GetMask("sensor"); }
    }
    public static LayerMask ScanLayerItem
    {
        get { return LayerMask.GetMask("item"); }
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
}