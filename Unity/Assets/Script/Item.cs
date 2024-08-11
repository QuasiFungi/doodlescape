using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Item : Entity
{
    // S A W C U R
    public enum ItemType
    {
        SUPPORT,
        AOE,
        WEAPON,
        COLLECTABLE,
        UTILITY,
        RECOVERY
    }
    [SerializeField] private ItemType _type;
    // * testing, used by player to drop item
    public void Show(Vector2 position)
    {
        // move to player position
        transform.position = new Vector3(position.x, position.y, transform.position.z);
        // 
        Show();
    }
    public Sprite Icon
    {
        // ? inefficient
        get { return transform.GetComponent<SpriteRenderer>().sprite; }
    }
    public ItemType Type
    {
        get { return _type; }
    }
}