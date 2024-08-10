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
    // // by default everything is inactive, otherwise active become mega active
    // private int _isActive = 0;
    // public void ToggleActive(bool isActive)
    // {
    //     _isActive += isActive ? 1 : -1;
    //     // 
    //     if (_isActive > 0) gameObject.SetActive(true);
    //     // if (IsActive) gameObject.SetActive(true);
    //     else Hide();
    // }
    public void ToggleActive(bool state)
    {
        if (state) Show();
        else Hide();
    }
    public string ID
    {
        get { return _id; }
    }
    // protected bool IsActive
    // {
    //     get { return _isActive > 0; }
    // }
}