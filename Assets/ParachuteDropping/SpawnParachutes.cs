using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParachutes : MonoBehaviour
{
	// put this on the spawn point, with two child objects for left/right
	public float Interval;
	public GameObject Prefab;

	IEnumerator Start ()
	{
		while( true)
		{
			yield return new WaitForSeconds( Interval);

			float fraction = Random.Range( 0.10f, 0.90f);

			var child1 = transform.GetChild(0);
			var child2 = transform.GetChild(1);

			var position = Vector3.Lerp( child1.position, child2.position, fraction);

			Instantiate<GameObject>( Prefab, position, Quaternion.identity);
		}
	}
}
