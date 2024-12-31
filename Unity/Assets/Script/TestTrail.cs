using UnityEngine;
public class TestTrail : MonoBehaviour
{
    void OnEnable()
    {
        GameClock.onTickEarly += DoTick;
    }
    void OnDisable()
    {
        GameClock.onTickEarly -= DoTick;
    }
    public int _time = 3;
    private int _timer;
    private SpriteRenderer _sprite;
    void Awake()
    {
        _timer = _time;
        _sprite = GetComponent<SpriteRenderer>();
    }
    private Color temp;
    private void DoTick()
    {
        if (_timer > 0)
        {
            _timer--;
            temp = _sprite.color;
            temp.a = Mathf.Lerp(0f, 1f, (float)_timer / _time);
            _sprite.color = temp;
        }
        else Destroy(gameObject);
    }
}