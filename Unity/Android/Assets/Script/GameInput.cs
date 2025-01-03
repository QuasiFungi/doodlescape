using UnityEngine;
using UnityEngine.EventSystems;
public class GameInput : MonoBehaviour
{
    public delegate void OnTap(int typeButton, int typeInput, int index);
    public static event OnTap onTap;
    // public BaseButton auxLeft, auxRight, actionUL, actionU, actionUR, actionL, actionR, actionDL, actionDR, storageUL, storageU, storageUR, storageL, storageR, storageDL, storageD, storageDR;
    // private float _holdTime = .35f;
    // private float[] _holdTimers = 0f;
    // void Awake()
    // {
    //     _holdTimers = new float[16];
    // }
    // void Update()
    // {
    //     // testing w/ keyboard
    //     // 
    //     // - auxillary
    //     if (auxLeft.IsEnabled && Input.GetKeyDown(KeyCode.LeftAlt)) onTap?.Invoke(auxLeft.TypeButton, 0, auxLeft.Index);
    //     if (auxRight.IsEnabled && Input.GetKeyDown(KeyCode.RightAlt)) onTap?.Invoke(auxRight.TypeButton, 0, auxRight.Index);
    //     // 
    //     if (_isDisabled) return;
    //     // 
    //     // - movement
    //     // UL
    //     if (actionUL.IsEnabled && Input.GetKeyDown(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.LeftArrow)) onTap?.Invoke(0, 0, 0);
    //     // UR
    //     else if (actionUR.IsEnabled && Input.GetKeyDown(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.RightArrow)) onTap?.Invoke(0, 0, 2);
    //     // DL
    //     else if (actionDL.IsEnabled && Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.LeftArrow)) onTap?.Invoke(0, 0, 5);
    //     // DR
    //     else if (actionDR.IsEnabled && Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.RightArrow)) onTap?.Invoke(0, 0, 7);
    //     // U
    //     else if (actionU.IsEnabled && Input.GetKeyDown(KeyCode.UpArrow)) onTap?.Invoke(0, 0, 1);
    //     // D
    //     else if (actionD.IsEnabled && Input.GetKeyDown(KeyCode.DownArrow)) onTap?.Invoke(0, 0, 6);
    //     // L
    //     else if (actionL.IsEnabled && Input.GetKeyDown(KeyCode.LeftArrow)) onTap?.Invoke(0, 0, 3);
    //     // R
    //     else if (actionR.IsEnabled && Input.GetKeyDown(KeyCode.RightArrow)) onTap?.Invoke(0, 0, 4);
    //     // - inventory
    //     // UL
    //     if (storage.IsEnabled && Input.GetKeyDown(KeyCode.W) && Input.GetKeyDown(KeyCode.A)) onTap?.Invoke(1, 0, 4);
    //     // UR
    //     else if (storageUR.IsEnabled && Input.GetKeyDown(KeyCode.W) && Input.GetKeyDown(KeyCode.D)) onTap?.Invoke(1, 0, 5);
    //     // DL
    //     else if (storageDL.IsEnabled && Input.GetKeyDown(KeyCode.S) && Input.GetKeyDown(KeyCode.A)) onTap?.Invoke(1, 0, 6);
    //     // DR
    //     else if (storageDR.IsEnabled && Input.GetKeyDown(KeyCode.S) && Input.GetKeyDown(KeyCode.D)) onTap?.Invoke(1, 0, 7);
    //     // U
    //     else if (storageU.IsEnabled && Input.GetKeyDown(KeyCode.W)) onTap?.Invoke(1, 0, 0);
    //     // D
    //     else if (storageD.IsEnabled && Input.GetKeyDown(KeyCode.S)) onTap?.Invoke(1, 0, 3);
    //     // L
    //     else if (storageL.IsEnabled && Input.GetKeyDown(KeyCode.A)) onTap?.Invoke(1, 0, 1);
    //     // R
    //     else if (storageR.IsEnabled && Input.GetKeyDown(KeyCode.D)) onTap?.Invoke(1, 0, 2);
    //     // // - cancel
    //     // if (Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftControl)) onTap?.Invoke(2, 0, 0);
    //     // // - tick
    //     // if (Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.LeftShift)) onTap?.Invoke(3, 0, 0);
    // }
    void OnEnable()
    {
        BaseButton.onInteract += OnAction;
        GameMaster.onStartupInput += Initialize;
    }
    void OnDisable()
    {
        BaseButton.onInteract -= OnAction;
        GameMaster.onStartupInput -= Initialize;
    }
    private bool _isDisabled = true;
    private void Initialize()
    {
        _isDisabled = false;
    }
    // pipeline between buttons and player/clock
    private void OnAction(int typeButton, int typeInput, int id)
    {
        // allow instructions only
        if (_isDisabled && typeButton != 3) return;
        // 
        onTap?.Invoke(typeButton, typeInput, id);
    }
}