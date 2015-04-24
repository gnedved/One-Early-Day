using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Path : MonoBehaviour
{
	List <GameObject> Nodes = new List<GameObject>();
	private int nodes = 0;

	void Start ()
	{
		for(int i = 0; i < transform.childCount; i++)
		{
			Nodes.Add(transform.GetChild(i).gameObject);
			nodes++;
		}
	}

	public int nodeCount
	{
        get { return nodes; }
	}

    public GameObject GetNextNode(GameObject currentNode)
    {
        int curIndex = Nodes.IndexOf(currentNode);
        int nextIndex = (curIndex + 1) % nodeCount;
        return Nodes[nextIndex];
    }
}
