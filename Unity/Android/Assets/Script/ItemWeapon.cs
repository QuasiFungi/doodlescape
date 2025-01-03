using UnityEngine;
using System.Collections.Generic;
// axe
public class ItemWeapon : ItemConsume
{
    [Header("Weapon")]
    public GameObject _effect;
    // bad way to go about this
    public Transform _spawn;
    public void Effect(Creature source)
    {
        // all entities in current chunk
        List<Entity> entities = ManagerChunk.Instance.GetEntites();
        Entity mob = null;
        float min = float.MaxValue;
        float distance;
        for (int i = entities.Count - 1; i > -1; i--)
        {
            // target mobs ? only player can use it the way this is
            if (entities[i] && GameVariables.IsMob(entities[i].gameObject))
            {
                // living mobs only
                if (!entities[i].IsActive) continue;
                // first detected mob
                else if (mob == null) mob = entities[i];
                else
                {
                    // don't need exact distance
                    distance = (source.Position - entities[i].Position).sqrMagnitude;
                    // first mob chosen in case of multiple at same distance, probably random?
                    if (distance < min)
                    {
                        mob = entities[i];
                        min = distance;
                    }
                }
            }
        }
        // track weapon to source
        SetPosition(source.Position);
        // shoot in direction player facing if no mobs nearby
        if (mob == null) _spawn.eulerAngles = source.Rotation;
        // otherwise rotate weapon spawn to face target and use its rotation
        else
        {
            Vector2 direction = mob.Position - _spawn.position;
            _spawn.eulerAngles = new Vector3(_spawn.eulerAngles.x, _spawn.eulerAngles.y, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f);
        }
        GameObject temp = Instantiate(_effect, source.Position, _spawn.rotation);
        temp.GetComponent<BaseHitbox>().Initialize(source, source.Position, GameData.Room);
    }
}