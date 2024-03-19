using UnityEngine;
// switch lever ?button ?pulley
public class InteractReact : Interact
{
    [Tooltip("Reference to react entity")] [SerializeField] protected React _target = null;
    [Tooltip("Target signal")] [SerializeField] protected int _id = 0;
    public override void TryAction(Creature source)
    {
        base.TryAction(source);
        // state changed
        // if (_state != _default) _target?.Ping(_id, _active, this);
        if (_state != _default)
        {
            // notify react
            if (_target) _target.Ping(_id, _state);
            // throw warning of unassigned react
            else Debug.Log(gameObject.name + "\t<color=red>No assigned react target</color>", this);
        }
    }
    // * testing switch sequence
    public void Clear()
    {
        _state = _default;
        // feedback_popup.Instance.RegisterMessage(transform, _valid ? "closed" : "off", game_variables.Instance.ColorInteract);
    }
}
