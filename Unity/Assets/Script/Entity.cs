using UnityEngine;
using UnityEngine.UI;
public class Entity : MonoBehaviour
{
    protected Vector2 _position;
    // protected int _id;
    // protected Sprite[] _sprites;
    // protected GameObject[] _sfx;
    // protected GameObject[] _vfx;
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