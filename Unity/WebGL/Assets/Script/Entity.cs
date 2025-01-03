using UnityEngine;
using System.Collections;
public class Entity : MonoBehaviour
{
    [Header("Entity")]
    // protected Vector2 _position;
    [Tooltip("unique identifier for this entity")] [SerializeField] private string _id;
    // [SerializeField] private string _idUnique;
    private string _idUnique;
    // protected Sprite[] _sprites;
    // protected Collider2D _collider;
    // [SerializeField] protected string[] _vfxPooled;
    // [SerializeField] protected string[] _attackPooled;
    // [SerializeField] protected string[] _damagePooled;
    // [SerializeField] protected string[] _sfxPooled;
    // [Tooltip("entity destroyed on discard")] [SerializeField] private bool _isPooled = false;
    protected Item _loot;
    protected string _lootID;
    protected virtual void Awake()
    {
        // * testing save/load
        _idUnique = _id + Position.ToString();
        // if (_idUnique == "") print(gameObject.name);
        // if (IDUnique == null) print(Position);
        // initial blank slate
        _loot = null;
        _lootID = "";
        // items and sensors dont have loot ? use hasLoot or isItem
        if (gameObject.layer == GameVariables.LayerItem || gameObject.layer == GameVariables.LayerSensor) return;
        // get item on top
        Collider2D hit = Physics2D.OverlapCircle(transform.position, .1f, GameVariables.ScanLayerItem);
        // item detected ? case for multiple over same tile
        if (hit)
            // store item reference
            _loot = hit.GetComponent<Item>();
        // throw error to notify dev
        // else print(gameObject.name + " : no assigned item drop");
        // else Debug.Log(gameObject.name + "\t<color=yellow>No assigned item drop</color>", this);
    }
    // // * testing save/load, since gameData's awake is somehow being called AFTER this entity's onEnable...
    protected virtual void Start()
    {
    //     // overwrite save data if exists
    //     if (_isPersistent) DataLoad();
    //     // // delay item-entity linkage to after data loading completes for all entities
    //     // if (IsActive)
    //     // {
    //     //     // items and sensors dont have loot ? use hasLoot or isItem
    //     //     if (gameObject.layer != GameVariables.LayerItem && gameObject.layer != GameVariables.LayerSensor)
    //     //         StartCoroutine("LootCheck");
    //     // }
    //     // // hide item if exists/valid ? can be moved to data load
    //     // if (_loot)
    //     // {
    //     //     _lootID = _loot.IDUnique;
    //     //     _loot.ToggleActive(false);
    //     // }
    }
    // IEnumerator LootCheck()
    private void LootCheck()
    {
        // 
        // yield return null;
        // {
        //     // get item on top
        //     Collider2D hit = Physics2D.OverlapCircle(transform.position, .1f, GameVariables.ScanLayerItem);
        //     // item detected ? multiple over same tile
        //     if (hit)
        //     {
        //         // store item reference
        //         _loot = hit.GetComponent<Item>();
        //         // hide item if exists/valid
        //         if (_loot) _loot.ToggleActive(false);
        //         // throw error to notify dev
        //         else Debug.Log(gameObject.name + "\t<color=red>Attempted to assign invalid item drop: object missing core script</color>", this);
        //     }
        // }
        // hide item if exists/valid
        if (_loot)
        {
            _lootID = _loot.IDUnique;
            _loot.ToggleActive(false);
        }
        // // throw error to notify dev
        // else Debug.Log(gameObject.name + "\t<color=red>Attempted to assign invalid item drop: object missing core script</color>", this);
    }
    // // use toggleActive to hide even internally
    // private void Hide()
    // {
    //     gameObject.SetActive(false);
    // }
    // // use toggleActive to show even internally
    // private void Show()
    // {
    //     gameObject.SetActive(true);
    // }
    // * testing
    [Tooltip("Save entity data and don't destroy on discard")] [SerializeField] private bool _isPersistent = true;
    // ? use only by pool system
    // used when certain entity will not be reenabled
    public virtual void Discard()
    {
        // * testing ? pool instead
        // if (!_isPersistent) Destroy(gameObject, 1f);
        if (!_isPersistent) Destroy(gameObject);
        // unsubscribe from all events ? child or parent hide called
        // Hide();
        else
        {
            // drop loot
            if (_loot)
            {
                // move to current position
                _loot.SetPosition(Position);
                // make visible
                _loot.ToggleActive(true);
                // clear record
                _loot = null;
                _lootID = "";
            }
            // 
            ToggleActive(false);
            // // explicit save
            // if (_isPersistent) DataSave();
        }
        // // ? scale with tick duration ..? disallow discard unless pooled
        // if (_isPooled) Destroy(gameObject, 1f);
    }
    // variant with instant item drop but delayed disable
    protected IEnumerator Discard(float delay)
    {
        // * testing ? pool instead
        // if (!_isPersistent) Destroy(gameObject, 1f);
        if (!_isPersistent) Destroy(gameObject);
        // unsubscribe from all events ? child or parent hide called
        else
        {
            // drop loot
            if (_loot)
            {
                // move to current position
                _loot.SetPosition(Position);
                // make visible
                _loot.ToggleActive(true);
                // clear record
                _loot = null;
                _lootID = "";
            }
            // 
            yield return new WaitForSeconds(delay);
            // 
            ToggleActive(false);
            // // explicit save
            // if (_isPersistent) DataSave();
        }
    }
    // prevents chunk from changing entity state
    private bool _isActive = true;
    // * testing chunk loading, just a wrapper for show/hide
    public virtual void ToggleActive(bool state, bool isChunk = false)
    {
        // ? prevent chunk from changing entity state
        if (isChunk)
        {
            // ignore if already inactive
            if (!_isActive) return;
        }
        // record state change if by other than chunk
        else _isActive = state;
        // // 
        // if (state) Show();
        // else Hide();
        // if (state) gameObject.SetActive(true);
        // else gameObject.SetActive(false);
        // gameObject.SetActive(_isActive);
        gameObject.SetActive(state);
        // if (ID == "sensor_fortress_roots") print(gameObject.activeSelf);
        // if (ID == "sensor_fortress_mycelium") print(_isActive);
        // explicit save
        if (_isPersistent) DataSave();
    }
    // // * testing object pooling, wrapper for toggleActive
    // public void ToggleActive(bool state, Vector3 position, Quaternion rotation)
    // {
    //     SetPosition(position);
    //     SetRotation(rotation);
    //     // 
    //     ToggleActive(state);
    // }
    // // * testing chunk loading, only enable entities disabled by chunk
    // public void ToggleLoad(bool state)
    // {
    //     // prevent chunk from changing entity state
    //     // if (_isActive) ToggleActive(state);
    //     if (!_isActive) return;
    //     // 
    //     if (state) Show();
    //     else Hide();
    // }
    protected virtual void SetPosition(Vector3 position)
    {
        // 
        transform.position = position;
    }
    // ? not protected since not used elsewhere
    private void SetRotation(Quaternion rotation)
    {
        // 
        transform.rotation = rotation;
    }
    protected virtual void SetRotation(Vector3 rotation)
    {
        transform.eulerAngles = rotation;
    }
    // public abstract void Initialize()
    // {
    //     // 
    // }
    public string ID
    {
        get { return _id; }
    }
    // // * testing object pooling
    // public bool IsActive
    // {
    //     get { return gameObject.activeSelf; }
    // }
    // // ? use id + quantity struct or class
    // public string[] PooledVFX
    // {
    //     get { return _vfxPooled; }
    // }
    // // ? not all mobs have attack ? pooledAttackPrimary ? better naming scheme pls rn is too confusing
    // public string[] PooledAttack
    // {
    //     get { return _attackPooled; }
    // }
    // // ? how handle damage spawned by attacks ? pooledAttackSecondary ? what about multi stage ? all non primary as secondary
    // public string[] PooledDamage
    // {
    //     get { return _damagePooled; }
    // }
    // public string[] PooledSFX
    // {
    //     get { return _sfxPooled; }
    // }
    // * testing save/load
    protected virtual void OnEnable()
    {
        // // trigger whenever game master requests load
        // GameMaster.onLoad += OnInitialize;
        // * testing startup sequence ? move to onEnable ? unsubscribe in onDisable
        GameMaster.onStartupLoad += DataLoad;
        GameMaster.onStartupLoot += LootCheck;
    }
    protected virtual void OnDisable()
    {
        // GameMaster.onLoad -= OnInitialize;
        // * testing startup sequence ? move to onEnable ? unsubscribe in onDisable
        GameMaster.onStartupLoad -= DataLoad;
        GameMaster.onStartupLoot -= LootCheck;
    }
    // protected virtual void OnInitialize()
    // {
    //     // insert data application here
    // }
    // 
    // * testing save/load
    public string IDUnique
    {
        get { return _idUnique; }
    }
    // deferred to start method
    // protected virtual void OnEnable()
    // {
    //     if (_isPersistent) DataLoad();
    // }
    // ? becoming less relevant as more entites save data immediately on change
    // ? don't save room until leave at which point assume cleared
    // ? case for haphazardly player destroying cover, or sequence of covers timed to mob fovs
    // // implicit saving
    // protected virtual void OnDisable()
    // {
    //     if (_isPersistent) DataSave();
    // }
    // protected virtual void DataLoad()
    // {
    //     // do not make base calls, otherwise multiple retrievals of partial data
    //     // override entire functionality instead
    //     // ? make abstract but not used by every child
    // }
    // protected virtual void DataSave()
    // {
    //     // 
    // }
    protected virtual void DataLoad()
    {
        // if (IDUnique == null) print(Position);
        // * testing save/load
        Vector3 position, rotation;
        bool isActive;
        // ignore if no data found, do not overwrite loot
        if (GameData.DataLoadEntity(IDUnique, out position, out rotation, out isActive, out _lootID))
        {
            // apply loaded values
            SetPosition(position);
            SetRotation(rotation);
            // if holding an item
            if (_lootID != "")
                // try retrieving item from manager ? null on fail
                _loot = ManagerItem.Instance.GetItemByIDUnique(_lootID);
        }
        // apply dead state
        ToggleActive(isActive);
    }
    protected virtual void DataSave()
    {
        GameData.DataSaveEntity(IDUnique, Position, Rotation, IsActive, _lootID);
    }
    public Vector3 Position
    {
        get { return transform.position; }
    }
    public Vector3 Rotation
    {
        get { return transform.eulerAngles; }
    }
    // // only one use case
    // public Vector3 RotationQ
    // {
    //     get { return transform.rotation; }
    // }
    public bool IsActive
    {
        get { return _isActive; }
    }
}