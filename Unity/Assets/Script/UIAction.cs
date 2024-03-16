using UnityEngine;
using UnityEngine.UI;
public class UIAction : MonoBehaviour
{
    private SpriteRenderer[] actions;
    public Sprite spriteWalk;
    public Sprite spriteAttack;
    public Sprite spritePickup;
    public Sprite spriteInteract;
    // private bool flagExtended;
    void Awake()
    {
        actions = new SpriteRenderer[8];
        for (int i = 0; i < 8; i++) actions[i] = transform.GetChild(i).GetComponent<SpriteRenderer>();
        // flagExtended = false;
    }
    void OnEnable()
    {
        GameClock.onTickLate += SpriteUpdate;
    }
    void OnDisable()
    {
        GameClock.onTickLate -= SpriteUpdate;
    }
    // void Update()
    // {
    //     // ? use item pickup/drop events to set bool flag
    //     // ? use tick to update sprites
    // }
    private void SpriteUpdate()
    {
        GameObject temp;
        int layer;
        for (int i = 0; i < 8; i++)
        {
            layer = GameNavigation.GetTileAtPosition(actions[i].transform.position, out temp);
            // Empty
            if (!temp) actions[i].sprite = spriteWalk;
            // Breakable
            else if (layer == 5) actions[i].sprite = spriteAttack;
            // Interact
            else if (layer == 6) actions[i].sprite = spriteInteract;
            // Creature
            else if (layer == 7) actions[i].sprite = spriteAttack;
            // Item
            else if (layer == 8) actions[i].sprite = spritePickup;
            // ?
            else actions[i].sprite = null;
        }
    }
}