using UnityEngine;
using System.Collections.Generic;
// ?multi pattern
// ? inherit from entity for save load integration
public class React : Entity
{
    [Tooltip("Valid pattern")] [SerializeField] protected bool[] _match = new bool[0];
    [Tooltip("Disable source interacts on state change")] [SerializeField] protected bool _oneWay = false;
    [Tooltip("Initial state")] [SerializeField] protected bool _default = false;
    // [Tooltip("Minimum pattern match")] [SerializeField] protected int _count = 0;
    protected int _count;
    protected bool[] _signal;
    private bool _state;
    private int _size;
    private Collider2D[] _colliders;
    private SpriteRenderer[] _sprites;
    // ? auto select all in radius
    [Tooltip("All interacts that can control this react")] [SerializeField] protected List<InteractReact> _sources = null;
    // protected List<InteractReact> _sources = null;
    // [Tooltip("Auto select all interacts in radius to control this react")] [SerializeField] private float _radius = 1f;
    void Awake()
    {
        _count = _match.Length;
        _signal = new bool [_count];
        // _collider = GetComponent<Collider2D>();
        // _sprite = GetComponent<SpriteRenderer>();
        // ? save load
        _state = _default;
        System.Array.Clear(_signal, 0, _signal.Length);
        // 
        _size = transform.childCount;
        _colliders = new Collider2D[_size];
        _sprites = new SpriteRenderer[_size];
        Transform temp;
        for (int i = 0; i < _size; i++)
        {
            temp = transform.GetChild(i);
            // 
            _colliders[i] = temp.GetComponent<Collider2D>();
            // ? save load
            _colliders[i].enabled = true;
            _sprites[i] = temp.GetComponent<SpriteRenderer>();
        }
    }
    void OnEnable()
    {
        foreach (InteractReact source in _sources) source.onPing += Ping;
    }
    void OnDisable()
    {
        foreach (InteractReact source in _sources) source.onPing -= Ping;
    }
    // public virtual void Ping(int id, bool value, Interact source)
    protected virtual void Ping(int id, bool value)
    {
        // record signal
        if (id > -1 && id < _count) _signal[id] = value;
        // invalid signal
        else return;
        // update signal state
        TryMatch();
        // pattern matched
        if (_state != _default)
        {
            // deactivate self
            Deactivate();
            // deactivate interacts
            if (_oneWay) foreach (InteractReact source in _sources) source.Deactivate();
        }
        // ? called every failed ping
        else Reactivate();
    }
    protected void TryMatch()
    {
        // already activated, ignore
        if (_oneWay && _state != _default) return;
        // track matched signals
        int count = 0;
        for (int i = 0; i < _count; i++) if (_signal[i] == _match[i]) count++;
        // update signal state
        _state = count == _count ? !_default : _default;
    }
    private void Deactivate()
    {
        for (int i = 0; i < _size; i++)
        {
            // allow player to walk over
            _colliders[i].enabled = false;
            // appear like decal
            _sprites[i].color = new Color(.25f, .25f, .25f, 1f);
        }
    }
    private void Reactivate()
    {
        for (int i = 0; i < _size; i++)
        {
            // disallow player to walk over
            // ? damage/push/telefrag entity
            _colliders[i].enabled = true;
            // appear like wall
            _sprites[i].color = new Color(1f, 1f, 1f, 1f);
        }
    }
    // for debug purposes
    void OnDrawGizmosSelected()
    {
        // Draw a yellow line from this to every source
        foreach (InteractReact source in _sources)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, source.transform.position);
        }
    }
}