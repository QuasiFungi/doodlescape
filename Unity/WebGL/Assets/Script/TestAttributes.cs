using UnityEngine;
// [AddComponentMenu("Transform/Test Attributes")]
public class TestAttributes : MonoBehaviour
{
    // ? ColorUsage Attribute
    // 
    // Add a context menu named "Do Something" in the inspector
    // of the attached script.
    [ContextMenu("Do Something")]
    void DoSomething()
    {
        // Debug.Log("Perform operation");
        Debug.Log(playerBiography);
    }
    // 
    [ContextMenuItem("Reset", "ResetBiography")]
    [Multiline(8)]
    [SerializeField] 
    string playerBiography = "";

    void ResetBiography()
    {
        playerBiography = "";
    }
    // 
    // ? CreateAssetMenuAttribute
    // 
    // ? CustomGridBrushAttribute
    // 
    // DelayedAttribute
    // 
    [Header("Health Settings")]
    public int health = 0;
    public int maxHealth = 100;

    [Header("Shield Settings")]
    public int shield = 0;
    public int maxShield = 0;
}