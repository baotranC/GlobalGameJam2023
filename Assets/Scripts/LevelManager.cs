using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LevelManager : MonoBehaviour {

    public AudioClip concertAudio;
    [SerializeField] private Color[] noteColors;

    MelodyManager[] melodies;
    int currentMelody;
    AudioSource concertSource;

    [SerializeField] private VolumeProfile profile;
    [HideInInspector] private ChromaticAberration aberration;
    [HideInInspector] private Vignette vignette;

    [HideInInspector] private Light2D light;
    [HideInInspector] public GameObject[] desactivables;
    [HideInInspector] public GameObject[] refNotesPrefabs;
    public PlayableDirector director;
    SoundManager soundManager;


    public void StartLevel(GameObject[] refNotesPrefabs, SoundManager soundManager) {
        print("Start of " + gameObject.name);
        melodies = GetComponentsInChildren<MelodyManager>();
        concertSource = GetComponent<AudioSource>();
        this.refNotesPrefabs = refNotesPrefabs;
        this.soundManager = soundManager;

        foreach (MelodyManager melody in melodies) {
            melody.gameObject.SetActive(false);
        }

        melodies[0].gameObject.SetActive(true);
        melodies[0].Init(noteColors, refNotesPrefabs, soundManager);
    }

    public void GoToNextMelody() {
        melodies[currentMelody].gameObject.SetActive(false);

        currentMelody++;
        if (currentMelody == melodies.Length) {
            EndLevel();
            return;
        }

        melodies[currentMelody].gameObject.SetActive(true);
        melodies[currentMelody].Init(noteColors, refNotesPrefabs, soundManager);
    }

    public void EndLevel() {
        print("End of " + gameObject.name);
        StartCoroutine(PerformEndLevel());
    }

    IEnumerator PerformEndLevel() {
        // Re-display notes from the last melody
        // TODO change to custom notes from all screens
        melodies[currentMelody - 1].gameObject.SetActive(true);


        light = GameObject.FindGameObjectsWithTag("GlobalLight")[0].GetComponent<Light2D>();

        director.Play();

        yield return new WaitForSeconds(2.3f);

        foreach (GameObject go in desactivables) {
            go.active = !go.active;
        };
        melodies[currentMelody - 1].ConcertMode();

        // Play concert music
        if (profile.TryGet<ChromaticAberration>(out aberration)) {
            aberration.active = true;
        }
        if (profile.TryGet<Vignette>(out vignette)) {
            vignette.active = true;
        }
        light.intensity = .2f;

        yield return new WaitForSeconds(1f);


        concertSource.clip = concertAudio;
        concertSource.Play();
        // TODO setup all effects

        yield return new WaitForSeconds(concertAudio.length);

        director.Play();

        yield return new WaitForSeconds(2.3f);

        light.intensity = .8f;
        if (profile.TryGet<ChromaticAberration>(out aberration)) {
            aberration.active = false;
        }
        if (profile.TryGet<Vignette>(out vignette)) {
            vignette.active = false;
        }

        foreach (GameObject go in desactivables) {
            go.active = !go.active;
        };
        // TODO remove all effects
        GetComponentInParent<GameManager>().GoToNextLevel();
    }

    public void ResetCursor() {
        if (currentMelody < melodies.Length) {
            melodies[currentMelody].ResetCursor();
        }
    }

}
