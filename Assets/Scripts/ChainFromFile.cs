using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ChainFromFile : MonoBehaviour {

	public TextAsset mapAsset;
	public Node nodePrefab;
	public Node spawnPrefab;
	public Node exitPrefab;
	public IntersectionController switchPrefab;

	public List<Node> nodeList;

	public Material lineMat;
	public GameObject lines;

	public List<Node> forcedSpawnNodes;
	

	public List<Node> Create() {
		//		Debug.Log ("Begin Generation");
		GameObject nodes = new GameObject ();
		nodes.name = "Nodes";
		List<Node> nodeList = new List<Node> ();

		string[] map = mapAsset.text.Split ("\n"[0]);
		Vector3 v = new Vector3 ();
		v.z = 0;

		Node[,] graph = new Node[map.Length, map[0].Length];

		int forcedSpawnNodeIndex = 0;

		for (int j = 0; j < graph.GetLength(0); j ++) {
			v.y = -(j);
			for (int i = 0; i < graph.GetLength(1); i ++) {

				v.x = i;

				if (i > map[j].Length-1){
					Node node = (Node)GameObject.Instantiate (nodePrefab, v, Quaternion.identity) as Node;
					node.direction = Node.Direction.EMPTY;
					node.transform.parent = nodes.transform;
					graph[j,i] = node;
					node.row = j;
					node.col = i;

					nodeList.Add(node);
					continue;
				}
				else{

					Node node;
					switch (map[j][i]){
					case '^':
						node = (Node)GameObject.Instantiate (nodePrefab, v, Quaternion.identity) as Node;
						node.direction = Node.Direction.UP;
						node.transform.parent = nodes.transform;
						graph[j,i] = node;
						nodeList.Add(node);

						break;
					
					case 'v':
						node = (Node)GameObject.Instantiate (nodePrefab, v, Quaternion.identity) as Node;
						node.direction = Node.Direction.DOWN;
						node.transform.parent = nodes.transform;
						graph[j,i] = node;
						nodeList.Add(node);

						break;
					
					case '<': 
						node = (Node)GameObject.Instantiate (nodePrefab, v, Quaternion.identity) as Node;
						node.direction = Node.Direction.LEFT;
						node.transform.parent = nodes.transform;
						graph[j,i] = node;
						nodeList.Add(node);

						break;
				
					case '>': 
						node = (Node)GameObject.Instantiate (nodePrefab, v, Quaternion.identity) as Node;
						node.direction = Node.Direction.RIGHT;
						node.transform.parent = nodes.transform;
						graph[j,i] = node;
						nodeList.Add(node);

						break;

					case '+':
						node = (Node)GameObject.Instantiate (nodePrefab, v, Quaternion.identity) as Node;
						node.direction = Node.Direction.INTERSECTION;
						node.transform.parent = nodes.transform;
						graph[j,i] = node;
						nodeList.Add(node);

						break;

					case 'E':
						node = (Node)GameObject.Instantiate (exitPrefab, v, Quaternion.Euler(90,0,0)) as Node;
						node.direction = Node.Direction.EXIT;
						node.transform.parent = nodes.transform;
						graph[j,i] = node;
						nodeList.Add(node);

						break;

					case 'S':
						Debug.Log("Spawn Node");

						if (forcedSpawnNodeIndex < forcedSpawnNodes.Count && forcedSpawnNodes[forcedSpawnNodeIndex] != null){
							node = forcedSpawnNodes[forcedSpawnNodeIndex];
							node.transform.position = v;
							Debug.Log("Grabbing Spawn Node from list " + forcedSpawnNodeIndex);
							forcedSpawnNodeIndex++;

						}
						else{
							node = (Node)GameObject.Instantiate (spawnPrefab, v, Quaternion.identity) as Node;
						}
						node.direction = Node.Direction.SPAWN;
						node.transform.parent = nodes.transform;
						graph[j,i] = node;
						nodeList.Add(node);

						break;

					default:
						node = (Node)GameObject.Instantiate (nodePrefab, v, Quaternion.identity) as Node;
						node.direction = Node.Direction.EMPTY;
						node.transform.parent = nodes.transform;
						graph[j,i] = node;
						nodeList.Add(node);

						break;
					}

					node.row = j;
					node.col = i;


				}
			}
		}


		for (int row = 0; row < graph.GetLength(0); row ++) {
			for (int col = 0; col < graph.GetLength(1); col ++) {
				/*
				if (row>0){
					graph[row,row].up = graph[row,row-1];
				}
				if (row < graph.GetLength(0)-1){
					graph[row,row].down = graph[row,row+1];
				}
				if (x > 0){
					graph[row,row].left = graph[row-1,row];
				}
				if (x < graph.GetLength(1) -1){
					graph[row,row].right = graph[row+1,row];

				}
				*/
//				Debug.Log(row +" " + col);
				switch (graph[row,col].direction){
				case Node.Direction.UP: 
					graph[row,col].children.Add(graph[row-1,col]);
					break;
				case Node.Direction.DOWN: 
					graph[row,col].children.Add(graph[row+1,col]);
					break;
				case Node.Direction.LEFT: 
					graph[row,col].children.Add(graph[row,col-1]);
					break;
				case Node.Direction.RIGHT: 
					graph[row,col].children.Add(graph[row,col+1]);
					break;
				case Node.Direction.EXIT: 
					graph[row,col].isExit = true;
					break;
				case Node.Direction.INTERSECTION:
					//if the node to the right points left
					if ((col < graph.GetLength(1)-1 	&& graph[row,col+1].direction == Node.Direction.RIGHT) || 
					    (col > 0 						&& graph[row,col-1].direction == Node.Direction.RIGHT)){
						if (graph[row,col+1].direction != Node.Direction.EMPTY)
							graph[row,col].children.Add(graph[row,col+1]);
					}
					else if ((col < graph.GetLength(1)-1 	&& graph[row,col+1].direction == Node.Direction.LEFT) || 
					    	(col > 0 						&& graph[row,col-1].direction == Node.Direction.LEFT)){
						if (graph[row,col-1].direction != Node.Direction.EMPTY)
							graph[row,col].children.Add(graph[row,col-1]);
					}

					/*
					if (col < graph.GetLength(1)-1 	&& graph[row,col+1].direction == Node.Direction.INTERSECTION){
						graph[row,col].children.Add(graph[row,col+1]);

					}
					if (col > 0 					&& graph[row,col-1].direction == Node.Direction.INTERSECTION){
						graph[row,col].children.Add(graph[row,col-1]);
					}
					*/
					
					if ((row < graph.GetLength(0)-1 && graph[row+1,col].direction == Node.Direction.DOWN) || 
					    (row > 0 					&& graph[row-1,col].direction == Node.Direction.DOWN)){
						if (graph[row+1,col].direction != Node.Direction.EMPTY)
							graph[row,col].children.Add(graph[row+1,col]);
					}
					else if ((row < graph.GetLength(0)-1 	&& graph[row+1,col].direction == Node.Direction.UP) || 
					    	(row > 0 						&& graph[row-1,col].direction == Node.Direction.UP)){
						if (graph[row-1,col].direction != Node.Direction.EMPTY)
							graph[row,col].children.Add(graph[row-1,col]);
					}

					/*
					if (row < graph.GetLength(0)-1 	&& graph[row+1,col].direction == Node.Direction.INTERSECTION){
						graph[row,col].children.Add(graph[row+1,col]);

					}
					if (row > 0 					&& graph[row-1,col].direction == Node.Direction.INTERSECTION){
						graph[row,col].children.Add(graph[row-1,col]);
					}
					*/


					if (!graph[row,col].intersectionClaimed){

						IntersectionController controller = GameObject.Instantiate(switchPrefab, graph[row,col].transform.position, Quaternion.identity) as IntersectionController;
						controller.nodes.Add(graph[row,col]);
						graph[row,col].intersectionClaimed = true;

						for (int r = -1; r <= 1; r++){
							for (int c = -1; c <= 1; c++){

								if (row+r >= 0 && row+r<graph.GetLength(0) && col + c >= 0 && col+c <graph.GetLength(1)){
									Node other = graph[row+r, col+c];
									if (other.direction == Node.Direction.INTERSECTION && !other.intersectionClaimed){
										other.intersectionClaimed = true;
										controller.nodes.Add(other);
									}

								}
							}
						}

						Vector3 midpoint = Vector3.zero;
						foreach(Node n in controller.nodes){
							midpoint = midpoint + n.transform.position;
						}
						midpoint = midpoint/(float)(controller.nodes.Count);

						controller.transform.position = midpoint;

					}
					break;
				case Node.Direction.SPAWN:
//					Debug.Log("Finding children for " + graph[row,col]);

					//if the node to the right points left
					if (col < graph.GetLength(1)-1 && graph[row,col+1].direction == Node.Direction.RIGHT){
						graph[row,col].children.Add(graph[row,col+1]);
					}
					if (col > 0 && graph[row,col-1].direction == Node.Direction.LEFT){
						graph[row,col].children.Add(graph[row,col-1]);
					}
					
					if (row < graph.GetLength(0)-1 && graph[row+1,col].direction == Node.Direction.DOWN){
						graph[row,col].children.Add(graph[row+1,col]);
					}
					if (row > 0 && graph[row-1,col].direction == Node.Direction.UP){
						graph[row,col].children.Add(graph[row-1,col]);
					}
					break;
				}

//				Debug.Log(graph[row,col].children.Count);
			}
		}
		GameManager.instance.graph = graph;
		return nodeList;
	}

	
	void Start(){
		nodeList = Create ();
		lines = new GameObject ();
		DrawLines ();
	}

	void Update(){
	}

	LineRenderer NewLine(Vector3 pos1, Vector3 pos2){
		GameObject obj = new GameObject ();
		LineRenderer line = obj.AddComponent<LineRenderer> ();
		line.useWorldSpace = true;
		line.SetWidth (.2f, .2f);
		line.SetVertexCount (2);
		line.SetPosition (0, pos1 - Vector3.back *.1f);
		line.SetPosition (1, pos2 - Vector3.back *.1f);
		line.material = lineMat;

		return line;
	}

	void DrawLines(){
		GameObject.Destroy (lines.gameObject);
		lines = new GameObject ();
		//GameObject lines = new GameObject ();
		foreach (Node n in nodeList) {
			foreach(Node c in n.children){
//				Debug.Log("line between " + n + " and " + c);
				if (!c.IsClosed(n)){
				LineRenderer line = NewLine(n.transform.position, c.transform.position);
				
				line.transform.parent = lines.transform;
				}
			}
		}
	}

}
