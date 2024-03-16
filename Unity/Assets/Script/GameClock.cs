using UnityEngine;
public class GameClock : MonoBehaviour
{
    public delegate void OnTick();
    public static event OnTick onTick;
    public static event OnTick onTickLate;
    private float tickDuration = 1f;
    private float tickTimer;
    void Awake()
    {
        tickTimer = tickDuration;
    }
    void Update()
    {
        if (tickTimer > 0f) tickTimer -= Time.deltaTime;
        else
        {
            tickTimer += tickDuration;
            // creature actions
            // ui clock
            onTick?.Invoke();
            // ui action
            onTickLate?.Invoke();
        }
    }
}