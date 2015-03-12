using UnityEngine;
using System.Collections;

public class PlayerEmployeeXZ : MonoBehaviour
{

	private float lockPosition = 0.0f;
	public float rayDistance = 100.0f;
	public float playerSpeed = 10.0f;
	public float rotateSpeed = 30.0f;
	public float mouseZ = 0.0f;
	private Vector3 lockVector;

	void Start ()
	{
		lockVector =  new Vector3(lockPosition, lockPosition, lockPosition);
	}
	
	void Update ()
	{
		Vector3 tempVec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Quaternion newRot = Quaternion.LookRotation(transform.position - tempVec, Vector3.forward);
		newRot.x = 0.0f;
		newRot.z = 0.0f;
		transform.rotation = Quaternion.Slerp(transform.rotation, newRot, Time.deltaTime * rotateSpeed);

		if(Input.GetButtonDown("Fire1"))
		{
			gameObject.transform.GetChild(0).gameObject.SetActive(true);
		}

		if(Input.GetButtonDown("Fire2"))
		{
			gameObject.transform.GetChild(0).gameObject.SetActive(false);
		}

		if(Input.GetKey(KeyCode.W))
		{
			//rigidbody.angularVelocity = lockVector;
			rigidbody.velocity = Vector3.forward * playerSpeed;
		}
		else
		{
			rigidbody.velocity = lockVector;
		}

		if(Input.GetKey(KeyCode.A))
		{
			//rigidbody.angularVelocity = new Vector3(lockPosition, lockPosition, lockPosition);
			rigidbody.velocity = Vector3.left * playerSpeed;
		}

		if(Input.GetKey (KeyCode.S))
		{
			//rigidbody.angularVelocity = new Vector3(lockPosition, lockPosition, lockPosition);
			rigidbody.velocity = Vector3.back * playerSpeed;
		}

		if(Input.GetKey(KeyCode.D))
		{
			//rigidbody.angularVelocity = new Vector3(lockPosition, lockPosition, lockPosition);
			rigidbody.velocity = Vector3.right * playerSpeed;
		}

	}

}
