using UnityEngine;
using UnityEngine.UI;
public class TestUITheme : MonoBehaviour
{
    private static Color[,] _themes;
    public Image[] _toneAImg;
    public SpriteRenderer[] _toneASprite;
    public Image[] _toneBImg;
    public Text[] _toneBTxt;
    private static int _index;
    void Awake()
    {
        // _index = 0;
        // 
        _themes = new Color[9*2,2];
        // light gray | dark gray
        _themes[0,0] = new Color(.25f, .25f, .25f);
        _themes[0,1] = new Color(.75f, .75f, .75f);
        _themes[1,0] = _themes[0,1];
        _themes[1,1] = _themes[0,0];
        // light green | dark green
        _themes[2,0] = new Color(0.592156862745098f, 0.7372549019607844f, 0.3843137254901961f);
        _themes[2,1] = new Color(0.17254901960784313f, 0.37254901960784315f, 0.17647058823529413f);
        _themes[3,0] = _themes[2,1];
        _themes[3,1] = _themes[2,0];
        // yellow | grey
        _themes[4,0] = new Color(0.06274509803921569f, 0.09411764705882353f, 0.12549019607843137f);
        _themes[4,1] = new Color(0.996078431372549f, 0.9058823529411765f, 0.08235294117647059f);
        _themes[5,0] = _themes[4,1];
        _themes[5,1] = _themes[4,0];
        // peach | purple
        _themes[6,0] = new Color(0.37254901960784315f, 0.29411764705882354f, 0.5450980392156862f);
        _themes[6,1] = new Color(0.9019607843137255f, 0.6039215686274509f, 0.5529411764705883f);
        _themes[7,0] = _themes[6,1];
        _themes[7,1] = _themes[6,0];
        // orange | blue
        _themes[8,0] = new Color(0.3568627450980392f, 0.5176470588235295f, 0.6941176470588235f);
        _themes[8,1] = new Color(0.9882352941176471f, 0.4627450980392157f, 0.41568627450980394f);
        _themes[9,0] = _themes[8,1];
        _themes[9,1] = _themes[8,0];
        // green | blue
        _themes[10,0] = new Color(0f, 0.12549019607843137f, 0.24705882352941178f);
        _themes[10,1] = new Color(0.6784313725490196f, 0.9372549019607843f, 0.8196078431372549f);
        _themes[11,0] = _themes[10,1];
        _themes[11,1] = _themes[10,0];
        // green | pink
        _themes[12,0] = new Color(0.796078431372549f, 0.807843137254902f, 0.5686274509803921f);
        _themes[12,1] = new Color(0.9176470588235294f, 0.45098039215686275f, 0.5529411764705883f);
        _themes[13,0] = _themes[12,1];
        _themes[13,1] = _themes[12,0];
        // blue | white
        _themes[14,0] = new Color(0.5372549019607843f, 0.6705882352941176f, 0.8901960784313725f);
        _themes[14,1] = new Color(0.9882352941176471f, 0.9647058823529412f, 0.9607843137254902f);
        _themes[15,0] = _themes[14,1];
        _themes[15,1] = _themes[14,0];
        // purple | pale blue
        _themes[16,0] = new Color(0.3764705882352941f, 0.24705882352941178f, 0.5137254901960784f);
        _themes[16,1] = new Color(0.7803921568627451f, 0.8274509803921568f, 0.8313725490196079f);
        _themes[17,0] = _themes[16,1];
        _themes[17,1] = _themes[16,0];
        // // sample | sample
        // _themes[,0] = new Color(f, f, f);
        // _themes[,1] = new Color(f, f, f);
        // _themes[,0] = _themes[,1];
        // _themes[,1] = _themes[,0];
        // bound to event for scene duration
        UISettings.onTheme += DoTheme;
        // GameMaster.onStartupSettings += Initialize;
        GameData.onReset += Initialize;
        // 
        Initialize();
    }
    void OnDestroy()
    {
        // unsubcribe when reload scene
        UISettings.onTheme -= DoTheme;
        // GameMaster.onStartupSettings -= Initialize;
        GameData.onReset -= Initialize;
    }
    // wrapper
    private void Initialize()
    {
        // _index = PlayerPrefs.GetInt("theme", 0);
        _index = GameData.Theme;
        DoTheme(_index);
    }
    public delegate void OnUpdate();
    public static event OnUpdate onUpdate;
    // 
    private void DoTheme(int index)
    {
        index = Mathf.Clamp(index, 0, _themes.GetLength(0));
        _index = index;
        for (int i = _toneAImg.GetLength(0) - 1; i > -1; i--) if (_toneAImg[i]) _toneAImg[i].color = _themes[index, 0];
        for (int i = _toneASprite.GetLength(0) - 1; i > -1; i--) if (_toneASprite[i]) _toneASprite[i].color = _themes[index, 0];
        for (int i = _toneBImg.GetLength(0) - 1; i > -1; i--) if (_toneBImg[i]) _toneBImg[i].color = _themes[index, 1];
        for (int i = _toneBTxt.GetLength(0) - 1; i > -1; i--) if (_toneBTxt[i]) _toneBTxt[i].color = _themes[index, 1];
        // notify buttons of theme change
        onUpdate?.Invoke();
    }
    // public static Color ToneB
    // {
    //     get { return _themes[_index,1]; }
    // }
    // public static Color ToneA
    // {
    //     get { return _themes[_index,0]; }
    // }
}