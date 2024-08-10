using UnityEngine;
public class TestAnimated : MonoBehaviour
{
    public Sprite[] _sprites;
    private SpriteRenderer _sprite;
    private float _step;
    private float _timer;
    private int _index;
    void Awake()
    {
        // store reference
        _sprite = GetComponent<SpriteRenderer>();
        // wait time
        _step = 1f / _sprites.Length;
        // initialize
        _timer = 0f;
        _index = 0;
    }
    void Update()
    {
        // wait
        if (_timer < _step) _timer += Time.deltaTime;
        // tick
        else
        {
            // iterate
            _index++;
            // reset
            _timer = 0f;
            if (_index == _sprites.Length) _index = 0;
            // update
            _sprite.sprite = _sprites[_index];
        }
    }
}