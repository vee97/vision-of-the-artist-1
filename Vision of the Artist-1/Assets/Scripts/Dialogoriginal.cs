using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogoriginal : MonoBehaviour
{

	public TextMeshProUGUI textDisplay;
	// holds all sentences that make up dialog
	public string[] sentences;
	private int index;
	public float typingSpeed;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Type());
    }

	// get text display to actually display sentences
	// implement typing affect
	// coroutines basically act as functions
	// but all code inside doesn't need to be run immediately
	// by running yield return new, will be dealing with amount

	IEnumerator Type(){
		foreach(char letter in sentences[index].ToCharArray()){
			textDisplay.text += letter;
			yield return new WaitForSeconds(typingSpeed);
		}
	}
}
