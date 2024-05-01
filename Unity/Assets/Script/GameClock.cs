using UnityEngine;
using System.Collections;
public class GameClock : MonoBehaviour
{
    public delegate void OnTick();
    public static event OnTick onTickEarly;
    public static event OnTick onTick;
    public static event OnTick onTickLate;
    public static event OnTick onTickUI;
    private float tickDuration = 1f;
    private float tickTimer;
    [Tooltip("tick on keypress")] [SerializeField] private bool _devManualTick = false;
    void OnEnable()
    {
        GameInput.onTap += ManualTick;
    }
    void OnDisable()
    {
        GameInput.onTap += ManualTick;
    }
    private void ManualTick(Vector2 position)
    {
        if (position.sqrMagnitude > 0f) return;
        // 
        if (_devManualTick) StartCoroutine("Tick");
    }
    void Start()
    {
        tickTimer = 0f;
        // // * testing ? things move forward one tick immediately
        // onTick?.Invoke();
        // onTickLate?.Invoke();
        if (_devManualTick) Debug.LogWarning(gameObject.name + ":\tTick manual override (press Space to advance time)");
    }
    // ? dev manual override to space
    void Update()
    {
        if (_devManualTick) return;
        // 
        if (tickTimer > 0f) tickTimer -= Time.deltaTime;
        else if (!_isBusy)
        {
            StartCoroutine("Tick");
        }
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
        // - game grid, tick grid
        // - ui inventory, icons update
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
}