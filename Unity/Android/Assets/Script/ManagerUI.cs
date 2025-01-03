using UnityEngine;
// using UnityEngine.UI;
public class ManagerUI : MonoBehaviour
{
    private enum StateUI
    {
        MONO_IN, INSTRUCTION, GAMEPLAY, SETTING, AD, DATA_RESET, DATA_DELETE, DEAD, MONO_OUT, CLEAR, SCOREBOARD, PROMPT_RESTART, PROMPT_EXIT
    }
    private StateUI _state;
    // each ui self-disable in respective awake calls, except monoIn
    public BaseMenu _monoIn, _instruction, _setting, _ad, _dataReset, _dataDelete, _dead, _monoOut, _clear, _scoreboard, _promptRestart, _promptExit;
    public BaseButton _auxLeft, _auxRight;
    public Sprite[] _iconsA;
    public Sprite[] _iconsB;
    void Awake()
    {
        _monoIn.onComplete += ProcessState;
        _instruction.onComplete += ProcessState;
        _setting.onComplete += ProcessState;
        _ad.onComplete += ProcessState;
        _dataReset.onComplete += ProcessState;
        _dataDelete.onComplete += ProcessState;
        _dead.onComplete += ProcessState;
        _monoOut.onComplete += ProcessState;
        _clear.onComplete += ProcessState;
        _promptRestart.onComplete += ProcessState;
        _promptExit.onComplete += ProcessState;
        GameMaster.onClear += DoClear;
        // Breakable.onDead += DoDead;
        // 
        // _monoIn.gameObject.SetActive(true);
        _state = StateUI.MONO_IN;
    }
    void OnDestroy()
    {
        _monoIn.onComplete -= ProcessState;
        _instruction.onComplete -= ProcessState;
        _setting.onComplete -= ProcessState;
        _ad.onComplete -= ProcessState;
        _dataReset.onComplete -= ProcessState;
        _dataDelete.onComplete -= ProcessState;
        _dead.onComplete -= ProcessState;
        _monoOut.onComplete -= ProcessState;
        _clear.onComplete -= ProcessState;
        _promptRestart.onComplete -= ProcessState;
        _promptExit.onComplete -= ProcessState;
        GameMaster.onClear -= DoClear;
        // Breakable.onDead -= DoDead;
    }
    public delegate void OnReady();
    public static event OnReady onReady;
    // public static event OnReady onClear;
    public delegate void OnPause(bool state);
    public static event OnPause onPause;
    private void DoClear(Vector2Int room)
    {
        ProcessState();
    }
    // private void DoDead()
    // {
    //     ProcessState();
    // }
    // assume state supplied will always be according to flow chart
    private void ProcessState()
    {
        switch (_state)
        {
            case StateUI.MONO_IN:
                // assumed already active
                _monoIn.gameObject.SetActive(false);
                // next up is instructions
                _state = StateUI.INSTRUCTION;
                _instruction.gameObject.SetActive(true);
                // configure auxiliary buttons for instructions
                _auxLeft.Configure(BaseButton.ButtonType.INSTRUCTION, 0, _iconsA[1], _iconsB[1]);
                _auxRight.Configure(BaseButton.ButtonType.INSTRUCTION, 1, _iconsA[2], _iconsB[2]);
                break;
            case StateUI.INSTRUCTION:
                // assumed already active
                _instruction.gameObject.SetActive(false);
                // next up is gameplay
                _state = StateUI.GAMEPLAY;
                // configure auxiliary buttons for gameplay
                _auxLeft.Configure(BaseButton.ButtonType.SETTING, 0, _iconsA[3], _iconsB[3]);
                _auxRight.Configure(BaseButton.ButtonType.AD, 0, _iconsA[4], _iconsB[4]);
                // inform game master
                onReady?.Invoke();
                break;
            case StateUI.GAMEPLAY:
                // settings opened
                if (_setting.IsActive)
                {
                    // ? activate here
                    _state = StateUI.SETTING;
                    // configure auxiliary buttons for settings
                    _auxLeft.Configure(BaseButton.ButtonType.SETTING, 1, _iconsA[5], _iconsB[5]);
                    _auxRight.Configure(BaseButton.ButtonType.SETTING, 2, _iconsA[6], _iconsB[6]);
                    // inform game clock, action/storage buttons
                    onPause?.Invoke(false);
                }
                // ads opened
                else if (_ad.IsActive)
                {
                    // ? activate here
                    _state = StateUI.AD;
                    // configure auxiliary buttons for ads
                    _auxLeft.Configure(BaseButton.ButtonType.AD, 1, _iconsA[5], _iconsB[5]);
                    _auxRight.Configure(BaseButton.ButtonType.AD, 2, _iconsA[6], _iconsB[6]);
                    // inform game clock, action/storage buttons
                    onPause?.Invoke(false);
                }
                // settings reset prompt
                else if (_dataReset.IsActive)
                {
                    // ? activate here
                    _state = StateUI.DATA_RESET;
                    // configure auxiliary buttons for prompt
                    _auxLeft.Configure(BaseButton.ButtonType.SETTING, 3, _iconsA[7], _iconsB[7]);
                    _auxRight.Configure(BaseButton.ButtonType.SETTING, 4, _iconsA[8], _iconsB[8]);
                    // inform game clock, action/storage buttons
                    onPause?.Invoke(false);
                }
                // data delete prompt
                else if (_dataDelete.IsActive)
                {
                    // ? activate here
                    _state = StateUI.DATA_DELETE;
                    // configure auxiliary buttons for prompt
                    _auxLeft.Configure(BaseButton.ButtonType.AD, 3, _iconsA[7], _iconsB[7]);
                    _auxRight.Configure(BaseButton.ButtonType.AD, 4, _iconsA[8], _iconsB[8]);
                    // inform game clock, action/storage buttons
                    onPause?.Invoke(false);
                }
                // dead
                else if (_dead.IsActive)
                {
                    // ? activate here
                    _state = StateUI.DEAD;
                    // inform game clock, action/storage buttons
                    onPause?.Invoke(false);
                }
                // game cleared
                else
                {
                    // assumed inactive
                    _monoOut.gameObject.SetActive(true);
                    // next up is gameplay
                    _state = StateUI.MONO_OUT;
                    // configure auxiliary buttons for monologue
                    _auxLeft.Configure(BaseButton.ButtonType.DISABLED, 0, _iconsA[0], _iconsB[0], false);
                    _auxRight.Configure(BaseButton.ButtonType.DISABLED, 0, _iconsA[0], _iconsB[0], false);
                    // inform game clock, action/storage buttons
                    onPause?.Invoke(false);
                    // onClear?.Invoke();
                }
                break;
            case StateUI.SETTING:
                // assumed already active
                _setting.gameObject.SetActive(false);
                // dead
                if (_dead.IsActive)
                {
                    // ? activate here
                    _state = StateUI.DEAD;
                    // configure auxiliary buttons for gameplay
                    _auxLeft.Configure(BaseButton.ButtonType.SETTING, 0, _iconsA[3], _iconsB[3]);
                    _auxRight.Configure(BaseButton.ButtonType.AD, 0, _iconsA[4], _iconsB[4]);
                    // inform game clock, action/storage buttons
                    onPause?.Invoke(false);
                }
                else
                {
                    // back to gameplay
                    _state = StateUI.GAMEPLAY;
                    // configure auxiliary buttons for gameplay
                    _auxLeft.Configure(BaseButton.ButtonType.SETTING, 0, _iconsA[3], _iconsB[3]);
                    _auxRight.Configure(BaseButton.ButtonType.AD, 0, _iconsA[4], _iconsB[4]);
                    // inform game clock, action/storage buttons
                    onPause?.Invoke(true);
                }
                break;
            case StateUI.AD:
                // assumed already active
                _ad.gameObject.SetActive(false);
                // dead
                if (_dead.IsActive)
                {
                    // ? activate here
                    _state = StateUI.DEAD;
                    // configure auxiliary buttons for gameplay
                    _auxLeft.Configure(BaseButton.ButtonType.SETTING, 0, _iconsA[3], _iconsB[3]);
                    _auxRight.Configure(BaseButton.ButtonType.AD, 0, _iconsA[4], _iconsB[4]);
                    // inform game clock, action/storage buttons
                    onPause?.Invoke(false);
                }
                else
                {
                    // back to gameplay
                    _state = StateUI.GAMEPLAY;
                    // configure auxiliary buttons for gameplay
                    _auxLeft.Configure(BaseButton.ButtonType.SETTING, 0, _iconsA[3], _iconsB[3]);
                    _auxRight.Configure(BaseButton.ButtonType.AD, 0, _iconsA[4], _iconsB[4]);
                    // inform game clock, action/storage buttons
                    onPause?.Invoke(true);
                }
                break;
            case StateUI.DATA_RESET:
                // assumed already active
                _dataReset.gameObject.SetActive(false);
                // dead
                if (_dead.IsActive)
                {
                    // ? activate here
                    _state = StateUI.DEAD;
                    // configure auxiliary buttons for gameplay
                    _auxLeft.Configure(BaseButton.ButtonType.SETTING, 0, _iconsA[3], _iconsB[3]);
                    _auxRight.Configure(BaseButton.ButtonType.AD, 0, _iconsA[4], _iconsB[4]);
                    // inform game clock, action/storage buttons
                    onPause?.Invoke(false);
                }
                else
                {
                    // back to gameplay
                    _state = StateUI.GAMEPLAY;
                    // configure auxiliary buttons for gameplay
                    _auxLeft.Configure(BaseButton.ButtonType.SETTING, 0, _iconsA[3], _iconsB[3]);
                    _auxRight.Configure(BaseButton.ButtonType.AD, 0, _iconsA[4], _iconsB[4]);
                    // inform game clock, action/storage buttons
                    onPause?.Invoke(true);
                }
                break;
            case StateUI.DATA_DELETE:
                // assumed already active
                _dataDelete.gameObject.SetActive(false);
                // dead
                if (_dead.IsActive)
                {
                    // ? activate here
                    _state = StateUI.DEAD;
                    // configure auxiliary buttons for gameplay
                    _auxLeft.Configure(BaseButton.ButtonType.SETTING, 0, _iconsA[3], _iconsB[3]);
                    _auxRight.Configure(BaseButton.ButtonType.AD, 0, _iconsA[4], _iconsB[4]);
                    // inform game clock, action/storage buttons
                    onPause?.Invoke(false);
                }
                else
                {
                    // back to gameplay
                    _state = StateUI.GAMEPLAY;
                    // configure auxiliary buttons for gameplay
                    _auxLeft.Configure(BaseButton.ButtonType.SETTING, 0, _iconsA[3], _iconsB[3]);
                    _auxRight.Configure(BaseButton.ButtonType.AD, 0, _iconsA[4], _iconsB[4]);
                    // inform game clock, action/storage buttons
                    onPause?.Invoke(true);
                }
                break;
            case StateUI.DEAD:
                // settings opened
                if (_setting.IsActive)
                {
                    // ? activate here
                    _state = StateUI.SETTING;
                    // configure auxiliary buttons for settings
                    _auxLeft.Configure(BaseButton.ButtonType.SETTING, 1, _iconsA[5], _iconsB[5]);
                    _auxRight.Configure(BaseButton.ButtonType.SETTING, 2, _iconsA[6], _iconsB[6]);
                    // inform game clock, action/storage buttons
                    onPause?.Invoke(false);
                }
                // ads opened
                else if (_ad.IsActive)
                {
                    // ? activate here
                    _state = StateUI.AD;
                    // configure auxiliary buttons for ads
                    _auxLeft.Configure(BaseButton.ButtonType.AD, 1, _iconsA[5], _iconsB[5]);
                    _auxRight.Configure(BaseButton.ButtonType.AD, 2, _iconsA[6], _iconsB[6]);
                    // inform game clock, action/storage buttons
                    onPause?.Invoke(false);
                }
                // 
                else if (_dataReset.IsActive)
                {
                    // ? activate here
                    _state = StateUI.DATA_RESET;
                    // configure auxiliary buttons for prompt
                    _auxLeft.Configure(BaseButton.ButtonType.SETTING, 3, _iconsA[7], _iconsB[7]);
                    _auxRight.Configure(BaseButton.ButtonType.SETTING, 4, _iconsA[8], _iconsB[8]);
                    // inform game clock, action/storage buttons
                    onPause?.Invoke(false);
                }
                // 
                else if (_dataDelete.IsActive)
                {
                    // ? activate here
                    _state = StateUI.DATA_DELETE;
                    // configure auxiliary buttons for prompt
                    _auxLeft.Configure(BaseButton.ButtonType.AD, 3, _iconsA[7], _iconsB[7]);
                    _auxRight.Configure(BaseButton.ButtonType.AD, 4, _iconsA[8], _iconsB[8]);
                    // inform game clock, action/storage buttons
                    onPause?.Invoke(false);
                }
                else
                {
                    // assumed already active
                    _dataDelete.gameObject.SetActive(false);
                    // back to gameplay
                    _state = StateUI.GAMEPLAY;
                    // configure auxiliary buttons for gameplay
                    _auxLeft.Configure(BaseButton.ButtonType.SETTING, 0, _iconsA[3], _iconsB[3]);
                    _auxRight.Configure(BaseButton.ButtonType.AD, 0, _iconsA[4], _iconsB[4]);
                    // inform game clock, action/storage buttons
                    onPause?.Invoke(true);
                }
                break;
            case StateUI.MONO_OUT:
                // assumed already active
                _monoOut.gameObject.SetActive(false);
                // next up is clear
                _state = StateUI.CLEAR;
                _clear.gameObject.SetActive(true);
                // // configure auxiliary buttons for scoreboard ? temporary
                // _auxLeft.Configure(BaseButton.ButtonType.DISABLED, 0, _iconsA[5], _icons[0]);
                // _auxRight.Configure(BaseButton.ButtonType.DISABLED, 0, _iconsA[5], _icons[0]);
                // configure auxiliary buttons for scoreboard ? temporary
                _auxLeft.Configure(BaseButton.ButtonType.CLEARED, 0, _iconsA[9], _iconsB[9]);
                _auxRight.Configure(BaseButton.ButtonType.EXIT, 0, _iconsA[10], _iconsB[10]);
                break;
            case StateUI.CLEAR:
                // prompt restart opened
                if (_promptRestart.IsActive)
                {
                    // ? activate here
                    _state = StateUI.PROMPT_RESTART;
                    // configure auxiliary buttons for prompt
                    _auxLeft.Configure(BaseButton.ButtonType.CLEARED, 3, _iconsA[7], _iconsB[7]);
                    _auxRight.Configure(BaseButton.ButtonType.CLEARED, 4, _iconsA[8], _iconsB[8]);
                }
                // prompt exit opened
                else if (_promptExit.IsActive)
                {
                    // ? activate here
                    _state = StateUI.PROMPT_EXIT;
                    // configure auxiliary buttons for prompt
                    _auxLeft.Configure(BaseButton.ButtonType.EXIT, 3, _iconsA[7], _iconsB[7]);
                    _auxRight.Configure(BaseButton.ButtonType.EXIT, 4, _iconsA[8], _iconsB[8]);
                }
                // else
                // {
                //     // // assumed already active
                //     // _clear.gameObject.SetActive(false);
                //     // // next up is scoreboard
                //     // _state = StateUI.SCOREBOARD;
                //     // _scoreboard.gameObject.SetActive(true);
                //     // configure auxiliary buttons for scoreboard ? temporary
                //     _auxLeft.Configure(BaseButton.ButtonType.CLEARED, 0, _iconsA[9], _iconsB[9]);
                //     _auxRight.Configure(BaseButton.ButtonType.EXIT, 0, _iconsA[10], _iconsB[10]);
                // }
                break;
            case StateUI.PROMPT_RESTART:
                // assumed already active
                _promptRestart.gameObject.SetActive(false);
                // back to gameplay
                _state = StateUI.CLEAR;
                // configure auxiliary buttons for scoreboard ? temporary
                _auxLeft.Configure(BaseButton.ButtonType.CLEARED, 0, _iconsA[9], _iconsB[9]);
                _auxRight.Configure(BaseButton.ButtonType.EXIT, 0, _iconsA[10], _iconsB[10]);
                // // inform game clock
                // onPause?.Invoke(true);
                break;
            case StateUI.PROMPT_EXIT:
                // assumed already active
                _promptExit.gameObject.SetActive(false);
                // back to gameplay
                _state = StateUI.CLEAR;
                // configure auxiliary buttons for scoreboard ? temporary
                _auxLeft.Configure(BaseButton.ButtonType.CLEARED, 0, _iconsA[9], _iconsB[9]);
                _auxRight.Configure(BaseButton.ButtonType.EXIT, 0, _iconsA[10], _iconsB[10]);
                // // inform game clock
                // onPause?.Invoke(true);
                break;
        }
    }
}