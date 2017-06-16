using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {


		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100);
			if (hit.collider!=null && hit.transform.tag == "Switch"){
				hit.transform.GetComponent<IntersectionController>().Switch();
			}
		}
	}
}
