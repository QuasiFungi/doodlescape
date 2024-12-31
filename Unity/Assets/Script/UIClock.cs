using UnityEngine;
using UnityEngine.UI;
public class UIClock : MonoBehaviour
{
    private Image image;
    public Sprite[] sprites;
    private float index;
    private Text _timer;
    void Awake()
    {
        image = GetComponent<Image>();
        index = sprites.Length - 1;
        _timer = transform.GetChild(0).GetComponent<Text>();
    }
    void OnEnable()
    {
        GameClock.onTickUIEarly += SpriteReset;
    }
    void OnDisable()
    {
        GameClock.onTickUIEarly -= SpriteReset;
    }
    private string _time = "";
    private string _zeros = "";
    private void SpriteReset()
    {
        index = 0f;
        _time = GameClock.Ticks.ToString();
        _zeros = "";
        for(int i = 5 - _time.Length; i > 0; i--) _zeros += "0";
        _timer.text = _zeros + _time;
    }
    void Update()
    {
        index += Time.deltaTime * sprites.Length;
        image.sprite = sprites[Mathf.Clamp(Mathf.FloorToInt(index), 0, sprites.Length - 1)];
    }
}