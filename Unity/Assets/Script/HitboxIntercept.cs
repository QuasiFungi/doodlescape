using UnityEngine;
[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class HitboxIntercept : BaseHitbox
{
    [Header("Intercept")]
    [Range(0, 7)] [SerializeField] private int _radius = 0;
    protected override void OnEnable()
    {
        base.OnEnable();
        // 
        GameClock.onTick += Tick;
        // get all creatures in radius
        Collider2D[] targets = Physics2D.OverlapCircleAll(Position, Radius, GameVariables.ScanLayerCreature);
        // iterate all targets
        for (int i = targets.Length - 1; i > -1; i--)
            // ignore non trigger colliders and dead
            if (targets[i].isTrigger && targets[i].GetComponent<Creature>().HealthInst > 0) OnTriggerEnter2D(targets[i]);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        // 
        GameClock.onTick -= Tick;
        // get all creatures in radius
        Collider2D[] targets = Physics2D.OverlapCircleAll(Position, Radius, GameVariables.ScanLayerCreature);
        // iterate all targets
        for (int i = targets.Length - 1; i > -1; i--)
            // ignore non trigger colliders and dead
            if (targets[i].isTrigger && targets[i].GetComponent<Creature>().HealthInst > 0) OnTriggerExit2D(targets[i]);
    }
    [Tooltip("Effect duration (-1 for permanent)")] [SerializeField] private int _timer = -1;
    void Tick()
    {
        if (_timer > 0) _timer--;
        else if (_timer == 0) Discard();
    }
    public delegate void OnIntercept(string id, GameAction.ActionType type, bool state, Breakable source);
    public static event OnIntercept onIntercept;
    [Tooltip("Block actions of this type")] [SerializeField] private GameAction.ActionType[] _type;
    void OnTriggerEnter2D(Collider2D other)
    {
        for(int i = _type.Length - 1; i > -1; i--)
            onIntercept?.Invoke(other.gameObject.GetComponent<Entity>().IDUnique, _type[i], true, _source);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        for(int i = _type.Length - 1; i > -1; i--)
            onIntercept?.Invoke(other.gameObject.GetComponent<Entity>().IDUnique, _type[i], false, _source);
    }
    private float Radius
    {
        get { return _radius + .4f; }
    }
}