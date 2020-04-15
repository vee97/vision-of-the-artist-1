using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class FadingSpriteHighlight : MonoBehaviour
{

    SpriteRenderer rend2;
    public float delayBeforeFading2 = 10f;
    public float seconds2 = 3f;
    private Color textColor2;

    // textmeshproUGUI is used when working with canvas elements
    // textmeshpro is used for meshes in 3D world space
    private TextMeshPro textMesh2;

    private Transform findme;
    //private Transform findmechild;

    // Start is called before the first frame update
    void Start()
    {
        //Transform t = gameObject.transform;

        //transform.FindChild(GameObject.FindGameObjectWithTag("u").transform.name);

        //findme = transform.Find(GameObject.FindGameObjectWithTag("Highlight").transform.name);
        //findme = transform.Find(GameObject.FindGameObjectWithTag("Highlight").transform.name);
        //Debug.Log("Found me!:" + findme);

        //findme.gameObject.SetActive(false);

        rend2 = GetComponent<SpriteRenderer>();
        Color c2 = rend2.color;
        c2.a = 0;
        rend2.material.color = c2;

        textMesh2 = GetComponentInChildren<TextMeshPro>();

        textColor2 = textMesh2.color;
        textColor2.a = 0;
        textMesh2.color = textColor2;

        // check if gameobject is tagged with InteractBubble or Interacted
        if ((this.tag == "InteractBubble") || (this.tag == "Interacted"))
        {
            StartCoroutine(FadeInOnly2(delayBeforeFading2));
        }

        else
        {
            StartCoroutine(fadeTimerSprite2(delayBeforeFading2, seconds2));
        }
    }


    IEnumerator Wait2(float seconds2)
    {
        yield return new WaitForSeconds(seconds2);
    }

    IEnumerator fadeTimerSprite2(float delayBeforeFading2, float seconds2)
    {
        yield return StartCoroutine(Wait2(delayBeforeFading2));
        yield return StartCoroutine(Fadein2());
        yield return StartCoroutine(Wait2(seconds2));
        yield return StartCoroutine(Fadeout2());
    }

    IEnumerator FadeInOnly2(float delayBeforeFading2)
    {
        yield return StartCoroutine(Wait2(delayBeforeFading2));
        yield return StartCoroutine(Fadein2());
    }


    IEnumerator Fadein2()
    {
        for (float f = 0.05f; f <= 1; f += 0.05f)
        {
            Color c2 = rend2.material.color;
            c2.a = f;
            rend2.material.color = c2;

            // change alpha of text to f
            textColor2.a = f;
            textMesh2.color = textColor2;

            yield return new WaitForSeconds(0.05f);
        }
        //findme.gameObject.SetActive(true);

    }

    public void startFadingIn2()
    {
        StartCoroutine("Fadein2");
    }

    IEnumerator Fadeout2()
    {
        for (float f = 1f; f >= -0.05f; f -= 0.05f)
        {
            Color c2 = rend2.material.color;
            c2.a = f;
            rend2.material.color = c2;

            // change alpha of text to f
            textColor2.a = f;
            textMesh2.color = textColor2;
            yield return new WaitForSeconds(0.05f);

        }
        Destroy(this.gameObject);
    }

    public void startFadingOut2()
    {
        StartCoroutine("Fadeout2");
    }


    // Update is called once per frame
    void Update()
    {
    }

}
