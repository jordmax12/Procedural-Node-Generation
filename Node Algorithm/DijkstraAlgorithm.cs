using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class DijkstraAlgorithm 
{

	public static Stack<GameObject> Dijkstra(GameObject[] Graph, GameObject source, GameObject target)
	{

		Dictionary<GameObject, float> dist = new Dictionary<GameObject, float>();
		Dictionary<GameObject, GameObject> previous = new Dictionary<GameObject, GameObject>(); //previous NODE to the node that is currently on this script
		List<GameObject> Q = new List<GameObject>();

		foreach(GameObject v in Graph)
		{
			dist[v] = Mathf.Infinity;
			//for all gameobjects (nodes) it will insert it into our dictionary as a key (kind of like a vector in c++)
			previous[v] = null;
			Q.Add (v);

		}

		dist[source] = 0;


		//up until this point we have done what?

		// Grabbed all the nodes via Graph (Graph = all the GameObjects with tag "Node")
		// Grabbed the source which is the closest node to whatever GameObject you're on
		// Grabbed the players closest node via target
		//
		//initializing dist[v] (dist is a dictionary holding a GameObject and a float, we are 
		//accessing the GameObject key of it.) to be positive infinity
		//initializing previous 'key' = GameObject v to null
		//Adding each GameObject v to Q which is a List of GameObjects
		while(Q.Count > 0)
		{
			float shortestDistance = Mathf.Infinity; //init
			GameObject shortestDistanceNode = null; //init
			foreach(GameObject obj in Q)
			{
				if(dist[obj] < shortestDistance) //very complicated if statement that basically finds the shortest distance based on whichever gameobject distance is not infinity.
				{
					shortestDistance = dist[obj];
					shortestDistanceNode = obj;
				}
			}

			GameObject u = shortestDistanceNode;
			Q.Remove (u);
			//
			//Up Until Here We Have Done:
			//init shortest distance to positive infinity
			// init shortestdistancenode to null for now
			//looping through list of nodes
			//if the gameobject key's value in the dist dictionary is smaller than the value of the gameobject
			//equate shortest distance to that gameobject's key's value
			//define shortestdistance node to the actual game object 

			//Check to see if we made it to target
			if(u == target)
			{
				Stack<GameObject> S = new Stack<GameObject>();
				while(previous[u] != null) //this gets set on line 87
				{
					S.Push (u); //puts node before target in Stack S
					u = previous[u]; //sets the target node as the previous node since we made it to target
				}
				return S;
			}

			if(dist[u] != null)
			{
				if(dist[u] == Mathf.Infinity)
				{
					break;
				}
			}

			foreach(GameObject v in u.GetComponent<Node>().neighbors)
			{
				if(v.activeSelf)
				{
					float alt = dist[u] + (u.transform.position - v.transform.position).magnitude;
					//since the value for key dist[u] is a float we are adding that distance to the distance
					//between the target and each neighbor node of the current game object
					if (alt < dist[v]) //if alt (distance between current position and target + target position
						//minus position of each neighbor node) is less than the float for 
					{
						dist[v] = alt; 
						previous[v] = u;
					}
				}
			}
		}
		return null;
	}
}

/*DIJKSTRA PSEUODCODE
 * function Dijkstra(Graph, source):
 *      for each vertex v in Graph:                                // Initializations
 *          dist[v]  := infinity ;                                  // Unknown distance function from 
 *                                                                 // source to v
 *          previous[v]  := undefined ;                             // Previous node in optimal path
 *      end for                                                    // from source
 *
 *      dist[source]  := 0 ;                                        // Distance from source to source
 *      Q := the set of all nodes in Graph ;                       // All nodes in the graph are
 *                                                                 // unoptimized – thus are in Q
 *      while Q is not empty:                                      // The main loop
 *          u := vertex in Q with smallest distance in dist[] ;    // Source node in first case
 *          remove u from Q ;
 *          if dist[u] = infinity:
 *              break ;                                            // all remaining vertices are
 *          end if                                                 // inaccessible from source
 *
 *          for each neighbor v of u:                              // where v has not yet been 
 *                                                                 // removed from Q.
 *              alt := dist[u] + dist_between(u, v) ;
 *              if alt < dist[v]:                                  // Relax (u,v,a)
 *                  dist[v]  := alt ;
 *                  previous[v]  := u ;
 *                  decrease-key v in Q;                           // Reorder v in the Queue (that is, heapify-down) 
 *              end if
 *          end for
 *      end while
 *      return dist[], previous[];
 *  end function
 * 
 * 
 * */