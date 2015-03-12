using UnityEngine;
using System.Collections;

public class MonitorCameraXZ : SecurityCamera
{
	public float rotateDegrees = 30.0f;
	private float initialRotation = 0.0f;
	public bool rotateDirection = true;
	// Use this for initialization
	public override void Start ()
	{
		base.Start();
		initialRotation = transform.eulerAngles.y;
		print (initialRotation);
	}
	
	// Update is called once per frame
	public override void Update ()
	{
		base.Update();
		transform.rotation = Quaternion.Euler(0, Mathf.Sin(Time.realtimeSinceStartup) * rotateDegrees, 0);
		//transform.rotation = Quaternion.Euler(0,0, Mathf.Sin(Time.realtimeSinceStartup) * rotateDegrees);
	}

	
}