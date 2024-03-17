using UnityEngine;
public class GameInput : MonoBehaviour
{
    public delegate void OnTap(Vector2 position);
    public static event OnTap onTap;
    void Update()
    {
        Vector2 direction = Vector2.zero;
        // testing w/ keyboard
        // - movement
        if (Input.GetKeyDown(KeyCode.UpArrow)) direction += Vector2.up;
        if (Input.GetKeyDown(KeyCode.DownArrow)) direction += Vector2.down;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) direction += Vector2.left;
        if (Input.GetKeyDown(KeyCode.RightArrow)) direction += Vector2.right;
        if (direction.sqrMagnitude > 0f) onTap?.Invoke(direction);
        // - inventory
        // if (Input.GetKeyDown(KeyCode.Alpha1)) 
        // if (Input.GetKeyDown(KeyCode.Alpha2)) 
        // if (Input.GetKeyDown(KeyCode.Alpha3)) 
        // if (Input.GetKeyDown(KeyCode.Alpha4)) 
        // if (Input.GetKeyDown(KeyCode.Alpha5)) 
    }
}