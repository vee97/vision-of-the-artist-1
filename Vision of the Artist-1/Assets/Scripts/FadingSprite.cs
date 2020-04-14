using System.Collections;
using System.Collections.Generic;
using OculusSampleFramework;
using TMPro;
using UnityEngine;

public class FadingSprite : MonoBehaviour
{
    SpriteRenderer rend;
    public float delayBeforeFading = 10f;
    public float seconds = 3f;
    private Color textColor;

    // textmeshproUGUI is used when working with canvas elements
    // textmeshpro is used for meshes in 3D world space
    //public TextMeshPro textMesh;
    private TextMeshPro textMesh;
    //public Color myColor;

    private MonoBehaviour script;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        Color c = rend.color;
        c.a = 0;
        rend.material.color = c;

        // GetComponent vs get components
        textMesh = GetComponentInChildren<TextMeshPro>();
        //textMesh = GetComponent<TextMeshPro>();

        textColor = textMesh.color;
        textColor.a = 0;
        // this line saved my ass
        textMesh.color = textColor;
        //Debug.Log("Hello I am new:" +textMesh.name);

        //Color textColor = textMesh.color;

        // if gameObject tag is "InteractBubble or Interacted"
        // StartCoroutine(FadeIn());

        // check if gameObject has component button listener
        // if it does, set button listener to disabled

        script = gameObject.GetComponent<ButtonListener>();
        
        //Interactable script2 = gameObject.GetComponent<ButtonController>();

        //if (gameObject.GetComponent<ButtonListener>() != null)
        //if (script != null)
        //{
        //    script.enabled = false;
        //    //script2.enabled = false; // nvm can't disable this script
        //    // instead change Valid tool tips to none?

        //}

        // check if gameBoject is tagged with InteractBubble or Interacted
        if ((this.tag == "InteractBubble") || (this.tag == "Interacted")) 
        {
            script.enabled = false;
            StartCoroutine(FadeInOnly(delayBeforeFading));
        }

        else
        {
            script.enabled = false;
            StartCoroutine(fadeTimerSprite(delayBeforeFading, seconds));
            //StartCoroutine(Fadein());
        }
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    IEnumerator fadeTimerSprite(float delayBeforeFading, float seconds)
    {
        yield return StartCoroutine(Wait(delayBeforeFading));
        yield return StartCoroutine(Fadein());
        yield return StartCoroutine(Wait(seconds));
        yield return StartCoroutine(Fadeout());
    }

    IEnumerator FadeInOnly(float delayBeforeFading)
    {
        yield return StartCoroutine(Wait(delayBeforeFading));
        yield return StartCoroutine(Fadein());
    }


    IEnumerator Fadein()
    {
        for (float f = 0.05f; f <= 1; f += 0.05f)
        {
            Color c = rend.material.color;
            c.a = f;
            rend.material.color = c;

            // change alpha of text to f
            textColor.a = f;
            textMesh.color = textColor;

            yield return new WaitForSeconds(0.05f);
        }
        //turn on speech bubble game objects only after fade in animation is done
        // NO it's actually gameObject get component Button Listener - turns on!!!
        //MonoBehaviour script = gameObject.GetComponent<ButtonListener>();
        //script.enabled = true;

        //gameObject.bubbleListener
        //if (gameObject.GetComponent<ButtonListener>() != null)
        //{
        //gameObject.GetComponent<ButtonListener>().enabled = true;
        //}

    }

    public void startFadingIn()
    {
        StartCoroutine("Fadein");
    }

    IEnumerator Fadeout()
    {
        for (float f = 1f; f >= -0.05f; f -= 0.05f)
        {
            Color c = rend.material.color;
            c.a = f;
            rend.material.color = c;

            // change alpha of text to f
            textColor.a = f;
            textMesh.color = textColor;
            yield return new WaitForSeconds(0.05f);
           
        }
        Destroy(this.gameObject);
    }

    public void startFadingOut()
    {
        StartCoroutine("Fadeout");
    }


    // Update is called once per frame
    void Update()
    {
    }

}
