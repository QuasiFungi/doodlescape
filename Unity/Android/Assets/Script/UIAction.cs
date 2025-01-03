using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class UIAction : MonoBehaviour
{
    public Sprite spriteDefault;
    public Sprite spriteDisable;
    public Sprite spriteWalk;
    public Sprite spriteWalkA;
    public Sprite spriteAttack;
    public Sprite spriteAttackA;
    public Sprite spritePickup;
    public Sprite spritePickupA;
    public Sprite spriteInteract;
    public Sprite spriteInteractA;
    public Sprite spriteTransition;
    public Sprite spriteTransitionA;
    // public Sprite spriteSelected;
    // private Image[] markers;
    private Image[,] actions;
    public Creature source;
    private bool flagExtended;
    void Awake()
    {
        // ? assign sprite through baseButton reference instead
        actions = new Image[8,2];
        // markers = new Image[8];
        for (int i = 0; i < 9; i++)
        {
            // print (transform.GetChild(i).gameObject.name);
            if (i < 4)
            {
                actions[i,0] = transform.GetChild(i).GetChild(1).GetComponent<Image>();
                actions[i,1] = transform.GetChild(i).GetChild(2).GetComponent<Image>();
            }
            // {
            //     // markers[i] = transform.GetChild(i).GetComponent<Image>();
            //     actions[i] = markers[i].transform.GetChild(0).GetComponent<Image>();
            // }
            else if (i > 4)
            {
                actions[i - 1,0] = transform.GetChild(i).GetChild(1).GetComponent<Image>();
                actions[i - 1,1] = transform.GetChild(i).GetChild(2).GetComponent<Image>();
            }
            // {
            //     // markers[i - 1] = transform.GetChild(i).GetComponent<Image>();
            //     actions[i - 1] = markers[i - 1].transform.GetChild(0).GetComponent<Image>();
            // }
        }
        // marker = transform.GetChild(8).gameObject;
        flagExtended = false;
    }
    void OnEnable()
    {
        // Player.onAction += MarkerUpdate;
        // GameClock.onTickEarly += MarkerClear;
        GameClock.onTickUI += SpriteUpdate;
        BaseTransition.onTrigger += OverrideAction;
        // * testing save/load
        GameMaster.onStartupInput += SpriteUpdate;
    }
    void OnDisable()
    {
        // Player.onAction -= MarkerUpdate;
        // GameClock.onTickEarly -= MarkerClear;
        GameClock.onTickUI -= SpriteUpdate;
        BaseTransition.onTrigger -= OverrideAction;
        // * testing save/load
        GameMaster.onStartupInput -= SpriteUpdate;
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
        // print("updating sprite at " + Time.time);
        flagExtended = source.ItemHas("item_feather");
        int layer = -1;
        GameObject temp = null;
        for (int i = 0; i < 8; i++)
        {
            // // * testing brighten icons
            // if (actions[i]) actions[i].color = new Color(1f, 1f, 1f, 1f);
            // 
            if (!flagExtended && (i == 0 || i == 2 || i == 5 || i == 7))
            {
                actions[i,0].sprite = spriteDisable;
                actions[i,1].sprite = spriteDefault;
            }
            // if (!flagExtended && (i == 0 || i == 2 || i == 5 || i == 7)) actions[i].sprite = null;
            else
            {
                // layer = GameNavigation.GetTileAtPosition(actions[i].transform.position, out temp);
                switch (i)
                {
                    case 0:
                        if (actionOverrides.Contains(0)) layer = -2;
                        else layer = GameNavigation.GetTileAtPosition(source.Position + new Vector3(-1f, 1f), out temp);
                        break;
                    case 1:
                        if (actionOverrides.Contains(1)) layer = -2;
                        else layer = GameNavigation.GetTileAtPosition(source.Position + new Vector3(0f, 1f), out temp);
                        break;
                    case 2:
                        if (actionOverrides.Contains(2)) layer = -2;
                        else layer = GameNavigation.GetTileAtPosition(source.Position + new Vector3(1f, 1f), out temp);
                        break;
                    case 3:
                        if (actionOverrides.Contains(3)) layer = -2;
                        else layer = GameNavigation.GetTileAtPosition(source.Position + new Vector3(-1f, 0f), out temp);
                        break;
                    case 4:
                        if (actionOverrides.Contains(4)) layer = -2;
                        else layer = GameNavigation.GetTileAtPosition(source.Position + new Vector3(1f, 0f), out temp);
                        break;
                    case 5:
                        if (actionOverrides.Contains(5)) layer = -2;
                        else layer = GameNavigation.GetTileAtPosition(source.Position + new Vector3(-1f, -1f), out temp);
                        break;
                    case 6:
                        if (actionOverrides.Contains(6)) layer = -2;
                        else layer = GameNavigation.GetTileAtPosition(source.Position + new Vector3(0f, -1f), out temp);
                        break;
                    case 7:
                        if (actionOverrides.Contains(7)) layer = -2;
                        else layer = GameNavigation.GetTileAtPosition(source.Position + new Vector3(1f, -1f), out temp);
                        break;
                }
                // Transition
                if (layer == -2)
                {
                    actions[i,0].sprite = spriteTransitionA;
                    actions[i,1].sprite = spriteTransition;
                }
                // Breakable
                else if (layer == 5)
                {
                    actions[i,0].sprite = spriteAttackA;
                    actions[i,1].sprite = spriteAttack;
                }
                // Interact
                else if (layer == 6)
                {
                    actions[i,0].sprite = spriteInteractA;
                    actions[i,1].sprite = spriteInteract;
                }
                // Creature
                else if (layer == 7)
                {
                    actions[i,0].sprite = spriteAttackA;
                    actions[i,1].sprite = spriteAttack;
                }
                // Item
                else if (layer == 8)
                {
                    actions[i,0].sprite = spritePickupA;
                    actions[i,1].sprite = spritePickup;
                }
                // Empty
                else if (!temp)
                {
                    actions[i,0].sprite = spriteWalkA;
                    actions[i,1].sprite = spriteWalk;
                }
                // ?
                else
                {
                    actions[i,0].sprite = spriteDefault;
                    actions[i,1].sprite = spriteDefault;
                }
                // else actions[i].sprite = null;
            }
        }
    }
    // private void MarkerUpdate(GameAction action)
    // {
    //     // // snap position to grid
    //     // Vector3 position = GameGrid.Instance.WorldToGrid((Vector3)action.Position);
    //     // // update marker position
    //     // // marker.transform.position = new Vector3(position.x, position.y, marker.transform.position.z);
    //     // marker.transform.position = new Vector3(action.Position.x, action.Position.y, marker.transform.position.z);
    //     // show marker
    //     // marker.SetActive(true);
    //     // 
    //     // int index = -1;
    //     // // action button pressed
    //     // if (action.TypeButton == 0)
    //     // {
    //     //     // direction to index
    //     //     Vector2 direction = action.Direction;
    //     //     if (direction.x > 0f)
    //     //     {
    //     //         if (direction.y > 0f)
    //     //         {
    //     //             // UR
    //     //             index = 2;
    //     //         }
    //     //         else if (direction.y < 0f)
    //     //         {
    //     //             // DR
    //     //             index = 7;
    //     //         }
    //     //         else
    //     //         {
    //     //             // R
    //     //             index = 4;
    //     //         }
    //     //     }
    //     //     else if (direction.x < 0f)
    //     //     {
    //     //         if (direction.y > 0f)
    //     //         {
    //     //             // UL
    //     //             index = 0;
    //     //         }
    //     //         else if (direction.y < 0f)
    //     //         {
    //     //             // DL
    //     //             index = 5;
    //     //         }
    //     //         else
    //     //         {
    //     //             // L
    //     //             index = 3;
    //     //         }
    //     //     }
    //     //     else
    //     //     {
    //     //         if (direction.y > 0f)
    //     //         {
    //     //             // U
    //     //             index = 1;
    //     //         }
    //     //         else if (direction.y < 0f)
    //     //         {
    //     //             // D
    //     //             index = 6;
    //     //         }
    //     //         else
    //     //         {
    //     //             // invalid
    //     //         }
    //     //     }
    //     // }
    //     // show marker
    //     // for (int i = 0; i < 8; i++) markers[i].sprite = i == index ? spriteSelected : spriteDisable;
    //     // for (int i = 0; i < 8; i++) markers[i].sprite = i == index ? spriteSelected : null;
    //     for (int i = 0; i < 8; i++) markers[i].sprite = i == action.Index ? spriteSelected : spriteDisable;
    // }
    // private void MarkerClear()
    // {
    //     // hide marker
    //     for (int i = 0; i < 8; i++) markers[i].sprite = spriteDisable;
    //     // for (int i = 0; i < 8; i++) markers[i].sprite = null;
    // }
    // private int transitionDirection = -1;
    private int[] actionOverrides = {-1, -1, -1};
    private void OverrideAction(Vector2Int direction, bool state)
    {
        if (!state) actionOverrides = new int[3] {-1, -1, -1};
        // right
        else if (direction.x > 0) actionOverrides = new int[3] {2, 4, 7};
        // left
        else if (direction.x < 0) actionOverrides = new int[3] {0, 3, 5};
        // up
        else if (direction.y > 0) actionOverrides = new int[3] {0, 1, 2};
        // down
        else if (direction.y < 0) actionOverrides = new int[3] {5, 6, 7};
    }
}