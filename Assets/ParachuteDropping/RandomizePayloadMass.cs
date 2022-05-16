using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizePayloadMass : MonoBehaviour
{
	[Header( "Fractionally randomly changes existing mass.")]
	public float MinMass;
	public float MaxMass;

	void Start ()
	{
		var rb = GetComponent<Rigidbody>();

		float fraction = Random.Range( MinMass, MaxMass);

		rb.mass *= fraction;
	}
}
