using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Car : MonoBehaviour {
	public Node targetNode;
	public float speed;
	public AudioClip chime;
	public bool reachedExit = false;
	Node[] path;
	public int dest = -1;
	int index = 0;
	GameObject arrow;

	bool blinking = false;
	bool waiting = false;

	bool initialized = false;

	// Use this for initialization


	void Start () {
		arrow = transform.FindChild("Arrow").gameObject;
		Debug.Log ("Car Start");

//		Debug.Log ("Path Found"  + path[0].ToString());
	}
	
	// Update is called once per frame
	void Update () {
		if (!reachedExit && path != null){
			Move ();
		}
	}

	void Move(){


		if (waiting) {
						if (path [index + 1].IsOpen (this.targetNode)) {
								index++;
								SetTargetNode (path [index]);
								waiting = false;
						}
				} else if (Vector3.Distance (this.transform.position, targetNode.transform.position) < .05f) {
						this.transform.position = targetNode.transform.position;
						if (targetNode.isExit) {
								StartCoroutine ("Exit");
						} else if (index < path.Length - 1) { 
								this.transform.LookAt (path [index + 1].transform, Vector3.forward);

								if (path [index + 1].IsOpen (this.targetNode)) {
										index++;
										SetTargetNode (path [index]);
								} else
										waiting = true;

						}
				} else {
			float moveDistance = speed * Time.deltaTime;
			this.transform.position = (Vector3.MoveTowards (this.transform.position, targetNode.transform.position, moveDistance));		
		}


	}

	public void SetTargetNode(Node node){
		if (targetNode != null) {
						targetNode.isOccupied = false;
						targetNode.occupant = null;
				}
		this.targetNode = node;
		node.isOccupied = true;
		targetNode.occupant = this;

	}

	IEnumerator Exit(){
		GameManager.instance.score++;
		audio.PlayOneShot (chime);
		this.renderer.enabled = false;
		reachedExit = true;
		this.targetNode.isOccupied = false;
		targetNode.occupant = null;
		targetNode.exitScore ++;

		yield return new WaitForSeconds (chime.length);
		GameObject.Destroy(this.gameObject);

	}

	public void Init(int forceDest){
		dest = forceDest;
		if (dest==-1) dest = Random.Range (0,GameManager.instance.exits.Count);
		if (dest < GameManager.instance.colors.Length) {
						this.renderer.material.color = GameManager.instance.colors [dest];
				} else {
						this.renderer.material.color = Color.black;
				}
		path = GraphSearch.BFS(this.targetNode, GameManager.instance.exits[dest]);

		if (GameManager.instance.allowSound.Length > dest && GameManager.instance.allowSound [dest])
						audio.enabled = true;
	}

	public Node NextNode(){
		if (index < path.Length - 1) { 
						return path[index+1];
				}
		return null;
	}

	IEnumerator Blink(){
		blinking = true;
		do {
						//arrow.renderer.enabled = false;
						yield return new WaitForSeconds (.1f);
						//arrow.renderer.enabled = true;
						//yield return new WaitForSeconds (.5f);

				} while (blinking);

		arrow.renderer.enabled = true;
	}
	
}
