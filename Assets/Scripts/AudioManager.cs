using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    [SerializeField] AudioMixer audioMixer;

    public void FadeAudio(float duration, float minVol, float maxVol) {
        StartCoroutine(StartFade("volMaster", duration, minVol, maxVol));
    }

    IEnumerator StartFade(string exposedParam, float duration, float minVol, float maxVol)
    {
        float currentTime = 0;
        float currentVol;

        audioMixer.SetFloat(exposedParam, minVol);

        currentVol = Mathf.Pow(10, minVol / 20);
        
        float targetValue = Mathf.Clamp(maxVol, 0.0001f, 1);

        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }
        yield break;
    }
}
