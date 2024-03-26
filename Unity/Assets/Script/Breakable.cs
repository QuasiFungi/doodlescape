using UnityEngine;
// rubble
public class Breakable : Entity
{
    [SerializeField] protected int _health = 1;
    protected int _healthInst;
    // protected GameObject[] _loot;
    // protected void InitializeHealth()
    // {
    //     _healthInst = _health;
    // }
    protected virtual void Start()
    {
        // ? save load
        _healthInst = _health;
    }
    // 
    public virtual void HealthModify(int value, Creature source)
    {
        _healthInst = Mathf.Clamp(_healthInst + value, 0, _health);
        // * testing
        if (_healthInst == 0) Discard();
        // if (_healthInst == 0) Hide();
    }
    public float HealthInst
    {
        get { return _healthInst; }
    }
}