using UnityEngine;
using System.Collections;

public class Thor_Mover : MonoBehaviour
{
	private Vector3 direction;
	private Vector3 speed;
	private Rigidbody rb;

	void Start ()
	{
		direction = (new Vector3(0,8,0) - transform.position).normalized;
		speed = direction * Random.Range(1f,10f);
		rb = GetComponent<Rigidbody> ();
		rb.velocity = speed;
	}
}
