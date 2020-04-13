using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeOut : MonoBehaviour
{
    SpriteRenderer rend;

    public TextMeshPro textMesh;
    //public Color myColor;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        textMesh = GetComponent<TextMeshPro>();

    }

    IEnumerator Fadeout()
    {
        for (float f = 1f; f >= -0.05f; f -= 0.05f)
        {
            Color c = rend.material.color;
            c.a = f;
            rend.material.color = c;

            //Color myColor = textMesh.color;
            //myColor.a = f;
            //textMesh.color = myColor;
            //myColor.a = 0f;
            //textMesh.color = myColor;

            yield return new WaitForSeconds(0.05f);

        }
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
