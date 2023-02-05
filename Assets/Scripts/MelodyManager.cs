using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelodyManager : MonoBehaviour {

    public int[] melody;
    public NoteSpot[] notesLineRef;
    [Tooltip("Space_between_steps * 60 / bpm")]
    public float cursorHorizontalSpeed = 2f;
    public float cursorVerticalSpeed = 10f;
    public bool hasDarkness;

    int currentNote;
    Cursor cursor;
    bool hasFailed;
    List<List<NoteSpot>> notesPerColumn;
    [HideInInspector] public Color[] noteColors;
    [HideInInspector] public GameObject[] refNotesPrefabs;
    List<Animator> refNotesInstancesAnimators;
    SoundManager soundManager;
    private ShakeController shake;


    public void Init(Color[] noteColors, GameObject[] refNotesPrefabs, SoundManager soundManager) {
        print("Start of" + gameObject.name);
        if (melody.Length != notesLineRef.Length) {
            throw new System.Exception("Melody " + gameObject.name +
                " has different numbers of notes in its melody (" + melody.Length.ToString() +
                ") and its reference line (" + notesLineRef.Length + ")");
        }
        cursor = FindObjectOfType<Cursor>();
        cursor.horizontalSpeed = cursorHorizontalSpeed;
        cursor.verticalSpeed = cursorVerticalSpeed;
        cursor.SetDarkness(hasDarkness);
        shake = FindObjectOfType<ShakeController>();
        this.noteColors = noteColors;
        this.refNotesPrefabs = refNotesPrefabs;
        this.soundManager = soundManager;

        // Find what notes are in each column
        notesPerColumn = new List<List<NoteSpot>>();
        for (int i = 0; i < notesLineRef.Length; ++i) {
            notesLineRef[i].Init(noteColors, soundManager);

            List<NoteSpot> columnNotes = new List<NoteSpot>();
            RaycastHit2D[] hits = Physics2D.RaycastAll(notesLineRef[i].transform.position, Vector2.down);

            foreach (RaycastHit2D hit in hits) {
                NoteSpot noteSpot = hit.collider.gameObject.GetComponent<NoteSpot>();
                if (noteSpot != null && noteSpot != notesLineRef[i]) {
                    noteSpot.Init(noteColors, soundManager);
                    columnNotes.Add(noteSpot);
                }
            }

            notesPerColumn.Add(columnNotes);
        }

        // Tell each note what column they are in
        for (int i = 0; i < notesPerColumn.Count; ++i) {
            foreach (NoteSpot noteSpot in notesPerColumn[i]) {
                noteSpot.column = i;
            }
        }

        // Set right note for the reference track AND one random note in its column
        refNotesInstancesAnimators = new List<Animator>();
        for (int i = 0; i < melody.Length; ++i) {
            int randomIndex = Random.Range(0, notesPerColumn[i].Count);
            // Random note in column
            notesPerColumn[i][randomIndex].SetNoteIndex(melody[i]);

            // Reference track
            notesLineRef[i].SetNoteIndex(melody[i]);
            notesLineRef[i].reference = true;
            notesLineRef[i].sprite.enabled = false;
            Animator refNoteAnimator = Instantiate(refNotesPrefabs[melody[i]], notesLineRef[i].transform).GetComponent<Animator>();
            refNotesInstancesAnimators.Add(refNoteAnimator);
        }
    }

    public void ResetCursor() {
        cursor.Reset();
        hasFailed = false;
        currentNote = 0;
        for (int i = 0; i < notesPerColumn.Count; ++i) {
            foreach (NoteSpot noteSpot in notesPerColumn[i]) {
                noteSpot.Show();
            }
            refNotesInstancesAnimators[i].SetBool("Triggered", false);
        }
    }

    public void RegisterNote(NoteSpot noteSpot) {
        int noteIndex = noteSpot.GetNoteIndex();

        if (melody[noteSpot.column] == noteIndex) {
            //print("Note is valid (" + (noteSpot.column + 1) + "/" + melody.Length + ")");
            refNotesInstancesAnimators[noteSpot.column].SetBool("Triggered", true);
        } else {
            //print("Wrong note !  (" + (noteSpot.column + 1) + "/" + melody.Length + ")");
            hasFailed = true;
            DisplayFailedRow(noteSpot.column);
            shake.StartShake();
        }

        currentNote++;

        if (noteSpot.column + 1 == melody.Length) {
            if (hasFailed || currentNote != noteSpot.column + 1) {
                print("Melody failed");
            } else {
                EndMelody();
            }
        }

        foreach (NoteSpot noteSpot1 in notesPerColumn[noteSpot.column]) {
            noteSpot1.Hide(hideSprite: (noteSpot1 != noteSpot));
        }
    }

    public void DisplayFailedRow(int currentNote) {
        for (int i = 0; i < notesPerColumn[currentNote].Count; ++i) {
            notesPerColumn[currentNote][i].sprite.color = Color.black;
            notesPerColumn[currentNote][i].animator.SetBool("Error",true);

        }
    }

    void EndMelody() {
        print(gameObject.name + " succeeded !");
        StartCoroutine(PerformEndMelody());
    }

    IEnumerator PerformEndMelody() {
        yield return new WaitForSeconds(1f);
        GetComponentInParent<LevelManager>().GoToNextMelody();
    }
}
