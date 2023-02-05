using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Reference : How to Code a Screen Shake Effect | Unity Tutorial
		- https://www.google.com/search?q=unity+shaking+screen&oq=unity+shaking+screen+&aqs=chrome..69i57j0i22i30l9.3167j0j7&sourceid=chrome&ie=UTF-8#fpstate=ive&vld=cid:0117d80e,vid:BQGTdRhGmE4
*/

public class ShakeController : MonoBehaviour
{
	[SerializeField] private bool start;
	[SerializeField] private float duration = 1f;
	[SerializeField] private AnimationCurve curve;

	private void Update()
	{
		if (start)
		{
			start = !start;
			StartCoroutine(Shaking());
		}
	}

	IEnumerator Shaking()
	{
		Vector3 startPosition = transform.position;
		float elapsedTime = 0f;

		while (elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;
			float strength = curve.Evaluate(elapsedTime / duration);
			transform.position = startPosition + Random.insideUnitSphere * strength;
			yield return null;
		}

		transform.position = startPosition;
	}

	public void StartShake(){
		start = true;
	}
}

