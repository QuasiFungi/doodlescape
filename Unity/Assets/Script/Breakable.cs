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
    public void HealthModify(int value)
    {
        _healthInst = Mathf.Clamp(_healthInst + value, 0, _health);
        // * testing
        if (_healthInst == 0) Discard();
    }
}