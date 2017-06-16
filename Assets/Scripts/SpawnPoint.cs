using UnityEngine;
using System.Collections;

[RequireComponent (typeof(LineRenderer))]
public class SpawnPoint : MonoBehaviour {
	public Car carPrefab;
	//public Color color;
	//this spawn point will only create this many cars
	public float spawnsRemaining;
	public bool unlimited;
	public int forceCarDestination = -1;

	public float spawnRate = 1;
	Node node;

	void Start () {
		node = this.GetComponent<Node> ();
		StartCoroutine ("RandomSpawn");
	}

	void Update(){
	}


	void SpawnCar(){
		Car newCar = (Car)GameObject.Instantiate (carPrefab, this.transform.position, Quaternion.identity) as Car;
		newCar.SetTargetNode (this.node);
		newCar.Init (forceCarDestination);
		//newCar.speed = ;
	}


	IEnumerator RandomSpawn(){
		while (unlimited || spawnsRemaining > 0) {
			yield return new WaitForSeconds(Random.Range(2,6)/spawnRate);
			while(!node.IsOpen(this.node) && !node.IsOpen(this.node.children[0])){
				yield return null;
			}
			SpawnCar();
			spawnsRemaining --;
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(transform.position, .5f);
	}




}
