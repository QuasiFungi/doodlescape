using UnityEngine;
// fortressDoorWide
public class ReactSequence : React
{
    [Tooltip("False: Lock (fail on input) | True: Trap (fail on sequence)")] [SerializeField] private bool _sequenceType = false;
    // ? gonna make lots of garbage
    private class CachePing
    {
        public int id { get; private set; }
        public bool value { get; private set; }
        public CachePing(int id, bool value)
        {
            this.id = id;
            this.value = value;
        }
    }
    // store signal for trap type activation
    private CachePing[] _cache;
    void Start()
    {
        // 
        _cache = new CachePing[_count];
    }
    //implicit sequence
    private int _next = 0;
    // ? doesnt account for case when press button A then B then A again ! patch by make button one way
    protected override void Ping(int id, bool value)
    {
        // trap
        if (_sequenceType)
        {
            // count if new source
            if (_cache[_next] == null) _next++;
            // store ping data
            _cache[_next - 1] = new CachePing(id, value);
            // ping recorded from all sources
            if (_next == _count)
            {
                // verify sequence
                for (int i = 0; i < _count; i++)
                    // ping valid
                    if (_cache[i].id == i)
                    {
                        // process signal
                        base.Ping(i, _cache[i].value);
                        // ping accepted
                        _next--;
                    }
                // signal fully processed
                if (_next == 0) return;
            }
            else return;
        }
        // lock
        // signal from next in sequence
        else if (id == _next)
        {
            // process signal
            base.Ping(id, value);
            // wait for next signal
            _next++;
            return;
        }
        // clear signal record
        for (int i = 0; i < _count; i++)
        {
            _signal[i] = false;
            _cache[i] = null;
        }
        // reset all sources ? show icon activate then deactive with half second delay for feedback other than vfx
        foreach (InteractReact source in _sources) source.Initialize();
        // // 
        // for (int i = 0; i <= _next; i++)
        // {
        //     // clear signal record
        //     _signal[i] = false;
        //     // reset all sources
        //     _sources[i].Initialize();
        // }
        // wait for first signal
        _next = 0;
    }
}