using UnityEngine;
using UnityEngine.UI;
public class UIClock : MonoBehaviour
{
    private Image image;
    public Sprite[] sprites;
    private float index;
    void Awake()
    {
        image = GetComponent<Image>();
        index = 0f;
    }
    void OnEnable()
    {
        GameClock.onTick += SpriteReset;
    }
    void OnDisable()
    {
        GameClock.onTick -= SpriteReset;
    }
    private void SpriteReset()
    {
        index = 0;
    }
    void Update()
    {
        index += Time.deltaTime * sprites.Length;
        image.sprite = sprites[Mathf.Clamp(Mathf.FloorToInt(index), 0, sprites.Length - 1)];
    }
}