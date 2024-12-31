using UnityEngine;
using System.Collections;
// destructible unlock toggle oneWay
// door
public class Interact : Entity
{
    // protected int _state;
    private bool _locked = false;
    [Header("Interact")]
    [Tooltip("Require this item to unlock")] [SerializeField] protected string _valid;
    // used by chest and button/switch
    [Tooltip("Lock to state change")] [SerializeField] protected bool _oneWay = false;
    // ? make private along with state, have protected get property, use initialize function to set value
    [Tooltip("Initial state")] [SerializeField] private bool _default = false;
    // ? make private with get property
    protected bool _state;
    // ? different name maybe
    [Tooltip("Fade out on state change")] [SerializeField] private bool _disableOnChange = false;
    protected SpriteRenderer _sprite;
    private Sprite _spriteDefault;
    [Tooltip("Sprite for when state changes")] [SerializeField] private Sprite _spriteChanged = null;
    // ? also used by creatures, though some have multiple
    protected Collider2D _collider;
    [SerializeField] private string _msgFail = "It's locked";
    protected override void Awake()
    {
        base.Awake();
        // 
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        // ? copy vs reference
        _spriteDefault = _sprite.sprite;
    }
    // 
    // protected override void Start()
    // {
    //     base.Start();
    //     // 
    //     // // ? save file info
    //     // _state = _default;
    //     // gameObject.SetActive(true);
    //     // // if (_sprite) _sprite.enabled = true; // ? chunk/fog
    //     // SetSprite();
    //     // * testing save/load, actual data intialization handled by dedicated function
    //     gameObject.SetActive(true);
    // }
    protected void SetSprite()
    {
        // fade out entity if required
        if (_disableOnChange && _state != _default)
        {
            // StartCoroutine(FadeOut());
            // Deactivate();
            // ToggleActive(false);
            // trigger item drop if any
            Discard();
            // StartCoroutine(Discard(Time.timeScale));
            return;
        }
        // change sprite based on active state if possible
        if (_spriteChanged) _sprite.sprite = _state == _default ? _spriteDefault : _spriteChanged;
    }
    // doesn't look/feel nice, snappy instant is more rewarding, sfx would help
    // // door specific ? not reused
    // IEnumerator FadeOut()
    // {
    //     // allow move action over occupied tiles in next tick
    //     _collider.enabled = false;
    //     // fade alpha over tick duration ? account for dynamic tick size
    //     Color color = _sprite.color;
    //     for(float a = 1f - Time.deltaTime; a > 0f; a -= Time.deltaTime)
    //     {
    //         color.a = a;
    //         _sprite.color = color;
    //         yield return null;
    //     }
    //     // // hide entity
    //     // Hide();
    // }
    // 
    // public void Deactivate()
    // {
    //     // allow player to walk over
    //     _collider.enabled = false;
    //     // appear like decal
    //     _sprite.color = new Color(.25f, .25f, .25f, 1f);
    // }
    // 0 - fail | 1 - succeed | 2 - succeed with item
    public virtual void TryAction(Creature source)
    {
        // print(source.ItemHas(_valid));
        // print("doh");
        // try unlock
        if (_locked && source.ItemHas(_valid))
        {
            _locked = false;
            // notify player of consumption
            Teleprompter.Register(source.ItemGet(source.ItemGet(_valid)).Consumed);
            // discard item
            source.ItemRemove(_valid);
            // ? unlock vfx particle
            // Teleprompter.Register("It's locked");
        }
        // ? custom entity messages
        // else if (_locked) Teleprompter.Register(_msgFail);
        // still locked
        if (_locked)
        {
            // flash white
            StartCoroutine("Flash");
            // // * testing vibrate
            // GameData.DoVibrate(GameData.IntensityVibrate.HIGH);
            Teleprompter.Register(_msgFail);
            // * testing sfx fail
            GameAudio.Instance.Register(8, GameAudio.AudioType.ENTITY);
        }
        // else if (_locked) Teleprompter.Register(_valid);
        // {
        //     feedback_popup.Instance.RegisterMessage(transform, "unlocked", game_variables.Instance.ColorInteract);
        //     return true;
        // }
        // open
        else
        {
            _state = _oneWay ? !_default : !_state;
            // _state = !_state;
            // if (_oneWay && _active)
            //     feedback_popup.Instance.RegisterMessage(transform, _valid ? "opened" : "on", game_variables.Instance.ColorInteract);
            // else if (_active)
            //     feedback_popup.Instance.RegisterMessage(transform, "on", game_variables.Instance.ColorInteract);
            // else
            //     feedback_popup.Instance.RegisterMessage(transform, "off", game_variables.Instance.ColorInteract);
            // * testing ? duration type
            // if (game_variables.Instance.Vibration == 0 || game_variables.Instance.Vibration == 2)
            //     Handheld.Vibrate();
            // SetSprite();
            // * testing sfx success
            if (!_isWait) GameAudio.Instance.Register(9, GameAudio.AudioType.ENTITY);
        }
        // // * testing
        // if (_valid)
        //     // feedback_popup.Instance.RegisterMessage(transform, "need " + _valid.gameObject.name, game_variables.Instance.ColorInteract);
        //     feedback_toaster.Instance.RegisterMessage(gameObject.name + " : need " + _valid.gameObject.name, game_variables.Instance.ColorInteract);
        // else
        //     // feedback_popup.Instance.RegisterMessage(transform, "jammed", game_variables.Instance.ColorInteract);
        //     feedback_toaster.Instance.RegisterMessage(gameObject.name + " : jammed", game_variables.Instance.ColorInteract);
        SetSprite();
        // * test save/load, otherwise wont save till disabled by chunk when 2 or more rooms away
        DataSave();
    }
    protected IEnumerator Flash()
    {
        // fade alpha over tick duration
        Color colorA = GameVariables.ColorDefault;
        Color colorB = GameVariables.ColorInteract;
        // 
        for(float t = 0f; t < 1f; t += Time.deltaTime)
        {
            _sprite.color = Color.Lerp(colorA, colorB, t);
            yield return null;
        }
        // reapply default color
        _sprite.color = colorB;
    }
    protected bool _isWait = false;
    // used by reactSequence
    public void Initialize()
    {
        // flash white
        StartCoroutine("Flash");
        // ping was sent by this entity
        if (_isWait)
        {
            // notify failure
            Teleprompter.Register(_msgFail);
            // * testing sfx fail
            GameAudio.Instance.Register(7, GameAudio.AudioType.ENTITY);
            // 
            _isWait = false;
        }
        // 
        _state = _default;
        _locked = _valid != "";
        SetSprite();
        // * testing save/load
        DataSave();
    }
    protected bool IsActivated
    {
        get { return _state != _default; }
    }
    // * testing save/load
    // protected override void DataLoad()
    // {
    //     // Initialize();
    //     _state = _default;
    //     _locked = _valid != "";
    //     SetSprite();
    // }
    protected override void DataLoad()
    {
        // * testing save/load
        Vector3 position, rotation;
        bool isActive;
        // use defaults if no data found
        if (!GameData.DataLoadInteract(IDUnique, out position, out rotation, out isActive, out _lootID, out _state, out _locked))
        {
            // position rotation isActive already assigned correctly
            _state = _default;
            _locked = _valid != "";
        }
        // apply loaded values
        else
        {
            // ? unnecessary since these never move
            SetPosition(position);
            SetRotation(rotation);
            // ToggleActive(isActive);
            // if holding an item
            if (_lootID != "")
                // try retrieving item from manager ? null on fail
                _loot = ManagerItem.Instance.GetItemByIDUnique(_lootID);
        }
        // set visibility
        SetSprite();
        ToggleActive(isActive);
    }
    protected override void DataSave()
    {
        GameData.DataSaveInteract(IDUnique, Position, Rotation, IsActive, _lootID, _state, _locked);
    }
}