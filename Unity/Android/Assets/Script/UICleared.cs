using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class UICleared : BaseMenu
{
    [Header("Cleared")]
    private Image _bg;
    public Sprite[] _frames;
    private GameObject[] _trophies;
    private Text _tickNow;
    private Text _tickBest;
    [SerializeField] private AudioClip _sfxCycle;
    void Awake()
    {
        _bg = transform.GetComponent<Image>();
        _trophies = new GameObject[6];
        for (int i = 0; i < 6; i++)
        {
            _trophies[i] = transform.GetChild(i).gameObject;
            _trophies[i].SetActive(false);
        }
        _tickNow = transform.GetChild(6).GetComponent<Text>();
        _tickBest = transform.GetChild(7).GetComponent<Text>();
        _tickNow.text = "";
        _tickBest.text = "";
        // Player.onClear += OnClear;
        // GameMaster.onClear += OnClear;
        _skipSFX = true;
        gameObject.SetActive(false);
    }
    // private void OnClear(Vector2Int room)
    // {
    //     gameObject.SetActive(true);
    //     // 
    //     StartCoroutine("AnimIn");
    // }
    protected override void OnEnable()
    {
        base.OnEnable();
        // 
        StartCoroutine("AnimIn");
    }
    private float _animSpeed = .5f;
    IEnumerator AnimIn()
    {
        // fade alpha out over tick duration ? account for dynamic tick size
        Color colorA = _bg.color;
        // Color colorA = TestUITheme.ToneB;
        Color colorB = _bg.color;
        // Color colorB = TestUITheme.ToneB;
        colorB.a = 1f;
        for(float t = 0f; t < 1f; t += Time.deltaTime)
        {
            _bg.color = Color.Lerp(colorA, colorB, t);
            yield return null;
        }
        // 
        for (int i = 0; i < _frames.Length; i++)
        {
            yield return new WaitForSeconds(_animSpeed);
            // * testing sfx entry cycle
            if (_sfxCycle != null) GameAudio.Instance?.Register(_sfxCycle, GameAudio.AudioType.UI);
            // 
            _bg.sprite = _frames[i];
            // 
            string time, zeros;
            if (i == 0)
            {
                time = GameData.Ticks.ToString();
                zeros = "";
                for(int j = 5 - time.Length; j > 0; j--) zeros += "0";
                _tickNow.text = zeros + time;
            }
            if (i == 1)
            {
                time = GameData.TicksBest.ToString();
                zeros = "";
                for(int j = 5 - time.Length; j > 0; j--) zeros += "0";
                _tickBest.text = zeros + time;
            }
            // * testing trophy
            if (i == 3)
            {
                if (GameData.IsTrophyNinja) _trophies[0].SetActive(true);
                if (GameData.IsTrophySlayer) _trophies[1].SetActive(true);
            }
            else if (i == 4)
            {
                if (GameData.IsTrophyPacifist) _trophies[2].SetActive(true);
                if (GameData.IsTrophyPlunderer) _trophies[3].SetActive(true);
            }
            else if (i == 5)
            {
                if (GameData.IsTrophyGhost) _trophies[4].SetActive(true);
                if (GameData.IsTrophyRaider) _trophies[5].SetActive(true);
            }
        }
        // ? auto switch to scoreboard
        yield return new WaitForSeconds(2f);
        DoComplete();
    }
}