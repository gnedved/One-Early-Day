using UnityEngine;
using System.Collections;

public class MonitorCamera : SecurityCamera
{
	public float rotateDegrees = 60.0f;
	public bool rotateDirection = false;



	public override void Start ()
	{
		base.Start();
	}
	
	// Update is called once per frame
	public override void Update ()
	{
		base.Update();

		transform.rotation = Quaternion.Euler(0,0, Mathf.Sin(Time.realtimeSinceStartup) * rotateDegrees);
	}
}