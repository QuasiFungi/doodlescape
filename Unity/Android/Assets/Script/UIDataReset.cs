using UnityEngine;
public class UIDataReset : BaseMenu
{
    void Awake()
    {
        // bound to event for scene duration
        GameInput.onTap += ConfirmReset;
        // start disabled
        _skipSFX = true;
        gameObject.SetActive(false);
    }
    void OnDestroy()
    {
        // unsubcribe when reload scene
        GameInput.onTap -= ConfirmReset;
    }
    public delegate void OnConfirm();
    public static event OnConfirm onConfirm;
    private void ConfirmReset(int typeButton, int typeInput, int index)
    {
        // only setting button
        if (typeButton != 4) return;
        // reset requested ? pop in animation
        if (index == 0)
        {
            // hold
            if (typeInput == 1) gameObject.SetActive(true);
        }
        // aux button
        else
        {
            // confirmed ? fade screen out
            if (index == 2) onConfirm?.Invoke();
            // denied ? pop out animation
            // gameObject.SetActive(false);
            DoComplete();
        }
    }
}