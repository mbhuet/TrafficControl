using UnityEngine;
using System.Collections;

public class TrackExit : MonoBehaviour {
	GUIText text;
	Node exit;
	public int exitNumber;
	public int targetNumber;
	// Use this for initialization
	void Start () {
		StartCoroutine ("WaitForIt");
		text = this.GetComponent<GUIText> ();
		text.fontSize = Screen.width/30;
	}
	
	// Update is called once per frame
	void Update () {
		if (exit != null) {
						text.text = exit.exitScore + "/" + targetNumber;

			if (exit.exitScore == targetNumber) GameManager.instance.Win();
				}

	
	}

	IEnumerator WaitForIt(){
		while (GameManager.instance.exits.Count<=exitNumber) {
			yield return null;
		}
		exit = GameManager.instance.exits [exitNumber];
		//text.transform.position = Camera.main.WorldToViewportPoint (exit.transform.position) + Vector3.;
	}
}
