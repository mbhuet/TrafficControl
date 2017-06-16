using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Node : MonoBehaviour {


	public List<Node> children;
	public enum Direction {UP, DOWN, LEFT, RIGHT, EMPTY, INTERSECTION, EXIT, SPAWN};


	public Direction direction;
	public bool isExit = false;
	public bool isOccupied = false;

	public int exitNo;
	public Car occupant;
	bool allowStacking = false;
	public bool intersectionClaimed = false;

	public int row;
	public int col;

	public int exitScore = 0;

	//used if this is an intersection, false is vertical, true is horizontal;
	public bool flowDirection = false;
	
	void Start(){
		//children = new List<Node> ();
		if (isExit) {
						RegisterExit ();		
				} else
						RegisterNode ();
	}

	void OnDrawGizmos() {
				if (isExit) {
						Gizmos.color = Color.red;
						Gizmos.DrawSphere (transform.position, .5f);		
				} else {
			Gizmos.color = Color.yellow;
						Gizmos.DrawWireSphere (transform.position, .3f);
				}
		foreach (Node n in children) {
						
								Gizmos.DrawLine (this.transform.position, n.transform.position);
				}
	}





	public bool IsOpen(Node from){
		return ((!isOccupied || allowStacking) && !IsClosed(from));
	}

	public bool IsClosed(Node from){
		if (direction == Node.Direction.INTERSECTION) {
			if (from.direction == Node.Direction.UP || from.direction == Node.Direction.DOWN){
				return (!flowDirection);
			}
			else if (from.direction == Node.Direction.LEFT || from.direction == Node.Direction.RIGHT){
				return (flowDirection);
			}
		}
		return false;
	}

	/*
	public void ConnectNeighbors(Node u, Node d, Node l, Node r){
		if (u != null){
			up = u;
		}
		if (d != null){
			down = d;
		}
		if (l != null){
			left = l;
		}
		if (r != null){
			right = r;
		}
	}
*/
	void RegisterNode(){
		int num = GameManager.instance.AddNode (this);
		this.name = "Node " + num; 
	}

	void RegisterExit(){
		exitNo = GameManager.instance.AddExit (this);
		LineRenderer line = this.GetComponent<LineRenderer> ();
		if (exitNo < GameManager.instance.colors.Length) {
						
						line.material.color = GameManager.instance.colors [exitNo];
				}
		else line.material.color = Color.black;

	}

	public void SwitchFlow(){
		flowDirection = !flowDirection;
		//StartCoroutine ("AllowStacking", 1);

	}

	IEnumerator AllowStacking(int frames){
		allowStacking = true;
		int i = frames;
		while (i>0) {
			i--;
			yield return null;
		}
		allowStacking = false;

	
	}
	


}
