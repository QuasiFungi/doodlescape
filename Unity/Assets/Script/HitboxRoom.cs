using UnityEngine;
using System.Collections.Generic;
// charm, bell
public class HitboxRoom : BaseHitbox
{
    [Header("Room")]
    // [Range(0, 5)] [SerializeField] private int _radius;
    [SerializeField] private GameObject _effect;
    protected override void Start()
    {
        base.Start();
        // get all entities in active domain(s)
        List<Entity> entities = ManagerChunk.Instance.GetEntites();
        // 
        for (int i = entities.Count - 1; i > -1; i--)
        {
            // ignore invalid, non creatures and itself 
            if (entities[i] == null || entities[i].gameObject.layer != GameVariables.LayerCreature || entities[i] == _source as Entity) continue;
            // ? use breakable position
            Vector3 position = GameVariables.PositionDamage(entities[i].Position);
            // 
            GameObject temp = Instantiate(_effect, position, transform.rotation);
            // 
            temp.GetComponent<BaseHitbox>().Initialize(_source, position, _domain);
            // 
            temp.SetActive(true);
        }
    }
    // scale visiblity with tick duration ? overkill
    protected override void OnEnable()
    {
        base.OnEnable();
        // 
        GameClock.onTickUIEarly += Tick;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        // 
        GameClock.onTickUIEarly -= Tick;
    }
    // ? hard coded
    void Tick()
    {
        Discard();
    }
}