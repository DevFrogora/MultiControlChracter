using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParachuteOpenAtYVelocity : MonoBehaviour
{
	public GameObject ChuteRigidbody;
	public GameObject ChuteVisual;

	float TriggerYVelocity;

	Rigidbody rb;

	bool popped;

	float chuteCanopyHeight;

	void Awake()
	{
		// close the chute on drop
		ChuteRigidbody.SetActive(false);
		ChuteVisual.SetActive(false);

		chuteCanopyHeight = Mathf.Abs( ChuteRigidbody.transform.localPosition.y);
	}

	void Start ()
	{
		rb = GetComponent<Rigidbody>();

		TriggerYVelocity = Random.Range( 18, 24);
	}

	IEnumerator PopChuteOpen()
	{
		// show the chute immediately
		ChuteVisual.SetActive( true);

		popped = true;

		// note where it began to open
		Vector3 PoppedPosition = transform.position;

		// observe the shape of the chute visual
		Vector3 chuteVisualScale = ChuteVisual.transform.localScale;

		// open the chute as it falls (no rigidbody braking yet)
		while( true)
		{
			// how much should we open?
			float distanceBelow = Mathf.Abs( PoppedPosition.y - transform.position.y);

			float fraction = distanceBelow / chuteCanopyHeight;

			Vector3 scale = Mathf.Lerp( 0.2f, 1.0f, fraction) * chuteVisualScale;

			ChuteVisual.transform.localScale = scale;

			ChuteVisual.transform.position = PoppedPosition;

			// parachute fully open
			if (fraction > 1.0f)
			{
				ChuteRigidbody.SetActive( true);

				Destroy(this);
			}

			yield return new WaitForFixedUpdate();
		}
	}

	void FixedUpdate()
	{
		if (popped) return;

		// pop the chute open when you reach a certain absolute velocity
		if (Mathf.Abs( rb.velocity.y) > TriggerYVelocity)
		{
			StartCoroutine( PopChuteOpen());
		}
	}
}
