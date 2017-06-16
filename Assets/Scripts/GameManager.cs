using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameManager : MonoBehaviour {
	public static GameManager instance;
	public GUIText scoreText;
	public float score;
	public GUIText timerText;
	public float timer;

	public GUIText gameMessage;

	public List<Node> exits;
	public List<Node> nodes;

	public Node[,] graph;

	public Color[] colors;
	public bool[] allowSound;



	// Use this for initialization
	void Awake () {
		gameMessage.enabled = false;
		instance = this;
		exits = new List<Node> ();
		timerText.fontSize = Screen.width/30;
		gameMessage.fontSize = Screen.width/18;


		//colors = new Color[]{Color.red, Color.blue, Color.green, Color.yellow, Color.cyan, Color.magenta, Color.black};
	
	}
	
	// Update is called once per frame
	void Update () {
		if (timer <= 0) {
			timer = 0;
			Lose();
		}

		timer -= Time.deltaTime;
		timerText.text = "TIME: " + timer.ToString ("F2");
		scoreText.text = score.ToString();

	}

	public int AddExit(Node exit){
		//Debug.Log (exit);
		exits.Add (exit);
		return exits.Count-1;
	}

	public int AddNode(Node n){
		nodes.Add (n);
		return nodes.Count - 1;
	}

	public void Win(){
		gameMessage.enabled = true;
		Time.timeScale = 0;
		gameMessage.text = "YOU WIN";
	}

	public void Lose(){
		gameMessage.enabled = true;
		Time.timeScale = 0;
		gameMessage.text = "YOU LOST";

	}

	void OnGUI(){
		if (GUI.Button(new Rect(0,0,Screen.width/10f,Screen.height/10f), "RESET")){
			Time.timeScale = 1;
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
