using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// rubble
public class Breakable : Entity
{
    [Header("Breakable")]
    [SerializeField] protected int _health = 1;
    protected int _healthInst;
    // protected GameObject[] _loot;
    // protected void InitializeHealth()
    // {
    //     _healthInst = _health;
    // }
    // ? taken from anim or entity
    protected SpriteRenderer _sprite;
    private Color _colorHurt;
    private Color _colorHeal;
    private Color _colorDefault;
    // ? multiple colliders as in case of mob/boss must be assigned explicitly
    protected List<Collider2D> _colliders;
    protected override void Awake()
    {
        base.Awake();
        // 
        // * testing walk animation ? move to anim
        _sprite = _body.GetComponent<SpriteRenderer>();
        // flash red when hurt
        _colorHurt = new Color(1f, 0f, 0f, 1f);
        // flash white when heal
        _colorHeal = new Color(1f, 1f, 1f, 1f);
        // ? conflict with BT color assign, wait few frames for BT execution
        // remember current color
        _colorDefault = _sprite.color;
        // * testing ? move to motor
        _colliders = new List<Collider2D>();
        _colliders.Add(GetComponent<Collider2D>());
        // * testing ? lots of unparented bodies..? case for collider as child?
        // if (_body.parent)
        // ? have bool variable for toggle later on
        if (_body != transform)
        {
            // for readability
            _body.name += "-" + gameObject.name;
            // print(_body.name + ":\t" + _body.position);
            // keep all bodies under one parent to keep inspector clean ? inelegant
            if (transform.parent != null) _body.SetParent(transform.parent.parent.GetChild(0));
            // case for testing entities
            else _body.SetParent(null);
            // // dunno why it moves one unit up in z space ? made do with changing player spriteRenderer layer order, not proper fix
            // _body.localPosition = new Vector3(_body.localPosition.x, _body.localPosition.y, 0f);
            // print(_body.name + ":\t" + _body.position);
        }
    }
    protected override void DataLoad()
    {
        // * testing save/load
        Vector3 position, rotation;
        bool isActive;
        // use defaults if no data found
        if (!GameData.DataLoadBreakable(IDUnique, out position, out rotation, out isActive, out _lootID, out _healthInst))
            // position rotation isActive already assigned correctly
            _healthInst = _health;
        // apply loaded values
        else
        {
            SetPosition(position);
            SetRotation(rotation);
            // ToggleActive(isActive);
            // if holding an item
            if (_lootID != "")
                // try retrieving item from manager ? null on fail
                _loot = ManagerItem.Instance.GetItemByIDUnique(_lootID);
        }
        // apply dead state
        // if (IsDead) Hide();
        // if (IsDead) ToggleActive(false);
        // print(IDUnique + ": " + isActive + ", " + !IsDead);
        ToggleActive(isActive && !IsDead);
    }
    protected override void DataSave()
    {
        GameData.DataSaveBreakable(IDUnique, Position, Rotation, IsActive, _lootID, _healthInst);
    }
    public AudioClip _sfxHurt, _sfxDead, _sfxMove;
    public delegate void OnDead();
    public static event OnDead onDead;
    public static event OnDead onAlive;
    // 
    public virtual void HealthModify(int value, Creature source)
    {
        // apply health modifier
        _healthInst = Mathf.Clamp(_healthInst + value, 0, _health);
        // * testing ? coroutine being called after dead... unknown behaviour with mob
        StopCoroutine("Flash");
        // harmed
        if (value < 0f)
        {
            StartCoroutine(Flash(_colorHurt));
            // * testing trophy
            if (ID == "player")
            {
                GameData.SetTrophy(GameData.Trophy.GHOST);
                // * testing vibrate
                GameData.DoVibrate(GameData.IntensityVibrate.HIGH);
                // // * testing sfx player hurt
                // GameAudio.Instance.Register(6);
                // * testing item tunic
                Creature player = (this as Creature);
                int id = player.ItemGet("item_tunic");
                // one or more tunic held
                if (id > -1)
                {
                    // revert health modifier
                    _healthInst = Mathf.Clamp(_healthInst - value, 0, _health);
                    // discard if uses depleted
                    if ((player.ItemGet(id) as ItemConsume).Consume()) player.ItemRemove("item_tunic");
                    // // inform player of effect
                    // else Teleprompter.Register("Tunic absorbed hit");
                    // ? abort further calculation
                }
            }
            // * testing trophy
            else if (gameObject.layer == GameVariables.LayerCreature) GameData.SetTrophy(GameData.Trophy.PACIFIST);
            // * testing sfx hurt
            if (_sfxHurt != null) GameAudio.Instance.Register(_sfxHurt, GameAudio.AudioType.ENTITY);
        }
        // healed
        else if (value > 0f)
        {
            StartCoroutine(Flash(_colorHeal));
            // * testing menu dead, only true when resurrected
            if (_healthInst == value)
            {
                // 
                onAlive?.Invoke();
                // 
                ToggleActive(true);
            }
        }
        // 
        // if (IsDead) Discard();
        if (IsDead)
        {
            if (ID == "player")
            {
                onDead?.Invoke();
                // // * testing sfx
                // GameAudio.Instance.Register(7);
            }
            // * testing trophy
            else
            {
                if (gameObject.layer == GameVariables.LayerCreature) GameData.SetTrophy(GameData.Trophy.SLAYER);
                // ? don't disable player cause bugs
                StartCoroutine(Discard(Time.timeScale));
                // // * testing sfx
                // GameAudio.Instance.Register(4);
            }
            // * testing sfx
            if (_sfxDead != null) GameAudio.Instance.Register(_sfxDead, GameAudio.AudioType.ENTITY);
        }
        // * testing
        // if (_healthInst == 0) Discard();
        // if (_healthInst == 0) Hide();
        // 
    }
    // protected override void Hide()
    // {
    //     // * testing ? delete or pool
    //     _body.gameObject.SetActive(false);
    //     // 
    //     base.Hide();
    // }
    // protected override void Show()
    // {
    //     // deny if dead
    //     if (!IsDead)
    //         // * testing ? delete or pool
    //         _body.gameObject.SetActive(true);
    //     // 
    //     base.Show();
    // }
    public override void ToggleActive(bool state, bool isChunk = false)
    {
        // try enable
        if (state)
        {
            // deny if dead
            if (IsDead) return;
            // * testing ? delete or pool
            _body.gameObject.SetActive(true);
        }
        else _body.gameObject.SetActive(false);
        // 
        base.ToggleActive(state, isChunk);
    }
    // public override void Discard()
    // {
    //     // * testing ? delete or pool
    //     _body.gameObject.SetActive(false);
    //     // 
    //     base.Discard();
    // }
    IEnumerator Flash(Color colorFlash)
    {
        // fade alpha over tick duration ? account for dynamic tick size
        Color color;
        // * testing
        if (IsDead)
        {
            color = GameVariables.ColorDamage;
            // fade out on dead
            color.a = 0f;
            // disable colliders for all except player
            if (ID != "player") SetColliders(false);
            // Discard();
        }
        else color = _colorDefault;
        // flash to white then slowly back to original color ? make slightly faster than one tick so save gets called before a next tick transition
        // for(float t = 0f; t < 1f; t += Time.deltaTime * 1f)
        for(float t = 0f; t < 1f; t += Time.deltaTime)
        {
            // _sprite.color = Color.Lerp(_colorFlash, color, t);
            _sprite.color = Color.Lerp(colorFlash, color, t);
            yield return null;
        }
        // reapply default color
        _sprite.color = color;
        // // * testing ? mob logic for death must be provided in override discard ? never delete entities
        // // one tick delay between kill and item pickup
        // if (IsDead) Discard();
        // if (IsDead) Hide();
        // if (IsDead) ToggleActive(false);
        // * testing save/load, record partial damage too ? guarantee will save data before push to file in case of next tick transition
        DataSave();
    }
    protected void SetColliders(bool state, bool isTrigger = false)
    {
        // * testing
        foreach (Collider2D collider in _colliders)
        {
            // disable collisions only
            if (isTrigger) collider.isTrigger = !state;
            // disable fully
            else collider.enabled = state;
        }
    }
    protected bool IsDead
    {
        get { return _healthInst == 0; }
    }
    public int HealthInst
    {
        get { return _healthInst; }
    }
    // 
    // * testing move animation
    // 
    private Vector3 _position;
    // * testing separate collider and sprite ? case for unassigned body
    public Transform _body;
    private Coroutine _routineMove;
    public void Move(Vector3 target, bool isAudible = true)
    {
        // 
        if (_routineMove != null) StopCoroutine(_routineMove);
        // StopCoroutine("LerpPosition");
        // 
        _position = target;
        // 
        _routineMove = StartCoroutine(LerpPosition(isAudible));
        // StartCoroutine("LerpPosition");
    }
    [Tooltip("Rotate in move direction")] [SerializeField] private bool _isRotate = false;
    IEnumerator LerpPosition(bool isAudible)
    // IEnumerator LerpPosition()
    {
        // // * testing sfx
        // if (ID == "player") GameAudio.Instance.Register(10);
        // else if (gameObject.layer == GameVariables.LayerCreature)
        if (isAudible && _sfxMove != null) GameAudio.Instance.Register(_sfxMove, GameAudio.AudioType.ENTITY, true);
        // if (_sfxMove != null) GameAudio.Instance.Register(_sfxMove, GameAudio.AudioType.ENTITY, true);
        // look in direction to move
        if (_isRotate)
        {
            // transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(_position.y - Position.y, _position.x - Position.x) * Mathf.Rad2Deg - 90f);
            // _body.eulerAngles = transform.eulerAngles;
            SetRotation(new Vector3(0f, 0f, Mathf.Atan2(_position.y - Position.y, _position.x - Position.x) * Mathf.Rad2Deg - 90f));
        }
        // * testing, check if target tile clear ? handled for player by game action
        // if (Physics2D.OverlapCircleAll(_position, .45f, GameVariables.ScanLayerAction).Length > 0) yield break;
        if (GameNavigation.IsTileAtPosition(_position)) yield break;
        // record start position
        Vector3 start = Position;
        // move collider to target position
        transform.position = _position;
        // lerp counter
        float lerp = 0f;
        // not at target position
        while (Vector3.Distance(start, _position) > 0f)
        {
            // advance lerp timer
            lerp += Time.deltaTime * 2f;
            // update position offset for this frame
            _body.position = Vector3.Lerp(start, _position, lerp);
            // wait till next frame
            yield return null;
        }
    }
    // * testing
    // protected void SetPosition(Vector3 position)
    protected override void SetPosition(Vector3 position)
    {
        // stop current movement
        StopCoroutine("LerpPosition");
        // move sprite
        _body.position = position;
        // move collider
        // transform.position = position;
        base.SetPosition(position);
    }
    // * testing, player face room enter direction on transition ? disgraceful ? just a wrapper
    public void SetRotation(Vector2Int direction)
    {
        // transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f);
        // _body.eulerAngles = transform.eulerAngles;
        SetRotation(new Vector3(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f));
    }
    protected override void SetRotation(Vector3 rotation)
    {
        // if (ID == "player") print(ID + " " + rotation);
        _body.eulerAngles = rotation;
        // 
        base.SetRotation(rotation);
    }
}