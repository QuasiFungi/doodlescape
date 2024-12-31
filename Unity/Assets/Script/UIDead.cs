using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class UIDead : BaseMenu
{
    private Image _spriteA, _spriteB;
    void Awake()
    {
        Breakable.onDead += DoDead;
        Breakable.onAlive += DoAlive;
        // 
        // UIAdvertisement.onBanner += DoBanner;
        // UIAdvertisement.onPopUp += DoPopUp;
        // UIAdvertisement.onVideo += DoVideo;
        UIAdvertisement.onApply += DoApply;
        // start disabled
        _skipSFX = true;
        gameObject.SetActive(false);
        // 
        _spriteA = transform.GetComponent<Image>();
        _spriteB = transform.GetChild(0).GetComponent<Image>();
    }
    void OnDestroy()
    {
        Breakable.onDead -= DoDead;
        Breakable.onAlive -= DoAlive;
        // 
        // UIAdvertisement.onBanner -= DoBanner;
        // UIAdvertisement.onPopUp -= DoPopUp;
        // UIAdvertisement.onVideo -= DoVideo;
        UIAdvertisement.onApply -= DoApply;
    }
    // // do not play menu open sfx ? only exception
    // protected override void OnEnable(){}
    private void DoDead()
    {
        gameObject.SetActive(true);
        // 
        DoComplete();
        // 
        StartCoroutine(FadeIn());
    }
    IEnumerator FadeIn()
    {
        Color colorAA = _spriteA.color;
        Color colorAB = _spriteA.color;
        colorAA.a = 0f;
        _spriteA.color = colorAA;
        Color colorBA = _spriteB.color;
        Color colorBB = _spriteB.color;
        colorBA.a = 0f;
        _spriteB.color = colorBA;
        yield return new WaitForSeconds(1f);
        for(float t = 0f; t < 1f; t += Time.deltaTime / 3f)
        {
            _spriteA.color = Color.Lerp(colorAA, colorAB, t);
            _spriteB.color = Color.Lerp(colorBA, colorBB, t);
            yield return null;
        }
        // fully visible
        _spriteA.color = colorAB;
        _spriteB.color = colorBB;
    }
    private bool _flagAlive = false;
    private void DoAlive()
    {
        _flagAlive = true;
    }
    private void DoApply(int value)
    {
        // ? show in sequence in case multiple enabled
        // if (_isBanner) 
        // if (_isPopUp) 
        // if (_isVideo) 
        // 
        if (_flagAlive)
        {
            // reset
            _flagAlive = false;
            // 
            gameObject.SetActive(false);
            // 
            DoComplete();
        }
    }
}