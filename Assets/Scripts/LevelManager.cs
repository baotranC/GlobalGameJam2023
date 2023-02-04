using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    MelodyManager[] melodies;
    int currentMelody;

    public void StartLevel() {
        print("Start of " + gameObject.name);
        melodies = GetComponentsInChildren<MelodyManager>();

        foreach (MelodyManager melody in melodies) {
            melody.gameObject.SetActive(false);
        }

        melodies[0].gameObject.SetActive(true);
        melodies[0].Init();
    }

    public void GoToNextMelody() {
        melodies[currentMelody].gameObject.SetActive(false);

        currentMelody++;
        if (currentMelody == melodies.Length) {
            EndLevel();
            return;
        }

        melodies[currentMelody].gameObject.SetActive(true);
        melodies[currentMelody].Init();
    }

    public void EndLevel() {
        print("End of " + gameObject.name);
        GetComponentInParent<GameManager>().GoToNextLevel();
    }

    public void ResetCursor() {
        melodies[currentMelody].ResetCursor();
    }

}
