using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public AudioClip concertAudio;
    [SerializeField] private Color[] noteColors;

    MelodyManager[] melodies;
    int currentMelody;
    AudioSource concertSource;

    public void StartLevel() {
        print("Start of " + gameObject.name);
        melodies = GetComponentsInChildren<MelodyManager>();
        concertSource = GetComponent<AudioSource>();

        foreach (MelodyManager melody in melodies) {
            melody.gameObject.SetActive(false);
        }

        melodies[0].gameObject.SetActive(true);
        melodies[0].Init(noteColors);
    }

    public void GoToNextMelody() {
        melodies[currentMelody].gameObject.SetActive(false);

        currentMelody++;
        if (currentMelody == melodies.Length) {
            EndLevel();
            return;
        }

        melodies[currentMelody].gameObject.SetActive(true);
        melodies[currentMelody].Init(noteColors);
    }

    public void EndLevel() {
        print("End of " + gameObject.name);
        StartCoroutine(PerformEndLevel());
    }

    IEnumerator PerformEndLevel() {
        melodies[currentMelody - 1].gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        concertSource.clip = concertAudio;
        concertSource.Play();

        yield return new WaitForSeconds(concertAudio.length + 1f);

        GetComponentInParent<GameManager>().GoToNextLevel();
    }

    public void ResetCursor() {
        if (currentMelody < melodies.Length) {
            melodies[currentMelody].ResetCursor();
        }
    }

}
