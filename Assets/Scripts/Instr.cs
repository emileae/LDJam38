using UnityEngine;
using System.Collections;

public class Instr : MonoBehaviour {

	public GameObject instructions;

	public void ToggleInstructions(){
		instructions.SetActive(!instructions.active);
	}

}
