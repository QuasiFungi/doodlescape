using UnityEngine;
using UnityEngine.UI;
public class UIAction : MonoBehaviour
{
    private SpriteRenderer[] actions;
    public Sprite spriteWalk;
    public Sprite spriteAttack;
    public Sprite spritePickup;
    public Sprite spriteInteract;
    private GameObject marker;
    public Creature source;
    private bool flagExtended;
    void Awake()
    {
        actions = new SpriteRenderer[8];
        for (int i = 0; i < 8; i++) actions[i] = transform.GetChild(i).GetComponent<SpriteRenderer>();
        marker = transform.GetChild(8).gameObject;
        flagExtended = false;
    }
    void OnEnable()
    {
        Player.onAction += MarkerUpdate;
        GameClock.onTick += MarkerClear;
        GameClock.onTickLate += SpriteUpdate;
    }
    void OnDisable()
    {
        Player.onAction -= MarkerUpdate;
        GameClock.onTick -= MarkerClear;
        GameClock.onTickLate -= SpriteUpdate;
    }
    // private float colorA;
    // void Update()
    // {
    //     Color temp;
    //     foreach (SpriteRenderer icon in actions)
    //     {
    //         temp = icon.color;
    //         if (temp.a > .5f) temp.a -= Time.deltaTime * .5f;
    //         else temp.a += .5f;
    //         icon.color = temp;
    //     }
    // }
    // ? use item pickup/drop events to set bool flag
    // ? use tick to update sprites
    private void SpriteUpdate()
    {
        // print("updating");
        GameObject temp;
        int layer;
        flagExtended = source.ItemHas("item_feather");
        for (int i = 0; i < 8; i++)
        {
            if (!flagExtended && i > 3) actions[i].sprite = null;
            else
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
    private void MarkerUpdate(GameAction action)
    {
        // position
        marker.transform.position = new Vector3(action.Position.x, action.Position.y, marker.transform.position.z);
        // visibility
        marker.SetActive(true);
    }
    private void MarkerClear()
    {
        // visibility
        marker.SetActive(false);
    }
}