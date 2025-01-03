using UnityEngine;
public class BaseMenu : MonoBehaviour
{
    public delegate void OnComplete();
    public event OnComplete onComplete;
    protected void DoComplete()
    {
        onComplete?.Invoke();
    }
    public bool IsActive
    {
        get { return gameObject.activeSelf; }
    }
    [Header("Menu")]
    [SerializeField] private bool _isSFXOpen;
    [SerializeField] private bool _isSFXClose;
    protected bool _skipSFX = false;
    protected virtual void OnEnable()
    {
        if (_skipSFX) return;
        // * testing sfx menu open
        if (_isSFXOpen) GameAudio.Instance?.Register(10, GameAudio.AudioType.UI);
    }
    protected virtual void OnDisable()
    {
        if (_skipSFX)
        {
            _skipSFX = false;
            return;
        }
        // * testing sfx menu close
        if (_isSFXClose) GameAudio.Instance?.Register(13, GameAudio.AudioType.UI);
    }
}