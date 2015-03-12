using UnityEngine;
using System.Collections;

public class SmoothFollow2D : MonoBehaviour
{
	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform playerTransform;


	void Start ()
	{
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}
	

	void Update ()
	{
		if (playerTransform)
		{
			Vector3 point = camera.WorldToViewportPoint(playerTransform.position);
			Vector3 delta = playerTransform.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}
	}
}
