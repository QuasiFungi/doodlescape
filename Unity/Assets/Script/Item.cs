using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Item : Entity
{
    // protected int _type;
    // public string _id;
    // // ? used by drop type interact only
    // public void Reveal()
    // {
    //     gameObject.SetActive(true);
    // }
    // * testing, used by player to drop item
    // public void Reveal(Vector2 position)
    // {
    //     // move to player position
    //     // transform.position = new Vector3(position.x, position.y, GameVariables.LayerItem);
    //     transform.position = new Vector3(position.x, position.y, transform.position.z);
    //     // 
    //     gameObject.SetActive(true);
    // }
    public void Show(Vector2 position)
    {
        // move to player position
        transform.position = new Vector3(position.x, position.y, transform.position.z);
        // 
        Show();
    }
    // public string ID
    // {
    //     get { return _id; }
    // }
    public Sprite Icon
    {
        get { return transform.GetComponent<SpriteRenderer>().sprite; }
    }
}