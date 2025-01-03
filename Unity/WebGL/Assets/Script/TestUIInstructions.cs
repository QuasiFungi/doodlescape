using UnityEngine;
using UnityEngine.UI;
public class TestUIInstructions : MonoBehaviour
{
    // ? also stop time ? pause tick
    public Sprite[] _slides;
    void OnEnable()
    {
        GameInput.onTap += Iterate;
    }
    void OnDisable()
    {
        GameInput.onTap -= Iterate;
    }
    private int _index = 0;
    private Image _image;
    void Awake()
    {
        _image = transform.GetChild(0).GetComponent<Image>();
    }
    private void Iterate(int typeButton, int typeInput, int index)
    {
        // ignore non instruction input
        if (typeButton != 3) return;
        // tap
        if (typeInput == 0)
        {
            // forward
            if (index == 0) _index++;
            else _index--;
            // 
            _index = Mathf.Clamp(_index, 0, _slides.Length);
        }
        // hold ? just to demonstrate the two input types
        else
        {
            // jump to last
            if (index == 0) _index = _slides.Length - 1;
            // jump to first
            else _index = 0;
        }
        // iterate
        if (_index < _slides.Length) _image.sprite = _slides[_index];
        // finish
        else Complete();
    }
    private void Complete()
    {
        // fade out
        gameObject.SetActive(false);
        // inform game master
        onReady?.Invoke();
    }
    public delegate void OnReady();
    public static event OnReady onReady;
}