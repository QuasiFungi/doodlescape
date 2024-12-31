using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class GameMaster : MonoBehaviour
{
    void Awake()
    {
        Player.onAction += RegisterAction;
        GameClock.onTickEarly += ExecuteActions;
        HitboxIntercept.onIntercept += FilterModify;
        BaseTransition.onTrigger += PrepareTransition;
        GameAction.onTransition += OnTransition;
        ManagerUI.onReady += OnReady;
        // 
        _roomCurrent = Vector2Int.zero;
        _roomNext = Vector2Int.zero;
        _roomDirection = Vector2Int.zero;
    }
    void OnDestroy()
    {
        Player.onAction -= RegisterAction;
        GameClock.onTickEarly -= ExecuteActions;
        HitboxIntercept.onIntercept -= FilterModify;
        BaseTransition.onTrigger -= PrepareTransition;
        GameAction.onTransition -= OnTransition;
        ManagerUI.onReady -= OnReady;
    }
    private List<GameAction> actions = new List<GameAction>();
    // private bool _isBusy = false;
    private void RegisterAction(GameAction action)
    {
        // // * testing block new events on transition
        // if (_isBusy) return;
        // 
        GameAction flag = null;
        foreach (GameAction temp in actions)
        {
            // check if an action by same entity already exists
            if (temp.Source == action.Source)
            {
                // action already registered
                if (temp.Direction == action.Direction) return;
                // different action
                flag = temp;
                break;
            }
        }
        // remove previous action by same source
        if (flag != null) actions.Remove(flag);
        // verify new action should be ignored
        if (FilterContains(action.Source.GetComponent<Entity>().IDUnique, action.Type)) return;
        // overwrite with new action
        actions.Add(action);
        // * testing vibrate actions/storage
        GameData.DoVibrate(action.TypeInput == 0 ? GameData.IntensityVibrate.LOW : (action.TypeButton == 0 ? GameData.IntensityVibrate.LOW : GameData.IntensityVibrate.MEDIUM));
        // * testing sfx tap/hold storage/actions
        GameAudio.Instance.Register(action.TypeInput == 0 ? 0 : (action.TypeButton == 0 ? 0 : 1), GameAudio.AudioType.UI);
    }
    private void ExecuteActions()
    {
        foreach (GameAction action in actions) action.Process();
        actions.Clear();
    }
    // entites to ignore actions from
    private List<FilterAction> _filter = new List<FilterAction>();
    private struct FilterAction
    {
        public string idUnique;
        public GameAction.ActionType type;
        public FilterAction(string id, GameAction.ActionType type)
        {
            this.idUnique = id;
            this.type = type;
        }
    }
    private void FilterModify(string idUnique, GameAction.ActionType type, bool state, Breakable source)
    {
        if (state)
        {
            if (FilterContains(idUnique, type)) return;
            // 
            _filter.Add(new FilterAction(idUnique, type));
        }
        else FilterRemove(idUnique, type);
    }
    private bool FilterContains(string idUnique, GameAction.ActionType type)
    {
        // match on entity ID and action type pair ? multiple action block possible
        foreach (FilterAction filter in _filter) if (filter.idUnique == idUnique && filter.type == type) return true;
        return false;
    }
    private void FilterRemove(string idUnique, GameAction.ActionType type)
    {
        // FilterAction temp;
        foreach (FilterAction filter in _filter)
        {
            // temp = filter;
            // break;
            if (filter.idUnique == idUnique && filter.type == type)
            {
                _filter.Remove(filter);
                break;
            }
        }
    }
    private static Vector2Int _roomCurrent;
    private static Vector2Int _roomNext;
    private static Vector2Int _roomDirection;
    private void PrepareTransition(Vector2Int direction, bool state)
    {
        _roomNext = state ? _roomCurrent + direction : _roomCurrent;
        // print("next room: " + _roomNext);
    }
    // used by game action
    public static bool IsTransition(Vector2 direction)
    {
        // return new Vector2Int((int)direction.x, (int)direction.y) == _roomNext - _roomCurrent;
        _roomDirection = _roomNext - _roomCurrent;
        // non zero axis must match
        return (_roomDirection.x != 0 && (int)direction.x == _roomDirection.x) || (_roomDirection.y != 0 && (int)direction.y == _roomDirection.y);
    }
    // ? no better alternative
    public GameCamera _camera;
    public GameClock _clock;
    public Creature _player;
    public delegate void OnTransitionState(bool state);
    public static event OnTransitionState onTransition;
    private void OnTransition()
    {
        // // disallow register new events at source
        // _isBusy = true;
        onTransition?.Invoke(false);
        // 
        StartCoroutine("DoTransition");
    }
    // * testing ? too special purpose, what about the other one time reference uses
    public delegate void OnTransitionRoom(Vector2Int room);
    public static event OnTransitionRoom onTransitionBegin;
    public static event OnTransitionRoom onTransitionReady;
    public static event OnTransitionRoom onTransitionComplete;
    public static event OnTransitionRoom onClear;
    // consistent time regardless of difficulty
    IEnumerator DoTransition()
    {
        // cache to prevent transition animation from bugging halfway
        Vector2Int roomDirection = _roomDirection;
        // // prevent player from walking ? prevent all mobs from walking
        // FilterModify("player", GameAction.ActionType.WALK, true);
        // FilterModify("spikeSnail", GameAction.ActionType.WALK, true);
        // 
        // print(roomDirection);
        // turn player
        // _player.SetRotation(roomDirection);
        Player.Instance.SetRotation(roomDirection);
        yield return null;
        // ! manual reference because sequence important ? events for each
        _clock.ToggleClock(false);
        yield return null;
        // disable auto transition detection
        _camera.ToggleTriggers(false);
        yield return null;
        // fog fade in ? use timescale
        _camera.ToggleFog(true, roomDirection);
        // chunk state update, hitbox pause, player data save
        onTransitionBegin?.Invoke(_roomNext);
        yield return new WaitForSeconds(1f);
        // push save data to file
        onTransitionReady?.Invoke(_roomNext);
        yield return null;
        // ? make rest into separate fade out coroutine
        // move camera
        _camera.SetPosition(_roomNext);
        yield return null;
        // swap tracked rooms
        Vector2Int temp = _roomCurrent;
        _roomCurrent = _roomNext;
        _roomNext = temp;
        yield return null;
        // enable auto transition detection
        _camera.ToggleTriggers(true);
        yield return null;
        // reached final/escape room
        if (_roomCurrent == _roomClear)
        {
            // * testing trophy
            // if (_player.ItemHas("item_coin") && _player.ItemHas("item_gem") && _player.ItemHas("item_necklace") && _player.ItemHas("item_ring")
            //     && _player.ItemHas("item_goblet") && _player.ItemHas("item_grimoire") && _player.ItemHas("item_tablet") && _player.ItemHas("item_talon")) GameData.SetTrophy(GameData.Trophy.PLUNDERER);
            if (Player.Instance.ItemHas("item_coin") && Player.Instance.ItemHas("item_gem") && Player.Instance.ItemHas("item_necklace") && Player.Instance.ItemHas("item_ring")
                && Player.Instance.ItemHas("item_goblet") && Player.Instance.ItemHas("item_grimoire") && Player.Instance.ItemHas("item_tablet") && Player.Instance.ItemHas("item_talon")) GameData.SetTrophy(GameData.Trophy.PLUNDERER);
            // 
            onClear?.Invoke(_roomCurrent);
        }
        // 
        else
        {
            onTransitionComplete?.Invoke(_roomCurrent);
            yield return null;
            // fog fade out ? use timescale
            _camera.ToggleFog(false, roomDirection);
            yield return new WaitForSeconds(1f);
            // 
            _clock.ToggleClock(true);
            // // allow player to walk ? allow all mobs to walk
            // FilterModify("player", GameAction.ActionType.WALK, false);
            // FilterModify("spikeSnail", GameAction.ActionType.WALK, false);
            // 
            // allow register new events
            // _isBusy = false;
            onTransition?.Invoke(true);
        }
    }
    // * testing game clear
    private Vector2Int _roomClear = new Vector2Int(0, -10);
    // public delegate void OnLoad();
    // public static event OnLoad onLoad;
    // void Start()
    // {
    //     // make call to game data to load initial entity states?
    //     // initialize all entities? those in room?
    //     onLoad?.Invoke();
    // }
    // * testing save/load
    void Start()
    {
        // print("master " + GameData.Room);
        _roomCurrent = GameData.Room;
        // // * testing startup sequence
        // StartCoroutine(Startup());
        StartCoroutine(Preload());
    }
    // reduce lag on intro sequence
    IEnumerator Preload()
    {
        onStartupLoad?.Invoke();
        yield return null;
        onStartupLoot?.Invoke();
        yield return null;
        onStartupChunk?.Invoke();
        yield return null;
    }
    // * testing instructions
    private void OnReady()
    {
        // * testing startup sequence
        StartCoroutine(Startup());
    }
    public delegate void OnStartup();
    public static event OnStartup onStartupLoad;
    // public static event OnStartup onStartupSettings;
    public static event OnStartup onStartupLoot;
    public static event OnStartup onStartupChunk;
    public static event OnStartup onStartupMob;
    public static event OnStartup onStartupCamera;
    public static event OnStartup onStartupClock;
    public static event OnStartup onStartupInput;
    private IEnumerator Startup()
    {
        // onStartupLoad?.Invoke();
        // yield return null;
        // // onStartupSettings?.Invoke();
        // // yield return null;
        // onStartupLoot?.Invoke();
        // yield return null;
        // onStartupChunk?.Invoke();
        // yield return null;
        onStartupMob?.Invoke();
        yield return null;
        onStartupCamera?.Invoke();
        yield return new WaitForSeconds(1f);
        onStartupClock?.Invoke();
        yield return null;
        onStartupInput?.Invoke();
    }
    // // * testing game clear
    // private void OnClear()
    // {
    //     // freeze on black screen ? ideal would be to split in/out transitions so can continue using ads
    //     StopCoroutine("DoTransition");
    // }
}