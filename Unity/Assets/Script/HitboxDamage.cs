using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(Rigidbody2D))]
public class HitboxDamage : BaseHitbox
{
    [SerializeField] protected int _damage = 0;
    [SerializeField] protected bool _impactSensitive = false;
    // * testing [self destruct tuning]
    [Tooltip("Ticks till self destruct (-1 off)")] [SerializeField] protected float _timer = -1;
    protected Rigidbody2D _rb;
    // // ? why generic, breakable
    // protected List<Breakable> _targets;
    protected override void Awake()
    {
        base.Awake();
        // 
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
        _rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().isTrigger = true;
        // _targets = new List<Breakable>();
    }
    // void OnEnable()
    // {
    //     GameClock.onTick += UpdateTick;
    // }
    // void OnDisable()
    // {
    //     GameClock.onTick -= UpdateTick;
    // }
    // private void UpdateTick()
    // {
    //     // check if timer not disabled
    //     if (_timer != -1)
    //     {
    //         // tick down
    //         _timer--;
    //         // ? object pool ? toggle instead
    //         if (_timer == 0) Discard();
    //     }
    // }
    void Update()
    {
        // check if timer not disabled
        if (_timer != -1)
        {
            // tick down
            _timer -= Time.deltaTime;
            // ? object pool ? toggle instead
            if (_timer <= 0) Discard();
        }
    }
    protected void OnTriggerEnter2D(Collider2D other)
    {
        // only effect creatures and breakables
        if (other.gameObject.layer != GameVariables.LayerCreature && other.gameObject.layer != GameVariables.LayerBreakable) return;
        // treat both creatures and breakables as breakables
        Breakable target = other.transform.GetComponent<Breakable>();
        // dont damage itself
        if (target == _source) return;
        // apply damage ? why ignore zero, what case when zero used
        // if (_damage != 0) target.HealthModify(-_damage, other.gameObject.layer == GameVariables.LayerCreature ? _source as Creature : null);
        // if (_damage != 0) target.HealthModify(-_damage, _source as Creature);
        // * testing damage flash
        target.HealthModify(-_damage, _source as Creature);
        // ? for bore type damage
        if (_impactSensitive) Discard();
    }
    // void OnTriggerExit2D(Collider2D other)
    // {
    //     // only effect creatures and breakables
    //     if (other.gameObject.layer != GameVariables.LayerCreature && other.gameObject.layer != GameVariables.LayerBreakable) return;
    //     // treat both creatures and breakables as breakables
    //     Breakable target = other.transform.GetComponent<Breakable>();
    //     // forget entity
    //     if (_targets.Contains(target)) _targets.Remove(target);
    // }
}