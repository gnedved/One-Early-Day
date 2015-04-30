using UnityEngine;
using System.Collections;

public class RobotSwitch : MonoBehaviour
{
    private AI parentScript;
    private PlayerEmployee playerScript;

    void Start ()
    {
        parentScript = transform.parent.GetComponent<AI>();
        playerScript = GameObject.Find("Player").GetComponent<PlayerEmployee>();
    }

    void OnTriggerEnter (Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (playerScript.disableTarget)
            {
                parentScript.disableStatus();
            }
        }
    }

}
