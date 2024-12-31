using UnityEngine;
// fortressDoorNarrow
public class ReactTrigger : React
{
    [Header("Trigger")]
    private int _cacheID = -1;
    private bool _cacheValue = false;
    protected override void Ping(int id, bool value)
    {
        // ? do something with tryMatch since accessible
        _cacheID = id;
        _cacheValue = value;
    }
    private int _creatures = 0;
    void OnTriggerEnter2D(Collider2D other)
    {
        // track individuals to avoid duplicate/redetection chance
        if (other.gameObject.layer == GameVariables.LayerCreature) _creatures++;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        // track individuals to avoid duplicate/redetection chance
        if (other.gameObject.layer == GameVariables.LayerCreature) _creatures--;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        // 
        GameClock.onTick += TryPing;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        // 
        GameClock.onTick -= TryPing;
    }
    private void TryPing()
    {
        // already activated, ignore
        if (_oneWay && _state != _default) return;
        // ping recieved ? overwritten by last recieved signal, always only has only one signal source
        if (_cacheID > -1 && _creatures == 0) Ping(_cacheID, _cacheValue);
    }
    // // * testing save/load
    // protected override void DataLoad()
    // {
    //     // * testing save/load
    //     Vector3 position, rotation;
    //     bool isActive;
    //     // use defaults if no data found
    //     if (!GameData.DataLoadReactSequence(IDUnique, out position, out rotation, out isActive, out _lootID, out _state, out _signal, out _cache, out _next))
    //     {
    //         // position rotation isActive already assigned correctly
    //         _state = _default;
    //         _signal = new bool[_count];
    //         // System.Array.Clear(_signal, 0, _signal.Length);
    //         _cache = new CachePing[_count];
    //         for (int i = _count - 1; i > -1; i--) _cache[i] = new CachePing();
    //         _next = 0;
    //     }
    //     // apply loaded values
    //     else
    //     {
    //         // ? unnecessary since these never move
    //         SetPosition(position);
    //         SetRotation(rotation);
    //         // if holding an item
    //         if (_lootID != "")
    //             // try retrieving item from manager ? null on fail
    //             _loot = ManagerItem.Instance.GetItemByIDUnique(_lootID);
    //     }
    //     // set visibility
    //     ToggleActive(isActive);
    // }
    // protected override void DataSave()
    // {
    //     GameData.DataSaveReactSequence(IDUnique, Position, Rotation, IsActive, _lootID, _state, _signal, _cache, _next);
    // }
}