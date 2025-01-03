using UnityEngine;
// charm, bell
public class HitboxRadius : BaseHitbox
{
    [Header("Radius")]
    [Range(0, 5)] [SerializeField] private int _radius;
    [SerializeField] private GameObject _effect;
    protected override void Start()
    {
        base.Start();
        // 
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _radius + .5f, GameVariables.ScanLayerCreature);
        // 
        foreach (Collider2D target in colliders)
        {
            // ignore itself
            if (target.transform.GetComponent<Breakable>() == _source) continue;
            // ? use breakable position
            Vector3 position = GameVariables.PositionDamage(target.transform.position);
            // 
            GameObject temp = Instantiate(_effect, position, transform.rotation);
            // 
            temp.GetComponent<BaseHitbox>().Initialize(_source, position, _domain);
            // 
            temp.SetActive(true);
        }
        // // 
        // Discard();
    }
    // scale visiblity with tick duration ? overkill
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
    // ? hard coded
    // private int _timer = 1;
    void Tick()
    {
        // if (_timer > 0) _timer--;
        // else if (_timer == 0) Discard();
        Discard();
    }
    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, _radius + .5f);
    // }
}