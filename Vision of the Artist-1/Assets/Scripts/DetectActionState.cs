using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using OculusSampleFramework;

public class DetectActionState : MonoBehaviour
{

    GameObject[] interactBubbles;
    bool allActive = false;

    bool inActionState;


    public GameObject Hug;

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
            foreach (GameObject r in interactBubbles)
            {
                // get component of button listeners
                //ButtonListener listener = r.GetComponent<ButtonListener>;
                //listener.NewInteractableState == InteractableState.ActionState; 

                //if ()
                if (r.tag == "Interacted")
                {
                    allActive = true;
                    Hug.SetActive(true);
                }

                else
                {
                    Hug.SetActive(false);
                    // do nothing

                }

                // if interacted with - if action state triggered
                // return this
            }

            // if all actions interacted with
            // do this

        }
    }