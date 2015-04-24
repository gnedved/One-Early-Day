using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathManager : MonoBehaviour
{
    //List<GameObject> allNodes;
    GameObject[] allNodes;
    GameObject[] allPaths;

	void Start ()
    {
        allNodes = GameObject.FindGameObjectsWithTag("Node");
        allPaths = GameObject.FindGameObjectsWithTag("Path");
	}
	
    public GameObject[] GetAllNodes()
    {
        return allNodes;
    }

    public GameObject GetNextNode(GameObject currentNode, GameObject currentPath)
    {
        return currentPath.GetComponent<Path>().GetNextNode(currentNode);
    }
    
}
