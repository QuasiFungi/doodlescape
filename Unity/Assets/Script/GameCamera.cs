using UnityEngine;
using System.Collections;
public class GameCamera : MonoBehaviour
{
    // void OnEnable()
    // {
    //     GameAction.onTransition += SetPosition;
    // }
    // void OnDisable()
    // {
    //     GameAction.onTransition -= SetPosition;
    // }
    private float _step = 9f;
    public void SetPosition(Vector2 direction)
    {
        // transform.position = direction * _step;
        transform.position = new Vector2((float)direction.x * _step, (float)direction.y * _step);
    }
    public BaseTransition[] _triggers;
    public void ToggleTriggers(bool state)
    {
        foreach (BaseTransition trigger in _triggers) trigger.ToggleActive(state);
    }
    public Transform _fog;
    public void ToggleFog(bool state, Vector2Int direction)
    {
        // ..? feels illegal
        direction *= 10;
        // start from outside screen, and slide in
        if (state) StartCoroutine(AnimateFog(state, transform.position - new Vector3(direction.x, direction.y), transform.position));
        // start from screen center, and slide out
        else StartCoroutine(AnimateFog(state, transform.position, transform.position + new Vector3(direction.x, direction.y)));
    }
    // ? better way than passing state parameter
    IEnumerator AnimateFog(bool state, Vector3 start, Vector3 end)
    {
        // show fog
        if (state) _fog.gameObject.SetActive(true);
        // snap to start position
        _fog.position = start;
        // _fog.position = new Vector3(start.x, start.y);
        // lerp counter, in reverse because of simpler bool check
        float lerp = 1f;
        // not at target position
        while (lerp > 0f)
        {
            // wait till next frame
            yield return null;
            // advance lerp timer
            // lerp += Time.deltaTime * 2f;
            lerp -= Time.deltaTime;
            // update position offset for this frame
            _fog.position = Vector3.Lerp(end, start, lerp);
            // * testing, move in steps of tile size
            _fog.position = new Vector3(Mathf.Round(_fog.position.x), Mathf.Round(_fog.position.y));
        }
        // hide fog
        if (!state) _fog.gameObject.SetActive(false);
    }
}