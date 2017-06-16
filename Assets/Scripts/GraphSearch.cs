using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class GraphSearch{

	public static Node[] BFS(Node begin, Node end){

		List<Node[]> queue = new List<Node[]> ();
		List<Node> visited = new List<Node> ();

		Node[] start = new Node[]{begin};
		queue.Add (start);
		visited.Add (begin);
//		Debug.Log(begin + " " + end);

//		Debug.Log(queue[queue.Count-1]);

		while (queue.Count > 0) {
			Node[] curPath = queue[0];

			queue.RemoveAt(0);

			Node curNode = curPath[curPath.Length-1];
			visited.Add(curNode);
//			Debug.Log(curNode);
			if (curNode == end){
				return curPath;
			}

			//iterate through children of last node in the path
			for (int c = 0; c < curNode.children.Count; c++){
//				Debug.Log(curNode + " has child " + curNode.children[c]);
				//make a new path with the current child added to the end
				Node[] newPath = new Node[curPath.Length+1];
				for (int i = 0; i< curPath.Length; i++){
					newPath[i] = curPath[i];
				}
				newPath[newPath.Length-1] = curPath[curPath.Length-1].children[c];
				if (!visited.Contains(curPath[curPath.Length-1].children[c])){
					queue.Add(newPath);
				}
			}
			/*
			foreach(Node n in curPath[curPath.Length-1].children){

			}
			*/
		}

		Debug.Log ("Path returning null");
		return null;

	}
}
