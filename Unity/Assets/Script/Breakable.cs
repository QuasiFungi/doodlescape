using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// rubble
public class Breakable : Entity
{
    [SerializeField] protected int _health = 1;
    protected int _healthInst;
    // protected GameObject[] _loot;
    // protected void InitializeHealth()
    // {
    //     _healthInst = _health;
    // }
    // ? taken from anim or entity
    protected SpriteRenderer _sprite;
    private Color _colorFlash;
    private Color _colorDefault;
    protected List<Collider2D> _colliders;
    protected virtual void Start()
    {
        // ? save load
        _healthInst = _health;
        // * testing walk animation ? move to anim
        _sprite = _body.GetComponent<SpriteRenderer>();
        // color to take when hurt
        _colorFlash = new Color(1f, 0f, 0f, 1f);
        // ? conflict with BT color assign, wait few frames for BT execution
        // remember current color
        _colorDefault = _sprite.color;
        // * testing ? move to motor
        _colliders = new List<Collider2D>();
        _colliders.Add(GetComponent<Collider2D>());
    }
    // 
    public virtual void HealthModify(int value, Creature source)
    {
        // 
        _healthInst = Mathf.Clamp(_healthInst + value, 0, _health);
        // * testing ? coroutine being called after dead... unknown behaviour with mob
        StopCoroutine("Flash");
        StartCoroutine("Flash");
        // if (IsDead) Discard();
        // else StartCoroutine("Flash");
        // * testing
        // if (_healthInst == 0) Discard();
        // if (_healthInst == 0) Hide();
    }
    public override void Discard()
    {
        // * testing ? delete or pool
        _body.gameObject.SetActive(false);
        // 
        base.Discard();
    }
    IEnumerator Flash()
    {
        // fade alpha over tick duration ? account for dynamic tick size
        Color color = _colorDefault;
        // * testing
        if (IsDead)
        {
            // fade out on dead
            color.a = 0f;
            // disable colliders
            SetColliders(false);
        }
        // flash to white then slowly back to original color
        for(float t = 0f; t < 1f; t += Time.deltaTime * 1f)
        {
            _sprite.color = Color.Lerp(_colorFlash, color, t);
            yield return null;
        }
        // reapply default color
        _sprite.color = color;
        // // * testing
        // if (IsDead) Discard();
    }
    protected void SetColliders(bool value)
    {
        // * testing
        foreach (Collider2D collider in _colliders) collider.enabled = value;
    }
    private bool IsDead
    {
        get { return _healthInst == 0; }
    }
    public float HealthInst
    {
        get { return _healthInst; }
    }
    // 
    // * testing move animation
    // 
    private Vector3 _position;
    // * testing separate collider and sprite ? case for unassigned body
    public Transform _body;
    public void Move(Vector3 target)
    {
        // 
        StopCoroutine("LerpPosition");
        // * testing ? remove in awake, lots of unparented bodies..? collider as child?
        if (_body.parent) _body.SetParent(null);
        // 
        _position = target;
        // 
        StartCoroutine("LerpPosition");
    }
    IEnumerator LerpPosition()
    {
        // * testing
        // look in direction to move always
        // if (_testRotation)
        transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(_position.y - Position.y, _position.x - Position.x) * Mathf.Rad2Deg - 90f);
        _body.eulerAngles = transform.eulerAngles;
        // _body.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(_position.y - transform.position.y, _position.x - transform.position.x) * Mathf.Rad2Deg);
        // record start position
        Vector3 start = Position;
        // move collider to target position
        transform.position = _position;
        // lerp counter
        float lerp = 0f;
        // not at target position
        while (Vector3.Distance(start, _position) > 0f)
        {
            // advance lerp timer
            lerp += Time.deltaTime * 2f;
            // update position offset for this frame
            _body.position = Vector3.Lerp(start, _position, lerp);
            // wait till next frame
            yield return null;
        }
    }
    protected Vector3 Position
    {
        get { return transform.position; }
    }
}