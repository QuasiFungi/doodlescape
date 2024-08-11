using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class GameMaster : MonoBehaviour
{
    void OnEnable()
    {
        Player.onAction += RegisterAction;
        GameClock.onTickEarly += ExecuteActions;
        HitboxIntercept.onIntercept += FilterModify;
        BaseTransition.onTrigger += PrepareTransition;
        GameAction.onTransition += OnTransition;
    }
    void OnDisable()
    {
        Player.onAction -= RegisterAction;
        GameClock.onTickEarly -= ExecuteActions;
        HitboxIntercept.onIntercept -= FilterModify;
        BaseTransition.onTrigger -= PrepareTransition;
        GameAction.onTransition -= OnTransition;
    }
    private List<GameAction> actions = new List<GameAction>();
    private void RegisterAction(GameAction action)
    {
        // 
        GameAction flag = null;
        foreach (GameAction temp in actions)
        {
            // check if an action by same entity already exists
            if (temp.Source == action.Source)
            {
                flag = temp;
                break;
            }
        }
        // remove previous action by same source
        if (flag != null) actions.Remove(flag);
        // verify new action should be ignored
        // if (action.Type == GameAction.ActionType.CANCEL || FilterContains(action.Source.GetComponent<Entity>().ID, action.Type)) return;
        if (FilterContains(action.Source.GetComponent<Entity>().ID, action.Type)) return;
        // overwrite with new action
        actions.Add(action);
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
        public string id;
        public GameAction.ActionType type;
        public FilterAction(string id, GameAction.ActionType type)
        {
            this.id = id;
            this.type = type;
        }
    }
    private void FilterModify(string id, GameAction.ActionType type, bool state)
    {
        if (state)
        {
            if (FilterContains(id, type)) return;
            // 
            _filter.Add(new FilterAction(id, type));
        }
        else FilterRemove(id, type);
    }
    private bool FilterContains(string id, GameAction.ActionType type)
    {
        // match on entity ID and action type pair ? multiple action block possible
        foreach (FilterAction filter in _filter) if (filter.id == id && filter.type == type) return true;
        return false;
    }
    private void FilterRemove(string id, GameAction.ActionType type)
    {
        // FilterAction temp;
        foreach (FilterAction filter in _filter)
        {
            // temp = filter;
            // break;
            if (filter.id == id && filter.type == type)
            {
                _filter.Remove(filter);
                break;
            }
        }
    }
    private static Vector2Int _roomCurrent = Vector2Int.zero;
    private static Vector2Int _roomNext = Vector2Int.zero;
    private static Vector2Int _roomDirection = Vector2Int.zero;
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
    public GameCamera _camera;
    public GameClock _clock;
    public Breakable _player;
    private void OnTransition()
    {
        // 
        StartCoroutine("DoTransition");
    }
    // * testing ? too special purpose, what about the other one time reference uses
    public delegate void OnTransitionComplete(Vector2Int room);
    public static event OnTransitionComplete onTransitionComplete;
    IEnumerator DoTransition()
    {
        // cache to prevent transition animation from bugging halfway
        Vector2Int roomDirection = _roomDirection;
        // prevent player from walking ? prevent all mobs from walking
        FilterModify("player", GameAction.ActionType.WALK, true);
        // 
        _clock.ToggleClock(false);
        yield return null;
        // disable auto transition detection
        _camera.ToggleTriggers(false);
        yield return null;
        // fog fade in ? use timescale
        _camera.ToggleFog(true, roomDirection);
        yield return new WaitForSeconds(1f);
        // move camera
        _camera.SetPosition(_roomNext);
        yield return null;
        // swap tracked rooms
        Vector2Int temp = _roomCurrent;
        _roomCurrent = _roomNext;
        _roomNext = temp;
        yield return null;
        // turn player
        _player.SetRotation(roomDirection);
        yield return null;
        // fog fade out ? use timescale
        _camera.ToggleFog(false, roomDirection);
        yield return new WaitForSeconds(1f);
        // enable auto transition detection
        _camera.ToggleTriggers(true);
        yield return null;
        // 
        onTransitionComplete?.Invoke(_roomCurrent);
        // 
        _clock.ToggleClock(true);
        // allow player to walk ? prevent all mobs from walking
        FilterModify("player", GameAction.ActionType.WALK, false);
    }
}