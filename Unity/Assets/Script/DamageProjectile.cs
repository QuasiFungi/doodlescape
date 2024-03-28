using UnityEngine;
public class DamageProjectile : HitboxDamage
{
    [SerializeField] protected float _impulse = 1f;
    [SerializeField] protected float _distance = 1f;
    protected override void Awake()
    {
        base.Awake();
        // * testing ? start
        _rb.velocity = transform.up * _impulse;
        _timer = _distance / _impulse;
    }
    void OnEnable()
    {
        _rb.bodyType = RigidbodyType2D.Dynamic;
    }
}