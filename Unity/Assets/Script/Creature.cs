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
    public bool ItemHas(string id)
    {
        foreach (Item item in _inventory) if (item != null && item.ID == id) return true;
        return false;
    }
    // // * testing
    // public bool ItemHas(int id)
    // {
    //     foreach (Item item in _inventory) if (item != null && item.ID == id) return true;
    //     return false;
    // }
    public void ItemAdd(Item item)
    {
        for (int i = 0; i < 8; i++)
            if (_inventory[i] == null)
            {
                _inventory[i] = item;
                // item.Hide();
                // hide physical item entity, not discard
                item.ToggleActive(false);
                break;
            }
    }
    public void ItemRemove(string id)
    {
        for (int i = 0; i < 8; i++)
            if (_inventory[i] && _inventory[i].ID == id)
            {
                _inventory[i].Discard();
                _inventory[i] = null;
                break;
            }
    }
    public void ItemDrop(int index)
    {
        if (index < 0 || index >= 8) return;
        // * testing
        // _inventory[index].Reveal(Position);
        _inventory[index].Show(Position);
        _inventory[index] = null;
    }
    // // used by crate ? filter based on type
    // public string[] GetItemIDs()
    // {
    //     string[] items = new string[5];
    //     for (int i = 0; i < 5; i++) if (_inventory[i]) items[i] = _inventory[i].ID;
    //     return items;
    // }
    // item drop
}