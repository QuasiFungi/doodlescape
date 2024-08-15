using UnityEngine;
// ? inherit from entity
// public class BaseHitbox : MonoBehaviour
public class BaseHitbox : Entity
{
    // reference to entity that spawned this
    protected Breakable _source;
    // ? not all hitboxes have/need sprite, move to hitboxDamage
    protected SpriteRenderer _renderer;
    protected Vector3 _target;
    protected override void Awake()
    {
        // does not have loot
        // base.Awake();
        // 
        _renderer = GetComponent<SpriteRenderer>();
    }
    public void Initialize(Breakable source, Vector3 target)
    {
        _source = source;
        _target = target;
    }
    // protected void Discard()
    // {
    //     // * testing
    //     gameObject.SetActive(false);
    //     // allow unsubscribe from tick events
    //     Destroy(gameObject, 1f);
    // }
}