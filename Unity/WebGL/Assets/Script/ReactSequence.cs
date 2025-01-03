using UnityEngine;
// fortressDoorWide
public class ReactSequence : React
{
    [Header("Sequence")]
    [Tooltip("False: Lock (fail on input) | True: Trap (fail on sequence)")] [SerializeField] private bool _sequenceType = false;
    // store signal for trap type activation
    private CachePing[] _cache;
    // implicit sequence
    private int _next;
    // protected override void Start()
    // {
    //     base.Start();
    //     // 
    //     // ? tie to save/load
    //     _cache = new CachePing[_count];
    //     _next = 0;
    // }
    // ? doesnt account for case when press button A then B then A again ! patch by make button one way
    protected override void Ping(int id, bool value)
    {
        // trap
        if (_sequenceType)
        {
            // count if new source ? always true
            // if (_cache[_next] == null) _next++;
            if (_cache[_next].ID == -1) _next++;
            // store ping data
            // _cache[_next - 1] = new CachePing(id, value);
            _cache[_next - 1].Update(id, value);
            // ping recorded from all sources
            if (_next == _count)
            {
                // verify sequence
                for (int i = 0; i < _count; i++)
                    // ping valid
                    if (_cache[i].ID == i)
                    {
                        // process signal
                        base.Ping(i, _cache[i].Value);
                        // ping accepted
                        _next--;
                    }
                // signal fully processed
                if (_next == 0)
                {
                    // ? necessary duplicate X
                    DataSave();
                    return;
                }
            }
            // await next ping
            else
            {
                // ? necessary duplicate X
                DataSave();
                return;
            }
        }
        // lock
        // signal from next in sequence
        else if (id == _next)
        {
            // process signal
            base.Ping(id, value);
            // wait for next signal
            _next++;
            // ? necessary duplicate 
            DataSave();
            return;
        }
        // clear signal record
        for (int i = 0; i < _count; i++)
        {
            _signal[i] = false;
            // _cache[i] = null;
            _cache[i].Clear();
        }
        // reset all sources ? show icon activate then deactive with half second delay for feedback other than vfx
        foreach (InteractReact source in _sources) source.Initialize();
        // // 
        // for (int i = 0; i <= _next; i++)
        // {
        //     // clear signal record
        //     _signal[i] = false;
        //     // reset all sources
        //     _sources[i].Initialize();
        // }
        // wait for first signal
        _next = 0;
        // ? necessary duplicate X
        DataSave();
    }
    // * testing save/load
    protected override void DataLoad()
    {
        // * testing save/load
        Vector3 position, rotation;
        bool isActive;
        // use defaults if no data found
        if (!GameData.DataLoadReactSequence(IDUnique, out position, out rotation, out isActive, out _lootID, out _state, out _signal, out _cache, out _next))
        {
            // position rotation isActive already assigned correctly
            _state = _default;
            _signal = new bool[_count];
            // System.Array.Clear(_signal, 0, _signal.Length);
            _cache = new CachePing[_count];
            for (int i = _count - 1; i > -1; i--) _cache[i] = new CachePing();
            _next = 0;
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
        }
        // set visibility
        ToggleActive(isActive);
    }
    protected override void DataSave()
    {
        GameData.DataSaveReactSequence(IDUnique, Position, Rotation, IsActive, _lootID, _state, _signal, _cache, _next);
    }
}
// reduce garbage, reuse not replace
[System.Serializable]
public class CachePing
{
    public int ID;
    public bool Value;
    public CachePing()
    {
        ID = -1;
        // ? should be opposite of match
        Value = false;
    }
    public void Clear()
    {
        ID = -1;
        // ? should be opposite of match, or is -ve id enough
        Value = false;
    }
    public void Update(int id, bool value)
    {
        ID = id;
        Value = value;
    }
}