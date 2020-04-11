using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
	public TextMeshProUGUI textDisplay;
	// holds all sentences that make up dialog
	public string[] sentences;
	private int index;
	public float typingSpeed;

	// Add time float
	public float seconds = 3f; // Seconds to read the text
	public GameObject continueButton;
	public CanvasGroup uiElement;

	IEnumerator Wait(float seconds)
	{
		yield return new WaitForSeconds(seconds);
	}

    IEnumerator StartAll()
	{
		for (int i = 0; i < sentences.Length; i++)
		{
			textDisplay.text = "";
			textDisplay.text = sentences[i];
			//textDisplay.text = sentences[index];

			if (i == 0)
            {
				yield return StartCoroutine(Wait(2f));
				yield return StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 1));
				yield return StartCoroutine(Wait(seconds));
				yield return StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 0));
			}
            else
            {
                yield return StartCoroutine(Wait(0.4f));
                yield return StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 1));
				yield return StartCoroutine(Wait(seconds));
				yield return StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 0));
			}
			
		}
    }

// Start is called before the first frame update
void Start()
    {
        uiElement.alpha = 0;
        //index = 0;
        //if (index < sentences.Length - 1)
        //{
        //	textDisplay.text = sentences[index];
        //	index++;
        StartCoroutine(StartAll());

        //}
        //Debug.Log("Index: "+ index);
    }

    // if (textDisplay.text == sentences[index])

	// get text display to actually display sentences
	// implement typing affect
	// coroutines basically act as functions
	// but all code inside doesn't need to be run immediately
	// by running yield return new, will be dealing with amount

	public void FadeIn()
	{
		StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 1));
	}

	public void FadeOut()
	{
		StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 0));
	}

	public void FadeInOut()
	{
  //      uiElement.alpha = 0;
  //      for (int i = 0; i < sentences.Length - 1; i++)
  //      {
		//	textDisplay.text = "";
		//	textDisplay.text = sentences[i];
		//	StartCoroutine(StartAll());
		//}
  
		//uiElement.alpha = 0;
		//index = 0;
		//if (index < sentences.Length - 1)
		//{
		//	textDisplay.text = "";
		//	textDisplay.text = sentences[index];
		//	index++;
		//	StartCoroutine(StartAll());
			
		//}
		//else
		//{
		//	textDisplay.text = "";
		//}

		Debug.Log("Index: " + index);
	}

	// fading in and out
	public IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 0.5f)
	{

		float _timeStartedLerping = Time.time;
		float timeSinceStarted = Time.time - _timeStartedLerping;
		float percentageComplete = timeSinceStarted / lerpTime;

		while (true)
		{
			//textDisplay.text = sentences[index];
			timeSinceStarted = Time.time - _timeStartedLerping;
			percentageComplete = timeSinceStarted / lerpTime;

			// going to keep lerping value from start to end
			float currentValue = Mathf.Lerp(start, end, percentageComplete);

			// changing value by lerping, storing value into alpha
			cg.alpha = currentValue;

			// get out of whileloop if complete
			if (percentageComplete >= 1) break;

			yield return new WaitForEndOfFrame();
		}
		print("done");
	}

	IEnumerator Type()
	{
		foreach (char letter in sentences[index].ToCharArray())
		{
			textDisplay.text += letter;
			yield return new WaitForSeconds(typingSpeed);
		}
	}

	public void NextSentence()
	{
		continueButton.SetActive(false);

		if (index < sentences.Length - 1)
		{
			index++;
			// resets text
			textDisplay.text = "";
			StartCoroutine(Type());
		}
		else
		{
			textDisplay.text = "";
			continueButton.SetActive(false);
		}
	}
}