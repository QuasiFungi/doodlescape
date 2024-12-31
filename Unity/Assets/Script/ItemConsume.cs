using UnityEngine;
public class ItemConsume : Item
{
    [Header("Consume")]
    [Tooltip("maximum number of uses (-1 unlimited)")] [SerializeField] private int _uses = -1;
    private int _count = -1;
    [Tooltip("shown when item used")] [SerializeField] private string _msgUse = "Undefined";
    protected override void Awake()
    {
        base.Awake();
        // 
        if (_msgUse.Length > 19) Debug.LogWarning(IDUnique + ": Usage message too long! Currently " + _msgUse.Length + " characters");
    }
    public bool Consume()
    {
        // mark usage
        if (_count > 0) _count--;
        // record
        DataSave();
        // uses depleted
        if (_count == 0)
        {
            // inform of discard
            Teleprompter.Register(_consumed);
            // 
            return true;
        }
        // inform player of effect
        else Teleprompter.Register(_msgUse);
        // 
        return false;
    }
    // * testing save/load
    protected override void DataLoad()
    {
        // * testing save/load
        Vector3 position, rotation;
        bool isActive;
        // use defaults if no data found
        if (!GameData.DataLoadItemConsume(IDUnique, out position, out rotation, out isActive, out _lootID, out _count))
            // initialize local parameters to defaults, position rotation isActive already assigned correctly
            _count = _uses;
        // apply loaded values
        else
        {
            SetPosition(position);
            SetRotation(rotation);
            // if holding an item
            if (_lootID != "")
                // try retrieving item from manager ? null on fail
                _loot = ManagerItem.Instance.GetItemByIDUnique(_lootID);
            // ? apply dead state
            ToggleActive(isActive);
        }
    }
    protected override void DataSave()
    {
        GameData.DataSaveItemConsume(IDUnique, Position, Rotation, IsActive, _lootID, _count);
    }
}