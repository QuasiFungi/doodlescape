using UnityEngine;
using UnityEngine.UI;
public class UIInventory : MonoBehaviour
{
    public Creature source;
    public Sprite iconDefault;
    void OnEnable()
    {
        // Player.onAction += RegisterAction;
        GameClock.onTick += IconsUpdate;
    }
    void OnDisable()
    {
        // Player.onAction -= RegisterAction;
        GameClock.onTick -= IconsUpdate;
    }
    private Image[] slots;
    void Awake()
    {
        slots = new Image[5];
        for (int i = 0; i < 5; i++) slots[i] = transform.GetChild(i).GetComponent<Image>();
    }
    private void IconsUpdate()
    {
        Item temp;
        for (int i = 0; i < 5; i++)
        {
            temp = source.ItemGet(i);
            if (temp)
            {
                slots[i].sprite = temp.Icon;
                slots[i].color = new Color(0f, 1f, 1f, 1f);
            }
            else
            {
                slots[i].sprite = iconDefault;
                slots[i].color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }
}