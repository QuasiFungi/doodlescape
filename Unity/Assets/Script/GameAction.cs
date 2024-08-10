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
        PICKUP,
        TRANSITION,
        USE,
        CANCEL,
        DROP
        // , DASH
        // , BLOCK
    }
    private ActionType _type;
    private Vector3 _direction;
    private Vector3 _position;
    private GameObject _target;
    private GameObject _source;
    private int _typeButton;
    private int _index;
    public GameAction()
    {
        _type = ActionType.NULL;
    }
    // public GameAction(Vector2 position, GameObject source)
    // public GameAction(int typeButton, int typeInput, Vector2 direction, GameObject source)
    public GameAction(int typeButton, int typeInput, int index, GameObject source)
    {
        // float distance = Vector2.Distance(position, source.transform.position);
        // float distance = direction.magnitude;
        // // 
        // if (distance >= 2f)
        // {
        //     _type = ActionType.NULL;
        //     return;
        // }
        // 
        // _position = position;
        _index = index;
        _source = source;
        _typeButton = typeButton;
        switch (typeButton)
        {
            // ACTION
            case 0:
                // action input layout, index to direction
                switch (_index)
                {
                    case 0:
                        // UL
                        _direction = new Vector3(-1f, 1f);
                        break;
                    case 1:
                        // U
                        _direction = new Vector3(0f, 1f);
                        break;
                    case 2:
                        // UR
                        _direction = new Vector3(1f, 1f);
                        break;
                    case 3:
                        // L
                        _direction = new Vector3(-1f, 0f);
                        break;
                    case 4:
                        // R
                        _direction = new Vector3(1f, 0f);
                        break;
                    case 5:
                        // DL
                        _direction = new Vector3(-1f, -1f);
                        break;
                    case 6:
                        // D
                        _direction = new Vector3(0f, -1f);
                        break;
                    case 7:
                        // DR
                        _direction = new Vector3(1f, -1f);
                        break;
                    default:
                        // 
                        _direction = Vector3.zero;
                        break;
                }
                // direction to position
                _position = GameGrid.Instance.WorldToGrid(source.transform.position + _direction);
                // tile at target position
                int layer = GameNavigation.GetTileAtPosition(_position, out _target);
                // int layer = GameNavigation.GetTileAtPosition(GameGrid.Instance.WorldToGrid(source.transform.position + (Vector3)direction), out _target);
                // action allowed
                // if (_source.GetComponent<Creature>().ItemHas("item_feather") || distance == 1f)
                // if (_source.GetComponent<Creature>().ItemHas("item_feather") || direction.magnitude == 1f)
                if (_source.GetComponent<Creature>().ItemHas("item_feather") || _direction.magnitude == 1f)
                {
                    // ? inventory check
                    // Transition
                    // if (GameMaster.IsTransition(direction)) _type = ActionType.TRANSITION;
                    if (GameMaster.IsTransition(_direction)) _type = ActionType.TRANSITION;
                    // Walk
                    else if (!_target) _type = ActionType.WALK;
                    // Breakable
                    else if (layer == 5) _type = ActionType.ATTACK;
                    // Interact
                    else if (layer == 6) _type = ActionType.INTERACT;
                    // Creature
                    else if (layer == 7) _type = ActionType.ATTACK;
                    // Item
                    else if (layer == 8) _type = ActionType.PICKUP;
                }
                // invalid action
                else _type = ActionType.NULL;
                break;
            // INVENTORY
            case 1:
                // // inventory input layout
                // switch (_index)
                // {
                //     case 4:
                //         // UL
                //         _direction = new Vector3(-1f, 1f);
                //         break;
                //     case 0:
                //         // U
                //         _direction = new Vector3(0f, 1f);
                //         break;
                //     case 5:
                //         // UR
                //         _direction = new Vector3(1f, 1f);
                //         break;
                //     case 1:
                //         // L
                //         _direction = new Vector3(-1f, 0f);
                //         break;
                //     case 2:
                //         // R
                //         _direction = new Vector3(1f, 0f);
                //         break;
                //     case 6:
                //         // DL
                //         _direction = new Vector3(-1f, -1f);
                //         break;
                //     case 3:
                //         // D
                //         _direction = new Vector3(0f, -1f);
                //         break;
                //     case 7:
                //         // DR
                //         _direction = new Vector3(1f, -1f);
                //         break;
                //     default:
                //         // 
                //         _direction = Vector3.zero;
                //         break;
                // }
                // _position = GameGrid.Instance.WorldToGrid(source.transform.position + _direction);
                // 
                // slot holds item, is valid slot
                // if (_source.GetComponent<Creature>().ItemHas("item_pouch") || direction.magnitude == 1f) _type = ActionType.USE;
                // if (_source.GetComponent<Creature>().ItemHas("item_pouch") || _direction.magnitude == 1f) _type = ActionType.USE;
                if (_source.GetComponent<Creature>().ItemGet(_index) && (_source.GetComponent<Creature>().ItemHas("item_pouch") || index < 4))
                {
                    // TAP
                    if (typeInput == 0) _type = ActionType.USE;
                    // HOLD
                    else _type = ActionType.DROP;
                }
                // // Invalid
                // else _type = ActionType.NULL;
                break;
            // CANCEL
            case 2:
                _type = ActionType.CANCEL;
                break;
        }
    }
    public void Process()
    {
        // * testing ? cant redefine in parallel case for whatever reason
        Creature temp;
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
                // _source.GetComponent<Creature>().Move(GameGrid.Instance.WorldToGrid(source.transform.position + (Vector3)position));
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
            case ActionType.TRANSITION:
                // Debug.Log("transition");
                // GameMaster.DoTransition();
                // ? case for diagonal
                onTransition?.Invoke();
                // onTransition?.Invoke(Direction);
                // onTransition?.Invoke((_position + Vector2.one * 5f) / 9f);
                break;
            case ActionType.USE:
                // 
                // int index = -1;
                // // inventory button pressed
                // if (typeButton == 1)
                // {
                //     // direction to index
                //     if (_direction.x > 0f)
                //     {
                //         if (_direction.y > 0f)
                //         {
                //             // UR
                //             index = 5;
                //         }
                //         else if (_direction.y < 0f)
                //         {
                //             // DR
                //             index = 7;
                //         }
                //         else
                //         {
                //             // R
                //             index = 2;
                //         }
                //     }
                //     else if (_direction.x < 0f)
                //     {
                //         if (_direction.y > 0f)
                //         {
                //             // UL
                //             index = 4;
                //         }
                //         else if (_direction.y < 0f)
                //         {
                //             // DL
                //             index = 6;
                //         }
                //         else
                //         {
                //             // L
                //             index = 1;
                //         }
                //     }
                //     else
                //     {
                //         if (_direction.y > 0f)
                //         {
                //             // U
                //             index = 0;
                //         }
                //         else if (_direction.y < 0f)
                //         {
                //             // D
                //             index = 3;
                //         }
                //         else
                //         {
                //             // invalid
                //         }
                //     }
                // }
                temp = _source.GetComponent<Creature>();
                if (temp.ItemGet(_index)) Debug.Log("try use: " + temp.ItemGet(_index).ID);
                break;
            case ActionType.DROP:
                // temp = _source.GetComponent<Creature>();
                // if (temp.ItemGet(_index)) Debug.Log("try drop: " + temp.ItemGet(_index).ID);
                // 
                _source.GetComponent<Creature>().ItemDrop(_index);
                break;
        }
    }
    // public delegate void OnTransition(Vector2 room);
    public delegate void OnTransition();
    public static event OnTransition onTransition;
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
    // public Vector2 Position
    // {
    //     get { return _position; }
    // }
    public ActionType Type
    {
        get { return _type; }
    }
    // public Vector2 Direction
    // {
    //     // get { return _position - (Vector2)_source.transform.position; }
    //     get { return _direction; }
    // }
    public int TypeButton
    {
        get { return _typeButton; }
    }
    public int Index
    {
        get { return _index; }
    }
}