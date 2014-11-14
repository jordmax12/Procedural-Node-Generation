using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour 
{

	public List<GameObject> neighbors;
	public GameObject goal;
	private GameObject player, companion, nodeg;
	private GameObject[] walls;
	//Instances
	private CurrentNode _cN;
	private CompanionMachine _companionMachine;
	private NodeGeneratorController NGC;
	
	//Bools
	private bool _nodeListIsFull = false;
	public bool cantGoUp, cantGoDown, cantGoRight, cantGoLeft;



	void Start()
	{
		neighbors = new List<GameObject>();
		//companion = GameObject.FindGameObjectWithTag ("Companion");
		//_companionMachine = companion.GetComponent<CompanionMachine>();
		//goal = _companionMachine._target;
		walls = GameObject.FindGameObjectsWithTag ("block");
		nodeg = GameObject.FindGameObjectWithTag ("NodeGenerator");
		NGC = nodeg.GetComponent<NodeGeneratorController> ();
	}

	void Update()
	{
		//goal = _companionMachine._target;

	}

	IEnumerator DrawNeighbors()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(0.5f, 0.5f, 0.5f));
		foreach(GameObject neighor in neighbors)
		{
			Gizmos.DrawLine (transform.position, neighor.transform.position);
			//Gizmos.DrawWireSphere(neighor.transform.position, 0.25f);
		}
		
		if(goal)
		{
			Gizmos.color = Color.green;
			GameObject current = gameObject;
			
			Stack<GameObject> path = DijkstraAlgorithm.Dijkstra(GameObject.FindGameObjectsWithTag("Node"), gameObject, goal);
			
			foreach(GameObject obj in path)
			{
				/*Gizmos.DrawWireSphere(obj.transform.position, 1.0f);
				
				Gizmos.DrawLine(current.transform.position, obj.transform.position);
				current = obj;*/
			}
			yield return 0;
		}
		yield return 0;
	}

	void OnDrawGizmos() //editor function, if gizmos is turned on
	{
		//if(!NGC.blnNodeProbablyDone)
			StartCoroutine(DrawNeighbors());
	}

	void FixedUpdate()
	{
		player = GameObject.FindWithTag ("PlayerDos");
		//_cN = player.GetComponent<CurrentNode> ();
		//goal = _cN.currentNode;

		if (NGC.blnNodeProbablyDone) {
			for(int i = 0; i < 10; i++)
			{
				foreach (GameObject block in walls) {
					if((gameObject.transform.position - block.transform.position).magnitude <= 1.0f)
						gameObject.SetActive(false);
				}
			}
		}

	}


}
