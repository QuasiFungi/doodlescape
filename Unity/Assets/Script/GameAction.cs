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
    // ? store reference as creature instead of gameObject
    private GameObject _source;
    private int _typeButton;
    private int _index;
    public GameAction()
    {
        _type = ActionType.NULL;
    }
    public GameAction(int typeButton, int typeInput, int index, GameObject source)
    {
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
                // action allowed
                if (_source.GetComponent<Creature>().ItemHas("item_feather") || _direction.magnitude == 1f)
                {
                    // direction to position
                    _position = GameGrid.Instance.WorldToGrid(source.transform.position + _direction);
                    // tile at target position
                    int layer = GameNavigation.GetTileAtPosition(_position, out _target);
                    // Transition
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
                // // invalid action
                // else _type = ActionType.NULL;
                break;
            // INVENTORY
            case 1:
                // slot holds item, is valid slot
                if (_source.GetComponent<Creature>().ItemGet(_index) && (_source.GetComponent<Creature>().ItemHas("item_pouch") || _index < 4))
                {
                    // TAP
                    if (typeInput == 0) _type = ActionType.USE;
                    // HOLD
                    else _type = ActionType.DROP;
                }
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
        Creature creature = _source.GetComponent<Creature>();
        // parse action based on type
        switch (_type)
        {
            case ActionType.WALK:
                // 
                creature.Move(_position);
                break;
            case ActionType.ATTACK:
                // turn creature to face action direction
                creature.SetRotation(Direction_Int);
                // * testing
                // ? source position not necessarily adjacent to target for damage direction calculation
                _target.GetComponent<Breakable>().HealthModify(-1, _source.GetComponent<Creature>());
                break;
            case ActionType.INTERACT:
                // turn creature to face action direction
                creature.SetRotation(Direction_Int);
                // 
                _target.GetComponent<Interact>().TryAction(_source.GetComponent<Creature>());
                break;
            case ActionType.PICKUP:
                // turn creature to face action direction
                creature.SetRotation(Direction_Int);
                // logic offloaded because inventory info held by creature
                if (!creature.ItemAdd(_target.GetComponent<Item>()))
                // inform cant pickup item
                    Teleprompter.Register("Inventory full");
                // // 
                // else Teleprompter.Register(creature.ItemGet(_index).Description);
                break;
            case ActionType.TRANSITION:
                // 
                onTransition?.Invoke();
                break;
            case ActionType.USE:
                // // * testing
                // if (creature.ItemGet(_index)) Debug.Log("try use: " + creature.ItemGet(_index).ID);
                Item item = creature.ItemGet(_index);
                // parse by item category
                switch (item.Type)
                {
                    case Item.ItemType.SUPPORT:
                        // show item info/usage
                        Teleprompter.Register(creature.ItemGet(_index).Description);
                        break;
                    case Item.ItemType.AOE:
                        // ? teleprompt item info/usage
                        Teleprompter.Register("try use: " + creature.ItemGet(_index).ID);
                        break;
                    case Item.ItemType.WEAPON:
                        // ? teleprompt item info/usage
                        Teleprompter.Register("try use: " + creature.ItemGet(_index).ID);
                        break;
                    case Item.ItemType.COLLECTABLE:
                        // ? teleprompt item info/usage
                        Teleprompter.Register("try use: " + creature.ItemGet(_index).ID);
                        break;
                    case Item.ItemType.UTILITY:
                        // ? teleprompt item info/usage
                        Teleprompter.Register("try use: " + creature.ItemGet(_index).ID);
                        break;
                    case Item.ItemType.RECOVERY:
                        // 
                        // creature.HealthModify((item as ItemHeal).Consume(), creature);
                        // 
                        // creature.HealthModify((item as ItemHeal).Health, creature);
                        // (item as ItemHeal).Consume(););
                        // 
                        // create local reference
                        ItemHeal heal = item as ItemHeal;
                        // apply health modifier
                        creature.HealthModify(heal.Health, creature);
                        // fully consumed
                        if (heal.Consume())
                        {
                            // inform of discard
                            // Teleprompter.Register(creature.ItemGet(_index).Name + " uses depleted; discarded");
                            Teleprompter.Register(creature.ItemGet(_index).Consumed);
                            // discard
                            creature.ItemRemove(item.ID);
                        }
                        break;
                }
                break;
            case ActionType.DROP:
                // verify/get valid empty tile for item drop
                // F > L > R > B > FL > FR > BL > BR
                if (IsDirectionClear(new Vector3(0f, 1f)) || IsDirectionClear(new Vector3(-1f, 0f)) || IsDirectionClear(new Vector3(1f, 0f)) || IsDirectionClear(new Vector3(0f, -1f)) || 
                    IsDirectionClear(new Vector3(-1f, 1f)) || IsDirectionClear(new Vector3(1f, 1f)) || IsDirectionClear(new Vector3(-1f, -1f)) || IsDirectionClear(new Vector3(1f, -1f)))
                    creature.ItemDrop(_index, _position);
                // 
                // else Debug.Log(_source.name + ":\tNo empty space to drop item in");
                else Teleprompter.Register("No space to drop item");
                break;
        }
    }
    // check if tile in given direction occupied
    private bool IsDirectionClear(Vector3 direction)
    {
        // direction local to world
        _direction = _source.transform.TransformDirection(direction);
        // direction to position
        _position = GameGrid.Instance.WorldToGrid(_source.transform.position + _direction);
        // tile at target position
        GameNavigation.GetTileAtPosition(_position, out _target);
        // empty check
        return !_target;
    }
    public delegate void OnTransition();
    public static event OnTransition onTransition;
    public GameObject Source
    {
        get { return _source; }
    }
    public bool IsValid
    {
        get { return _type != ActionType.NULL; }
    }
    public ActionType Type
    {
        get { return _type; }
    }
    public int TypeButton
    {
        get { return _typeButton; }
    }
    public int Index
    {
        get { return _index; }
    }
    private Vector2Int Direction_Int
    {
        get { return new Vector2Int(Mathf.FloorToInt(_direction.x), Mathf.FloorToInt(_direction.y)); }
    }
}