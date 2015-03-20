﻿using UnityEngine;
using System.Collections;

public class SmoothFollow2DXZ : MonoBehaviour
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
			Vector3 point = GetComponent<Camera>().WorldToViewportPoint(playerTransform.position);
			Vector3 delta = playerTransform.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, point.y, 0.5f));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}
	}
}
