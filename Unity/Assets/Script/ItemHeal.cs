using UnityEngine;
public class ItemHeal : Item
{
    [Tooltip("amount of health restored per use")] [SerializeField] private int _health = 1;
    [Tooltip("maximum number of uses")] [SerializeField] private int _uses = 1;
    private int _count;
    void Start()
    {
        // ? load from save file
        _count = _uses;
    }
    // public int Consume()
    public bool Consume()
    {
        // mark usage
        _count--;
        // uses depleted
        // if (_count == 0) Discard();
        return _count == 0;
        // // inform of the health amount to restore ? done this way to keep code in single line
        // return _health;
    }
    public int Health
    {
        get { return _health; }
    }
}