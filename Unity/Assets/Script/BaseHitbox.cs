using UnityEngine;
// ? inherit from entity
// public class BaseHitbox : MonoBehaviour
public class BaseHitbox : Entity
{
    [Header("Hitbox")]
    // reference to entity that spawned this ? only creatures can deal damage
    protected Creature _source;
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
    protected Vector2Int _domain;
    public void Initialize(Creature source, Vector3 target, Vector2Int room)
    {
        _source = source;
        _target = target;
        _domain = room;
    }
    // protected void Discard()
    // {
    //     // * testing
    //     gameObject.SetActive(false);
    //     // allow unsubscribe from tick events
    //     Destroy(gameObject, 1f);
    // }
    // protected virtual void OnEnable()
    protected override void OnEnable()
    {
        GameMaster.onTransitionBegin += OnPause;
        GameMaster.onTransitionReady += OnDelete;
        // 
        // no need since data not tracked
        // base.OnEnable();
    }
    // protected virtual void OnDisable()
    protected override void OnDisable()
    {
        GameMaster.onTransitionBegin -= OnPause;
        GameMaster.onTransitionReady -= OnDelete;
    }
    protected virtual void OnPause(Vector2Int room)
    {
        // disable rigidbody if any
    }
    protected void OnDelete(Vector2Int room)
    {
        // discard entity
        if (_domain != room) Discard();
    }
}