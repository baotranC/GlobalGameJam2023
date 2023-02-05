using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public Animator sceneTransition;
    public GameObject rideauGO;

    public void Quit() {
        StartCoroutine(PerformQuit());
    }

    IEnumerator PerformQuit() {
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }

    public void Play() {
        StartCoroutine(PerformPlay());
    }

    IEnumerator PerformPlay() {
        rideauGO.SetActive(true);
        yield return new WaitForSeconds(2f);
        sceneTransition.SetTrigger("FadeIn");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("Main");
    }
}
