using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeCloneController : MonoBehaviour {

	//Data Structures
	public Stack<GameObject> temp_NodeList;
	public Stack<GameObject> perm_NodeList;
	//private List<GameObject> arrayOfNodes;
	private GameObject[] _arrayOfNodes;
	private GameObject _companion;
	private CompanionMachine _companion_M;

	//Instances
	private NodeGeneratorController _NGC;
	private GameObject _NGC_GO, _previous, _current;
	private Node _node, _thisNode, _otherNode;

	//Raycasts
	private RaycastHit _hit, _checkForGroundHit;

	//Vector3
	Vector3 changeYDependingOnGroundLevel;

	//Bools
	private bool blnDidCheckGround = false, blnBegin = true, blnUpDone = false, blnDownDone = false, blnRightDone = false, blnEndSearch = false;


	//Floats
	public float offset;

	//Ints
	public int nodeCount, nodeDoneCount;

	// Use this for initialization
	void Start () {
		temp_NodeList = new Stack<GameObject>();
		_NGC_GO = GameObject.FindGameObjectWithTag("NodeGenerator");
		_NGC = _NGC_GO.GetComponent<NodeGeneratorController>();
		temp_NodeList.Push(_NGC._original);
		_current = temp_NodeList.Pop ();
		temp_NodeList.Push (_current);
		_companion = GameObject.FindGameObjectWithTag ("Companion");
		_companion_M = _companion.GetComponent<CompanionMachine> ();

		StartCoroutine(RayCastForward());
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		GameObject[] numOfNodes = GameObject.FindGameObjectsWithTag("Node");
	}

	public bool checkIfPosEmpty(Vector3 targetPos)
	{
		//float offset = 1.0f;
		GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
		
		foreach(GameObject node in nodes)
		{
			if(node.transform.position == targetPos)
				return false;
			
		}
		return true;
	}

	public bool checkIfPosEmpty(Vector3 targetPos, Vector3 dir)
	{
		GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
		Ray ray = new Ray(_current.transform.position, dir);

		foreach(GameObject node in nodes)
		{
			if(node.transform.position == targetPos)
				return false;
			
		}

		if(Physics.Raycast (ray, out _hit, offset))
		{
			return false;	
		}
		return true;
	}
	
	public bool checkIfPosEmpty(Vector3 targetPos, GameObject current)
	{
		GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
		_thisNode = current.GetComponent<Node>();

		foreach(GameObject node in nodes)
		{
			_otherNode = node.GetComponent<Node>();
			if(node.transform.position == targetPos)
			{
				if(!_thisNode.neighbors.Contains(node))
				{
					_thisNode.neighbors.Add (node);
					_otherNode.neighbors.Add (current);
					return false;
				}
				return false;
			}



		}
		return true;
	}

	public bool checkIfPosEmpty(Vector3 targetPos, GameObject current, Vector3 dir)
	{
		GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
		_thisNode = current.GetComponent<Node>();

		foreach(GameObject node in nodes)
		{
			Ray ray = new Ray(_current.transform.position, dir);
			_otherNode = node.GetComponent<Node>();
			if(node.transform.position == targetPos)
			{
				if(!_thisNode.neighbors.Contains(node))
				{
					_thisNode.neighbors.Add (node);
					_otherNode.neighbors.Add (current);
				}
				return false;
			}

			if(Physics.Raycast (ray, out _hit, offset))
			{
				if(_hit.collider.tag == "Wall")
					return false;
			}

		}
		return true;
	}

	IEnumerator RayCastForward()
	{
		if (!_NGC.blnNodeProbablyDone) {

			Ray ray = new Ray(_current.transform.position, Vector3.forward);
			if(!Physics.Raycast (ray, out _hit, offset))
			{
				
				Debug.DrawRay(_current.transform.position, Vector3.forward * offset, Color.green);
				Vector3 nextPos = new Vector3(_current.transform.position.x, _current.transform.position.y, _current.transform.position.z  + offset);
				Quaternion nextRot = _current.transform.rotation;
				
				if(checkIfPosEmpty(nextPos, Vector3.forward))
				{
					//nodeCount++;
					_NGC.intNodeCount++;
					Object node;
					node = Instantiate(_current, nextPos, nextRot);
					GameObject temp = node as GameObject;
					node.name = "Node " + _NGC.intNodeCount;
				}
				
				_arrayOfNodes = GameObject.FindGameObjectsWithTag("Node");
				_previous = _current;
				
				foreach(GameObject obj in _arrayOfNodes)
				{
					temp_NodeList.Push (obj);
					_current = temp_NodeList.Pop ();
					temp_NodeList.Push (_current);
				}
				
				yield return 0;
				
			} else {
				
				if(_hit.collider.tag != "Wall")
				{
					Vector3 nextPos = new Vector3(_current.transform.position.x, _current.transform.position.y, _current.transform.position.z + offset);
					Quaternion nextRot = _current.transform.rotation;
					
					if(checkIfPosEmpty(nextPos, Vector3.forward))
					{
						_NGC.intNodeCount++;
						Object node;
						node = Instantiate(_current, nextPos, nextRot);
						node.name = "Node " + _NGC.intNodeCount;
					}
					
					_arrayOfNodes = GameObject.FindGameObjectsWithTag("Node");
					_previous = _current;
					
					foreach(GameObject obj in _arrayOfNodes)
					{
						temp_NodeList.Push (obj);
						_current = temp_NodeList.Pop ();
						temp_NodeList.Push (_current);
					}
				}
				_current = _NGC._original;
				blnBegin = false;
				blnUpDone = true;
				yield return 0;
			}
			StartCoroutine(RayCastDown());
		}
	}

	IEnumerator RayCastDown()
	{
		if (!_NGC.blnNodeProbablyDone) {

			blnBegin = false;
			
			Ray ray = new Ray(_current.transform.position, -Vector3.forward);
			
			if(!Physics.Raycast (ray, out _hit, offset))
			{
				Debug.DrawRay(_current.transform.position, -Vector3.forward * offset, Color.yellow);
				Vector3 nextPos = new Vector3(_current.transform.position.x, _current.transform.position.y, _current.transform.position.z - offset);
				Quaternion nextRot = _current.transform.rotation;
				
				if(checkIfPosEmpty(nextPos, -Vector3.forward))
				{
					_NGC.intNodeCount++;
					Object node;
					node = Instantiate(_current, nextPos, nextRot);
					node.name = "Node " + _NGC.intNodeCount;
				}
				
				_arrayOfNodes = GameObject.FindGameObjectsWithTag("Node");
				_previous = _current;
				
				foreach(GameObject obj in _arrayOfNodes)
				{
					temp_NodeList.Push (obj);
					_current = temp_NodeList.Pop ();
					temp_NodeList.Push (_current);
				}
				
				yield return 0;
				
				
			} else {
				blnDownDone = true;
				yield return 0;
			}
			StartCoroutine(RayCastRight());
		}
	}

	IEnumerator RayCastRight()
	{
		if (!_NGC.blnNodeProbablyDone) {
			Ray ray = new Ray(_current.transform.position, Vector3.right);
			if(!Physics.Raycast (ray, out _hit, offset))
			{
				Debug.DrawRay(_current.transform.position, Vector3.right * offset, Color.red);
				Vector3 nextPos = new Vector3(_current.transform.position.x + offset, _current.transform.position.y, _current.transform.position.z);
				Quaternion nextRot = _current.transform.rotation;
				
				if(checkIfPosEmpty(nextPos, Vector3.right))
				{
					_NGC.intNodeCount++;
					Object node;
					node = Instantiate(_current, nextPos, nextRot);
					node.name = "Node " + _NGC.intNodeCount;
				}
				
				_arrayOfNodes = GameObject.FindGameObjectsWithTag("Node");
				_previous = _current;
				
				foreach(GameObject obj in _arrayOfNodes)
				{
					temp_NodeList.Push (obj);
					_current = temp_NodeList.Pop ();
					temp_NodeList.Push (_current);	
				}
				yield return 0;
				
			} else {
				blnRightDone = true;
				yield return 0;
			}
			StartCoroutine(CheckAllNodes());
		}
	}

	void MoveAllNodesToCorrectYCoordinates()
	{
		//check to see if this node moved down yet
		//(is node position == offset of the value we put in for (ground + offset)
		//if not, move down
		if (!blnDidCheckGround) 
		{
			Ray ray = new Ray(gameObject.transform.position, -Vector3.up);

			if(Physics.Raycast(ray, out _checkForGroundHit))
			{
				if(_checkForGroundHit.collider.tag == "Ground")
				{
					Debug.Log(_checkForGroundHit.transform.position.y);
					transform.position = new Vector3 (transform.position.x, (_checkForGroundHit.transform.position.y + 2.0f), transform.position.z);
				}
			}
			blnDidCheckGround = true;
		}
	}

	IEnumerator CheckAllNodes()
	{
		if (!_NGC.blnNodeProbablyDone) {
			_arrayOfNodes = GameObject.FindGameObjectsWithTag("Node");
			_thisNode = gameObject.GetComponent<Node> ();
			_current = gameObject;
			
			
			if (!_thisNode.cantGoUp || !_thisNode.cantGoDown || !_thisNode.cantGoRight) {
				
				Vector3 nextPos = new Vector3 (_current.transform.position.x, _current.transform.position.y, _current.transform.position.z + offset);
				if (checkIfPosEmpty (nextPos, gameObject, Vector3.forward)) {
					StartCoroutine (RayCastForward ());
				} else {
					_thisNode.cantGoUp = true;
					yield return 0;
				}
				
				Vector3 nextPos2 = new Vector3 (_current.transform.position.x, _current.transform.position.y, _current.transform.position.z - offset);
				if (checkIfPosEmpty (nextPos2, gameObject, -Vector3.forward)) {
					StartCoroutine (RayCastDown ());
				} else {
					_thisNode.cantGoDown = true;
					yield return 0;
				}
				
				if (!_thisNode.cantGoRight) {
					Vector3 nextPos3 = new Vector3 (_current.transform.position.x + offset, _current.transform.position.y, _current.transform.position.z);
					if (checkIfPosEmpty (nextPos3, gameObject, Vector3.right)) {
						StartCoroutine (RayCastRight ());
					} else {
						_thisNode.cantGoRight = true;
						yield return 0;
					}
				}
			} 
		}

	}
	
}
