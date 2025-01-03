using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class UIMonologue : BaseMenu
{
    [Header("Monologue")]
    // // ? also stop time ? pause tick
    public Sprite[] _slides;
    public Sprite _slideClear;
    // void OnEnable()
    // {
    //     GameInput.onTap += Iterate;
    // }
    // void OnDisable()
    // {
    //     GameInput.onTap -= Iterate;
    // }
    private int _index = -1;
    private Image _fadeOut;
    private Image _fadeIn;
    void Awake()
    {
        _fadeOut = transform.GetChild(0).GetComponent<Image>();
        _fadeIn = transform.GetChild(1).GetComponent<Image>();
    }
    private Color _colorDefault;
    void Start()
    {
        // ? save/load, use game data reference
        // _colorDefault = TestUITheme.ToneB;
        _colorDefault = _fadeIn.color;
    }
    private float _timer = 0f;
    [Tooltip("Hold text for how long")] [SerializeField] private float _timePause = 1f;
    private float _timeFade = .5f;
    public bool _testSkip = false;
    void Update()
    {
        if (_testSkip) DoComplete();
        // print(_timer);
        if (_isPause) return;
        // shorter wait at start and end
        if (_timer >= (_index > -1 && _index < _slides.Length - 1 ? _timePause : 1f))
        {
            _index++;
            // iterate
            if (_index <= _slides.Length) StartCoroutine(Fade());
            // end
            else DoComplete();
        }
        // 
        else _timer += Time.deltaTime;
    }
    private bool _isPause = false;
    IEnumerator Fade()
    {
        // print(_index);
        _isPause = true;
        // current one fade out
        _fadeOut.sprite = _fadeIn.sprite;
        // new one fade in
        _fadeIn.sprite = _index < _slides.Length ? _slides[_index] : _slideClear;
        // faded out color
        Color color = _colorDefault;
        color.a = 0f;
        // 
        for(float t = 0f; t < 1f; t += Time.deltaTime / _timeFade)
        {
            _fadeOut.color = Color.Lerp(_colorDefault, color, t);
            _fadeIn.color = Color.Lerp(color, _colorDefault, t);
            yield return null;
        }
        _fadeOut.color = color;
        _fadeIn.color = _colorDefault;
        yield return null;
        // 
        _timer = 0f;
        _isPause = false;
    }
    // private void Iterate(int typeButton, int typeInput, int index)
    // {
    //     // ignore non instruction input
    //     if (typeButton != 3) return;
    //     // tap
    //     if (typeInput == 0)
    //     {
    //         // forward
    //         if (index == 0) _index++;
    //         else _index--;
    //         // 
    //         _index = Mathf.Clamp(_index, 0, _slides.Length);
    //     }
    //     // hold ? just to demonstrate the two input types
    //     else
    //     {
    //         // jump to last
    //         if (index == 0) _index = _slides.Length - 1;
    //         // jump to first
    //         else _index = 0;
    //     }
    //     // iterate
    //     if (_index < _slides.Length) _image.sprite = _slides[_index];
    //     // finish
    //     else Complete();
    // }
    // private void Complete()
    // {
    //     // fade out
    //     gameObject.SetActive(false);
    //     // inform game master
    //     onReady?.Invoke();
    // }
    // public delegate void OnReady();
    // public static event OnReady onReady;
}