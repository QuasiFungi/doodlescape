using UnityEngine;
using System.Collections;
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
    // 
    private Vector3 _position;
    // * testing separate collider and sprite
    public Transform _body;
    public void Move(Vector3 target)
    {
        // 
        StopCoroutine("LerpPosition");
        // * testing ? remove in awake, lots of unparented bodies..? collider as child?
        if (_body.parent) _body.SetParent(null);
        // 
        _position = target;
        // 
        StartCoroutine("LerpPosition");
    }
    IEnumerator LerpPosition()
    {
        // * testing
        // look in direction to move
        _body.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(_position.y - transform.position.y, _position.x - transform.position.x) * Mathf.Rad2Deg - 90f);
        // record start position
        Vector3 start = transform.position;
        // move collider to target position
        transform.position = _position;
        // lerp counter
        float lerp = 0f;
        // not at target position
        while (Vector3.Distance(start, _position) > 0f)
        {
            // advance lerp timer
            lerp += Time.deltaTime * 2f;
            // update position offset for this frame
            _body.position = Vector3.Lerp(start, _position, lerp);
            // wait till next frame
            yield return null;
        }
    }
}