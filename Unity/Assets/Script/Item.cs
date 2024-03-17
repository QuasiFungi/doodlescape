using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(SpriteRenderer))]
public class Item : Entity
{
    // protected int _type;
    public string _id;
    public string ID
    {
        get { return _id; }
    }
    public Sprite Icon
    {
        get { return transform.GetComponent<SpriteRenderer>().sprite; }
    }
}