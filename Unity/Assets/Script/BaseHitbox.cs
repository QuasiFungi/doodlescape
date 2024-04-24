using UnityEngine;
public class BaseHitbox : MonoBehaviour
{
    // reference to entity that spawned this
    protected Breakable _source;
    // ? not all hitboxes have/need sprite, move to hitboxDamage
    protected SpriteRenderer _renderer;
    protected virtual void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
    public void Initialize(Breakable source)
    {
        _source = source;
    }
    protected void Discard()
    {
        // * testing
        gameObject.SetActive(false);
        // allow unsubscribe from tick events
        Destroy(gameObject, 1f);
    }
}