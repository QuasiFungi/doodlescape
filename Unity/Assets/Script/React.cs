using UnityEngine;
// ?multi pattern
// ? inherit from entity for save load integration
public class React : MonoBehaviour
{
    [Tooltip("Valid pattern")] [SerializeField] protected bool[] _match = new bool[0];
    [SerializeField] protected bool _oneWay = false;
    [Tooltip("Initial state")] [SerializeField] protected bool _default = false;
    // [Tooltip("Minimum pattern match")] [SerializeField] protected int _count = 0;
    protected bool[] _signal;
    protected bool _state;
    void Awake()
    {
        _signal = new bool [_match.Length];
        // _collider = GetComponent<Collider2D>();
        // _sprite = GetComponent<SpriteRenderer>();
        // ? save load
        _state = _default;
        System.Array.Clear(_signal, 0, _signal.Length);
    }
    // public virtual void Ping(int id, bool value, Interact source)
    public virtual void Ping(int id, bool value)
    {
        // print(gameObject.name + "," + id + "," + value);
        if (id > -1 && id < _match.Length) _signal[id] = value;
        // 
        TryMatch();
    }
    protected void TryMatch()
    {
        // already switched activation state, ignore ? lock interacts
        if (_oneWay && _state != _default) return;
        // 
        int count = 0;
        for (int i = _match.Length - 1; i > -1; i--) if (_signal[i] == _match[i]) count++;
        // _active = (count >= _count) || (_active && _oneWay);
        _state = count == _match.Length ? !_default : _default;
    }
}