using UnityEngine;
// used by spikeSnail
public class HitboxPattern : BaseHitbox
{
    // assumes square grid of odd size ? case for rectangle, sparce matrix
    [Tooltip("Pattern sequence for attack")] [TextAreaAttribute] [SerializeField] string[] _pattern;
    // file format:
    // pattern 1
    // pattern 2
    // ...
    // pattern n
    private bool[,,] _sequence;
    [Tooltip("Damage effect to spawn in pattern")] [SerializeField] private GameObject _attack;
    // [Tooltip("Rotate hitbox sprites out from center")] [SerializeField] private bool _useRotation;
    protected override void Awake()
    {
        base.Awake();
        // 
        int size = _pattern[0].Split('\n').Length;
        int step = _pattern.Length;
        string[] lines = new string[size * step];
        for (int i = step - 1; i > -1; i--)
        {
            string[] pattern = _pattern[i].Split('\n');
            for (int j = pattern.Length - 1; j > -1; j--) lines[i * size + j] = pattern[j];
        }
        _sequence = new bool[size, size, step];
        string[] lineData;
        int value;
        for (int index = lines.Length - 1; index > -1; index--)
        {
            lineData = lines[index].Trim().Split(',');
            for (int character = lineData.Length - 1; character > -1; character--)
            {
                int.TryParse(lineData[character], out value);
                _sequence[character, size - 1 - (index % size), (index / size) % step] = value == 1;
            }
        }
        _index = 0;
        _offset = (_sequence.GetLength(0) - 1) / 2f;
        // process one tick immediately
        Iterate();
        // _testTime = Time.time;
    }
    // // * testing
    // public int _testPattern = -1;
    // void OnDrawGizmosSelected()
    // {
    //     if (_sequence == null || _testPattern < 0 || _testPattern >= _sequence.GetLength(2)) return;
    //     // 
    //     Gizmos.color = new Color(1f, 0f, 0f, .5f);
    //     float offset = (_sequence.GetLength(0) - 1) / 2f;
    //     for (int y = _sequence.GetLength(1) - 1; y > -1; y--)
    //         for (int x = _sequence.GetLength(0) - 1; x > -1; x--)
    //             if (_sequence[x, y, _testPattern]) Gizmos.DrawSphere(transform.position + new Vector3(x - offset, y - offset), .5f);
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
    // * testing ? use enum instead
    public bool _testRotation;
    // private float _testTime;
    private void Iterate()
    {
        // // 
        // print(Time.time - _testTime);
        // _testTime = Time.time;
        // // * testing
        // if (_testPattern > -1) return;
        // 
        if (_index == _sequence.GetLength(2))
        {
            Discard();
            // * testing, object pooling ? use toggleActive instead ? override discard in base class to use pool since explicit call every time feels iffy
            // ManagerPool.PooledRelease(this as Entity);
            return;
        }
        // 
        GameObject attack;
        for (int y = _sequence.GetLength(1) - 1; y > -1; y--)
            for (int x = _sequence.GetLength(0) - 1; x > -1; x--)
                if (_sequence[x, y, _index])
                {
                    Vector3 offset = transform.right * (x - _offset) + transform.up * (y - _offset) + Vector3.forward * -2f;
                    attack = Instantiate(_attack, transform.position + offset, transform.rotation, transform);
                    // attack = Instantiate(_attack, transform.position + offset, Quaternion.LookRotation(Vector3.up, offset), transform);
                    if (_testRotation) attack.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg - 90f);
                    // 
                    attack.GetComponent<BaseHitbox>().Initialize(_source, _target);
                    // // ? why damage disabled by default, not use toggle type attacks..?
                    // attack.SetActive(true);
                }
        // 
        _index++;
    }
}