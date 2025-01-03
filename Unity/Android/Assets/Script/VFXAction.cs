using UnityEngine;
using UnityEngine.UI;
public class VFXAction : MonoBehaviour
{
    public Camera _camera;
    public Transform _player;
    private Image _iconA;
    private Image _iconB;
    public Sprite _iconHide;
    public Sprite _iconWalkA;
    public Sprite _iconWalkB;
    public Sprite _iconInteractA;
    public Sprite _iconInteractB;
    public Sprite _iconAttackA;
    public Sprite _iconAttackB;
    public Sprite _iconPickupA;
    public Sprite _iconPickupB;
    public Sprite _iconTransitionA;
    public Sprite _iconTransitionB;
    void Awake()
    {
        _iconA = transform.GetChild(0).GetComponent<Image>();
        _iconB = transform.GetChild(1).GetComponent<Image>();
    }
    void OnEnable()
    {
        Player.onAction += SpriteShow;
        GameClock.onTickUIEarly += SpriteHide;
        GameMaster.onTransition += SpriteToggle;
    }
    void OnDisable()
    {
        Player.onAction -= SpriteShow;
        GameClock.onTickUIEarly -= SpriteHide;
        GameMaster.onTransition -= SpriteToggle;
    }
    private bool _isDisabled = false;
    private void SpriteShow(GameAction action)
    {
        if (_isDisabled) return;
        // 
        switch (action.Type)
        {
            case GameAction.ActionType.WALK:
                _iconA.sprite = _iconWalkA;
                _iconB.sprite = _iconWalkB;
                transform.position = _camera.WorldToScreenPoint(action.Position);
                break;
            case GameAction.ActionType.INTERACT:
                _iconA.sprite = _iconInteractA;
                _iconB.sprite = _iconInteractB;
                transform.position = _camera.WorldToScreenPoint(action.Position);
                break;
            case GameAction.ActionType.ATTACK:
                _iconA.sprite = _iconAttackA;
                _iconB.sprite = _iconAttackB;
                transform.position = _camera.WorldToScreenPoint(action.Position);
                break;
            case GameAction.ActionType.PICKUP:
                _iconA.sprite = _iconPickupA;
                _iconB.sprite = _iconPickupB;
                transform.position = _camera.WorldToScreenPoint(action.Position);
                break;
            case GameAction.ActionType.TRANSITION:
                _iconA.sprite = _iconTransitionA;
                _iconB.sprite = _iconTransitionB;
                transform.position = _camera.WorldToScreenPoint(Player.Instance.Position);
                break;
        }
    }
    private void SpriteHide()
    {
        _iconA.sprite = _iconHide;
        _iconB.sprite = _iconHide;
    }
    private void SpriteToggle(bool state)
    {
        _isDisabled = !state;
        // 
        if (state) return;
        // 
        SpriteHide();
    }
}