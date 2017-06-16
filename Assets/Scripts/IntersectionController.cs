using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class IntersectionController : MonoBehaviour {
	public List<Node> nodes;

	GameObject arrow;
	bool flippable = true;

	void Awake(){
		arrow = transform.FindChild("Arrow").gameObject;
	}

	public void Switch(){
				if (flippable) {
						StartCoroutine ("RotateArrow");
						foreach (Node node in nodes) {
								node.SwitchFlow ();	
						}
						GameManager.instance.SendMessage ("DrawLines");

						bool gridlock = GridLockCheck ();
						if (gridlock) {
								Debug.Log ("GridLock at node " + nodes [0].name);
								foreach (Node node in nodes) {
										node.StartCoroutine ("AllowStacking", 1);
								}

						} else
								Debug.Log ("No GridLock at node " + nodes [0].name);

				}
		}

	void Update(){

		/*
		if (nodes.Count == 4) {
						bool blocked = true;
						foreach (Node node in nodes) {
								if (node.isOccupied == false)
										blocked = false;	
						}

						if (blocked) {
								foreach (Node node in nodes) {
										node.StartCoroutine ("AllowStacking", 1);
								}	
						}
				}

*/
	}

	bool GridLockCheck(){
		int failsafe = 0;
		Node start = nodes [0];
		Car startCar = start.occupant;
		if (startCar == null)
						return false;
		Node current = startCar.NextNode();
		while (current != start && current.isOccupied) {
			failsafe ++;
			if (failsafe >100){
				Debug.Log("Failsafe tripped at Node " + current.name);
				return false;
			}
			current = current.occupant.NextNode();		
			if (current == null) return false;
		}

		if (current == start) {
			return true;		
		}
		return false;

	}

	IEnumerator RotateArrow(){
		Vector3 startScale = arrow.transform.localScale;
		flippable = false;
		float shrinkSpeed = 5;
		while (arrow.transform.localScale.x > 0) {
			arrow.transform.localScale = arrow.transform.localScale - Vector3.right * Time.deltaTime * shrinkSpeed;
			yield return null;
				
		}
		arrow.transform.Rotate (0,0,90);
		while (arrow.transform.localScale.x < startScale.x) {
			arrow.transform.localScale = arrow.transform.localScale + Vector3.right * Time.deltaTime * shrinkSpeed;			
			yield return null;
			
		}
		arrow.transform.localScale = startScale;
		flippable = true;
	}
}
