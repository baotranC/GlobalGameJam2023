using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelodyManager : MonoBehaviour
{

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


	public void Init(Color[] noteColors)
	{
		print("Start of" + gameObject.name);
		if (melody.Length != notesLineRef.Length)
		{
			throw new System.Exception("Melody " + gameObject.name +
				" has different numbers of notes in its melody (" + melody.Length.ToString() +
				") and its reference line (" + notesLineRef.Length + ")");
		}
		cursor = FindObjectOfType<Cursor>();
		cursor.horizontalSpeed = cursorHorizontalSpeed;
		cursor.verticalSpeed = cursorVerticalSpeed;
		cursor.SetDarkness(hasDarkness);
		this.noteColors = noteColors;

		// Find what notes are in each column
		notesPerColumn = new List<List<NoteSpot>>();
		for (int i = 0; i < notesLineRef.Length; ++i)
		{
			notesLineRef[i].Init(noteColors);

			List<NoteSpot> columnNotes = new List<NoteSpot>();
			RaycastHit2D[] hits = Physics2D.RaycastAll(notesLineRef[i].transform.position, Vector2.down);

			foreach (RaycastHit2D hit in hits)
			{
				NoteSpot noteSpot = hit.collider.gameObject.GetComponent<NoteSpot>();
				if (noteSpot != null && noteSpot != notesLineRef[i])
				{
					noteSpot.Init(noteColors);
					columnNotes.Add(noteSpot);
				}
			}

			notesPerColumn.Add(columnNotes);
		}

		// Tell each note what column they are in
		for (int i = 0; i < notesPerColumn.Count; ++i)
		{
			foreach (NoteSpot noteSpot in notesPerColumn[i])
			{
				noteSpot.column = i;
			}
		}

		// Set right note for the reference track AND one random note in its column
		for (int i = 0; i < melody.Length; ++i)
		{
			int randomIndex = Random.Range(0, notesPerColumn[i].Count);
			notesPerColumn[i][randomIndex].SetNoteIndex(melody[i]);
			notesLineRef[i].SetNoteIndex(melody[i]);
			notesLineRef[i].reference = true;
		}
	}

	public void ResetCursor()
	{
		cursor.Reset();
		hasFailed = false;
		currentNote = 0;
		for (int i = 0; i < notesPerColumn.Count; ++i)
		{
			foreach (NoteSpot noteSpot in notesPerColumn[i])
			{
				noteSpot.Show();
			}
		}
	}

	public void RegisterNote(NoteSpot noteSpot)
	{
		int noteIndex = noteSpot.GetNoteIndex();

		if (melody[currentNote] == noteIndex)
		{
			//print("Note is valid (" + (currentNote + 1) + "/" + melody.Length + ")");
		}
		else
		{
			//print("Wrong note !  (" + (currentNote + 1) + "/" + melody.Length + ")");
			hasFailed = true;
			DisplayFailedRow(currentNote);
			// TODO: HERE
		}

		currentNote++;

		if (currentNote == melody.Length)
		{
			if (hasFailed)
			{
				print("Melody failed");
			}
			else
			{
				EndMelody();
			}
		}

		foreach (NoteSpot noteSpot1 in notesPerColumn[noteSpot.column])
		{
			noteSpot1.Hide(hideSprite: (noteSpot1 != noteSpot));
		}
	}

	public void DisplayFailedRow(int currentNote)
	{
		for (int i = 0; i < notesPerColumn[currentNote].Count; ++i)
		{
			//notesPerColumn[currentNote][i].SetNoteIndex(5); // TODO: make this a constant
			notesPerColumn[currentNote][i].sprite.color = Color.black;
		}
	}

	void EndMelody()
	{
		print(gameObject.name + " succeeded !");
		StartCoroutine(PerformEndMelody());
	}

	IEnumerator PerformEndMelody()
	{
		yield return new WaitForSeconds(1f);
		GetComponentInParent<LevelManager>().GoToNextMelody();
	}
}
