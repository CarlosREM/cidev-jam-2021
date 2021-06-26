using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class CameraEffects : MonoBehaviour
{
    [SerializeField] Volume volume;
    VolumeProfile volumeProfile;

    [SerializeField] float shakeForce;
    [SerializeField] float startVignette = 0.3f;
    [SerializeField] float startSaturation = 0;

    // Start is called before the first frame update
    void Start()
    {
        volumeProfile = volume.profile;

        SetVignette(startVignette);
        SetSaturation(startSaturation);
    }


    void LateUpdate() {
        if (shakeForce > 0) {
            transform.localPosition = Vector3.zero;
            Vector3 camPos = transform.localPosition;

            camPos.x += Random.Range(-1, 1) * shakeForce;
            camPos.y += Random.Range(-1, 1) * shakeForce;

            transform.localPosition = camPos;
        }
    }


    public void TempShake(float shakeForce, float duration) {
       StartCoroutine(ShakeCoroutine(shakeForce, duration)); 
    }

    IEnumerator ShakeCoroutine(float shakeForce, float duration) {

        float delta = 0;
        float currentForce;

        while (delta < duration) {
            delta += Time.deltaTime;

            currentForce = Mathf.Lerp(0, shakeForce, ( (duration-delta) / duration ) );
            
            Vector3 camPos = Vector3.zero;

            camPos.x += Random.Range(-1, 1) * (currentForce);
            camPos.y += Random.Range(-1, 1) * (currentForce);

            transform.localPosition = camPos;
            

            yield return new WaitForSeconds(0.05f);
        }

        transform.localPosition = Vector3.zero;
    }


    public void SetVignette(float value) {
        float saturation = Mathf.Clamp(value, 0, 1);

        Vignette effectVignette;
        if (volumeProfile.TryGet(out effectVignette))
            effectVignette.intensity.Override(value);
    }

    public void SetSaturation(float value) {
        float saturation = Mathf.Clamp(value, -100, 100);

        ColorAdjustments effectColorAdj;
        if (volumeProfile.TryGet(out effectColorAdj))
            effectColorAdj.saturation.Override(value);
    }


}
