using UnityEngine;
using System.Collections;
public class GameClock : MonoBehaviour
{
    public delegate void OnTick();
    public static event OnTick onTickEarly;
    public static event OnTick onTickChunk;
    public static event OnTick onTickPool;
    public static event OnTick onTick;
    public static event OnTick onTickLate;
    public static event OnTick onTickUI;
    private float tickDuration = 1f;
    private float tickTimer;
    [Tooltip("tick on keypress")] [SerializeField] private bool _devManualTick = false;
    // * testing diffculty slider
    [Tooltip("difficulty: 1 - easy | 2 - normal | 3 - hard")] [SerializeField] [Range(1, 3)] private int _tickDuration = 2;
    void OnEnable()
    {
        GameInput.onTap += ManualTick;
    }
    void OnDisable()
    {
        GameInput.onTap += ManualTick;
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
        tickTimer = 0f;
        // // * testing ? things move forward one tick immediately
        // onTick?.Invoke();
        // onTickLate?.Invoke();
        if (_devManualTick) Debug.LogWarning(gameObject.name + ":\tTick manual override (press Left Shift to advance time)");
        else Debug.LogWarning(gameObject.name + ":\tDifficulty set to " + (_tickDuration == 1 ? "Easy" : (_tickDuration == 2 ? "Normal" : "Hard")));
    }
    // ? dev manual override to space
    void Update()
    {
        if (_devManualTick || !_isTick) return;
        // 
        // * testing
        // Time.timeScale = 1f / (float)_tickDuration;
        Time.timeScale = (float)_tickDuration / 2f;
        // 
        if (tickTimer > 0f) tickTimer -= Time.deltaTime;
        else if (!_isBusy) StartCoroutine("Tick");
    }
    private bool _isBusy = false;
    IEnumerator Tick()
    {
        // * testing ? possible crash if tick less than 4 frames
        // disallow further ticks
        _isBusy = true;
        // - game master, execute actions
        // - ui action, marker clear
        // - ui clock, sprite reset
        onTickEarly?.Invoke();
        yield return null;
        // - base chunk, local state
        onTickChunk?.Invoke();
        yield return null;
        // - manager pool, chunk update
        onTickPool?.Invoke();
        yield return null;
        // - game grid, tick grid
        // - ui inventory, icons update
        // - sensor vision, find targets
        onTick?.Invoke();
        yield return null;
        // - mob, behaviour tick
        onTickLate?.Invoke();
        // yield return null;
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
    private bool _isTick = true;
    public void ToggleClock(bool state)
    {
        _isTick = state;
        // clock enabled
        if (_isTick)
        {
            // reset countdown to next tick
            tickTimer = tickDuration;
            // show transition icon immediately
            onTickUI?.Invoke();
        }
    }
}