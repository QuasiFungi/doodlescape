using UnityEngine;
public class UIDataDelete : BaseMenu
{
    void Awake()
    {
        // bound to event for scene duration
        GameInput.onTap += ConfirmDelete;
        // start disabled
        _skipSFX = true;
        gameObject.SetActive(false);
    }
    void OnDestroy()
    {
        // unsubcribe when reload scene
        GameInput.onTap -= ConfirmDelete;
    }
    public delegate void OnConfirm();
    public static event OnConfirm onConfirm;
    private void ConfirmDelete(int typeButton, int typeInput, int index)
    {
        // only ad button
        if (typeButton != 5) return;
        // delete requested ? pop in animation
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