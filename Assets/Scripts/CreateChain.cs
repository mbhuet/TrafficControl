using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateChain : MonoBehaviour {
	public List<Node> chain;
	// Use this for initialization
	void Awake () {
		LinkNodes ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void LinkNodes(){
		/*
		for (int i = 0; i<chain.Count; i++) {
			if (i<chain.Count-1) chain[i].next = chain[i+1];
			if (i>0)chain[i].prev = chain[i-1];
		}
		*/
	}
}
