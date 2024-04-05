using UnityEngine;
// * testing track camera uiAction position only not rotation
public class TestTrack : MonoBehaviour
{
    public Transform _target;
    public Vector3 _offset;
    void LateUpdate()
    {
        transform.position = _target.position + _offset;
    }
}