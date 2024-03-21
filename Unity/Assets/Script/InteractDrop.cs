using UnityEngine;
// chest
// ? rename to InteractItem
public class InteractDrop : Interact
{
    // [Tooltip("Reference to item dropped on active")] [SerializeField] protected Item _drop = null;
    // place item on same tile as interact
    protected Item _drop = null;
    protected override void Start()
    {
        base.Start();
        // get item on top
        Collider2D hit = Physics2D.OverlapCircle(transform.position, .1f, LayerMask.GetMask("item"));
        // item detected ? multiple over same tile
        if (hit)
        {
            // store item reference
            _drop = hit.GetComponent<Item>();
            // hide item if exists/valid
            // _drop?.Hide();
            if (_drop) _drop.Hide();
            // throw error to notify dev
            // else print(gameObject.name + " : no assigned item drop");
            else Debug.Log(gameObject.name + "\t<color=red>Attempted to assign invalid item drop</color>", this);
        }
        // throw error to notify dev
        // else print(gameObject.name + " : no assigned item drop");
        else Debug.Log(gameObject.name + "\t<color=yellow>No assigned item drop</color>", this);
    }
    public override void TryAction(Creature source)
    {
        // try opening chest
        base.TryAction(source);
        // if open success
        if (IsActivated)
        {
            // allow pickup action on dropped item
            Deactivate();
            // show item if exists
            _drop?.Reveal();
        }
    }
}