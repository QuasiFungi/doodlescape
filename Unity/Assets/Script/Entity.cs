using UnityEngine;
using System.Collections;
public class Entity : MonoBehaviour
{
    // protected Vector2 _position;
    [Tooltip("unique identifier for this entity")] [SerializeField] protected string _id;
    // protected Sprite[] _sprites;
    // protected string[] _sfxIDs;
    // protected string[] _vfxIDs;
    // protected Collider2D _collider;
    protected virtual void Hide()
    {
        gameObject.SetActive(false);
    }
    protected virtual void Show()
    {
        gameObject.SetActive(true);
    }
    public virtual void Discard()
    {
        // unsubscribe from all events ? child or parent hide called
        // gameObject.SetActive(false);
        Hide();
        // ? scale with tick duration
        Destroy(gameObject, 1f);
    }
    // * testing chunk loading, just a wrapper for show/hide
    public void ToggleActive(bool state)
    {
        if (state) Show();
        else Hide();
    }
    public string ID
    {
        get { return _id; }
    }
}