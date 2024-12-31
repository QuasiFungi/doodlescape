using UnityEngine;
// switch lever ?button ?pulley
public class InteractReact : Interact
{
    [Header("React")]
    // [Tooltip("Reference to react entity")] [SerializeField] protected React _target = null;
    [Tooltip("Signal that will be pinged")] [SerializeField] protected int _index = 0;
    public delegate void OnPing(int index, bool value);
    public event OnPing onPing;
    public override void TryAction(Creature source)
    {
        // reject action if one way and already active
        if (_oneWay && IsActivated)
        {
            // ? custom message
            Teleprompter.Register("It's already active");
            // * testing sfx fail
            GameAudio.Instance.Register(8, GameAudio.AudioType.ENTITY);
            // show activation attempt
            StartCoroutine("Flash");
            // abort
            return;
        }
        _isWait = true;
        // process action
        base.TryAction(source);
        // state changed, notify reacts if exist
        // if (_state != _default) _target?.Ping(_id, _active, this);
        // if (_state != _default) onPing?.Invoke(_id, _state);
        // notify reacts of action attempt
        onPing?.Invoke(_index, _state);
        // * testing sfx success
        if (_isWait && gameObject.activeSelf)
        {
            GameAudio.Instance.Register(9, GameAudio.AudioType.ENTITY);
            // 
            _isWait = false;
        }
    }
    // // * testing switch sequence
    // public void Initialize()
    // {
    //     _state = _default;
    //     // feedback_popup.Instance.RegisterMessage(transform, _valid ? "closed" : "off", game_variables.Instance.ColorInteract);
    // }
    // // * testing
    // void OnDrawGizmosSelected()
    // {
    //     if (_target)
    //     {
    //         // Draws a blue line from this transform to the target
    //         Gizmos.color = Color.yellow;
    //         Gizmos.DrawLine(transform.position, _target.transform.position);
    //     }
    // }
    // public void Reactivate()
    // {
    //     // 
    // }
    // // * testing save/load, need to rewrite parent load function to add ping functionality ? ew duplicate code
    // protected override void DataLoad()
    // {
    //     // * testing save/load
    //     Vector3 position, rotation;
    //     bool isActive;
    //     // use defaults if no data found
    //     if (!GameData.DataLoadInteract(IDUnique, out position, out rotation, out isActive, out _state, out _locked))
    //     {
    //         // position rotation isActive already assigned correctly
    //         _state = _default;
    //         _locked = _valid != "";
    //     }
    //     // apply loaded values
    //     else
    //     {
    //         // ? unnecessary since these never move
    //         SetPosition(position);
    //         SetRotation(rotation);
    //         // ToggleActive(isActive);
    //         // 
    //         // notify reacts of restored state
    //         if (_oneWay && IsActivated) onPing?.Invoke(_index, _state);
    //     }
    //     // set visibility
    //     SetSprite();
    //     ToggleActive(isActive);
    // }
}