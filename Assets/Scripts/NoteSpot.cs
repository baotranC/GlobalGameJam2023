using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class NoteSpot : MonoBehaviour {

    [SerializeField] private AudioClip[] notes;

    [SerializeField] private bool isMovingVertical;
    [SerializeField] private bool isMovingHorizontal;
    [SerializeField] private float speedMov = 2f;
    [SerializeField] private float heightMov = 0.005f;

    private AudioSource source;
    [HideInInspector] public SpriteRenderer sprite;
    public Animator animator;
    public Animator forceAnimator;

    [HideInInspector] public Light2D light;

    private MelodyManager melodyManager;
    int noteIndex;
    bool isHidden;
    Vector3 initalPos;
    [HideInInspector] public int column;
    [HideInInspector] public bool reference;
    [HideInInspector] public Color[] noteColors;
    SoundManager soundManager;

    public void Init(Color[] noteColors, SoundManager soundManager) {
        source = GetComponent<AudioSource>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        light = GetComponentInChildren<Light2D>();

        melodyManager = FindObjectOfType<MelodyManager>();
        this.noteColors = noteColors;
        this.soundManager = soundManager;

        SetNoteIndex(Random.Range(0, noteColors.Length));
        initalPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!isHidden && collision.CompareTag("Cursor")) {
            Play();
            animator.SetBool("Interacted", true);
            melodyManager.RegisterNote(this);
        }
    }

    void Play() {
        source.clip = notes[noteIndex];
        soundManager.PlaySound(source, createTempSource: true);
    }

    public void Hide(bool hideSprite) {
        if (hideSprite) {
            Color hiddenColor = sprite.color;
            hiddenColor.a = 0.2f;
            sprite.color = hiddenColor;
        }
        isHidden = true;
    }

    public void Show() {
        Color normalColor = sprite.color;
        normalColor.a = 1f;
        sprite.material.SetColor("OverlayCol", normalColor);
        light.color = normalColor;
        animator.SetBool("Interacted", false);

        isHidden = false;
        SetNoteIndex(noteIndex);
    }

    public void ConcertMode() {
        Show();
        animator.SetBool("Interacted", true);
    }

    public void SetNoteIndex(int value) {
        noteIndex = value;
        sprite.color = noteColors[noteIndex];
        animator.SetBool("Error", false);
        forceAnimator.SetBool("LastNote", false);

        light.color = noteColors[noteIndex];

    }

    public int GetNoteIndex() {
        return noteIndex;
    }

    private void Update() {
        if (!reference) {
            float newY = initalPos.y;
            float newX = initalPos.x;

            if (isMovingVertical) {
                newY += (Mathf.Sin(Time.time * speedMov) * heightMov);
            }
            if (isMovingHorizontal) {
                newX += (Mathf.Sin(Time.time * speedMov) * heightMov);
            }

            transform.position = new Vector3(newX, newY, transform.position.z);
        }
    }

    private Vector3 RandomizePosition(float minX, float maxX, float minY, float maxY, float Z) {
        return new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Z);
    }
}
