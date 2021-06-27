using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionManager : MonoBehaviour
{
    Animator animator;
    [SerializeField] float transitionDuration;

    AudioManager audioManager;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    void Start() {
        Time.timeScale = 1;

        audioManager = GameObject.Find("Music Manager").GetComponent<AudioManager>();
        audioManager.FadeAudio(transitionDuration, 0, 1);
    }

    public void ChangeScene(string sceneName) {
        Debug.Log("TransitionManager > Loading Scene \""+sceneName+"\"");

        StartCoroutine(SceneSwitchCoroutine(sceneName));
    }

    IEnumerator SceneSwitchCoroutine(string sceneName) {
        animator.SetTrigger("Transition");
        audioManager.FadeAudio(transitionDuration, 1, 0);

        yield return new WaitForSecondsRealtime(transitionDuration);

        SceneManager.LoadScene(sceneName);
    }

    bool AnimatorIsPlaying() {
        return animator.GetCurrentAnimatorStateInfo(0).length >
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

}
