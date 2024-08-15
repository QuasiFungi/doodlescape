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
    // [SerializeField] private string _name = "Unnamed";
    // * testing ? character limit, separate consume discard messages, include name in prompt
    [Tooltip("shown when item pickup or use unusable")] [SerializeField] private string _description = "Undefined";
    [Tooltip("shown when discard (not drop) item")] [SerializeField] private string _consumed = "Undefined";
    // * testing, used by player to drop item
    public void Show(Vector2 position)
    {
        // move to player position
        transform.position = new Vector3(position.x, position.y, transform.position.z);
        // 
        // Show();
        ToggleActive(true);
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
    // public string Name
    // {
    //     get { return _name; }
    // }
    public string Description
    {
        get { return _description; }
    }
    public string Consumed
    {
        get { return _consumed; }
    }
}