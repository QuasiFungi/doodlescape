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
        CANCEL,
        INSTRUCTION,
        SETTING,
        AD,
        CLEARED,
        EXIT,
        DISABLED
    }
    [SerializeField] private ButtonType _typeButton = ButtonType.MOVE;
    private enum InputType
    {
        TAP,
        HOLD
    }
    [SerializeField] private KeyCode _keyBind;
    [SerializeField] private int _id = -1;
    private float _holdTime = .35f;
    private float _holdTimer = 0f;
    public void OnPointerDown(PointerEventData eventData)
    {
        // * testing input ignore in menu/transition
        if (!_isEnabled) return;
        // do button press logic
        ButtonDown();
    }
    // logic offloaded to reuse with keyboard
    private void ButtonDown()
    {
        // new input
        _flagInput = true;
        // expected global time for hold event
        _holdTimer = Time.realtimeSinceStartup + _holdTime;
        // immediate feedback
        if (!_isTick) MarkerShow();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        // do button unpress logic
        ButtonUp();
    }
    // logic offloaded to reuse with keyboard
    private void ButtonUp()
    {
        if (_flagInput && _holdTimer > Time.realtimeSinceStartup)
        {
            // input processed
            _flagInput = false;
            // detected tap
            onInteract?.Invoke((int)_typeButton, (int)InputType.TAP, _id);
            // manually end input
            if (!_isTick)
            {
                MarkerClear();
                // * testing vibrate auxiliary
                GameData.DoVibrate(GameData.IntensityVibrate.LOW);
            }
        }
    }
    private bool _flagInput = false;
    void Update()
    {
        // * testing input ignore in menu/transition
        if (!IsEnabled) return;
        // * testing keyboard input
        if (Input.GetKeyDown(_keyBind)) ButtonDown();
        else if (Input.GetKeyUp(_keyBind)) ButtonUp();
        // auto trigger input on timer reach
        if (_flagInput && _holdTimer <= Time.realtimeSinceStartup)
        {
            // input processed
            _flagInput = false;
            // detected hold
            onInteract?.Invoke((int)_typeButton, (int)InputType.HOLD, _id);
            // manually end input
            if (!_isTick)
            {
                MarkerClear();
                // * testing vibrate auxiliary
                GameData.DoVibrate(GameData.IntensityVibrate.MEDIUM);
            }
        }
    }
    [Tooltip("Button input tied to ticks")] [SerializeField] private bool _isTick = true;
    void OnEnable()
    {
        // action/storage buttons
        if (_isTick)
        {
            Player.onAction += MarkerUpdate;
            GameClock.onTickUIEarly += MarkerClear;
            ManagerUI.onPause += ToggleInput;
            TestUITheme.onUpdate += ToggleIcon;
        }
        GameMaster.onTransition += ToggleInput;
        // 
        GameMaster.onStartupInput += Initialize;
    }
    void OnDisable()
    {
        // action/storage buttons
        if (_isTick)
        {
            Player.onAction -= MarkerUpdate;
            GameClock.onTickUIEarly -= MarkerClear;
            ManagerUI.onPause -= ToggleInput;
            TestUITheme.onUpdate -= ToggleIcon;
        }
        GameMaster.onTransition -= ToggleInput;
        // 
        GameMaster.onStartupInput -= Initialize;
    }
    // * testing startup sequence
    private void Initialize()
    {
        ToggleInput(true);
    }
    // * testing input disable on transition
    private bool _isEnabled = false;
    private void ToggleInput(bool state)
    {
        _isEnabled = state;
        // update icon visibilty
        ToggleIcon();
        // update sprite
        MarkerClear();
    }
    private void ToggleIcon()
    {
        // hide icon if disabled ? garbage
        _iconA.color = new Color(_iconA.color.r, _iconA.color.g, _iconA.color.b, _isEnabled ? 1f : 0f);
        _iconB.color = new Color(_iconB.color.r, _iconB.color.g, _iconB.color.b, _isEnabled ? 1f : 0f);
    }
    private Image _marker;
    // ? move to game variables
    public Sprite spriteSelected;
    // public Sprite spriteUnselected;
    public Sprite spriteDisabled;
    void Awake()
    {
        _marker = transform.GetChild(0).GetComponent<Image>();
        _iconA = transform.GetChild(1).GetComponent<Image>();
        _iconB = transform.GetChild(2).GetComponent<Image>();
        // 
        MarkerClear();
    }
    // action/storage input reached player
    private void MarkerUpdate(GameAction action)
    {
        // * testing input ignore on transition
        if (_isEnabled)
        {
            // show marker
            // if (_id == action.Index && action.TypeButton == (int)_typeButton) _marker.sprite = spriteSelected;
            // else _marker.sprite = spriteDisabled;
            _marker.enabled = _id == action.Index && action.TypeButton == (int)_typeButton;
        }
    }
    private void MarkerShow()
    {
        // ignore input
        // if (_isEnabled)
        //     // 
        //     _marker.sprite = spriteSelected;
        _marker.enabled = _isEnabled;
    }
    private void MarkerClear()
    {
        // hide marker
        // _marker.sprite = spriteDisabled;
        _marker.enabled = false;
    }
    // * testing ui manager
    private Image _iconA;
    private Image _iconB;
    public void Configure(ButtonType type, int id, Sprite iconA, Sprite iconB, bool isEnabled = true)
    {
        _typeButton = type;
        _id = id;
        _iconA.sprite = iconA;
        _iconB.sprite = iconB;
        ToggleInput(isEnabled);
    }
    // * testing keyboard input
    public int TypeButton
    {
        get { return (int)_typeButton; }
    }
    public int Index
    {
        get { return _id; }
    }
    public bool IsEnabled
    {
        get { return _isEnabled; }
    }
}