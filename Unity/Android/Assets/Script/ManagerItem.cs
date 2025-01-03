using UnityEngine;
public class ManagerItem : MonoBehaviour
{
    public static ManagerItem Instance;
    private Item[] _items;
    void Awake()
    {
        // declare singleton
        if (Instance) Destroy(gameObject);
        else Instance = this;
        // 
        _items = new Item[transform.childCount];
        for (int i = _items.Length - 1; i > -1; i--)
            _items[i] = transform.GetChild(i).GetComponent<Item>();
    }
    public Item GetItemByIDUnique(string idUnique)
    {
        for (int i = _items.Length - 1; i > -1; i--)
            if (_items[i].IDUnique == idUnique) return _items[i];
        return null;
    }
    // * testing null error on reload
    void OnDestroy()
    {
        Instance = null;
    }
}