using UnityEngine;
using UnityEngine.UI;
public class UISettings : BaseMenu
{
    [Header("Settings")]
    private int _option;
    // private Image _selected;
    // public Sprite[] _options;
    private Image[,] _options;
    private Text _txtDifficulty;
    private Text _txtTheme;
    private Text _txtVibration;
    private int _indexDifficulty;
    private static int _indexTheme;
    private static int _indexVibration;
    [SerializeField] private string[] _valueDifficulty;
    [SerializeField] private string[] _valueTheme;
    [SerializeField] private string[] _valueVibration;
    void Awake()
    {
        // 
        // _option = _options.Length - 1;
        // 
        // _selected = transform.GetChild(1).GetComponent<Image>();
        // 
        _txtDifficulty = transform.GetChild(1).GetComponent<Text>();
        _txtTheme = transform.GetChild(2).GetComponent<Text>();
        _txtVibration = transform.GetChild(3).GetComponent<Text>();
        // 
        _options = new Image[transform.childCount - 4, 2];
        _option = _options.GetLength(0) - 1;
        for (int i = 0; i <= _option; i++)
        {
            _options[i,0] = transform.GetChild(i + 4).GetChild(0).GetComponent<Image>();
            _options[i,1] = transform.GetChild(i + 4).GetChild(1).GetComponent<Image>();
        }
        ToggleSelected();
        // bound to event for scene duration
        GameInput.onTap += DoSettings;
        // GameMaster.onStartupSettings += Initialize;
        GameData.onReset += Initialize;
        // 
        Initialize();
        // start disabled
        _skipSFX = true;
        gameObject.SetActive(false);
    }
    void OnDestroy()
    {
        // unsubcribe when reload scene
        GameInput.onTap -= DoSettings;
        // GameMaster.onStartupSettings -= Initialize;
        GameData.onReset -= Initialize;
    }
    private void Initialize()
    {
        // ? move to start
        _indexDifficulty = GameData.Difficulty;
        _indexTheme = GameData.Theme;
        _indexVibration = GameData.Vibrate;
        // 
        _txtDifficulty.text = _valueDifficulty[_indexDifficulty];
        _txtTheme.text = _valueTheme[_indexTheme];
        _txtVibration.text = _valueVibration[_indexVibration];
    }
    private void DoSettings(int typeButton, int typeInput, int index)
    {
        // only settings button
        if (typeButton != 4) return;
        // 
        // tap
        else if (typeInput == 0)
        {
            // open settings
            if (index == 0)
            {
                gameObject.SetActive(true);
                // notify manager ui
                DoComplete();
                // 
                return;
            }
            // last entry
            else if (index == 1) _option--;
            // next entry
            else if (index == 2) _option++;
            // loop on reach end
            if (_option == _options.GetLength(0)) _option = 0;
            else if (_option == -1) _option = _options.GetLength(0) - 1;
            // show
            // _selected.sprite = _options[_option];
            ToggleSelected();
            // * testing sfx menu cycle
            if (index < 3) GameAudio.Instance.Register(11, GameAudio.AudioType.UI);
        }
        // hold
        else
        {
            // cycle left
            if (index == 1)
            {
                // difficulty
                if (_option == 0)
                {
                    _indexDifficulty--;
                    // loop
                    if (_indexDifficulty == -1) _indexDifficulty = _valueDifficulty.Length - 1;
                    // save
                    // PlayerPrefs.SetInt("difficulty", _indexDifficulty);
                    GameData.Difficulty = _indexDifficulty;
                    // display
                    _txtDifficulty.text = _valueDifficulty[_indexDifficulty];
                    // notify game clock
                    onDifficulty?.Invoke(_indexDifficulty);
                    // * testing sfx menu modify
                    GameAudio.Instance.Register(12, GameAudio.AudioType.UI);
                }
                // theme
                else if (_option == 1)
                {
                    _indexTheme--;
                    // loop
                    if (_indexTheme == -1) _indexTheme = _valueTheme.Length - 1;
                    // save
                    // PlayerPrefs.SetInt("theme", _indexTheme);
                    GameData.Theme = _indexTheme;
                    // display
                    _txtTheme.text = _valueTheme[_indexTheme];
                    // notify 
                    onTheme?.Invoke(_indexTheme);
                    // * testing sfx menu modify
                    GameAudio.Instance.Register(12, GameAudio.AudioType.UI);
                }
                // vibration
                else if (_option == 2)
                {
                    _indexVibration--;
                    // loop
                    if (_indexVibration == -1) _indexVibration = _valueVibration.Length - 1;
                    // save
                    // PlayerPrefs.SetInt("vibration", _indexVibration);
                    GameData.Vibrate = _indexVibration;
                    // display
                    _txtVibration.text = _valueVibration[_indexVibration];
                    // notify 
                    onVibration?.Invoke(_indexVibration);
                    // * testing sfx menu modify
                    GameAudio.Instance.Register(12, GameAudio.AudioType.UI);
                }
                // close
                else if (_option == 3) DoComplete();
                // // default
                // else if (_option == 4) print("settings default");
            }
            // cycle right
            else if (index == 2)
            {
                // difficulty
                if (_option == 0)
                {
                    _indexDifficulty++;
                    // loop
                    if (_indexDifficulty == _valueDifficulty.Length) _indexDifficulty = 0;
                    // save
                    // PlayerPrefs.SetInt("difficulty", _indexDifficulty);
                    GameData.Difficulty = _indexDifficulty;
                    // display
                    _txtDifficulty.text = _valueDifficulty[_indexDifficulty];
                    // notify 
                    onDifficulty?.Invoke(_indexDifficulty);
                    // * testing sfx menu modify
                    GameAudio.Instance.Register(12, GameAudio.AudioType.UI);
                }
                // theme
                else if (_option == 1)
                {
                    _indexTheme++;
                    // loop
                    if (_indexTheme == _valueTheme.Length) _indexTheme = 0;
                    // save
                    // PlayerPrefs.SetInt("theme", _indexTheme);
                    GameData.Theme = _indexTheme;
                    // display
                    _txtTheme.text = _valueTheme[_indexTheme];
                    // notify 
                    onTheme?.Invoke(_indexTheme);
                    // * testing sfx menu modify
                    GameAudio.Instance.Register(12, GameAudio.AudioType.UI);
                }
                // vibration
                else if (_option == 2)
                {
                    _indexVibration++;
                    // loop
                    if (_indexVibration == _valueVibration.Length) _indexVibration = 0;
                    // save
                    // PlayerPrefs.SetInt("vibration", _indexVibration);
                    GameData.Vibrate = _indexVibration;
                    // display
                    _txtVibration.text = _valueVibration[_indexVibration];
                    // notify 
                    onVibration?.Invoke(_indexVibration);
                    // * testing sfx menu modify
                    GameAudio.Instance.Register(12, GameAudio.AudioType.UI);
                }
                // close
                else if (_option == 3) DoComplete();
                // // default
                // else if (_option == 4) print("settings default");
            }
            // // reset settings
            // else if (index == 4)
            // {
            //     // gameObject.SetActive(false);
            //     // // notify manager ui
            //     // OnComplete();
            //     print("settings default");
            // }
        }
    }
    private void ToggleSelected()
    {
        for (int i = _options.GetLength(0) - 1; i > -1; i--)
            if (_option == i)
            {
                _options[i,0].enabled = true;
                _options[i,1].enabled = true;
            }
            else
            {
                _options[i,0].enabled = false;
                _options[i,1].enabled = false;
            }
    }
    public delegate void OnSettings(int index);
    public static event OnSettings onDifficulty;
    public static event OnSettings onTheme;
    public static event OnSettings onVibration;
    public static int Theme
    {
        get { return _indexTheme; }
    }
    public static int Vibration
    {
        get { return _indexVibration; }
    }
}