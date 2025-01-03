using UnityEngine;
public class TestAnimated : MonoBehaviour
{
    [Tooltip("Stop animation on finish")] public bool _destroy = false;
    [Tooltip("Discard on complete")] public bool _discard = false;
    [Tooltip("Fade out as animate")] public bool _fade = false;
    [Tooltip("0 - pause | 1 - normal | 2 - fast")] [Range(0, 2)] public float _speed = 1f;
    public Sprite[] _sprites;
    private SpriteRenderer _sprite;
    private float _step;
    private float _timer;
    private int _index;
    private Color _colorA, _colorB;
    void Awake()
    {
        // store reference
        _sprite = GetComponent<SpriteRenderer>();
        // wait time
        _step = 1f / _sprites.Length;
        // initialize
        _timer = 0f;
        _index = 0;
        // use inspector assigned color
        _colorA = _sprite.color;
        _colorB = _sprite.color;
        _colorB.a = 0;
    }
    void Update()
    {
        // wait
        if (_timer < _step) _timer += Time.deltaTime * _speed;
        // tick
        else
        {
            // iterate
            _index++;
            // reset
            _timer = 0f;
            if (_index == _sprites.Length)
            {
                // * testing
                if (_destroy)
                {
                    // hide sprite
                    if (_fade) _sprite.color = _colorB;
                    // stop animating
                    Destroy(_discard ? gameObject : this);
                    // abort
                    return;
                }
                // 
                _index = 0;
            }
            // update
            _sprite.sprite = _sprites[_index];
            if (_fade) _sprite.color = Color.Lerp(_colorA, _colorB, (float)_index / _sprites.Length);
        }
    }
}