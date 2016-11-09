using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class Sticking : MonoBehaviour
{
    OtherGameControls OtherControls;
    int StickID;  // Used to coordinate sticking

    // Universal Sound Experiment
    public AudioSource UnivSource;
    public GameObject MainController;

    // Use this for initialization
    void Start()
    {
        OtherControls = MainController.GetComponent<OtherGameControls>();
        UnivSource = OtherControls.UnivAudioSource;

        StickID = OtherControls.StuckID;

        // RainStuck groups get stick-to priority
        if (transform.name == "RainStuck")
        {
            StickID *= -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        

        if ((col.collider.tag == this.tag || col.collider.tag == "Rainbow" ||
            (name == "RainStuck" && col.gameObject.name == "RainStuck" &&
            col.contacts[0].collider.tag == col.contacts[0].otherCollider.tag)) &&
            col.collider.tag != "bomb" && this.tag != "bomb")
        {
            // Audio and Particle Effects

            // Sticking with Stuck Groups
            if (col.gameObject.name == "StuckBlock" || col.gameObject.name == "RainStuck")
            {
                // if this block is not in a stuck group
                if (transform.name != "StuckBlock" && transform.name != "RainStuck")
                {
                    Effects(col);

                    // Remove MaxVel, Rigidbody, and Sticking
                    RemoveComponents(gameObject);

                    transform.SetParent(col.transform);
                    //Debug.Log("single " + transform.name + "--" + col.transform.name);
                }
                else
                {
                    // Only let group with higher StickID stick (group with lower ID becomes new parent)
                    // took out || col.transform.name == "RainStuck"
                    if (StickID > col.transform.GetComponent<Sticking>().StickID)
                    {
                        Effects(col);
                        //Debug.Log("double " + transform.name + "--" + col.transform.name);

                        // Reassign the children of this object to be children of the new 'parent' object
                        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
                        {
                            child.SetParent(col.transform);
                        }
                    }
                }
            }

            // Sticking with Rainbow blocks (single)
            else if (col.collider.tag == "Rainbow")
            {
                if (this.tag == "Rainbow")
                {
                    // Only let block with higher StickID stick (to avoid double execution)
                    if (StickID > col.transform.GetComponent<Sticking>().StickID)
                    {
                        Effects(col);
                        //Debug.Log("Rainbow " + transform.name + "--" + col.transform.name);

                        // New group is a RainStuck group
                        GameObject theParent = CreateNewParent("RainStuck");
                        theParent.tag = "RainStick";

                        // Reassign the children of this object to be children of the new 'parent' object
                        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
                        {
                            child.SetParent(theParent.transform);
                        }

                        // Remove MaxVel, Rigidbody, and Sticking from both objects
                        RemoveComponents(col.gameObject);
                        RemoveComponents(gameObject);

                        col.transform.SetParent(theParent.transform);
                        //Debug.Log("   Succeeded: " + theParent.name);
                    }
                }
                else
                {
                    Effects(col);
                    //Debug.Log("Rainbow " + transform.name + "--" + col.transform.name);

                    // New group is a RainStuck group
                    GameObject theParent = CreateNewParent("RainStuck");
                    theParent.tag = "RainStick";

                    // Reassign the children of this object to be children of the new 'parent' object
                    foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
                    {
                        child.SetParent(theParent.transform);
                    }

                    // Remove MaxVel, Rigidbody, and Sticking from both objects
                    RemoveComponents(col.gameObject);
                    RemoveComponents(gameObject);

                    col.transform.SetParent(theParent.transform);
                    //Debug.Log("   Succeeded: " + theParent.name);
                }
            }

            // Sticking between two single blocks
            else if (transform.name != "StuckBlock" && transform.name != "RainStuck")
            {
                // Only let block with higher StickID stick (to avoid double execution)
                if (StickID > col.transform.GetComponent<Sticking>().StickID)
                {
                    Effects(col);
                    //Debug.Log("seed " + transform.name + "--" + col.transform.name);

                    GameObject theParent = CreateNewParent("StuckBlock");
                    theParent.tag = tag;

                    col.transform.SetParent(theParent.transform);
                    transform.SetParent(theParent.transform);

                    // Remove MaxVel, Rigidbody, and Sticking from both objects
                    RemoveComponents(gameObject);
                    RemoveComponents(col.gameObject);
                    //Debug.Log("   Succeeded: " + theParent.name);
                }
            }
        }
    }

    // Particle Effect and Audio Handler
    void Effects(Collision2D colWith)
    {
        UnivSource.PlayOneShot(OtherControls.Sounds[0]);

        if (gameObject.GetComponent<SpriteRenderer>() != null)
        {
            OtherControls.stickEffect.GetComponent<ParticleSystem>().startColor = gameObject.GetComponent<SpriteRenderer>().color;
        }
        else if (colWith.gameObject.GetComponent<SpriteRenderer>() != null)
        {
            OtherControls.stickEffect.GetComponent<ParticleSystem>().startColor = colWith.gameObject.GetComponent<SpriteRenderer>().color;
        }
        OtherControls.stickEffect.transform.position = colWith.contacts[0].point;
        OtherControls.stickEffect.GetComponent<ParticleSystem>().Play();
    }

    // Remove certain components from target
    void RemoveComponents(GameObject target)
    {
        Destroy(target.GetComponent<MaxVelocity>());
        Destroy(target.GetComponent<Rigidbody2D>());
        Destroy(target.GetComponent<Sticking>());
    }

    // Create a new Parent Object with ParentName as its name
    GameObject CreateNewParent(string ParentName)
    {
        GameObject newParent = new GameObject();

        newParent.transform.SetParent(GameObject.Find("Boxes").transform);
        newParent.name = ParentName;

        newParent.AddComponent<Rigidbody2D>();
        Rigidbody2D parentRB = newParent.GetComponent<Rigidbody2D>();

        // Set Rigid Body Constraints
        parentRB.useAutoMass = true;
        parentRB.angularDrag = .2f;
        parentRB.gravityScale = .8f;
        parentRB.drag = .05f;

        newParent.transform.position = new Vector3(0, OtherControls.level_handler.Platform.position.y + 10, 0);

        newParent.AddComponent<Sticking>();
        newParent.GetComponent<Sticking>().MainController = MainController;

        newParent.AddComponent<MaxVelocity>();
        newParent.GetComponent<MaxVelocity>().OtherControls = OtherControls;

        return newParent;
    }
}

///////////// Old sticking handler (starting at void OnCollisionEnter...)

//    void OnCollisionEnter2D(Collision2D col)
//    {
//        if ((col.collider.tag == this.tag || col.collider.tag == "Rainbow") && col.collider.tag != "bomb" && this.tag != "bomb")
//        {
//            //float maxVel = 0.0f;
//            GameObject theParent = new GameObject();

//            UnivSource.PlayOneShot(MainController.GetComponent<OtherGameControls>().Sounds[0]);

//            if(gameObject.GetComponent<SpriteRenderer>() != null)
//            {
//                OtherControls.stickEffect.GetComponent<ParticleSystem>().startColor = gameObject.GetComponent<SpriteRenderer>().color;
//            }else if(col.gameObject.GetComponent<SpriteRenderer>() != null)
//            {
//                OtherControls.stickEffect.GetComponent<ParticleSystem>().startColor = col.gameObject.GetComponent<SpriteRenderer>().color;
//            }
//            OtherControls.stickEffect.transform.position = col.contacts[0].point;
//            OtherControls.stickEffect.GetComponent<ParticleSystem>().Play();

//            // Remove MaxVel, Rigidbody, and Sticking
//            RemoveComponents(gameObject);

//            if (col.gameObject.name == "StuckBlock" || col.gameObject.name == "RainStuck")
//            {
//                // Reassign the children of the colliding object to be children of the new 'parent' object
//                foreach (Transform child in col.gameObject.GetComponentsInChildren<Transform>())
//                {
//                    child.SetParent(theParent.transform);
//                }

//                // Reassign the children of this object to be children of the new 'parent' object
//                foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
//                {
//                    child.SetParent(theParent.transform);
//                }

//                if (col.gameObject.name == "StuckBlock")
//                {
//                    theParent.name = "StuckBlock";
//                    theParent.tag = this.tag;
//                }
//                else
//                {
//                    theParent.name = "RainStuck";
//                    theParent.tag = "RainStick";
//                }
//                Destroy(col.gameObject);
//            }
//            else if (col.collider.tag == "Rainbow")
//            {

//                // Reassign the children of this object to be children of the new 'parent' object
//                foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
//                {
//                    child.SetParent(theParent.transform);
//                }

//                theParent.gameObject.name = "RainStuck";
//                theParent.gameObject.tag = "RainStick";

//                // Remove MaxVel, Rigidbody, and Sticking
//                RemoveComponents(col.gameObject);

//                col.transform.SetParent(theParent.transform);
//            }
//            else
//            {

//                // Remove MaxVel, Rigidbody, and Sticking
//                RemoveComponents(col.gameObject);

//                col.transform.SetParent(theParent.transform);
//                theParent.name = "StuckBlock";
//                theParent.tag = this.tag;
//            }
//            this.transform.SetParent(theParent.transform);

//            theParent.transform.SetParent(GameObject.Find("Boxes").transform);

//            theParent.AddComponent<Rigidbody2D>();
//            Rigidbody2D parentRB = theParent.GetComponent<Rigidbody2D>();

//            parentRB.useAutoMass = true;
//            parentRB.angularDrag = .1f;
//            parentRB.gravityScale = .8f;
//            parentRB.drag = .04f;

//            theParent.AddComponent<Sticking>();
//            theParent.GetComponent<Sticking>().MainController = this.MainController;

//            theParent.AddComponent<MaxVelocity>();
//            theParent.GetComponent<MaxVelocity>().maxVelocity = gameObject.GetComponent<MaxVelocity>().maxVelocity;
//        }
//    }

//    void RemoveComponents(GameObject target)
//    {
//        //??Destroy(target.GetComponent<MaxVelocity>());
//        Destroy(target.GetComponent<Rigidbody2D>());
//        Destroy(target.GetComponent<Sticking>());
//    }
//}



