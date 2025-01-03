using UnityEngine;
using System.Collections;
public class GameClock : MonoBehaviour
{
    public delegate void OnTick();
    public static event OnTick onTickEarly;
    public static event OnTick onTickChunk;
    // public static event OnTick onTickPool;
    public static event OnTick onTick;
    public static event OnTick onTickLate;
    public static event OnTick onTickUIEarly;
    public static event OnTick onTickUI;
    private float tickDuration = 1f;
    private float tickTimer;
    [Tooltip("tick on keypress")] [SerializeField] private bool _devManualTick = false;
    // * testing diffculty slider
    // [Tooltip("0 - easy | 1 - normal | 2 - hard | 3 - speedrun")] [SerializeField] [Range(0, 2)] private int _difficulty = 1;
    private static int _difficulty;
    private float[] _tickDuration = {.75f, 1f, 2f, 2.5f};
    void OnEnable()
    {
        GameInput.onTap += ManualTick;
        GameMaster.onTransitionComplete += UpdateUI;
        // 
        GameMaster.onStartupClock += Initialize;
        UISettings.onDifficulty += SetDifficulty;
        ManagerUI.onPause += ToggleClock;
        // 
        GameData.onReset += ResetDifficulty;
    }
    void OnDisable()
    {
        GameInput.onTap -= ManualTick;
        GameMaster.onTransitionComplete -= UpdateUI;
        // 
        GameMaster.onStartupClock -= Initialize;
        UISettings.onDifficulty -= SetDifficulty;
        ManagerUI.onPause -= ToggleClock;
        // 
        GameData.onReset -= ResetDifficulty;
    }
    // * testing startup sequence
    private void Initialize()
    {
        ToggleClock(true);
        _ticks = GameData.Ticks;
    }
    // 
    private void SetDifficulty(int index)
    {
        _difficulty = Mathf.Clamp(index, 0, _tickDuration.Length);
        Time.timeScale = _tickDuration[_difficulty];
    }
    // wrapper
    private void ResetDifficulty()
    {
        // SetDifficulty(PlayerPrefs.GetInt("difficulty", 1));
        SetDifficulty(GameData.Difficulty);
    }
    // * testing icon update mid transition
    private void UpdateUI(Vector2Int room)
    {
        // print("updating UI at " + Time.time);
        onTickUI?.Invoke();
    }
    // private void ManualTick(int typeButton, int typeInput, Vector2 position)
    private void ManualTick(int typeButton, int typeInput, int index)
    {
        // if (position.sqrMagnitude > 0f) return;
        if (typeButton != 3) return;
        // 
        if (_devManualTick) StartCoroutine("Tick");
    }
    void Start()
    {
        _ticks = 0;
        tickTimer = 0f;
        // // * testing ? things move forward one tick immediately
        // onTick?.Invoke();
        // onTickLate?.Invoke();
        // 
        // Time.timeScale = _tickDuration[_difficulty];
        // ? normal difficulty for intro sequence
        SetDifficulty(0);
        // 
        if (_devManualTick) Debug.LogWarning(gameObject.name + ":\tTick manual override (press Shift to advance time)");
        // else Debug.LogWarning(gameObject.name + ":\tDifficulty set to " + (_difficulty == 0 ? "Easy" : (_difficulty == 1 ? "Normal" : "Hard")));
    }
    // ? dev manual override to space
    void Update()
    {
        // * testing ? float array for finer individual control
        // Time.timeScale = 1f / (float)_tickDuration;
        // Time.timeScale = (float)_tickDuration / 2f;
        // Time.timeScale = _tickDuration[_difficulty];
        // 
        if (_devManualTick || !_isTick || _isBusy) return;
        // 
        if (tickTimer > 0f) tickTimer -= Time.deltaTime;
        else StartCoroutine("Tick");
    }
    private bool _isBusy = false;
    // * save/load value ? double static values
    private static int _ticks;
    IEnumerator Tick()
    {
        if (_ticks < 99999) _ticks++;
        // * testing ? possible crash if tick less than 4 frames
        // disallow further ticks
        _isBusy = true;
        // - base button, marker clear
        // - ui clock, sprite reset, damage tick
        onTickUIEarly?.Invoke();
        yield return null;
        // - game master, execute actions
        onTickEarly?.Invoke();
        yield return null;
        // - base chunk, local state
        onTickChunk?.Invoke();
        yield return null;
        // // - manager pool, chunk update
        // onTickPool?.Invoke();
        // yield return null;
        // - game grid, tick grid
        // - ui inventory, icons update
        // - sensor vision trigger, find targets
        onTick?.Invoke();
        yield return null;
        // - mob, behaviour tick * testing ignore mob tick when transition in progress
        if (_isTick)
        {
            onTickLate?.Invoke();
            yield return null;
        }
        // // pause for animations to play out
        // yield return new WaitForSeconds(.5f);
        // - ui action, sprite update
        onTickUI?.Invoke();
        // 
        // allow next tick
        _isBusy = false;
        // countdown to next tick
        tickTimer += tickDuration;
    }
    // private bool _isTick = true;
    private bool _isTick = false;
    public void ToggleClock(bool state)
    {
        _isTick = state;
        // clock enabled
        if (_isTick)
        {
            // print("enabling clock at " + Time.time);
            // reset countdown to next tick
            tickTimer = tickDuration;
            // // show transition icon immediately
            // onTickUI?.Invoke();
            onTickUIEarly?.Invoke();
            // restore clock speed
            // SetDifficulty(PlayerPrefs.GetInt("difficulty", 1));
            SetDifficulty(GameData.Difficulty);
        }
        // clock normal speed
        else SetDifficulty(0);
        // // * testing causes freeze
        // else
        // {
        //     // cancel tick
        //     StopCoroutine("Tick");
        //     // ? reset action queue
        // }
    }
    public static int Ticks
    {
        get { return _ticks; }
    }
    public static int Difficulty
    {
        get { return _difficulty; }
    }
}