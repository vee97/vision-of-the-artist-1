using System.Collections;
using System.Collections.Generic;
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

        if ((this.tag == "InteractBubble") || (this.tag == "Interacted")) 
        {
            StartCoroutine(FadeInOnly(delayBeforeFading));
        }

        else
        {
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

    //IEnumerator fadeTimer3D()
    //{
    //    yield return StartCoroutine(Wait(2f));
    //    // yield return StartCoroutine(Fadein(); 3d game object
    //    yield return StartCoroutine(Wait(seconds));
    //    // yield return StartCoroutine(Fadeout())
    //}

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
