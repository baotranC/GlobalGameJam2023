using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int[] melody;
    public NoteSpot[] notesLineRef;

    int currentNote;
    Cursor cursor;
    bool hasFailed;
    List<List<NoteSpot>> notesPerColumn;


    private void Awake() {
        cursor = FindObjectOfType<Cursor>();

        notesPerColumn = new List<List<NoteSpot>>();

        for (int i = 0; i < notesLineRef.Length; ++i) {
            List<NoteSpot> columnNotes = new List<NoteSpot>();
            RaycastHit2D[] hits = Physics2D.RaycastAll(notesLineRef[i].transform.position, Vector2.down);
            foreach (RaycastHit2D hit in hits) {
                NoteSpot noteSpot = hit.collider.gameObject.GetComponent<NoteSpot>();
                if (noteSpot != null && noteSpot != notesLineRef[i]) {
                    columnNotes.Add(noteSpot);
                }
            }
            notesPerColumn.Add(columnNotes);
        }

        for (int i = 0; i < notesPerColumn.Count; ++i) {
            foreach (NoteSpot noteSpot in notesPerColumn[i]) {
                noteSpot.column = i;
            }
        }
    }

    private void Start() {
        for (int i = 0; i < melody.Length; ++i) {
            print(i.ToString() + " " + notesPerColumn[i].Count.ToString());
            print("   " + melody[i].ToString() + " " + notesLineRef[i].ToString());
            notesPerColumn[i][Random.Range(0, notesPerColumn[i].Count)].SetNoteIndex(melody[i]);
            print("a");
            notesLineRef[i].SetNoteIndex(melody[i]);
            print("b");
            notesLineRef[i].reference = true;
            print("c");
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
        }
    }

    public void StartPlay() {
        cursor.Play();
    }

    public void RegisterNote(NoteSpot noteSpot) {
        int noteIndex = noteSpot.GetNoteIndex();

        if (melody[currentNote] == noteIndex) {
            print("Note is valid (" + (currentNote + 1) + "/" + melody.Length + ")");
        } else {
            print("Wrong note !  (" + (currentNote + 1) + "/" + melody.Length + ")");
            hasFailed = true;
        }

        currentNote++;

        if (currentNote == melody.Length) {
            if (hasFailed) {
                print("Melody failed");
            } else {
                print("Melody succeeded !");
            }
        }

        foreach (NoteSpot noteSpot1 in notesPerColumn[noteSpot.column]) {
            if (noteSpot1 != noteSpot) {
                noteSpot1.Hide();
            }
        }
    }

}
