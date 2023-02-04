using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpot : MonoBehaviour {
    [SerializeField] private AudioClip[] notes;
    [SerializeField] private Color[] noteColors;

    [SerializeField] private bool isMovingVertical;
    [SerializeField] private bool isMovingHorizontal;
    [SerializeField] private float speedMov = 2f;
    [SerializeField] private float heightMov = 0.005f;

    private AudioSource source;
    private SpriteRenderer sprite;
    private MelodyManager melodyManager;
    int noteIndex;
    bool isHidden;
    Vector3 initalPos;
    [HideInInspector] public int column;
    [HideInInspector] public bool reference;

    private float dec = 0.0001f;

    public void Init() {
        source = GetComponent<AudioSource>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        melodyManager = FindObjectOfType<MelodyManager>();

        SetNoteIndex(Random.Range(0, notes.Length));
        initalPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!isHidden && collision.CompareTag("Cursor")) {
            Play();
            melodyManager.RegisterNote(this);
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

    private void Update() {
        if (!reference) {
            Vector3 pos = transform.position;
            float newY = initalPos.y;
            float newX = initalPos.x;

            if (isMovingVertical) {
                newY = (Mathf.Sin(Time.time * speedMov) * heightMov) + pos.y;
            }
            if (isMovingHorizontal) {
                newX = (Mathf.Sin(Time.time * speedMov) * heightMov) + pos.x;
            }

            transform.position = new Vector3(newX, newY, pos.z);
        }
    }

    private Vector3 RandomizePosition(float minX, float maxX, float minY, float maxY, float Z) {
        return new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Z);
    }
}
