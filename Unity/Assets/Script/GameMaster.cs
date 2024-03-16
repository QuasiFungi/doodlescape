using UnityEngine;
using System.Collections.Generic;
public class GameMaster : MonoBehaviour
{
    void OnEnable()
    {
        Player.onAction += RegisterAction;
        GameClock.onTick += ExecuteActions;
    }
    void OnDisable()
    {
        Player.onAction -= RegisterAction;
        GameClock.onTick -= ExecuteActions;
    }
    private List<GameAction> actions = new List<GameAction>();
    private void RegisterAction(GameAction action)
    {
        GameAction flag = null;
        foreach (GameAction temp in actions)
        {
            if (temp.Source == action.Source)
            {
                flag = temp;
                break;
            }
        }
        // remove previous action by same source
        if (flag != null) actions.Remove(flag);
        // overwrite with new action
        actions.Add(action);
    }
    private void ExecuteActions()
    {
        foreach (GameAction action in actions) action.Process();
        actions.Clear();
    }
}