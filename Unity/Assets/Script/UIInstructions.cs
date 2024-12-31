using UnityEngine;
using UnityEngine.UI;
public class UIInstructions : BaseMenu
{
    [Header("Instructions")]
    // ? also stop time ? pause tick
    public Sprite[] _slides;
    // void OnEnable()
    // {
    //     GameInput.onTap += Iterate;
    // }
    // void OnDisable()
    // {
    //     GameInput.onTap -= Iterate;
    // }
    private int _index = 0;
    private Image _image;
    private Image[] _arrows;
    void Awake()
    {
        _image = transform.GetChild(1).GetComponent<Image>();
        _skipSFX = true;
        gameObject.SetActive(false);
        GameInput.onTap += Iterate;
        // 
        _arrows = new Image[transform.childCount - 2];
        for (int i = _arrows.Length - 1; i > -1; i--)
            _arrows[i] = transform.GetChild(i + 2).GetComponent<Image>();
        // ToggleArrow(-1);
        ToggleArrow();
    }
    void OnDestroy()
    {
        GameInput.onTap -= Iterate;
    }
    public bool _isSkip = false;
    private void Iterate(int typeButton, int typeInput, int index)
    {
        if (_isSkip)
        {
            DoComplete();
            return;
        }
        // ignore non instruction input
        if (typeButton != 3) return;
        // tap
        if (typeInput == 0)
        {
            // forward
            if (index == 1) _index++;
            // backward
            else _index--;
            // 
            _index = Mathf.Clamp(_index, 0, _slides.Length);
            // * testing sfx button tap
            GameAudio.Instance.Register(0, GameAudio.AudioType.UI);
        }
        // hold ? just to demonstrate the two input types
        else
        {
            // jump to last
            if (index == 1) _index = _slides.Length - 1;
            // jump to first
            else _index = 0;
            // * testing sfx button hold
            GameAudio.Instance.Register(1, GameAudio.AudioType.UI);
        }
        // iterate
        if (_index < _slides.Length)
        {
            _image.sprite = _slides[_index];
            ToggleArrow();
        }
        // finish
        else DoComplete();
    }
    private void ToggleArrow()
    {
        for (int i = _arrows.Length - 1; i > -1; i--)
            _arrows[i].enabled = _index == i;
    }
    // private void Complete()
    // {
    //     // // fade out
    //     // gameObject.SetActive(false);
    //     // inform game master
    //     onReady?.Invoke();
    // }
    // public delegate void OnReady();
    // public static event OnReady onReady;
}