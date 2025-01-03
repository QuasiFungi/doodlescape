using UnityEngine;
// used by seedFlower
public class DamageProjectile : HitboxDamage
{
    [SerializeField] protected float _impulse = 1f;
    [SerializeField] protected float _distance = 1f;
    private Vector2 _start;
    private Vector2 _direction;
    private float _displacement;
    private float _smoothing;
    [Tooltip("Spawn on destroy")] [SerializeField] private GameObject _effect = null;
    protected override void Awake()
    {
        base.Awake();
        // * testing ? start
        _rb.bodyType = RigidbodyType2D.Kinematic;
        // _rb.velocity = transform.up * _impulse;
        _timer = _distance / _impulse;
        // 
        _start = transform.position;
        _direction = transform.up;
        _displacement = 0f;
        _smoothing = 0f;
    }
    void Update()
    {
        // overflow error otherwise
        if (_smoothing < 1f) _smoothing += Time.deltaTime;
        // assumed already at displacement
        transform.position = _start + _direction * Mathf.Lerp(_displacement, _displacement + _impulse, _smoothing);
    }
    protected override void Tick()
    {
        base.Tick();
        // step
        _displacement += _impulse;
        // reset
        _smoothing = 0f;
    }
    public override void Discard()
    {
        base.Discard();
        // * testing ? use particle effect
        if (_effect) Instantiate(_effect, transform.position, transform.rotation);
    }
    // protected override void OnEnable()
    // {
    //     base.OnEnable();
    //     // 
    //     _rb.bodyType = RigidbodyType2D.Dynamic;
    // }
    // protected override void OnPause(Vector2Int room)
    // {
    //     // base.OnPause();
    //     // 
    //     if (_domain != room) _rb.bodyType = RigidbodyType2D.Kinematic;
    // }
}