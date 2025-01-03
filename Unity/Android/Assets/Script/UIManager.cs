// using UnityEngine;
// public class UIManager : MonoBehaviour
// {
//     private enum StateUI
//     {
//         MONO_IN, INSTRUCTION, GAMEPLAY, MONO_OUT, SCOREBOARD, EXIT, SETTING, AD
//     }
//     private StateUI _state;
//     // each ui self-disable in respective awake calls, except monoIn
//     public GameObject _monoIn, _instruction, _gameplay, _monoOut, _scoreboard, _exit, _setting, _ad;
//     // void Start()
//     // {
//     //     // SetState(StateUI.MONO_IN);
//     // }
//     public BaseButton _auxLeft, _auxRight;
//     // assume state supplied will always be according to flow chart
//     private void ProcessState(StateUI state)
//     {
//         switch (state)
//         {
//             case StateUI.MONO_IN:
//                 // assumed already active
//                 _monoIn.SetActive(false);
//                 // next up is instructions
//                 _state = StateUI.INSTRUCTION;
//                 _instruction.SetActive(true);
//                 // configure auxiliary buttons for instructions
//                 _auxLeft.Configure(BaseButton.ButtonType.INSTRUCTION, 0, true);
//                 _auxRight.Configure(BaseButton.ButtonType.INSTRUCTION, 1, true);
//                 break;
//             case StateUI.INSTRUCTION:
//                 // assumed already active
//                 _instruction.SetActive(false);
//                 // next up is gameplay
//                 _state = StateUI.GAMEPLAY;
//                 // configure auxiliary buttons for gameplay
//                 _auxLeft.Configure(BaseButton.ButtonType.SETTING, 0, true);
//                 _auxRight.Configure(BaseButton.ButtonType.AD, 1, true);
//                 break;

//         }
//     }
// }