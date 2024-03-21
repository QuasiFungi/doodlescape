using UnityEngine;
using System.Collections;
// destructible unlock toggle oneWay
// door
public class Interact : Entity
{
    // protected int _state;
    [Tooltip("Require item to unlock")] [SerializeField] private bool _locked = false;
    [Tooltip("Item that unlocks")] [SerializeField] protected string _valid;
    // used by chest and button/switch
    [Tooltip("Lock to state change")] [SerializeField] protected bool _oneWay = false;
    // ? make private along with state, have protected get property, use initialize function to set value
    [Tooltip("Initial state")] [SerializeField] private bool _default = false;
    protected bool _state;
    // ? different name maybe
    [Tooltip("Fade out on state change")] [SerializeField] private bool _disableOnChange = false;
    protected SpriteRenderer _sprite;
    private Sprite _spriteDefault;
    [Tooltip("Sprite for when state changes")] [SerializeField] private Sprite _spriteChanged = null;
    // ? also used by creatures, though some have multiple
    protected Collider2D _collider;
    void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        // ? copy vs reference
        _spriteDefault = _sprite.sprite;
    }
    // 
    protected virtual void Start()
    {
        // ? save file info
        _state = _default;
        gameObject.SetActive(true);
        // if (_sprite) _sprite.enabled = true; // ? chunk/fog
        SetSprite();
    }
    protected void SetSprite()
    {
        // fade out entity if required
        if (_disableOnChange && _state != _default)
        {
            // StartCoroutine(FadeOut());
            Deactivate();
            return;
        }
        // change sprite based on active state if possible
        if (_spriteChanged) _sprite.sprite = _state == _default ? _spriteDefault : _spriteChanged;
    }
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
    //     // hide entity
    //     Hide();
    // }
    // 
    public void Deactivate()
    {
        // allow player to walk over
        _collider.enabled = false;
        // appear like decal
        _sprite.color = new Color(.25f, .25f, .25f, 1f);
    }
    // 0 - fail | 1 - succeed | 2 - succeed with item
    public virtual void TryAction(Creature source)
    {
        // print("doh");
        if (_locked && source.ItemHas(_valid))
        {
            _locked = false;
            source.ItemRemove(_valid);
            // ? unlock vfx particle
        }
        // {
        //     feedback_popup.Instance.RegisterMessage(transform, "unlocked", game_variables.Instance.ColorInteract);
        //     return true;
        // }
        if (!_locked)
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
        }
        // // * testing
        // if (_valid)
        //     // feedback_popup.Instance.RegisterMessage(transform, "need " + _valid.gameObject.name, game_variables.Instance.ColorInteract);
        //     feedback_toaster.Instance.RegisterMessage(gameObject.name + " : need " + _valid.gameObject.name, game_variables.Instance.ColorInteract);
        // else
        //     // feedback_popup.Instance.RegisterMessage(transform, "jammed", game_variables.Instance.ColorInteract);
        //     feedback_toaster.Instance.RegisterMessage(gameObject.name + " : jammed", game_variables.Instance.ColorInteract);
        SetSprite();
    }
    // ? doesnt consider lock
    public void Initialize()
    {
        // 
        _state = _default;
        SetSprite();
    }
    protected bool IsActivated
    {
        get { return _state != _default; }
    }
}