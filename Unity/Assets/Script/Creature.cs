using UnityEngine;
public class Creature : Breakable
{
    protected Item[] _inventory;
    protected void InventoryInitialize()
    {
        _inventory = new Item[5];
    }
    public Item ItemGet(int index)
    {
        if (index < 0 || index >= _inventory.Length) return null;
        return _inventory[index];
    }
    // ?
    protected void ItemSet(int index, Item item = null)
    {
        if (index < 0 || index >= _inventory.Length) return;
        _inventory[index] = item;
    }
    public bool ItemHas(string id)
    {
        foreach (Item item in _inventory) if (item != null && item.ID == id) return true;
        return false;
    }
    public void ItemAdd(Item item)
    {
        for (int i = 0; i < 5; i++)
            if (_inventory[i] == null)
            {
                _inventory[i] = item;
                item.Hide();
                break;
            }
    }
    public void ItemRemove(string id)
    {
        for (int i = 0; i < 5; i++)
            if (_inventory[i] && _inventory[i].ID == id)
            {
                _inventory[i].Discard();
                _inventory[i] = null;
                break;
            }
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