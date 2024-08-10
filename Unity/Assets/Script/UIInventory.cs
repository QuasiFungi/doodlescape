using UnityEngine;
using UnityEngine.UI;
public class UIInventory : MonoBehaviour
{
    public Creature source;
    public Sprite spriteDefault;
    public Sprite spriteDisable;
    // public Sprite spriteSelected;
    void OnEnable()
    {
        // Player.onAction += MarkerUpdate;
        // GameClock.onTickEarly += MarkerClear;
        // Player.onAction += RegisterAction;
        GameClock.onTick += IconsUpdate;
    }
    void OnDisable()
    {
        // Player.onAction -= MarkerUpdate;
        // GameClock.onTickEarly -= MarkerClear;
        // Player.onAction -= RegisterAction;
        GameClock.onTick -= IconsUpdate;
    }
    // private Image[] markers;
    private Image[] slots;
    private bool flagExtended;
    void Awake()
    {
        // ? use global variable
        slots = new Image[8];
        // for (int i = 0; i < 8; i++) slots[i] = transform.GetChild(i).GetComponent<Image>();
        // markers = new Image[8];
        // for (int i = 0; i < 9; i++)
        // {
        //     if (i < 4)
        //     {
        //         markers[i] = transform.GetChild(i).GetComponent<Image>();
        //         slots[i] = markers[i].transform.GetChild(0).GetComponent<Image>();
        //     }
        //     else if (i > 4)
        //     {
        //         markers[i - 1] = transform.GetChild(i).GetComponent<Image>();
        //         slots[i - 1] = markers[i - 1].transform.GetChild(0).GetComponent<Image>();
        //     }
        // }
        // 
        // fill cardinals before diagonals, priority top left
        // markers[0] = transform.GetChild(1).GetComponent<Image>();
        // markers[1] = transform.GetChild(3).GetComponent<Image>();
        // markers[2] = transform.GetChild(5).GetComponent<Image>();
        // markers[3] = transform.GetChild(7).GetComponent<Image>();
        // markers[4] = transform.GetChild(0).GetComponent<Image>();
        // markers[5] = transform.GetChild(2).GetComponent<Image>();
        // markers[6] = transform.GetChild(6).GetComponent<Image>();
        // markers[7] = transform.GetChild(8).GetComponent<Image>();
        // for (int i = 0; i < 8; i++) slots[i] = markers[i].transform.GetChild(0).GetComponent<Image>();
        slots[0] = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        slots[1] = transform.GetChild(3).GetChild(0).GetComponent<Image>();
        slots[2] = transform.GetChild(5).GetChild(0).GetComponent<Image>();
        slots[3] = transform.GetChild(7).GetChild(0).GetComponent<Image>();
        slots[4] = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        slots[5] = transform.GetChild(2).GetChild(0).GetComponent<Image>();
        slots[6] = transform.GetChild(6).GetChild(0).GetComponent<Image>();
        slots[7] = transform.GetChild(8).GetChild(0).GetComponent<Image>();
        // 
        flagExtended = false;
    }
    private void IconsUpdate()
    {
        flagExtended = source.ItemHas("item_pouch");
        // int layer = -1;
        Item temp;
        for (int i = 0; i < 8; i++)
        {
            if (!flagExtended && i > 3) slots[i].sprite = spriteDisable;
            else
            {
                temp = source.ItemGet(i);
                if (temp)
                {
                    slots[i].sprite = temp.Icon;
                    slots[i].color = new Color(0f, 1f, 1f, 1f);
                }
                else
                {
                    slots[i].sprite = spriteDefault;
                    slots[i].color = new Color(1f, 1f, 1f, 1f);
                }
            }
        }
    }
    // private void MarkerUpdate(GameAction action)
    // {
    //     // if (action.Type != GameAction.ActionType.USE) return;
    //     // 
    //     // int index = -1;
    //     // // inventory button pressed
    //     // if (action.TypeButton == 1)
    //     // {
    //     //     // direction to index
    //     //     Vector2 direction = action.Direction;
    //     //     if (direction.x > 0f)
    //     //     {
    //     //         if (direction.y > 0f)
    //     //         {
    //     //             // UR
    //     //             index = 5;
    //     //         }
    //     //         else if (direction.y < 0f)
    //     //         {
    //     //             // DR
    //     //             index = 7;
    //     //         }
    //     //         else
    //     //         {
    //     //             // R
    //     //             index = 2;
    //     //         }
    //     //     }
    //     //     else if (direction.x < 0f)
    //     //     {
    //     //         if (direction.y > 0f)
    //     //         {
    //     //             // UL
    //     //             index = 4;
    //     //         }
    //     //         else if (direction.y < 0f)
    //     //         {
    //     //             // DL
    //     //             index = 6;
    //     //         }
    //     //         else
    //     //         {
    //     //             // L
    //     //             index = 1;
    //     //         }
    //     //     }
    //     //     else
    //     //     {
    //     //         if (direction.y > 0f)
    //     //         {
    //     //             // U
    //     //             index = 0;
    //     //         }
    //     //         else if (direction.y < 0f)
    //     //         {
    //     //             // D
    //     //             index = 3;
    //     //         }
    //     //         else
    //     //         {
    //     //             // invalid
    //     //         }
    //     //     }
    //     // }
    //     // show marker
    //     // for (int i = 0; i < 8; i++) markers[i].sprite = i == index ? spriteSelected : spriteDisable;
    //     for (int i = 0; i < 8; i++) markers[i].sprite = i == action.Index ? spriteSelected : spriteDisable;
    // }
    // private void MarkerClear()
    // {
    //     // hide marker
    //     for (int i = 0; i < 8; i++) markers[i].sprite = spriteDisable;
    // }
}