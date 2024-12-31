using UnityEngine;
public class Player : Creature
{
    public delegate void OnAction(GameAction action);
    public static event OnAction onAction;
    // protected override void Start()
    // {
    //     InventoryInitialize();
    //     // 
    //     base.Start();
    // }
    // have singleton since only one player ever
    public static Player Instance;
    protected override void Awake()
    {
        base.Awake();
        // discard old copy
        if (Instance) Destroy(Instance.gameObject);
        Instance = this;
    }
    // protected void OnEnable()
    protected override void OnEnable()
    {
        base.OnEnable();
        // 
        GameInput.onTap += ActionConstruct;
        // * testing save/load
        // since player not tracked by chunks, done before other entities
        GameMaster.onTransitionBegin += DataSave;
    }
    // protected void OnDisable()
    protected override void OnDisable()
    {
        base.OnDisable();
        // 
        GameInput.onTap -= ActionConstruct;
        // * testing save/load
        // since player not tracked by chunks, done before other entities
        GameMaster.onTransitionBegin -= DataSave;
    }
    // * testing save/load, wrapper for room transition event ? inelegant
    private void DataSave(Vector2Int room)
    {
        DataSave();
    }
    // private void ActionConstruct(int typeButton, int typeInput, Vector2 position)
    // private void ActionConstruct(int typeButton, int typeInput, Vector2 direction)
    private void ActionConstruct(int typeButton, int typeInput, int index)
    {
        // GameAction playerAction = new GameAction();
        // action
        // if (typeButton == 0) playerAction = new GameAction(GameGrid.Instance.WorldToGrid(transform.position + (Vector3)position), gameObject);
        // if (typeButton == 0) playerAction = new GameAction(typeButton, direction, gameObject);
        // inventory
        // - which item slot
        // - use or drop
        // else if (typeButton == 1) playerAction = new GameAction(GameGrid.Instance.WorldToGrid(transform.position + (Vector3)position), gameObject);
        // else if (typeButton == 1) playerAction = new GameAction(direction, gameObject);
        // else playerAction = new GameAction();
        // // cancel
        // else if (typeButton == 2) playerAction = new GameAction(GameGrid.Instance.WorldToGrid(transform.position + (Vector3)position), gameObject);
        // GameAction playerAction = new GameAction(typeButton, typeInput, direction, gameObject);
        GameAction playerAction = new GameAction(typeButton, typeInput, index, gameObject);
        // only carry event if action is valid
        if (playerAction.IsValid) onAction?.Invoke(playerAction);
    }
    // // * testing game clear
    // public delegate void OnGeneric();
    // // public static event OnGeneric onDead;
    // public static event OnGeneric onClear;
    // // 
    // private Vector2Int _roomClear = new Vector2Int(0, -10);
    // protected override void DataSave()
    // {
    //     base.DataSave();
    //     // 
    //     // if (IsDead) onDead?.Invoke();
    //     if (GameData.Room == _roomClear) onClear?.Invoke();
    // }
}