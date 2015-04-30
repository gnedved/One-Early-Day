using UnityEngine;
using System.Collections;

public class PlayerEmployee : MonoBehaviour
{

	private float lockPosition = 0.0f;
	public float rayDistance = 100.0f;
	public float playerSpeed = 10.0f;
	public float rotateSpeed = 30.0f;
	public float mouseZ = 0.0f;
	private Vector3 lockVector;
    public bool disableTarget = false;

    Animator playerAnim;

	void Start ()
	{
		lockVector =  new Vector3(lockPosition, lockPosition, lockPosition);
        playerAnim = transform.GetComponent<Animator>();
	}
	
	void Update ()
	{
		Vector3 tempVec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Quaternion newRot = Quaternion.LookRotation(transform.position - tempVec, Vector3.forward);
		newRot.x = 0.0f;
		newRot.y = 0.0f;
		transform.rotation = Quaternion.Slerp(transform.rotation, newRot, Time.deltaTime * rotateSpeed);

		if(Input.GetButtonDown("Fire1"))
		{
            disableTarget = true;
            /*if (playerAnim.GetBool("Switch"))
            {
                playerAnim.SetBool("RightPunch", true);
                playerAnim.SetBool("Switch", false);
            }
            else
            {
                playerAnim.SetBool("LeftPunch", true);
                playerAnim.SetBool("Switch", true);
            }*/
			//gameObject.transform.GetChild(0).gameObject.SetActive(true);
		}
        else
        {
            playerAnim.SetBool("RightPunch", false);
            playerAnim.SetBool("LeftPunch", false);
            disableTarget = false;
        }

		if(Input.GetKey(KeyCode.W))
		{
			//rigidbody.angularVelocity = lockVector;
            playerAnim.SetBool("Moving", true);
			GetComponent<Rigidbody>().velocity = Vector3.up * playerSpeed;
		}
		else if(Input.GetKey(KeyCode.A))
		{
			//rigidbody.angularVelocity = new Vector3(lockPosition, lockPosition, lockPosition);
            playerAnim.SetBool("Moving", true);
			GetComponent<Rigidbody>().velocity = Vector3.left * playerSpeed;
		}
		else if(Input.GetKey (KeyCode.S))
		{
			//rigidbody.angularVelocity = new Vector3(lockPosition, lockPosition, lockPosition);
            playerAnim.SetBool("Moving", true);
			GetComponent<Rigidbody>().velocity = Vector3.down * playerSpeed;
		}
		else if(Input.GetKey(KeyCode.D))
		{
			//rigidbody.angularVelocity = new Vector3(lockPosition, lockPosition, lockPosition);
            playerAnim.SetBool("Moving", true);
			GetComponent<Rigidbody>().velocity = Vector3.right * playerSpeed;
		}
        else
        {
            playerAnim.SetBool("Moving", false);
            GetComponent<Rigidbody>().velocity = lockVector;
        }

	}

}
