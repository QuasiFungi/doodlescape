using UnityEngine;
public class GameInput : MonoBehaviour
{
    public delegate void OnTap(Vector2 position);
    public static event OnTap onTap;
    void Update()
    {
        // testing w/ keyboard
        if (Input.GetKeyDown("up")) onTap?.Invoke(Vector2.up);
        else if (Input.GetKeyDown("down")) onTap?.Invoke(Vector2.down);
        else if (Input.GetKeyDown("left")) onTap?.Invoke(Vector2.left);
        else if (Input.GetKeyDown("right")) onTap?.Invoke(Vector2.right);
    }
}