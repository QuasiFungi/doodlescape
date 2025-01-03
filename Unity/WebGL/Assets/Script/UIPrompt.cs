using UnityEngine;
public class UIPrompt : BaseMenu
{
    [Header("Prompt")]
    [SerializeField] private BaseButton.ButtonType _typeButton;
    [Tooltip("To open: 0 - Tap | 1 - Hold | 2 - Both")] [Range(0,2)] [SerializeField] private int _typeInput;
    private int _typeBtn;
    void Awake()
    {
        _typeBtn = (int)_typeButton;
        // bound to event for scene duration
        GameInput.onTap += DoConfirm;
        // start disabled
        _skipSFX = true;
        gameObject.SetActive(false);
    }
    void OnDestroy()
    {
        // unsubcribe when reload scene
        GameInput.onTap -= DoConfirm;
    }
    public delegate void OnConfirm(int typeButton);
    public static event OnConfirm onConfirm;
    private void DoConfirm(int typeButton, int typeInput, int index)
    {
        // only ad button
        if (typeButton != _typeBtn) return;
        // prompt requested ? pop in animation
        if (index == 0)
        {
            // // hold
            // if (typeInput == 1)
            // both hold or tap
            if (_typeInput == 2 || _typeInput == typeInput)
            {
                // show
                gameObject.SetActive(true);
                // notify ui manager
                DoComplete();
            }
        }
        // aux button
        else if (index == 3 || index == 4)
        {
            // confirmed ? fade screen out
            if (index == 4)
            {
                onConfirm?.Invoke(_typeBtn);
                // * testing sfx menu success
                GameAudio.Instance.Register(15, GameAudio.AudioType.UI);
            }
            else
                // * testing sfx menu close
                GameAudio.Instance.Register(13, GameAudio.AudioType.UI);
            // denied ? pop out animation
            DoComplete();
            // // * testing sfx button tap
            // GameAudio.Instance.Register(0, GameAudio.AudioType.UI);
        }
    }
}