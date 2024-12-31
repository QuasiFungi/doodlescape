using UnityEngine;
public class Creature : Breakable
{
    [Header("Creature")]
    protected Item[] _inventory;
    protected string[] _inventoryIDs;
    // ? global variable
    // [SerializeField] protected int _sizeInventory = 8;
    // protected void InventoryInitialize()
    // protected override void Awake()
    // {
    //     base.Awake();
    //     // 
    //     _inventory = new Item[8];
    //     // _inventoryIDs = new string[8];
    // }
    // ? bypass ItemHas check
    public Item ItemGet(int index)
    {
        if (index < 0 || index >= 8) return null;
        return _inventory[index];
    }
    // ?
    protected void ItemSet(int index, Item item = null)
    {
        if (index < 0 || index >= 8) return;
        _inventory[index] = item;
        _inventoryIDs[index] = item.IDUnique;
    }
    // public bool ItemHas(string id, bool checkExtra = false)
    public bool ItemHas(string id)
    {
        // // foreach (Item item in _inventory) if (item != null && item.ID == id) return true;
        // // run full or half inventory check
        // for (int i = checkExtra ? 7 : 3; i > -1; i--)
        //     // prevent item detect in extra slots unless has pouch, run check once on start
        //     if (i == 7 && !ItemHas("item_pouch", false)) break;
        //     // 
        //     else if (_inventory[i] != null && _inventory[i].ID == id) return true;
        // return false;
        // 
        // primary slots
        int limit = 4;
        // iterate
        for (int i = 0; i < limit; i++)
        {
            // item held
            if (_inventory[i] != null)
            {
                // item matched
                if (_inventory[i].ID == id) return true;
                // pouch held, check all slots
                else if (_inventory[i].ID == "item_pouch") limit = 8;
            }
        }
        // item not found
        return false;
    }
    public int ItemGet(string id)
    {
        // primary slots
        int limit = 4;
        // iterate
        for (int i = 0; i < limit; i++)
        {
            // item held
            if (_inventory[i] != null)
            {
                // item matched
                if (_inventory[i].ID == id) return i;
                // pouch held, check all slots
                else if (_inventory[i].ID == "item_pouch") limit = 8;
            }
        }
        // item not found
        return -1;
    }
    // private bool 
    // // * testing
    // public bool ItemHas(int id)
    // {
    //     foreach (Item item in _inventory) if (item != null && item.ID == id) return true;
    //     return false;
    // }
    public bool ItemAdd(Item item)
    {
        // fill items from slot marked 0 and onwards
        for (int i = 0; i < 8; i++)
            // prevent item pickup in extra slots unless has pouch, run check once on start
            // if (i == 4 && !ItemHas("item_pouch", false)) return false;
            if (i == 4 && !ItemHas("item_pouch")) return false;
            // found empty slot
            else if (_inventory[i] == null)
            {
                // inform of item info/usage ? only if player
                Teleprompter.Register(item.Description);
                // store item reference
                _inventory[i] = item;
                _inventoryIDs[i] = item.IDUnique;
                // hide physical item entity, not discard
                // item.Hide();
                item.ToggleActive(false);
                // * testing sfx pickup
                GameAudio.Instance.Register(3, GameAudio.AudioType.UI);
                // success
                return true;
            }
        // all valid slots full
        return false;
    }
    // ? bypass ItemHas check
    public void ItemRemove(string id)
    {
        for (int i = 0; i < 8; i++)
            if (_inventory[i] && _inventory[i].ID == id)
            {
                // // * testing
                // TestTeleprompt(_inventory[i].Name + " discarded");
                // 
                // ? never discard entities only disable/hide
                // _inventory[i].Discard();
                _inventory[i].ToggleActive(false);
                // _inventory[i].Hide();
                _inventory[i] = null;
                _inventoryIDs[i] = "";
                break;
            }
    }
    // ? bypass ItemHas check
    public void ItemDrop(int index, Vector3 position)
    {
        if (index < 0 || index >= 8) return;
        // * testing
        // _inventory[index].Reveal(Position);
        // _inventory[index].Show(Position);
        _inventory[index].Show(position);
        _inventory[index] = null;
        _inventoryIDs[index] = "";
        // * testing sfx drop
        GameAudio.Instance.Register(4, GameAudio.AudioType.UI);
    }
    // // used by crate ? filter based on type
    // public string[] GetItemIDs()
    // {
    //     string[] items = new string[5];
    //     for (int i = 0; i < 5; i++) if (_inventory[i]) items[i] = _inventory[i].ID;
    //     return items;
    // }
    // // * testing, preemptive check for item pickup action, at least one slot empty
    // public bool SlotEmpty(bool checkExtra)
    // {
    //     for (int i = checkExtra ? 7 : 3; i > -1; i--) if (_inventory[i] == null) return true;
    //     return false;
    // }
    // // for item drop
    // public bool IsDirectionClear(Vector3 direction)
    // {
    //     // 
    // }
    // // * testing save/load
    // protected override void DataLoad()
    // {
    //     Vector3 position, rotation;
    //     // use defaults if no data found
    //     if (!GameData.DataLoadCreature(IDUnique, out position, out rotation, out _healthInst))
    //         // position rotation already assigned correctly
    //         _healthInst = _health;
    //     // apply loaded values
    //     else
    //     {
    //         SetPosition(position);
    //         SetRotation(rotation);
    //     }
    // }
    // protected override void DataSave()
    // {
    //     GameData.DataSaveCreature(IDUnique, Position, Rotation, _healthInst);
    // }
    protected override void DataLoad()
    {
        // * testing save/load
        Vector3 position, rotation;
        bool isActive;
        // use defaults if no data found
        if (!GameData.DataLoadCreature(IDUnique, out position, out rotation, out isActive, out _lootID, out _healthInst, out _inventoryIDs))
        {
            // position rotation isActive already assigned correctly
            _healthInst = _health;
            _inventory = new Item[8];
            _inventoryIDs = new string[8];
        }
        // apply loaded values
        else
        {
            // * testing, do not use saved position/rotation for mobs
            if (gameObject.tag == "player" || gameObject.tag == "boss")
            {
                SetPosition(position);
                SetRotation(rotation);
            }
            // if holding an item
            if (_lootID != "")
                // try retrieving item from manager ? null on fail
                _loot = ManagerItem.Instance.GetItemByIDUnique(_lootID);
            // id to item
            _inventory = new Item[8];
            // iterate each inventory slot
            for (int i = 0; i < 8; i++)
                // those holding an item
                if (_inventoryIDs[i] != "")
                    // try retrieving item from manager ? null on fail
                    _inventory[i] = ManagerItem.Instance.GetItemByIDUnique(_inventoryIDs[i]);
        }
        // apply dead state
        ToggleActive(isActive && !IsDead);
    }
    protected override void DataSave()
    {
        GameData.DataSaveCreature(IDUnique, Position, Rotation, IsActive, _lootID, _healthInst, _inventoryIDs);
    }
}