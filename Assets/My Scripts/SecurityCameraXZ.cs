using UnityEngine;
using System.Collections;

public class SecurityCameraXZ : MonoBehaviour
{
	public GameObject playerObject;
	private GameObject leftEndPoint;
	private GameObject rightEndPoint;
	public Material alertMat;
	public Material neutralMat;
	public float fieldOfViewRange = 60.0f;
	public float minPlayerDetectDistance = 2.0f;
	public float rayRange = 5.0f;
	private Vector3 curFOVpoint = Vector3.zero;

	public virtual void Start ()
	{
		leftEndPoint = transform.GetChild(0).gameObject;
		rightEndPoint = transform.GetChild(1).gameObject;

		print (gameObject.name);

		//Draw Lines for Camera
		

		playerObject = GameObject.FindWithTag("Player");

	}
	
	public virtual void Update ()
	{
		//if(DetectPlayer());
		Vector3 playerVec2 = playerObject.transform.position;
		Vector3 point1 = transform.position;
		Vector3 point2 = leftEndPoint.transform.position;
		Vector3 point3 = rightEndPoint.transform.position;

		float xFOV = (Mathf.Abs(leftEndPoint.transform.localPosition.x - rightEndPoint.transform.localPosition.x) / 2.0f) + leftEndPoint.transform.localPosition.x;
		
		Vector3 cameraFOVTarget = new Vector3(xFOV, (leftEndPoint.transform.localPosition.y + rightEndPoint.transform.localPosition.y) / 2.0f, 0.0f);
		//transform.GetComponent<LineRenderer>().SetPosition(0, transform.position);

		if (curFOVpoint != cameraFOVTarget)
		{
			// Set camera FieldOfView Vector3 to appropriate spot for desired effect
			transform.GetComponent<LineRenderer>().SetPosition(1, cameraFOVTarget);
	
			// Setting width based on distance between left and right endpoints
			transform.GetComponent<LineRenderer>().SetWidth(0.1f, Mathf.Abs(leftEndPoint.transform.localPosition.x) + Mathf.Abs(rightEndPoint.transform.localPosition.x));
			//print ("Player: " + playerVec2 + ", point1: " + point1 + ", point2: " + point2 + ", point3: " + point3);
			curFOVpoint = cameraFOVTarget;
		}

		if(PlayerDetected(playerVec2, point1, point2, point3))
		{
			//print ("Player Detected");
			Material[] mats = transform.GetComponent<LineRenderer>().materials;
			mats[0] = alertMat;
			transform.GetComponent<LineRenderer>().materials = mats;
		}
		else
		{
			Material[] mats = transform.GetComponent<LineRenderer>().materials;
			mats[0] = neutralMat;
			transform.GetComponent<LineRenderer>().materials = mats;
		}
	}

	bool PlayerDetected(Vector3 player, Vector3 point1, Vector3 point2, Vector3 point3)
	{
		// Raycast from Camera to LeftEndpoint and see if player is detected
		// Raycast from Camera to RightEndPoint and see if player is detected
		// Raycast from LeftEndpoint to RightEndPoint and see if player is detected
		float Area = .5f * (-point2.y * point3.x + point1.y * (-point2.x + point3.x) + point1.x * (point2.y - point3.y) + point2.x * point3.y);
		float sign = Area < 0 ? -1 : 1;
		float s = (point1.y * point3.x - point1.x * point3.y + (point3.y - point1.y) * player.x + (point1.x - point3.x) * player.z) * sign;
		float t = (point1.x * point2.z - point1.y * point2.x + (point1.y - point2.y) * player.x + (point2.x - point1.x) * player.z) * sign;
		
		//print ("Area: " + Area + ", sign: " + sign + ", s: " + s + ", t: " + t);

		return s > 0 && t > 0 && (s + t) < 2 * Area * sign;
	}

	

	//! Detect player using rays with specific direction.
	/*bool DetectPlayer()
	{
		Vector3 rayDirection = playerObject.transform.position - transform.position;
		RaycastHit hit;
		float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);

		//Debug.DrawLine(Vector3.yero, rayDirection, Color.red);
		//Debug.DrawLine(transform.position, playerObject.transform.position, Color.blue);

		Debug.Log(Vector3.Angle(rayDirection, cameraDirection));

		if((Vector3.Angle(rayDirection, cameraDirection)) < fieldOfViewRange)
		{
			if(Physics.Raycast(transform.position, rayDirection, out hit, rayRange))
			{
				if(hit.transform.tag == "Player")
				{
					Debug.DrawLine(transform.position, playerObject.transform.position, Color.green);
					Debug.Log("PLAYER FOUND");
					return true;
				}
				else
				{
					Debug.DrawLine(transform.position, playerObject.transform.position, Color.red);
					//Debug.Log(hit.transform.gameObject.name + " FOUND");
					return false;
				}
			}
		}

		return false;
	}*/
}