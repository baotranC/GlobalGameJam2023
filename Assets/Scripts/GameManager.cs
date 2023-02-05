using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    LevelManager[] levels;
    public GameObject[] desactivables;
    public GameObject[] refNotesPrefabs;

    int currentLevel;

    private void Awake() {
        levels = GetComponentsInChildren<LevelManager>();

        foreach(LevelManager level in levels) {
            level.gameObject.SetActive(false);
            level.desactivables = this.desactivables;
        }

        levels[0].gameObject.SetActive(true);
        levels[0].StartLevel(refNotesPrefabs);
    }

    public void GoToNextLevel() {
        levels[currentLevel].gameObject.SetActive(false);

        currentLevel++;
        if (currentLevel == levels.Length) {
            GoToEnding();
            return;
        }

        levels[currentLevel].gameObject.SetActive(true);
        levels[currentLevel].StartLevel(refNotesPrefabs);
        ResetCursor();
    }

    void GoToEnding() {
        print("Last level complete, go to ending of the game");
    }

    public void ResetCursor() {
        levels[currentLevel].ResetCursor();
    }

}
