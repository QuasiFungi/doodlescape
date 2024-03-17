using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
// destructible unlock toggle oneWay
// door
public class Interact : Entity
{
    // protected int _state;
    // public bool _testDisable;
    [Tooltip("Item that unlocks")] [SerializeField] protected string _valid;
    [SerializeField] protected bool _locked = false;
    [Tooltip("Lock to state change")] [SerializeField] protected bool _oneWay = false;
    [Tooltip("Initial state")] [SerializeField] protected bool _default = false;
    protected bool _active;
    protected SpriteRenderer _sprite;
    [Tooltip("Sprite pool")] [SerializeField] protected List<Sprite> _sprites = new List<Sprite>();
    void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        // ?
        _active = _default;
        gameObject.SetActive(true);
        if (_sprite) _sprite.enabled = true;
        SetSprite();
    }
    // void Update()
    // {
    //     if (_testDisable && _active)
    //         gameObject.SetActive(false);
    //     // * testing
    //     // _sprite.enabled = Vector3.Distance(transform.position, controller_player.Instance.Motor.Position) <= game_variables.Instance.RadiusSprite;
    //     // _sprite.enabled =  game_camera.Instance.InView(transform.position);
    //     SetSprite();
    // }
    protected void SetSprite()
    {
        if (_sprites.Count < 2)
            return;
        _sprite.sprite = _sprites[_active ? 1 : 0];
    }
    // 0 - fail | 1 - succeed | 2 - succeed with item
    public virtual void TryAction(Creature target)
    {
        // print("doh");
        // int check = 0;
        if (_locked && target.ItemHas(_valid))
        {
            _locked = false;
            // check++;
            target.ItemRemove(_valid);
        }
        // {
        //     feedback_popup.Instance.RegisterMessage(transform, "unlocked", game_variables.Instance.ColorInteract);
        //     return true;
        // }
        if (!_locked)
        {
            _active = _oneWay ? true : !_active;
            // check++;
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
            // return check;
        }
        // // * testing
        // if (_valid)
        //     // feedback_popup.Instance.RegisterMessage(transform, "need " + _valid.gameObject.name, game_variables.Instance.ColorInteract);
        //     feedback_toaster.Instance.RegisterMessage(gameObject.name + " : need " + _valid.gameObject.name, game_variables.Instance.ColorInteract);
        // else
        //     // feedback_popup.Instance.RegisterMessage(transform, "jammed", game_variables.Instance.ColorInteract);
        //     feedback_toaster.Instance.RegisterMessage(gameObject.name + " : jammed", game_variables.Instance.ColorInteract);
        SetSprite();
        // return check;
    }
}