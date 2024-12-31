using UnityEngine;
using UnityEngine.UI;
public class UIAdvertisement : BaseMenu
{
    [Header("Advertisement")]
    private int _option;
    // private Image _selected;
    // public Sprite[] _options;
    private Image[,] _options;
    private Text _txtBanner;
    private Text _txtPopUp;
    private Text _txtVideo;
    private int _indexBanner;
    private int _indexPopUp;
    private int _indexVideo;
    [SerializeField] private string[] _valueBanner;
    [SerializeField] private string[] _valuePopUp;
    [SerializeField] private string[] _valueVideo;
    void Awake()
    {
        // 
        // _option = _options.Length - 1;
        // 
        // _selected = transform.GetChild(1).GetComponent<Image>();
        // 
        _txtBanner = transform.GetChild(1).GetComponent<Text>();
        _txtPopUp = transform.GetChild(2).GetComponent<Text>();
        _txtVideo = transform.GetChild(3).GetComponent<Text>();
        // ? move to start
        _indexBanner = 0;
        _indexPopUp = 0;
        _indexVideo = 0;
        _txtBanner.text = _valueBanner[_indexBanner];
        _txtPopUp.text = _valuePopUp[_indexPopUp];
        _txtVideo.text = _valueVideo[_indexVideo];
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
        GameInput.onTap += DoAdvertisement;
        // 
        GameClock.onTickUIEarly += DoHealth;
        // start disabled
        _skipSFX = true;
        gameObject.SetActive(false);
    }
    void OnDestroy()
    {
        // unsubcribe when reload scene
        GameInput.onTap -= DoAdvertisement;
        // 
        GameClock.onTickUIEarly -= DoHealth;
    }
    private void DoAdvertisement(int typeButton, int typeInput, int index)
    {
        // only ads button
        if (typeButton != 5) return;
        // 
        // tap
        else if (typeInput == 0)
        {
            // open ads
            if (index == 0)
            {
                gameObject.SetActive(true);
                // notify manager ui
                DoComplete();
                // ? check ad availability
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
                // banner
                if (_option == 0)
                {
                    // disabled
                    if (_indexBanner == 2)
                    {
                        // * testing sfx menu fail
                        GameAudio.Instance.Register(14, GameAudio.AudioType.UI);
                        return;
                    }
                    // cycle
                    _indexBanner--;
                    // loop
                    if (_indexBanner == -1) _indexBanner = 1;
                    // display
                    _txtBanner.text = _valueBanner[_indexBanner];
                    // // notify game clock
                    // onBanner?.Invoke(_indexBanner);
                    // * testing sfx menu modify
                    GameAudio.Instance.Register(12, GameAudio.AudioType.UI);
                    // * testing heals
                    DoBanner();
                }
                // popup
                else if (_option == 1)
                {
                    // disabled
                    if (_indexPopUp != 0)
                    {
                        // * testing sfx menu fail
                        GameAudio.Instance.Register(14, GameAudio.AudioType.UI);
                        return;
                    }
                    // cycle
                    _indexPopUp--;
                    // loop
                    if (_indexPopUp == -1) _indexPopUp = 1;
                    // display
                    _txtPopUp.text = _valuePopUp[_indexPopUp];
                    // // notify game clock
                    // onPopUp?.Invoke(_indexPopUp);
                    // * testing sfx menu modify
                    GameAudio.Instance.Register(12, GameAudio.AudioType.UI);
                    // * testing heals
                    DoPopUp();
                }
                // video
                else if (_option == 2)
                {
                    // disabled
                    if (_indexVideo != 0)
                    {
                        // * testing sfx menu fail
                        GameAudio.Instance.Register(14, GameAudio.AudioType.UI);
                        return;
                    }
                    // cycle
                    _indexVideo--;
                    // loop
                    if (_indexVideo == -1) _indexVideo = 1;
                    // display
                    _txtVideo.text = _valueVideo[_indexVideo];
                    // // notify game clock
                    // onVideo?.Invoke(_indexVideo);
                    // * testing sfx menu modify
                    GameAudio.Instance.Register(12, GameAudio.AudioType.UI);
                    // * testing heals
                    DoVideo();
                }
                // apply
                else if (_option == 3)
                {
                    DoComplete();
                    onApply?.Invoke(0);
                }
                // // default
                // if (_option == 4) print("game restart");
            }
            // cycle right
            else if (index == 2)
            {
                // banner
                if (_option == 0)
                {
                    // disabled
                    if (_indexBanner == 2)
                    {
                        // * testing sfx menu fail
                        GameAudio.Instance.Register(14, GameAudio.AudioType.UI);
                        return;
                    }
                    // cycle
                    _indexBanner++;
                    // loop
                    if (_indexBanner == _valueBanner.Length - 1) _indexBanner = 0;
                    // display
                    _txtBanner.text = _valueBanner[_indexBanner];
                    // // notify 
                    // onBanner?.Invoke(_indexBanner);
                    // * testing sfx menu modify
                    GameAudio.Instance.Register(12, GameAudio.AudioType.UI);
                    // * testing heals
                    DoBanner();
                }
                // popup
                else if (_option == 1)
                {
                    // disabled
                    if (_indexPopUp != 0)
                    {
                        // * testing sfx menu fail
                        GameAudio.Instance.Register(14, GameAudio.AudioType.UI);
                        return;
                    }
                    // cycle
                    _indexPopUp++;
                    // loop
                    if (_indexPopUp == _valuePopUp.Length - 1) _indexPopUp = 0;
                    // display
                    _txtPopUp.text = _valuePopUp[_indexPopUp];
                    // // notify 
                    // onPopUp?.Invoke(_indexPopUp);
                    // * testing sfx menu modify
                    GameAudio.Instance.Register(12, GameAudio.AudioType.UI);
                    // * testing heals
                    DoPopUp();
                }
                // video
                else if (_option == 2)
                {
                    // disabled
                    if (_indexVideo != 0)
                    {
                        // * testing sfx menu fail
                        GameAudio.Instance.Register(14, GameAudio.AudioType.UI);
                        return;
                    }
                    // cycle
                    _indexVideo++;
                    // loop
                    if (_indexVideo == _valueVideo.Length - 1) _indexVideo = 0;
                    // display
                    _txtVideo.text = _valueVideo[_indexVideo];
                    // // notify 
                    // onVideo?.Invoke(_indexVideo);
                    // * testing sfx menu modify
                    GameAudio.Instance.Register(12, GameAudio.AudioType.UI);
                    // * testing heals
                    DoVideo();
                }
                // apply
                else if (_option == 3)
                {
                    DoComplete();
                    onApply?.Invoke(0);
                }
                // // default
                // if (_option == 4) print("game restart");
            }
            // // reset save
            // else if (index == 4) 
            // {
            //     // gameObject.SetActive(false);
            //     // // notify manager ui
            //     // OnComplete();
            //     print("game restart");
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
    public delegate void OnAdvertisement(int index);
    // public static event OnAdvertisement onBanner;
    // public static event OnAdvertisement onPopUp;
    // public static event OnAdvertisement onVideo;
    public static event OnAdvertisement onApply;
    // ? cooldowns
    private void DoBanner()
    {
        // if valid time period
        if (_indexBanner == 1 && GameClock.Ticks >= GameData.AdBanner)
        {
            // apply heal 
            Player.Instance.HealthModify(1, Player.Instance as Creature);
            // move timer forward
            GameData.AdBanner = GameClock.Ticks + 60;
        }
    }
    private void DoPopUp()
    {
        if (_indexPopUp == 1)
        {
            // move timer forward
            GameData.AdPopup = GameClock.Ticks + 120;
            // apply heal
            Player.Instance.HealthModify(3, Player.Instance as Creature);
        }
    }
    private void DoVideo()
    {
        if (_indexVideo == 1)
        {
            // move timer forward
            GameData.AdVideo = GameClock.Ticks + 180;
            // apply heal
            Player.Instance.HealthModify(5, Player.Instance as Creature);
        }
    }
    // called every tick
    void DoHealth()
    {
        // ? show in sequence in case multiple enabled
        if (_indexBanner == 1 && GameClock.Ticks >= GameData.AdBanner)
        {
            // apply heal
            Player.Instance.HealthModify(1, Player.Instance as Creature);
            // forward timer
            GameData.AdBanner = GameClock.Ticks + 60;
        }
        // Make available again
        if (_indexPopUp == 1 && GameClock.Ticks >= GameData.AdPopup)
        {
            _indexPopUp = 0;
            // display
            _txtPopUp.text = _valuePopUp[_indexPopUp];
        }
        // Make available again
        if (_indexVideo == 1 && GameClock.Ticks >= GameData.AdVideo)
        {
            _indexVideo = 0;
            // display
            _txtVideo.text = _valueVideo[_indexVideo];
        }
    }
}