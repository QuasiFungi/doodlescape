using UnityEngine;
using UnityEngine.EventSystems;
// public class GameInput : MonoBehaviour, IPointerDownHandler
public class GameInput : MonoBehaviour
{
    // public delegate void OnTap(int typeButton, int typeInput, Vector2 position);
    // public delegate void OnTap(int typeButton, int typeInput, Vector2 direction);
    public delegate void OnTap(int typeButton, int typeInput, int index);
    public static event OnTap onTap;
    void Update()
    {
        Vector2 direction = Vector2.zero;
        // testing w/ keyboard
        // - movement
        // if (Input.GetKeyDown(KeyCode.UpArrow)) direction += Vector2.up;
        // if (Input.GetKeyDown(KeyCode.DownArrow)) direction += Vector2.down;
        // if (Input.GetKeyDown(KeyCode.LeftArrow)) direction += Vector2.left;
        // if (Input.GetKeyDown(KeyCode.RightArrow)) direction += Vector2.right;
        // if (direction.sqrMagnitude > 0f) onTap?.Invoke(0, 0, direction);
        // UL
        if (Input.GetKeyDown(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.LeftArrow)) onTap?.Invoke(0, 0, 0);
        // UR
        else if (Input.GetKeyDown(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.RightArrow)) onTap?.Invoke(0, 0, 2);
        // DL
        else if (Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.LeftArrow)) onTap?.Invoke(0, 0, 5);
        // DR
        else if (Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.RightArrow)) onTap?.Invoke(0, 0, 7);
        // U
        else if (Input.GetKeyDown(KeyCode.UpArrow)) onTap?.Invoke(0, 0, 1);
        // D
        else if (Input.GetKeyDown(KeyCode.DownArrow)) onTap?.Invoke(0, 0, 6);
        // L
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) onTap?.Invoke(0, 0, 3);
        // R
        else if (Input.GetKeyDown(KeyCode.RightArrow)) onTap?.Invoke(0, 0, 4);
        // - inventory
        // if (Input.GetKeyDown(KeyCode.Alpha1)) 
        // if (Input.GetKeyDown(KeyCode.Alpha2)) 
        // if (Input.GetKeyDown(KeyCode.Alpha3)) 
        // if (Input.GetKeyDown(KeyCode.Alpha4)) 
        // if (Input.GetKeyDown(KeyCode.Alpha5)) 
        // UL
        if (Input.GetKeyDown(KeyCode.W) && Input.GetKeyDown(KeyCode.A)) onTap?.Invoke(1, 0, 4);
        // UR
        else if (Input.GetKeyDown(KeyCode.W) && Input.GetKeyDown(KeyCode.D)) onTap?.Invoke(1, 0, 5);
        // DL
        else if (Input.GetKeyDown(KeyCode.S) && Input.GetKeyDown(KeyCode.A)) onTap?.Invoke(1, 0, 6);
        // DR
        else if (Input.GetKeyDown(KeyCode.S) && Input.GetKeyDown(KeyCode.D)) onTap?.Invoke(1, 0, 7);
        // U
        else if (Input.GetKeyDown(KeyCode.W)) onTap?.Invoke(1, 0, 0);
        // D
        else if (Input.GetKeyDown(KeyCode.S)) onTap?.Invoke(1, 0, 3);
        // L
        else if (Input.GetKeyDown(KeyCode.A)) onTap?.Invoke(1, 0, 1);
        // R
        else if (Input.GetKeyDown(KeyCode.D)) onTap?.Invoke(1, 0, 2);
        // - cancel
        if (Input.GetKeyDown(KeyCode.RightShift)) onTap?.Invoke(2, 0, 0);
        // - tick
        // if (Input.GetKeyDown(KeyCode.Space)) onTap?.Invoke(0, 0, direction);
        if (Input.GetKeyDown(KeyCode.LeftShift)) onTap?.Invoke(0, 0, 8);
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
    // * testing
    public Camera _camera;
    public Transform _player;
    // ? versus each button look for game input
    // public Transform _move;
    // public Transform _storage;
    // void Awake()
    // {
    //     // action
    //     // foreach (Transform child in transform.GetChild(0))
    //     //     child.GetComponent<BaseButton>().onInteract += OnMove;
    //     // // storage
    //     // transform.GetChild(1);
    // }
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
    // public void OnPointerDown(PointerEventData eventData)
    // {
    //     // print("input");
    //     // single finger input
    //     // if (_pointerTap != Vector2.zero) return;
    //     // _pointerTap = eventData.pressPosition;
    //     // 
    //     // Vector2Int index = GameGrid.Instance.WorldToIndex(_camera.ScreenToWorldPoint(eventData.pressPosition));
    //     // Vector3 direction = GameGrid.Instance.IndexToWorld(index.x, index.y) - _player.position;
    //     Vector3 direction = GameGrid.Instance.WorldToGrid(_camera.ScreenToWorldPoint(eventData.pressPosition)) - _player.position;
    //     // undo grid z offset
    //     direction = new Vector3(direction.x, direction.y, 0f);
    //     // player actions
    //     if (direction.magnitude < 2f) onTap?.Invoke(direction);
    //     // ? inventory
    // }
    // public void OnPointerUp(PointerEventData eventData)
    // {
    //     // single finger input
    //     if (_pointerTap == Vector2.zero) return;
    //     _pointerTap = Vector2.zero;
    // }
    // public void OnMove(BaseEventData eventData)
    // {
    //     PointerEventData data = eventData as PointerEventData;
    //     print(data.position);
    // }
    void OnEnable()
    {
        BaseButton.onInteract += OnAction;
        // BaseTransition.onTrigger += OverrideAction;
    }
    void OnDisable()
    {
        BaseButton.onInteract -= OnAction;
        // BaseTransition.onTrigger -= OverrideAction;
    }
    // pipeline between buttons and player
    private void OnAction(int typeButton, int typeInput, int id)
    {
        // print(type + " " + id + " " + state);
        // switch (typeButton)
        // {
        //     // MOVE
        //     case 0:
        //         switch (typeInput)
        //         {
        //             // TAP
        //             case 0:
        //                 // switch (id)
        //                 // {
        //                 //     case 0:
        //                 //         onTap?.Invoke(0, 0, new Vector2(-1f, 1f));
        //                 //         break;
        //                 //     case 1:
        //                 //         onTap?.Invoke(0, 0, new Vector2(0f, 1f));
        //                 //         break;
        //                 //     case 2:
        //                 //         onTap?.Invoke(0, 0, new Vector2(1f, 1f));
        //                 //         break;
        //                 //     case 3:
        //                 //         onTap?.Invoke(0, 0, new Vector2(-1f, 0f));
        //                 //         break;
        //                 //     case 4:
        //                 //         onTap?.Invoke(0, 0, new Vector2(1f, 0f));
        //                 //         break;
        //                 //     case 5:
        //                 //         onTap?.Invoke(0, 0, new Vector2(-1f, -1f));
        //                 //         break;
        //                 //     case 6:
        //                 //         onTap?.Invoke(0, 0, new Vector2(0f, -1f));
        //                 //         break;
        //                 //     case 7:
        //                 //         onTap?.Invoke(0, 0, new Vector2(1f, -1f));
        //                 //         break;
        //                 // }
        //                 break;
        //             // HOLD
        //             case 1:
        //                 // 
        //                 break;
        //         }
        //         break;
        //     // INVENTORY
        //     case 1:
        //         switch (typeInput)
        //         {
        //             // TAP
        //             case 0:
        //                 // switch (id)
        //                 // {
        //                 //     case 0:
        //                 //         onTap?.Invoke(1, 0, new Vector2(-1f, 1f));
        //                 //         break;
        //                 //     case 1:
        //                 //         onTap?.Invoke(1, 0, new Vector2(0f, 1f));
        //                 //         break;
        //                 //     case 2:
        //                 //         onTap?.Invoke(1, 0, new Vector2(1f, 1f));
        //                 //         break;
        //                 //     case 3:
        //                 //         onTap?.Invoke(1, 0, new Vector2(-1f, 0f));
        //                 //         break;
        //                 //     case 4:
        //                 //         onTap?.Invoke(1, 0, new Vector2(1f, 0f));
        //                 //         break;
        //                 //     case 5:
        //                 //         onTap?.Invoke(1, 0, new Vector2(-1f, -1f));
        //                 //         break;
        //                 //     case 6:
        //                 //         onTap?.Invoke(1, 0, new Vector2(0f, -1f));
        //                 //         break;
        //                 //     case 7:
        //                 //         onTap?.Invoke(1, 0, new Vector2(1f, -1f));
        //                 //         break;
        //                 // }
        //                 break;
        //             // HOLD
        //             case 1:
        //                 // 
        //                 break;
        //         }
        //         break;
        //     // CANCEL
        //     case 2:
        //         // onTap?.Invoke(2, 0, Vector2.zero);
        //         break;
        // }
        onTap?.Invoke(typeButton, typeInput, id);
    }
}