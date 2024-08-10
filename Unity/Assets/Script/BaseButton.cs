using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class BaseButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public delegate void OnInteract(int typeButton, int typeInput, int id);
    public static event OnInteract onInteract;
    public enum ButtonType
    {
        MOVE,
        INVENTORY,
        CANCEL
    }
    public ButtonType _typeButton = ButtonType.MOVE;
    private enum InputType
    {
        TAP,
        HOLD
    }
    // public InputType _typeInput = InputType.TAP;
    public int _id = -1;
    private float _holdTime = .3f;
    private float _holdTimer = 0f;
    public void OnPointerDown(PointerEventData eventData)
    {
        // onInteract?.Invoke(_type, _id, true);
        // 
        // new input
        _flagInput = true;
        // expected global time for hold event
        _holdTimer = Time.time + _holdTime;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        // onInteract?.Invoke((int)_typeButton, _holdTimer >= Time.time ? (int)InputType.TAP : (int)InputType.HOLD, _id);
        // 
        if (_flagInput && _holdTimer > Time.time)
        {
            // input processed
            _flagInput = false;
            // detected tap
            onInteract?.Invoke((int)_typeButton, (int)InputType.TAP, _id);
        }
    }
    private bool _flagInput = false;
    void Update()
    {
        // auto trigger input on timer reach
        if (_flagInput && _holdTimer <= Time.time)
        {
            // input processed
            _flagInput = false;
            // detected hold
            onInteract?.Invoke((int)_typeButton, (int)InputType.HOLD, _id);
        }
    }
    void OnEnable()
    {
        Player.onAction += MarkerUpdate;
        GameClock.onTickEarly += MarkerClear;
    }
    void OnDisable()
    {
        Player.onAction -= MarkerUpdate;
        GameClock.onTickEarly -= MarkerClear;
    }
    private Image _marker;
    // ? move to game variables
    public Sprite spriteSelected;
    public Sprite spriteDisable;
    void Awake()
    {
        _marker = GetComponent<Image>();
    }
    private void MarkerUpdate(GameAction action)
    {
        // // matches input pad
        // if (action.TypeButton == (int)_typeButton)
        // show marker
        _marker.sprite = (_id == action.Index && action.TypeButton == (int)_typeButton) ? spriteSelected : spriteDisable;
    }
    private void MarkerClear()
    {
        // hide marker
        _marker.sprite = spriteDisable;
    }
}