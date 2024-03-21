using UnityEngine;
public class Entity : MonoBehaviour
{
    protected Vector2 _position;
    // protected int _id;
    // protected Sprite[] _sprites;
    // protected string[] _sfxIDs;
    // protected string[] _vfxIDs;
    // protected Collider2D _collider;
    public void Hide()
    {
        // Destroy(gameObject);
        gameObject.SetActive(false);
    }
    public void Discard()
    {
        Destroy(gameObject);
    }
}