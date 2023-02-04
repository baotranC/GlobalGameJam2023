using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int[] melody;
    public NoteSpot[] notesColumn1;
    public NoteSpot[] notesColumn2;
    public NoteSpot[] notesColumn3;
    public NoteSpot[] notesColumn4;
    public NoteSpot[] notesColumn5;
    public NoteSpot[] notesColumn6;
    public NoteSpot[] notesLineRef;

    int currentNote;
    Cursor cursor;
    bool hasFailed;
    List<NoteSpot[]> notesPerColumn;


    private void Awake() {
        cursor = FindObjectOfType<Cursor>();

        notesPerColumn = new List<NoteSpot[]>();
        notesPerColumn.Add(notesColumn1);
        notesPerColumn.Add(notesColumn2);
        notesPerColumn.Add(notesColumn3);
        notesPerColumn.Add(notesColumn4);
        notesPerColumn.Add(notesColumn5);
        notesPerColumn.Add(notesColumn6);

        for (int i=0; i<notesPerColumn.Count; ++i) {
            foreach(NoteSpot noteSpot in notesPerColumn[i]) {
                noteSpot.column = i;
            }
        }
    }

    private void Start() {
        for (int i = 0; i < melody.Length; ++i) {
            notesPerColumn[i][Random.Range(0, notesPerColumn[i].Length)].SetNoteIndex(melody[i]);
            notesLineRef[i].SetNoteIndex(melody[i]);
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

        if(melody[currentNote] == noteIndex) {
            print("Note is valid (" + (currentNote + 1) + "/" + melody.Length + ")");
        } else {
            print("Wrong note !  (" + (currentNote + 1) + "/" + melody.Length + ")");
            hasFailed = true;
        }
        
        currentNote++;

        if(currentNote == melody.Length) {
            if(hasFailed) {
                print("Melody failed");
            } else {
                print("Melody succeeded !");
            }
        }

        foreach(NoteSpot noteSpot1 in notesPerColumn[noteSpot.column]) {
            if (noteSpot1 != noteSpot) {
                noteSpot1.Hide();
            }
        }
    }

}
