using UnityEngine;
// using System.IO;
public class HitboxPattern : BaseHitbox
{
    // assumes square grid of odd size ? case for rectangle, sparce matrix
    [Tooltip("Pattern sequence for attack")] [SerializeField]  TextAsset _file;
    // file format:
    // pattern 1
    // pattern 2
    // ...
    // pattern n
    private bool[,,] _pattern;
    // [Tooltip("Rotate hitbox sprites out from center")] [SerializeField] private bool _useRotation;
    [Tooltip("Damage effect to spawn in pattern")] [SerializeField] private GameObject _attack;
    protected override void Awake()
    {
        base.Awake();
        // var fileData : String  = System.IO.File.ReadAllText(path);
        // string fileData = System.IO.File.ReadAllText(path);
        // string fileData = _file.text;
        // var lines : String[] = fileData.Split('\n');
        // string[] lines = fileData.Split('\n');
        string[] lines = _file.text.Split('\n');
        // var lineData : String = (lines[0].Trim()).Split(',');
        // string lineData = (lines[0].Trim()).Split(',');
        // var x : float;
        // int x;
        // 
        // int.TryParse(lineData[0], x);
        // foreach (string line in lines)
        // {
        //     string[] lineData = (line.Trim()).Split(',');
        //     // print(line);
        // }
        // 
        int size = lines[0].Trim().Split(',').Length;
        int step = lines.Length / size;
        _pattern = new bool[size, size, step];
        // for (int p = lines.Length - 1; p > -1; p--)
        // {
        //     string[] lineData = (lines[p].Trim()).Split(',');
        // }
        // for (int z = _pattern.Length(2) - 1; z > -1; z--)
        //     for (int y = _pattern.Length(1) - 1; z > -1; z--)
        //         for (int x = _pattern.Length(0) - 1; z > -1; z--)
        //         {
        //             // 
        //         }
        string[] lineData;
        // for (int z = _pattern.Length(2) - 1; z > -1; z--)
        // {
        //     lineData = (lines[z].Trim()).Split(',');
        // }
        int value;
        for (int line = lines.Length - 1; line > -1; line--)
        {
            lineData = lines[line].Trim().Split(',');
            for (int character = lineData.Length - 1; character > -1; character--)
            {
                int.TryParse(lineData[character], out value);
                _pattern[character, line % size, line % step] = value == 1;
            }
        }
        // print(_pattern[1,1,0]);
        _index = 0;
        _offset = (_pattern.GetLength(0) - 1) / 2f;
        // process one tick immediately
        Iterate();
    }
    // void OnDrawGizmos()
    // {
    //     if (_pattern == null) return;
    //     // 
    //     Gizmos.color = new Color(1f, 0f, 0f, .1f);
    //     // for (int z = _pattern.Length(2) - 1; z > -1; z--)
    //     float offset = (_pattern.GetLength(0) - 1) / 2f;
    //     for (int y = _pattern.GetLength(1) - 1; y > -1; y--)
    //         for (int x = _pattern.GetLength(0) - 1; x > -1; x--)
    //             if (_pattern[x, y, 0]) Gizmos.DrawSphere(transform.position + new Vector3(x - offset, y - offset), .5f);
    // }
    void OnEnable()
    {
        GameClock.onTick += Iterate;
    }
    void OnDisable()
    {
        GameClock.onTick -= Iterate;
    }
    private int _index;
    private float _offset;
    private void Iterate()
    {
        if (_index == _pattern.GetLength(2))
        {
            Discard();
            return;
        }
        // 
        GameObject attack;
        for (int y = _pattern.GetLength(1) - 1; y > -1; y--)
            for (int x = _pattern.GetLength(0) - 1; x > -1; x--)
                if (_pattern[x, y, _index])
                {
                    attack = Instantiate(_attack, transform.position + new Vector3(x - _offset, y - _offset), transform.rotation, transform);
                    // 
                    attack.GetComponent<BaseHitbox>().Initialize(_source);
                    // // ? why damage disabled by default, not use toggle type attacks..?
                    // attack.SetActive(true);
                }
        // 
        _index++;
    }
}