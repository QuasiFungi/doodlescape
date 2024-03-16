using UnityEngine;
public class GameAction
{
    public enum ActionType
    {
        NULL,
        WALK,
        INTERACT,
        ATTACK,
        PICKUP
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
        if (!_target)
        {
            // ? inventory check
            if (distance == 1f) _type = ActionType.WALK;
            else _type = ActionType.NULL;
        }
        // Breakable
        else if (layer == 5)
        {
            // ? inventory check
            if (distance == 1f) _type = ActionType.ATTACK;
            else _type = ActionType.NULL;
        }
        // Interact
        else if (layer == 6)
        {
            // ? inventory check
            if (distance == 1f) _type = ActionType.INTERACT;
            else _type = ActionType.NULL;
        }
        // Creature
        else if (layer == 7)
        {
            // ? inventory check
            if (distance == 1f) _type = ActionType.ATTACK;
            else _type = ActionType.NULL;
        }
        // Item
        else if (layer == 8)
        {
            // ? inventory check
            if (distance == 1f) _type = ActionType.PICKUP;
            else _type = ActionType.NULL;
        }
        else _type = ActionType.NULL;
    }
    public void Process()
    {
        switch (_type)
        {
            case ActionType.WALK:
                _source.transform.position = _position;
                break;
            case ActionType.ATTACK:
                // * testing
                _target.GetComponent<Breakable>().HealthModify(-1);
                break;
            case ActionType.INTERACT:
                break;
            case ActionType.PICKUP:
                break;
        }
    }
    public GameObject Source
    {
        get { return _source; }
    }
}