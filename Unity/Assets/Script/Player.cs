using UnityEngine;
public class Player : Creature
{
    public delegate void OnAction(GameAction action);
    public static event OnAction onAction;
    protected override void Start()
    {
        base.Start();
        // 
        InventoryInitialize();
    }
    void OnEnable()
    {
        GameInput.onTap += ActionConstruct;
    }
    void OnDisable()
    {
        GameInput.onTap -= ActionConstruct;
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
}