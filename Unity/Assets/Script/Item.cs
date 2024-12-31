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
    [Header("Item")]
    [SerializeField] private ItemType _type;
    // [SerializeField] private string _name = "Unnamed";
    // * testing ? character limit, separate consume discard messages, include name in prompt
    [Tooltip("shown when item pickup or use unusable")] [SerializeField] private string _description = "Undefined";
    [Tooltip("shown when discard (not drop) item")] [SerializeField] protected string _consumed = "Undefined";
    private Sprite _iconA;
    [Tooltip("background variant of item icon")] [SerializeField] private Sprite _iconB;
    public AudioClip _sfxUse, _sfxDiscard;
    protected override void Awake()
    {
        base.Awake();
        // 
        if (_description.Length > 19) Debug.LogWarning(IDUnique + ": Descriptor message too long! Currently " + _description.Length + " characters");
        if (_consumed.Length > 19) Debug.LogWarning(IDUnique + ": Consumption message too long! Currently " + _consumed.Length + " characters");
        // 
        _iconA = transform.GetComponent<SpriteRenderer>().sprite;
    }
    // * testing, used by player to drop item
    public void Show(Vector2 position)
    {
        // move to player position
        transform.position = new Vector3(position.x, position.y, transform.position.z);
        // 
        // Show();
        ToggleActive(true);
        // // * testing save/load
        // DataSave();
    }
    public Sprite IconA
    {
        // ? inefficient
        get { return _iconA; }
    }
    public Sprite IconB
    {
        // ? inefficient
        get { return _iconB; }
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
    public AudioClip SFXUse
    {
        get { return _sfxUse; }
    }
    public AudioClip SFXDiscard
    {
        get { return _sfxDiscard; }
    }
}