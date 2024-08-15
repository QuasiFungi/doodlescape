using UnityEngine;
public class Creature : Breakable
{
    protected Item[] _inventory;
    // ? global variable
    // [SerializeField] protected int _sizeInventory = 8;
    protected void InventoryInitialize()
    {
        _inventory = new Item[8];
    }
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
    }
    public bool ItemHas(string id, bool checkExtra = true)
    {
        // foreach (Item item in _inventory) if (item != null && item.ID == id) return true;
        // run full or half inventory check
        for (int i = checkExtra ? 7 : 3; i > -1; i--)
            // prevent item detect in extra slots unless has pouch, run check once on start
            if (i == 7 && !ItemHas("item_pouch", false)) break;
            // 
            else if (_inventory[i] != null && _inventory[i].ID == id) return true;
        return false;
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
            if (i == 4 && !ItemHas("item_pouch", false)) return false;
            // found empty slot
            else if (_inventory[i] == null)
            {
                // inform of item info/usage ? only if player
                Teleprompter.Register(item.Description);
                // store item reference
                _inventory[i] = item;
                // hide physical item entity, not discard
                // item.Hide();
                item.ToggleActive(false);
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
                _inventory[i].Discard();
                // _inventory[i].Hide();
                _inventory[i] = null;
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
}