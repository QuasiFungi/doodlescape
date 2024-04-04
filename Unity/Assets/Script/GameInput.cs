using UnityEngine;
using UnityEngine.EventSystems;
public class GameInput : MonoBehaviour, IPointerDownHandler
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
        // - tick
        if (Input.GetKeyDown(KeyCode.Space)) onTap?.Invoke(direction);
    }
    // void OnEnable()
    // {
    //     GameClock.onTickUI += GetInput;
    // }
    // void OnDisable()
    // {
    //     GameClock.onTickUI -= GetInput;
    // }
    // private bool isInput = false;
    public Camera _camera;
    public Transform _player;
    // void Awake()
    // {
    //     _camera = GetComponent<Camera>();
    // }
    // private void GetInput()
    // {
    //     if (_pointerTap == Vector2.zero) return;
    //     // print("get input");
    //     // 
    //     Vector2Int index = GameGrid.Instance.WorldToIndex(_camera.ScreenToWorldPoint(_pointerTap));
    //     Vector3 direction = GameGrid.Instance.IndexToWorld(index.x, index.y) - _player.position;
    //     if (direction.magnitude < 2f) onTap?.Invoke(direction);
    //     print(direction);
    //     // 
    //     _pointerTap = Vector2.zero;
    // }
    // private Vector2 _pointerTap = Vector2.zero;
    public void OnPointerDown(PointerEventData eventData)
    {
        // print("input");
        // single finger input
        // if (_pointerTap != Vector2.zero) return;
        // _pointerTap = eventData.pressPosition;
        // 
        Vector2Int index = GameGrid.Instance.WorldToIndex(_camera.ScreenToWorldPoint(eventData.pressPosition));
        Vector3 direction = GameGrid.Instance.IndexToWorld(index.x, index.y) - _player.position;
        if (direction.magnitude < 2f) onTap?.Invoke(direction);
    }
    // public void OnPointerUp(PointerEventData eventData)
    // {
    //     // single finger input
    //     if (_pointerTap == Vector2.zero) return;
    //     _pointerTap = Vector2.zero;
    // }
}