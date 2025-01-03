using UnityEngine;
// seedFlower
public class HitboxSpread : BaseHitbox
{
    [Header("Spread")]
    [Tooltip("Damage effect to spawn")] [SerializeField] private GameObject _attack;
    [SerializeField] private int _angle;
    [SerializeField] private int _count;
    protected override void Awake()
    {
        base.Awake();
        // 
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        // 
        GameClock.onTickUIEarly += Tick;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        // 
        GameClock.onTickUIEarly -= Tick;
    }
    // hard coded
    void Tick()
    {
        Discard();
    }
}