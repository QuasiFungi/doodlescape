using UnityEngine;
// charm, bell, axe? rename to aoe
public class ItemEffect : ItemConsume
{
    [Header("Effect")]
    public GameObject _effect;
    public void Spawn(Creature source)
    {
        // ? player facing diagonal
        GameObject temp = Instantiate(_effect, source.Position, Quaternion.identity);
        // 
        temp.GetComponent<BaseHitbox>().Initialize(source, source.Position, GameData.Room);
    }
}