using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeGeneratorController : MonoBehaviour {


	public GameObject _original, clone, companion;
	private CompanionMachine _cm;
	public int intNodeCount;
	private int _intPreviousNodeCount, _intTemp;
	private NodeCloneController NCC;
	public bool blnNodeProbablyDone = false;
	private bool Stop = false;
	private RaycastHit  _checkForGroundHit;
	// Use this for initialization
	void Start () {
		companion = GameObject.FindWithTag ("Companion");
		_cm = companion.GetComponent<CompanionMachine> ();
		Instantiate(clone, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
		intNodeCount++;
		_original = GameObject.FindGameObjectWithTag("Node");	
		//StartCoroutine (test ());
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (!blnNodeProbablyDone) {
			_cm.nodeGenDone = true;
			_intPreviousNodeCount = intNodeCount;
			StartCoroutine (CheckIfDone ());
		}
	}

	void FixedUpdate()
	{
		if (!Stop) {
			if (blnNodeProbablyDone) {
				StartCoroutine(MoveNodesDownCorrectYCoordinate());
				//SetInitAllEnemyOutOfIdle();
				//StartCoroutine(MoveNodesDownCorrectYCoordinate());
				MoveAllToParent();
				Stop = true;
			}
		}
	}

	void MoveAllToParent()
	{
		GameObject[] nodes = GameObject.FindGameObjectsWithTag ("Node");
		foreach (GameObject no in nodes) {
			if(no.activeSelf)
				no.transform.parent = gameObject.transform;
		}
	}

	IEnumerator test()
	{
		yield return new WaitForSeconds (1);
	}

	IEnumerator CheckIfDone()
	{
		//Debug.Log ("int previous: " + _intPreviousNodeCount + " node count " + intNodeCount);
		yield return new WaitForSeconds (2);
		//Debug.Log ("DOS int previous: " + _intPreviousNodeCount + " node count " + intNodeCount);
		if (_intPreviousNodeCount == intNodeCount) {
			_intTemp++;
			if(_intTemp > 10)
			{
				//Debug.Log ("Done?");
				blnNodeProbablyDone = true;
			}
		}
		yield return 0;
	}

	IEnumerator MoveNodesDownCorrectYCoordinate()
	{
		GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
		foreach (GameObject node in nodes) 
		{
			Ray ray = new Ray(node.transform.position, -Vector3.up);
			if(Physics.Raycast (node.transform.position, -Vector3.up, out _checkForGroundHit))
			{
					node.transform.position = new Vector3(node.transform.position.x, (_checkForGroundHit.transform.position.y + 1.5f), node.transform.position.z);
			}

		}
		yield return 0;
	}

	void SetInitAllEnemyOutOfIdle()
	{
		GameObject[] enemiesAtStart = GameObject.FindGameObjectsWithTag ("EnemyAI");
		foreach (GameObject enemy in enemiesAtStart) {
			EnemyMachine enemyMachine = enemy.GetComponent<EnemyMachine>();
			EnemyState enemyState = enemy.GetComponent<EnemyState>();
			enemyMachine.e_MovementState = EnemyState.E_MovementState.M_Idle;
			//enemyState.e_MovementState = EnemyState.E_MovementState.M_Idle;
		}

		return;
	}


}
