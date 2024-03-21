using UnityEngine;
public class BreakableDrop : Breakable
{
    // reference to player
    // - singleton (only player expected to damage this entity)
    // - pass reference as parameter to healthmodify/discard (any creature can damage this entity therefore affecting its drops)
    // public override void Discard()
    // {
    //     // 
    //     // 
    //     base.Discard();
    // }
}