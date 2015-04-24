﻿using UnityEngine;
using System.Collections;

public class SmoothFollow2D : MonoBehaviour
{
	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform playerTransform;
    public bool extendVision = false;


	void Start ()
	{
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}
	

	void Update ()
	{
		if (playerTransform)
		{
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                extendVision = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                extendVision = false;
            }

            if (extendVision)
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = 0;
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
                Vector3 averagePos = (mouseWorldPos + playerTransform.position) / 2.0f;
                Vector3 point = GetComponent<Camera>().WorldToViewportPoint(averagePos);
                Vector3 delta = averagePos - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
                Vector3 destination = transform.position + delta;
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
            }
            else
            {
                Vector3 point = GetComponent<Camera>().WorldToViewportPoint(playerTransform.position);
                Vector3 delta = playerTransform.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
                Vector3 destination = transform.position + delta;
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
            }
		}
	}
}
