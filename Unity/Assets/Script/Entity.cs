using UnityEngine;
using System.Collections;
public class Entity : MonoBehaviour
{
    // protected Vector2 _position;
    [Tooltip("unique identifier for this entity")] [SerializeField] protected string _id;
    // protected Sprite[] _sprites;
    // protected Collider2D _collider;
    [SerializeField] protected string[] _vfxPooled;
    [SerializeField] protected string[] _attackPooled;
    [SerializeField] protected string[] _damagePooled;
    [SerializeField] protected string[] _sfxPooled;
    [Tooltip("entity destroyed on discard")] [SerializeField] private bool _isPooled = false;
    protected Item _loot = null;
    protected virtual void Awake()
    {
        // items dont have loot ? use hasLoot or isItem
        if (gameObject.layer == GameVariables.LayerItem) return;
        // get item on top
        Collider2D hit = Physics2D.OverlapCircle(transform.position, .1f, GameVariables.ScanLayerItem);
        // item detected ? multiple over same tile
        if (hit)
        {
            // store item reference
            _loot = hit.GetComponent<Item>();
            // hide item if exists/valid
            if (_loot) _loot.ToggleActive(false);
            // throw error to notify dev
            else Debug.Log(gameObject.name + "\t<color=red>Attempted to assign invalid item drop</color>", this);
        }
        // throw error to notify dev
        // else print(gameObject.name + " : no assigned item drop");
        // else Debug.Log(gameObject.name + "\t<color=yellow>No assigned item drop</color>", this);
    }
    // use toggleActive to hide even internally
    protected virtual void Hide()
    {
        gameObject.SetActive(false);
    }
    // use toggleActive to show even internally
    protected virtual void Show()
    {
        gameObject.SetActive(true);
    }
    // ? use only by pool system
    public virtual void Discard()
    {
        // unsubscribe from all events ? child or parent hide called
        Hide();
        // ? scale with tick duration ..? disallow discard unless pooled
        if (_isPooled) Destroy(gameObject, 1f);
    }
    // prevents chunk from changing entity state
    private bool _isActive = true;
    // * testing chunk loading, just a wrapper for show/hide
    public void ToggleActive(bool state, bool isChunk = false)
    {
        // ? prevent chunk from changing entity state
        if (isChunk)
        {
            if (!_isActive) return;
        }
        // ? value not unaffected by chunk loading
        else _isActive = state;
        // 
        if (state) Show();
        else Hide();
    }
    // * testing object pooling, wrapper for toggleActive
    public void ToggleActive(bool state, Vector3 position, Quaternion rotation)
    {
        SetPosition(position);
        SetRotation(rotation);
        // 
        ToggleActive(state);
    }
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
    // public abstract void Initialize()
    // {
    //     // 
    // }
    public string ID
    {
        get { return _id; }
    }
    // * testing object pooling
    public bool IsActive
    {
        get { return gameObject.activeSelf; }
    }
    // ? use id + quantity struct or class
    public string[] PooledVFX
    {
        get { return _vfxPooled; }
    }
    // ? not all mobs have attack ? pooledAttackPrimary ? better naming scheme pls rn is too confusing
    public string[] PooledAttack
    {
        get { return _attackPooled; }
    }
    // ? how handle damage spawned by attacks ? pooledAttackSecondary ? what about multi stage ? all non primary as secondary
    public string[] PooledDamage
    {
        get { return _damagePooled; }
    }
    public string[] PooledSFX
    {
        get { return _sfxPooled; }
    }
}