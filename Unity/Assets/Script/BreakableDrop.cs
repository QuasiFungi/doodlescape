using UnityEngine;
using System.Collections.Generic;
// crate ? deprecated by entity loot system ? exception
public class BreakableDrop : Breakable
{
    // damage sources
    // - player (melee, axe, special?) : standard
    // - seed flower (?) : unique drop?
    // - spike snail (melee) : 
    // - bone man (?) : 
    // - arrow : 
    // - ghoul merc (?) : 
    // - sonic bat (melee?) : 
    // - sling slime (?) : 
    // - blood maggot (?) : 
    // reference to player
    // - singleton (only player expected to damage this entity)
    // - pass reference as parameter to healthmodify/discard (any creature can damage this entity therefore affecting its drops)
    private Creature _source;
    // ? case where attacked via damage object like axe slash ? have gameobject source and typecast based on layer
    public override void HealthModify(int value, Creature source)
    {
        // print(transform.position + "\t" + source.transform.position);
        // Item temp;
        // List<string> loot = new List<string>();
        // for (int i = 0; i < 5; i++)
        // {
        //     temp = source.ItemGet(0);
        //     if (temp) loot.Add(temp.ID);
        // }
        // // ? drop guaranteed ? random item slot picked ? guarantee only when inventory full ? valid item type only
        // int drop = Random.Range(0, loot.Count);
        // print(drop < loot.Count ? loot[drop] : "fail");
        // 
        // record last creature that damaged
        _source = source;
        // 
        base.HealthModify(value, source);
    }
    public override void Discard()
    {
        // pick random inventory slot ? creature inventory sizes arent same
        Item drop = _source.ItemGet(Random.Range(0, 5));
        // ? check if item valid
        // print(drop ? drop.ID : "fail");
        if (drop)
        {
            // ? use similar approach to chest items
            // ? request copy from object pool
            // Instantiate(Resources.Load(drop.ID), transform.position, transform.rotation);
            // // * testing ? object pooling
            // ObjectPool.GetItem(drop.ID);
        }
        // 
        base.Discard();
    }
}