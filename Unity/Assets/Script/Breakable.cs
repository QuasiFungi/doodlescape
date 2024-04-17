using UnityEngine;
using System.Collections;
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
    protected virtual void Start()
    {
        // ? save load
        _healthInst = _health;
    }
    // 
    public virtual void HealthModify(int value, Creature source)
    {
        _healthInst = Mathf.Clamp(_healthInst + value, 0, _health);
        // * testing
        if (_healthInst == 0) Discard();
        // if (_healthInst == 0) Hide();
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
        transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(_position.y - transform.position.y, _position.x - transform.position.x) * Mathf.Rad2Deg - 90f);
        _body.eulerAngles = transform.eulerAngles;
        // _body.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(_position.y - transform.position.y, _position.x - transform.position.x) * Mathf.Rad2Deg);
        // record start position
        Vector3 start = transform.position;
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
}