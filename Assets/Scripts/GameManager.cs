using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour {

    LevelManager[] levels;
    public GameObject[] desactivables;
    public GameObject[] refNotesPrefabs;
    public PlayableDirector currentDirector;
    int currentLevel;
    SoundManager soundManager;

    private void Awake() {
        levels = GetComponentsInChildren<LevelManager>();
        soundManager = FindObjectOfType<SoundManager>();

        foreach(LevelManager level in levels) {
            level.gameObject.SetActive(false);
            level.desactivables = this.desactivables;
            level.director = currentDirector;
        }

        levels[0].gameObject.SetActive(true);
        levels[0].StartLevel(refNotesPrefabs, soundManager);
    }

    public void GoToNextLevel() {
        levels[currentLevel].gameObject.SetActive(false);

        currentLevel++;
        if (currentLevel == levels.Length) {
            GoToEnding();
            return;
        }

        levels[currentLevel].gameObject.SetActive(true);
        levels[currentLevel].StartLevel(refNotesPrefabs, soundManager);
        ResetCursor();
    }

    void GoToEnding() {
        print("Last level complete, go to ending of the game");
    }

    public void ResetCursor() {
        levels[currentLevel].ResetCursor();
    }

	public void ResetMelody(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			ResetCursor();
		}
	}
}
