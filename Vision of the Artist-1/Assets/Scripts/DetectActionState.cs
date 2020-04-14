using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using OculusSampleFramework;

public class DetectActionState : MonoBehaviour
{

    GameObject[] interactBubbles;
    bool allActive = false;
    //bool allActive = true;
    bool inActionState;


    public GameObject Slay;

    // Start is called before the first frame update
    void Start()
    {
        interactBubbles = GameObject.FindGameObjectsWithTag("InteractBubble");
        Debug.Log("InteractBubbles:" + interactBubbles);

        //foreach (GameObject r in interactBubbles)
        //{
        //    Destroy(r.gameObject);
        //}
    }

    //public void Check(InteractableStateArgs obj)
    //{
    //    bool inActionState = obj.NewInteractableState == InteractableState.ActionState;
    //    if (inActionState)
    //    {
    //        return;
    //    }
    //}

    // Update is called once per frame
    void Update()
    {

        //for (int i = 0; i < interactBubbles.Length; i++)
        //{
        //    if (interactBubbles[i].tag != "Interacted")
        //    {
        //        allActive = false;
        //        break;
        //    }
        //    else
        //    {
        //        allActive = true;
        //    }
        //}

        //if (allActive)
        //{
        //    Slay.SetActive();
        //}
        if (allActive == false)
        {
            Slay.SetActive(false);

            for (int i = 0; i < interactBubbles.Length; i++)
            {
                if (interactBubbles[i].tag != "Interacted")
                {
                    allActive = false;
                    break;
                }

                else
                {
                    allActive = true;
                }

                
            }
            // tried putting the check outside of the forloop
            if (allActive)
            {
                Slay.SetActive(true);
                return;
            }

            return;
        }
        return;
    }
}
            
        //else
        //{
        //    // do nothing
        //}
        //foreach (GameObject r in interactBubbles)
        //{
        //    // get component of button listeners
        //    //ButtonListener listener = r.GetComponent<ButtonListener>;
        //    //listener.NewInteractableState == InteractableState.ActionState; 

        //    //if ()
        //    if (r.tag == "Interacted")
        //    {
        //        allActive = true;
        //        Slay.SetActive(true);
        //    }

        //    else
        //    {
        //        //Slay.SetActive(false);
        //        // do nothing

        //    }
    //}

                // if interacted with - if action state triggered
                // return this
            //}

            // if all actions interacted with
            // do this
    //}