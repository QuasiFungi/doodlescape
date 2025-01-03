using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// ?multi pattern
// ? inherit from entity for save load integration
public class React : Entity
{
    [Header("React")]
    [Tooltip("Valid pattern")] [SerializeField] protected bool[] _match = new bool[0];
    [Tooltip("Disable source interacts on state change")] [SerializeField] protected bool _oneWay = false;
    [Tooltip("Initial state")] [SerializeField] protected bool _default = false;
    // [Tooltip("Minimum pattern match")] [SerializeField] protected int _count = 0;
    protected int _count;
    protected bool[] _signal;
    protected bool _state;
    private int _size;
    // private Collider2D[] _colliders;
    // private SpriteRenderer[] _sprites;
    // private Entity[] _interacts;
    // ? auto select all in radius
    [Tooltip("All interacts that can control this react")] [SerializeField] protected List<InteractReact> _sources = null;
    // protected List<InteractReact> _sources = null;
    // [Tooltip("Auto select all interacts in radius to control this react")] [SerializeField] private float _radius = 1f;
    [Tooltip("shown when react activated")] [SerializeField] private string _msgActivate = "Undefined";
    protected override void Awake()
    {
        base.Awake();
        // 
        _count = _match.Length;
        // _signal = new bool [_count];
        // _signal = null;
        // _collider = GetComponent<Collider2D>();
        // _sprite = GetComponent<SpriteRenderer>();
        // // ? save load
        // _state = _default;
        // System.Array.Clear(_signal, 0, _signal.Length);
        // 
        _size = transform.childCount;
        // _colliders = new Collider2D[_size];
        // _sprites = new SpriteRenderer[_size];
        // _interacts = new Entity[_size];
        // Transform temp;
        // for (int i = 0; i < _size; i++) _interacts[i] = transform.GetChild(i).GetComponent<Entity>();
        // {
        //     temp = transform.GetChild(i);
        //     // 
        //     _colliders[i] = temp.GetComponent<Collider2D>();
        //     // // ? save load ? consider inverted case
        //     // _colliders[i].enabled = true;
        //     _sprites[i] = temp.GetComponent<SpriteRenderer>();
        // }
        _sprites = new SpriteRenderer[transform.childCount];
        for (int i = _sprites.Length - 1; i > -1; i--)
            _sprites[i] = transform.GetChild(i).GetComponent<SpriteRenderer>();
        // 
        if (_msgActivate.Length > 19) Debug.LogWarning(IDUnique + ": Usage message too long! Currently " + _msgActivate.Length + " characters");
    }
    // protected void OnEnable()
    protected override void OnEnable()
    {
        base.OnEnable();
        // 
        foreach (InteractReact source in _sources) source.onPing += Ping;
    }
    // protected void OnDisable()
    protected override void OnDisable()
    {
        base.OnDisable();
        // 
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
        // // * testing save/load ? implicit save vs explicit
        // DataSave();
        ToggleActive();
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
    // private void Deactivate()
    // {
    //     for (int i = 0; i < _size; i++) _interacts.ToggleActive(false);
    //     // {
    //     //     // allow player to walk over
    //     //     _colliders[i].enabled = false;
    //     //     // appear like decal
    //     //     _sprites[i].color = new Color(.25f, .25f, .25f, 1f);
    //     // }
    // }
    // private void Reactivate()
    // {
    //     for (int i = 0; i < _size; i++) _interacts.ToggleActive(true);
    //     // {
    //     //     // disallow player to walk over
    //     //     // ? damage/push/telefrag entity
    //     //     _colliders[i].enabled = true;
    //     //     // appear like wall
    //     //     _sprites[i].color = new Color(1f, 1f, 1f, 1f);
    //     // }
    // }
    // active =/= enabled
    private void ToggleActive()
    {
        // pattern matched
        if (_state != _default)
        {
            // // deactivate self
            // // Deactivate();
            // ToggleActive(false);
            // allow walking over
            for (int i = _sprites.Length - 1; i > -1; i--)
                _sprites[i].GetComponent<Collider2D>().enabled = false;
            // fade out
            StartCoroutine("Flash");
            // deactivate interacts
            // if (_oneWay) foreach (InteractReact source in _sources) source.Deactivate();
            if (_oneWay) foreach (InteractReact source in _sources) source.ToggleActive(false);
            // notify player of mechanism activation
            Teleprompter.Register(_msgActivate);
        }
        // ? called every failed ping
        // else Reactivate();
        else ToggleActive(true);
    }
    private SpriteRenderer[] _sprites;
    IEnumerator Flash()
    {
        // * testing sfx react success
        GameAudio.Instance.Register(16, GameAudio.AudioType.ENTITY);
        // fade alpha over tick duration
        Color colorA = GameVariables.ColorDefault;
        Color colorB = GameVariables.ColorDecal;
        // 
        for(float t = 0f; t < 1f; t += Time.deltaTime)
        {
            for (int i = _sprites.Length - 1; i > -1; i--)
                _sprites[i].color = Color.Lerp(colorA, colorB, t);
            yield return null;
        }
        // deactivate self
        ToggleActive(false);
    }
    // for debug purposes
    void OnDrawGizmosSelected()
    {
        // Draw a yellow line from this to every source
        foreach (InteractReact source in _sources)
        {
            Gizmos.color = Color.yellow;
            if (source) Gizmos.DrawLine(transform.position, source.transform.position);
        }
    }
    // // * testing save/load
    // protected override void DataLoad()
    // {
    //     // ? save load
    //     _state = _default;
    //     System.Array.Clear(_signal, 0, _signal.Length);
    //     // // 
    //     // _size = transform.childCount;
    //     // _colliders = new Collider2D[_size];
    //     // _sprites = new SpriteRenderer[_size];
    //     // Transform temp;
    //     // for (int i = 0; i < _size; i++)
    //     // {
    //     //     temp = transform.GetChild(i);
    //     //     // 
    //     //     _colliders[i] = temp.GetComponent<Collider2D>();
    //     //     // ? save load
    //     //     _colliders[i].enabled = true;
    //     //     _sprites[i] = temp.GetComponent<SpriteRenderer>();
    //     // }
    //     // apply recovered state changes
    //     ToggleActive();
    // }
    // * testing save/load
    protected override void DataLoad()
    {
        // * testing save/load
        Vector3 position, rotation;
        bool isActive;
        // use defaults if no data found
        if (!GameData.DataLoadReact(IDUnique, out position, out rotation, out isActive, out _lootID, out _state, out _signal))
        {
            // position rotation isActive already assigned correctly
            _state = _default;
            _signal = new bool[_count];
            // // set visibility
            // ToggleActive(_state);
        }
        // apply loaded values
        else
        {
            // ? unnecessary since these never move
            SetPosition(position);
            SetRotation(rotation);
            // if holding an item
            if (_lootID != "")
                // try retrieving item from manager ? null on fail
                _loot = ManagerItem.Instance.GetItemByIDUnique(_lootID);
            // // set visibility
            // ToggleActive(isActive);
        }
        // set visibility
        // ToggleActive(isActive);
        ToggleActive(_state);
    }
    protected override void DataSave()
    {
        GameData.DataSaveReact(IDUnique, Position, Rotation, IsActive, _lootID, _state, _signal);
    }
}