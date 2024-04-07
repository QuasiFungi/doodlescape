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
    private void ActionConstruct(Vector2 position)
    {
        GameAction playerAction = new GameAction(GameGrid.Instance.WorldToGrid(transform.position + (Vector3)position), gameObject);
        // only carry event if action is valid
        if (playerAction.IsValid) onAction?.Invoke(playerAction);
    }
}