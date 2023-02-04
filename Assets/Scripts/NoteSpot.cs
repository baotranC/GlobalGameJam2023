using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpot : MonoBehaviour
{
	[SerializeField] private AudioClip[] notes;
	[SerializeField] private Color[] noteColors;

	// Lignes/colonnes bougent
	[SerializeField] private bool isMovingVertical;
	[SerializeField] private bool isMovingHorizontal;
	[SerializeField] private float speedMov = 2f;
	[SerializeField] private float heightMov = 0.005f;
	// Les notes alternent entre 2 couleurs
	[SerializeField] private bool isAlternateColor;
	[SerializeField] private float alternateColorDelay = 1000f;
	private int alternateColorNoteIndex;
	[HideInInspector] public bool isCurrentColorNote;

	private AudioSource source;
	private SpriteRenderer sprite;
	private MelodyManager melodyManager;
	private int noteIndex;
	bool isHidden;
	Vector3 initalPos;
	[HideInInspector] public int column;
	[HideInInspector] public bool isReferenceNote;

	private float dec = 0.0001f;

	public void Init()
	{
		source = GetComponent<AudioSource>();
		sprite = GetComponentInChildren<SpriteRenderer>();
		melodyManager = FindObjectOfType<MelodyManager>();

		SetNoteIndex(Random.Range(0, notes.Length));
		initalPos = transform.position;

		InvokeRepeating("AlternateColor", alternateColorDelay, alternateColorDelay);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!isHidden && collision.CompareTag("Cursor"))
		{
			Play();
			melodyManager.RegisterNote(this);
		}
	}

	void Play()
	{
		source.clip = notes[noteIndex];
		source.Play();
	}

	public void Hide()
	{
		Color hiddenColor = sprite.color;
		hiddenColor.a = 0.5f;
		sprite.color = hiddenColor;
		isHidden = true;
	}

	public void Show()
	{
		Color normalColor = sprite.color;
		normalColor.a = 1f;
		sprite.color = normalColor;
		isHidden = false;
	}

	public void SetNoteIndex(int value)
	{
		noteIndex = value;
		sprite.color = noteColors[noteIndex];
	}

	public void SetSpriteColor(int noteIndex)
	{
		sprite.color = noteColors[noteIndex];
	}

	public int GetNoteIndex()
	{
		return noteIndex;
	}

	private void Update()
	{
		if (!isReferenceNote)
		{
			Vector3 pos = transform.position;
			float newY = initalPos.y;
			float newX = initalPos.x;

			if (isMovingVertical)
			{
				newY = (Mathf.Sin(Time.time * speedMov) * heightMov) + pos.y;
			}
			if (isMovingHorizontal)
			{
				newX = (Mathf.Sin(Time.time * speedMov) * heightMov) + pos.x;
			}
			transform.position = new Vector3(newX, newY, pos.z);
		}
	}

	private void AlternateColor()
	{
		if (!isReferenceNote)
		{
			if (isCurrentColorNote)
			{
				// alternateColorNoteIndex = Random.Range(0, noteColors.Length);
				SetSpriteColor(2);
				isCurrentColorNote = !isCurrentColorNote;
			}
			else
			{
				SetSpriteColor(noteIndex);
				isCurrentColorNote = !isCurrentColorNote;
			}
		}
	}
}
