using UnityEngine;
using UnityEngine.UI;
public class TestUIHealth : MonoBehaviour
{
    public Breakable _target;
    // void OnEnable()
    // {
    //     GameClock.onTickUIEarly += Tick;
    //     GameClock.onTickUI += Tick;
    // }
    // void OnDisable()
    // {
    //     // ? tie to player hurt event instead
    //     GameClock.onTickUIEarly -= Tick;
    //     GameClock.onTickUI -= Tick;
    // }
    private float[] _fillLives = {0f, .25f, .4f, .6f, .75f, 1f};
    private Image _imageLives;
    private float[] _fillShards = {0f, .4f, .6f, 1f};
    private Image _imageShards;
    void Awake()
    {
        _imageLives = transform.GetChild(0).GetComponent<Image>();
        _imageShards = gameObject.GetComponent<Image>();
    }
    private int _lives;
    private int _shards;
    // ? tie to player health change
    // private void Tick()
    void Update()
    {
        // _target.HealthInst
        // print(Mathf.Ceil(_target.HealthInst / 3f));
        _lives = (int) Mathf.Ceil(_target.HealthInst / 3f);
        if (_lives > 0) _shards = _target.HealthInst - (_lives - 1) * 3;
        else _shards = 0;
        // print("Lives: " + _lives + ", Shards: " + _shards);
        _imageLives.fillAmount = _fillLives[_lives];
        _imageShards.fillAmount = _fillShards[_shards];
    }
}