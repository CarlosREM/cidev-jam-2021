using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudManager : MonoBehaviour
{

    [SerializeField] Image imgPortrait;
    [SerializeField] Image imgWeapon;
    
    [SerializeField] Slider barSanity;
    [SerializeField] Slider barEnergy;

    [SerializeField] Slider barNight;

    [SerializeField] RectTransform displayBit;
    [SerializeField] Vector2 displayBitHiddenPos;
    [SerializeField] Vector2 displayBitShowPos;

    TextMeshProUGUI txtBit;
    [SerializeField] bool showBits = false;
    [SerializeField] float bitShowDuration;
    [SerializeField] float bitLerpDuration;
    float bitDelta = 0;


    void Start() {
        txtBit = displayBit.GetComponentInChildren<TextMeshProUGUI>();

        displayBit.anchoredPosition = displayBitHiddenPos; // sets bit display hidden on game start
        SetNightProgress(0, 1); // sets night to 0 on game start
    }

    
    public void SetPortrait(Sprite sprite) {
        imgPortrait.sprite = sprite;
    }

    public void SetWeaponIcon(Sprite sprite) {
        imgWeapon.sprite = sprite;
    }

    public void SetSanityValue(float value, float maxValue) {
        barSanity.value = (value/maxValue)*100;
    }

    public void SetEnergyValue(float value, float maxValue) {
        barEnergy.value = (value/maxValue)*100;
    }

    public void SetNightProgress(float value, float maxValue) {
        barNight.value = (value/maxValue)*100;
    }

    public void SetBits(int value) {
        txtBit.text = value + "b";
        bitDelta = 0;
        showBits = true;
    }

    void Update() {

        if (showBits) {
            float t;

            if (bitDelta <= bitLerpDuration && displayBit.anchoredPosition != displayBitShowPos) {
                t = (bitDelta/bitLerpDuration);
                t = t * t * (3f - 2f * t);
                
                displayBit.anchoredPosition = Vector2.Lerp(displayBitHiddenPos, displayBitShowPos, t);
            }
            else if (bitDelta < bitShowDuration)
                displayBit.anchoredPosition = displayBitShowPos;

            else {
                
                t = ((bitDelta - bitShowDuration) / bitLerpDuration);
                t = t * t * (3f - 2f * t);
                
                displayBit.anchoredPosition = Vector2.Lerp(displayBitShowPos, displayBitHiddenPos, t);
            }
            bitDelta += Time.deltaTime;

            if (bitDelta >= (bitShowDuration + bitLerpDuration) )
                showBits = false;
        }
    }
}
