using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PolyNavAgent))]
public class AI : MonoBehaviour
{
    public GameObject pathManager;
    private GameController gameController;
    private PathManager pathManagerScript;
    //private bool patrolling, roaming, idle, nodeReached;
    public float fieldOfViewAngle = 110.0f;
    public float disableTimer = 10.0f;
    public float disableStart = 0.0f;
    private bool nodeReached;
    private bool playerInSight;
    private GameObject activePath;
    private GameObject currentNode;
    public Material alertMat;
    public Material neutralMat;
    private GameObject leftEndPoint;
    private GameObject rightEndPoint;
    public GameObject playerObject;
    public CircleCollider2D robotCollider;
    public ActivityStatus currentStatus;

    public enum ActivityStatus
    {
        Patrolling,
        Investigating,
        Chasing,
        Idle,
        Stand,
        Disabled
    }

    private PolyNavAgent _agent;
    public PolyNavAgent agent
    {
        get
        {
            if (!_agent)
                _agent = GetComponent<PolyNavAgent>();
            return _agent;
        }
    }


	void Start ()
    {
        leftEndPoint = transform.GetChild(0).gameObject;
        rightEndPoint = transform.GetChild(1).gameObject;
        playerObject = GameObject.Find("Player");
        pathManager = GameObject.Find("PathManager");
        pathManagerScript = pathManager.GetComponent<PathManager>();
        robotCollider = GetComponent<CircleCollider2D>();
        nodeReached = false;
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
	}
	

	void Update ()
    {
        Vector3 playerPoint = playerObject.transform.position;
        Vector3 point1 = transform.position;
        Vector3 point2 = leftEndPoint.transform.position;
        Vector3 point3 = rightEndPoint.transform.position;

        /*if (gameController.playerPosition != gameController.resetPosition)
        {
            playerPoint = gameController.playerPosition;
            currentStatus = ActivityStatus.Chasing;
        }*/

        if (currentStatus == ActivityStatus.Disabled)
        {
            //Robot is deactivated, not sure if for period of time or permanent
            Disabled();
        }
        else if (currentStatus == ActivityStatus.Stand)
        {
            Stand();
        }
        else if (currentStatus == ActivityStatus.Chasing && playerInSight)
        {
            Chasing(playerPoint);
        }
        else if (currentStatus == ActivityStatus.Idle)
        {
            GameObject tempNode = FindNearestPath();
            Debug.Log("TempNode: " + tempNode.name);
            if (tempNode != null)
            {
                currentNode = tempNode;
            }
            activePath = currentNode.transform.parent.gameObject;
            agent.SetDestination(currentNode.transform.position);
            currentStatus = ActivityStatus.Patrolling;
        }
        else if (currentStatus == ActivityStatus.Patrolling && nodeReached)
        {
            Patrolling();
        }

        //! Checking if player is in line of sight
        if (PlayerInSight(playerPoint))
        {
            currentStatus = ActivityStatus.Chasing;
        }

        if (PlayerDetected(playerPoint, point1, point2, point3))
        {
            ChangeMaterial(true);
            //Sound Silent Alarm, update global last player position
            currentStatus = ActivityStatus.Chasing;
        }
        else
        {
            ChangeMaterial(false);
        }

        // Might have a function to detect player being within Line Of Sight like OnTriggerEnter
	}

    public GameObject FindNearestPath()
    {
        GameObject[] allNodes = pathManagerScript.GetAllNodes();
        GameObject closestNode = null;
        float closestDistance = Mathf.Infinity;
        //Vector3 currentPosition = transform.position;

        foreach(GameObject potentialNode in allNodes)
        {
            //Vector3 directionToTarget = potentialNode.transform.position - currentPosition;
            //float dSqrToTarget = directionToTarget.sqrMagnitude;
            agent.SetDestination(potentialNode.transform.position);
            float remain = agent.remainingDistance;
            agent.Stop();

            if(remain < closestDistance)
            {
                closestDistance = remain; //= dSqrToTarget;
                closestNode = potentialNode;
            }
        }

        return closestNode;
    }

    void Disabled()
    {
        if (Time.time > (disableStart + disableTimer))
        {
            currentStatus = ActivityStatus.Idle;
        }
    }

    public void disableStatus()
    {
        currentStatus = ActivityStatus.Disabled;
        disableStart = Time.time;
        agent.Stop();
    }

    void Patrolling()
    {
        GameObject nextNode = pathManagerScript.GetNextNode(currentNode, activePath);
        agent.SetDestination(nextNode.transform.position);
        currentNode = nextNode;
        nodeReached = false;
    }
    
    void Chasing(Vector3 playerPosition)
    {
        // Check if player in sight
        agent.SetDestination(playerPosition);
    }

    void Investigating(Vector3 investigationPosition)
    {
        agent.SetDestination(investigationPosition);
    }

    void Stand()
    {

    }

    /*void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            PlayerInSight(col.transform.position);
        }
    }*/

    void OnDestinationReached()
    {
        // Set nodeReached to true
        if (currentStatus == ActivityStatus.Patrolling)
        {
            //Wait 5 seconds
            //StartCoroutine(WaitForTime(5f));
            nodeReached = true;
        }
        else if (currentStatus == ActivityStatus.Investigating)
        {
            // Investigation point reached
            print("Investigation Dest Reached");
            //StartCoroutine(WaitForTime(5.0f));
            currentStatus = ActivityStatus.Idle;
            print("Post Coroutine Investigation");
        }
        else if (currentStatus == ActivityStatus.Chasing)
        {
            print("Chasing Dest Reached");
            // Check if player dead?
            if (!playerInSight)
            {
                currentStatus = ActivityStatus.Investigating;
                //StartCoroutine(WaitForTime(5.0f));
                currentStatus = ActivityStatus.Idle;
            }
        }
    }

    bool PlayerDetected(Vector3 player, Vector3 point1, Vector3 point2, Vector3 point3)
    {
        // Raycast from Camera to LeftEndpoint and see if player is detected
        // Raycast from Camera to RightEndPoint and see if player is detected
        // Raycast from LeftEndpoint to RightEndPoint and see if player is detected
        float Area = .5f * (-point2.y * point3.x + point1.y * (-point2.x + point3.x) + point1.x * (point2.y - point3.y) + point2.x * point3.y);
        float sign = Area < 0 ? -1 : 1;
        float s = (point1.y * point3.x - point1.x * point3.y + (point3.y - point1.y) * player.x + (point1.x - point3.x) * player.y) * sign;
        float t = (point1.x * point2.y - point1.y * point2.x + (point1.y - point2.y) * player.x + (point2.x - point1.x) * player.y) * sign;

        //print ("Area: " + Area + ", sign: " + sign + ", s: " + s + ", t: " + t);

        bool detected = s > 0 && t > 0 && (s + t) < 2 * Area * sign;

        // Might call helper function to see if there is direct line from robot to player just in case they are trying to hide behind a door or something
        if (PlayerInSight(player) && detected)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool PlayerInSight(Vector3 playerPosition)
    {
        // Create a vector from the enemy to the player and store the angle between it and forward.
        Vector3 direction = playerPosition - transform.position;
        float angle = Vector3.Angle(direction, transform.up);
        //Debug.DrawLine(transform.position + transform.up, direction, Color.green, 1.0f, false);

        // If the angle between forward and where the player is, is less than half the angle of view...
        if (angle < fieldOfViewAngle * 0.5f)
        {
            RaycastHit hit;

            // ... and if a raycast towards the player hits something...
            if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, robotCollider.radius))
            {
                // ... and if the raycast hits the player...
                if (hit.transform.tag == "Player")
                {
                    // ... the player is in sight.
                    playerInSight = true;
                    // Set the last global sighting is the players current position.
                    gameController.playerPosition = playerPosition;
                    return true;
                }
            }
        }
        playerInSight = false;
        gameController.playerPosition = gameController.resetPosition;
        return false;
    }

    void ChangeMaterial(bool detected)
    {
        if (detected)
        {
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

    IEnumerator WaitForTime(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
