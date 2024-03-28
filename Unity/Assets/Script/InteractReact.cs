using UnityEngine;
// switch lever ?button ?pulley
public class InteractReact : Interact
{
    // [Tooltip("Reference to react entity")] [SerializeField] protected React _target = null;
    [Tooltip("Signal that will be pinged")] [SerializeField] protected int _index = 0;
    public delegate void OnPing(int index, bool value);
    public event OnPing onPing;
    public override void TryAction(Creature source)
    {
        // reject action if one way and already active
        if (_oneWay && IsActivated) return;
        // process action
        base.TryAction(source);
        // state changed, notify reacts if exist
        // if (_state != _default) _target?.Ping(_id, _active, this);
        // if (_state != _default) onPing?.Invoke(_id, _state);
        // notify reacts of action attempt
        onPing?.Invoke(_index, _state);
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
}