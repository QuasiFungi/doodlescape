using UnityEngine;
public class Creature : Breakable
{
    protected Item[] _inventory;
    protected void InventoryInitialize()
    {
        _inventory = new Item[5];
    }
    protected Item InventoryGet(int index)
    {
        if (index < 0 || index >= _inventory.Length) return null;
        return _inventory[index];
    }
    protected void InventorySet(int index, Item item = null)
    {
        if (index < 0 || index >= _inventory.Length) return;
        _inventory[index] = item;
    }
}