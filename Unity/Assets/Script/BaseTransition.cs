using UnityEngine;
public class BaseTransition : MonoBehaviour
{
    public delegate void OnTrigger(Vector2Int direction, bool state);
    public static event OnTrigger onTrigger;
    // public Vector2Int _rooms = Vector2Int.zero;
    public Direction direction = Direction.UP;
    public enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // auto detection off
        if (!_state) return;
        // print("detected");
        if (GameVariables.IsPlayer(other.gameObject))
        {
            // print("trigger enter: " + direction);
            switch (direction)
            {
                case Direction.UP:
                    onTrigger?.Invoke(Vector2Int.up, true);
                    break;
                case Direction.DOWN:
                    onTrigger?.Invoke(Vector2Int.down, true);
                    break;
                case Direction.LEFT:
                    onTrigger?.Invoke(Vector2Int.left, true);
                    break;
                case Direction.RIGHT:
                    onTrigger?.Invoke(Vector2Int.right, true);
                    break;
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        // auto detection off
        if (!_state) return;
        // 
        if (GameVariables.IsPlayer(other.gameObject))
        {
            // print("trigger exit: " + direction);
            onTrigger?.Invoke(Vector2Int.zero, false);
        }
    }
    // void OnEnable()
    // {
    //     // GameAction.OnTransition += 
    // }
    private Collider2D _trigger;
    void Awake()
    {
        _trigger = GetComponent<Collider2D>();
    }
    private bool _state = true;
    public void ToggleActive(bool state)
    {
        // _state = state;
        // print("trigger " + direction + ": " + _state);
        // 
        // disable auto detection
        if (!state) _state = false;
        // enable auto detection
        else
        {
            // disable triggers
            _trigger.enabled = false;
            // enable auto detection
            _state = true;
            // refire trigger events
            _trigger.enabled = true;
        }
    }
}