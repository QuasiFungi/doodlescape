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
        // if (_devManualTick) Tick();
        if (_devManualTick) StartCoroutine("Tick");
    }
    void Start()
    {
        tickTimer = tickDuration;
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
        else
        {
            tickTimer += tickDuration;
            // Tick();
            StartCoroutine("Tick");
        }
    }
    // IEnumerator Tick()
    // {
    //     // 
    // }
    // private void Tick()
    IEnumerator Tick()
    {
        // // * testing
        // // game grid
        // onTickEarly?.Invoke();
        // // creature actions
        // // ui clock
        // onTick?.Invoke();
        // // ui action
        // // ui inventory
        // onTickLate?.Invoke();
        // 
        // ? possible crash if tick less than 4 frames
        onTickEarly?.Invoke();
        yield return null;
        onTick?.Invoke();
        yield return null;
        onTickLate?.Invoke();
        yield return null;
        onTickUI?.Invoke();
    }
}