using UnityEngine;
public class ItemHeal : ItemConsume
{
    [Header("Heal")]
    [Tooltip("amount of health restored per use")] [SerializeField] private int _health = 1;
    // [Tooltip("maximum number of uses")] [SerializeField] private int _uses = 1;
    // private int _count = 1;
    // [Tooltip("shown when item used")] [SerializeField] private string _msgUse = "Undefined";
    // protected override void Awake()
    // {
    //     base.Awake();
    //     // 
    //     if (_msgUse.Length > 19) Debug.LogWarning(ID + ": Usage message too long! Currently " + _msgUse.Length + " characters");
    // }
    // public bool Consume()
    // {
    //     // inform player of effect
    //     Teleprompter.Register(_msgUse);
    //     // mark usage
    //     _count--;
    //     // record
    //     DataSave();
    //     // uses depleted
    //     // if (_count == 0) Discard();
    //     return _count == 0;
    //     // // inform of the health amount to restore ? done this way to keep code in single line
    //     // return _health;
    // }
    public int Health
    {
        get { return _health; }
    }
    // // * testing save/load
    // protected override void DataLoad()
    // {
    //     // * testing save/load
    //     Vector3 position, rotation;
    //     bool isActive;
    //     // use defaults if no data found
    //     if (!GameData.DataLoadItemHeal(IDUnique, out position, out rotation, out isActive, out _lootID, out _count))
    //         // position rotation isActive already assigned correctly
    //         _count = _uses;
    //     // apply loaded values
    //     else
    //     {
    //         SetPosition(position);
    //         SetRotation(rotation);
    //         // if holding an item
    //         if (_lootID != "")
    //             // try retrieving item from manager ? null on fail
    //             _loot = ManagerItem.Instance.GetItemByIDUnique(_lootID);
    //         // ? apply dead state
    //         ToggleActive(isActive);
    //     }
    //     // // apply dead state
    //     // ToggleActive(isActive);
    // }
    // protected override void DataSave()
    // {
    //     GameData.DataSaveItemHeal(IDUnique, Position, Rotation, IsActive, _lootID, _count);
    // }
}