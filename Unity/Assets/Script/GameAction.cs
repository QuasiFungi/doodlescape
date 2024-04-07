using UnityEngine;
public class GameAction
{
    // ? assign number to typecast from layer directly
    public enum ActionType
    {
        NULL,
        WALK,
        INTERACT,
        ATTACK,
        PICKUP
        // , DROP
        // , USE
    }
    private ActionType _type;
    private Vector2 _position;
    private GameObject _target;
    private GameObject _source;
    public GameAction(Vector2 position, GameObject source)
    {
        float distance = Vector2.Distance(position, source.transform.position);
        // 
        if (distance >= 2f)
        {
            _type = ActionType.NULL;
            return;
        }
        // 
        _position = position;
        _source = source;
        int layer = GameNavigation.GetTileAtPosition(_position, out _target);
        // Empty
        if (_source.GetComponent<Creature>().ItemHas("item_feather") || distance == 1f)
        {
            // ? inventory check
            if (!_target) _type = ActionType.WALK;
            // Breakable
            else if (layer == 5) _type = ActionType.ATTACK;
            // Interact
            else if (layer == 6) _type = ActionType.INTERACT;
            // Creature
            else if (layer == 7) _type = ActionType.ATTACK;
            // Item
            else if (layer == 8) _type = ActionType.PICKUP;
        }
        else _type = ActionType.NULL;
    }
    public void Process()
    {
        switch (_type)
        {
            case ActionType.WALK:
                // // * testing
                // // look in direction to move
                // _source.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(_position.y - _source.transform.position.y, _position.x - _source.transform.position.x) * Mathf.Rad2Deg - 90f);
                // ? lerp to position over time, diagonal movement feels jarring
                // _source.transform.position = _position;
                // StartCoroutine("LerpPosition");
                _source.GetComponent<Creature>().Move(_position);
                break;
            case ActionType.ATTACK:
                // * testing
                // ? source position not necessarily adjacent to target for damage direction calculation
                _target.GetComponent<Breakable>().HealthModify(-1, _source.GetComponent<Creature>());
                break;
            case ActionType.INTERACT:
                _target.GetComponent<Interact>().TryAction(_source.GetComponent<Creature>());
                break;
            case ActionType.PICKUP:
                _source.GetComponent<Creature>().ItemAdd(_target.GetComponent<Item>());
                break;
        }
    }
    // IEnumerator LerpPosition()
    // {
    //     // record start position
    //     Vector3 start = _source.transform.position;
    //     // lerp counter
    //     float lerp = 0f;
    //     // not at target position
    //     while (Vector3.Distance(start, _position) > 0f)
    //     {
    //         // advance lerp timer
    //         lerp += Time.deltaTime;
    //         // update position offset for this frame
    //         _source.transform.position = Vector3.Lerp(start, _position, lerp);
    //         // wait till next frame
    //         yield return null;
    //     }
    // }
    public GameObject Source
    {
        get { return _source; }
    }
    public bool IsValid
    {
        get { return _type != ActionType.NULL; }
    }
    public Vector2 Position
    {
        get { return _position; }
    }
}