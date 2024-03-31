using UnityEngine;
public class BaseHitbox : MonoBehaviour
{
    // reference to entity that spawned this
    protected Breakable _source;
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
        Destroy(gameObject);
    }
}