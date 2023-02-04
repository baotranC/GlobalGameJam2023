using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpot : MonoBehaviour {

    public AudioClip[] notes;
    public Color[] noteColors;

    AudioSource source;
    SpriteRenderer sprite;
    GameManager gameManager;
    int noteIndex;
    bool isHidden;
    [HideInInspector] public int column;
    
    void Awake() {
        source = GetComponent<AudioSource>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();

        SetNoteIndex(Random.Range(0, notes.Length));
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(!isHidden && collision.CompareTag("Cursor")) {
            Play();
            gameManager.RegisterNote(this);
        }
    }

    void Play() {
        source.clip = notes[noteIndex];
        source.Play();
    }

    public void Hide() {
        Color hiddenColor = sprite.color;
        hiddenColor.a = 0.5f;
        sprite.color = hiddenColor;
        isHidden = true;
    }

    public void Show() {
        Color normalColor = sprite.color;
        normalColor.a = 1f;
        sprite.color = normalColor;
        isHidden = false;
    }

    public void SetNoteIndex(int value) {
        noteIndex = value;
        sprite.color = noteColors[noteIndex];
    }

    public int GetNoteIndex() {
        return noteIndex;
    }
}
