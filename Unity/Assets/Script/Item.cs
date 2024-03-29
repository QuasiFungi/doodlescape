using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Item : Entity
{
    // protected int _type;
    public string _id;
    // ? used by drop type interact only
    public void Reveal()
    {
        gameObject.SetActive(true);
    }
    public string ID
    {
        get { return _id; }
    }
    public Sprite Icon
    {
        get { return transform.GetComponent<SpriteRenderer>().sprite; }
    }
}