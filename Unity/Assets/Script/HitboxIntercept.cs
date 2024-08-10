using UnityEngine;
[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class HitboxIntercept : BaseHitbox
{
    void OnEnable()
    {
        GameClock.onTick += Tick;
    }
    void OnDisable()
    {
        GameClock.onTick -= Tick;
    }
    [Tooltip("Effect duration (-1 for permanent)")] [SerializeField] private int _timer = -1;
    void Tick()
    {
        if (_timer > 0) _timer--;
        else if (_timer == 0) Discard();
    }
    public delegate void OnIntercept(string id, GameAction.ActionType type, bool state);
    public static event OnIntercept onIntercept;
    [Tooltip("Block actions of this type")] [SerializeField] private GameAction.ActionType _type;
    void OnTriggerEnter2D(Collider2D other)
    {
        // GameMaster.Instance.
        onIntercept?.Invoke(other.gameObject.GetComponent<Entity>().ID, _type, true);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        onIntercept?.Invoke(other.gameObject.GetComponent<Entity>().ID, _type, false);
    }
}