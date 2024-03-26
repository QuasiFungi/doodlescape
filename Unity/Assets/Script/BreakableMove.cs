using UnityEngine;
// slab ? boulder
public class BreakableMove : Breakable
{
    // ways of inflicting damage
    // - melee
    // - axe
    // - special?
    // need damage/move direction
    // check if tile to move to is clear
    // perform the move
    public override void HealthModify(int value, Creature source)
    {
        // print(transform.position + "\t" + source.transform.position);
        // ? destroy before push
        // ? flies further the further its hit from
        // direction + position
        Vector3 position = (transform.position - source.transform.position) + transform.position;
        // move to tile if clear
        if (!GameNavigation.IsTileAtPosition(position)) transform.position = new Vector3(position.x, position.y, transform.position.z);
        // 
        base.HealthModify(value, source);
    }
}