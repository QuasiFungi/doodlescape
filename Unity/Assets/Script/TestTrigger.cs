using UnityEngine;
using System.Collections.Generic;
// [RequireComponent(typeof(Rigidbody2D))]
public class TestTrigger : Entity
{
    protected List<Transform> _targets;
    protected SpriteRenderer _sprite;
    void Awake()
    {
        _targets = new List<Transform>();
        _sprite = GetComponent<SpriteRenderer>();
        GetComponent<Collider2D>().isTrigger = true;
        // GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }
    // void Update()
    // {
    //     // * testing
    //     if (_sprite)
    //         // _sprite.enabled = Vector3.Distance(transform.position, controller_player.Instance.Motor.Position) <= game_variables.Instance.RadiusSprite;
    //         _sprite.enabled = game_camera.Instance.InView(transform.position);
    // }
    public void SetActive(bool value)
    {
        if (gameObject.activeSelf != value)
            gameObject.SetActive(value);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // print(other.gameObject.name);
        // ignore repeat, non creature/breakable, trigger colliders
        if (_targets.Contains(other.transform) || (other.gameObject.layer != GameVariables.LayerCreature && other.gameObject.layer != GameVariables.LayerBreakable) || other.isTrigger) return;
        // target visible
        if (Physics2D.Raycast(transform.position, (other.transform.position - transform.position).normalized, Vector3.Distance(transform.position, other.transform.position), GameVariables.ScanLayerObstruction)) return;
        // exclude dead new
        if (other.gameObject.layer == GameVariables.LayerCreature)
        {
            // // * testing invisible
            // if (other.gameObject.layer == game_variables.Instance.LayerPlayer && other.GetComponent<data_player>().ModeInvisible)
            //     return;
            if (other.GetComponent<Creature>().HealthInst > 0)
                _targets.Add(other.transform);
        }
        else _targets.Add(other.transform);
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (_targets.Contains(other.transform))
            _targets.Remove(other.transform);
    }
    public List<Transform> Targets
    {
        get { return _targets; }
    }
}