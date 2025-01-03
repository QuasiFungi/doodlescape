using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class Teleprompter : MonoBehaviour
{
    private static List<string> _messages = new List<string>();
    private static Text[] _display = new Text[2];
    void Start()
    {
        _time = 3;
        _timer = 0;
        // get references
        _display[0] = transform.GetChild(0).GetComponent<Text>();
        _display[1] = transform.GetChild(1).GetComponent<Text>();
        // clear placeholder text
        SetDisplay("", "");
    }
    private static void SetDisplay(string msgA, string msgB)
    {
        _display[0].text = msgA;
        _display[1].text = msgB;
    }
    // ? character limit, anti spam by filter on source+type
    public static void Register(string message)
    {
        // // * testing sfx
        // GameAudio.Instance.Register(2);
        // record received message
        _messages.Add(message.Substring(0, Mathf.Min(19, message.Length)).ToUpper());
        // one new message
        if (_messages.Count == 1)
            // show
            SetDisplay(_messages[0], "");
        // one new one old
        else if (_messages.Count == 2)
            // just show and wait for natural tock
            SetDisplay(_messages[0], _messages[1]);
        // message queue full
        else if (_messages.Count > 2)
        {
            // discard oldest message
            _messages.RemoveAt(0);
            // push in new message
            SetDisplay(_messages[0], _messages[1]);
        }
        // start ticking
        _timer = _time;
    }
    void OnEnable()
    {
        // sync with clock
        GameClock.onTickUI += Tick;
    }
    void OnDisable()
    {
        // desync from clock
        GameClock.onTickUI -= Tick;
    }
    private static int _time;
    private static int _timer;
    private static void Tick()
    {
        // tick
        if (_timer > 0) _timer--;
        // tock with message(s) in queue
        else if (_messages.Count > 0)
        {
            // remove oldest message
            _messages.RemoveAt(0);
            // two or more messages to go
            if (_messages.Count > 1)
            {
                // shift up messages
                SetDisplay(_messages[0], _messages[1]);
                // reset
                _timer = _time;
            }
            // last message
            else if (_messages.Count > 0)
            {
                // shift up message
                SetDisplay(_messages[0], "");
                // reset, appears to last ~4 ticks otherwise
                _timer = _time - 1;
            }
            // message stack empty
            else
                // clear
                SetDisplay("", "");
        }
    }
}