using UnityEngine;
using System.Collections;

public class BombBehavior : MonoBehaviour
{
    ////original sound options//
    //public AudioClip BoomSound;
    //private AudioSource Source;

    // Universal Sound experiment
    public AudioSource UnivSource;
    public GameObject MainController;
    public GameObject explosion;

    float bombRadius = 1.5f;

    Rigidbody2D rigidBody;
    float prevVelocity;
    float Velocity;

    bool Exploded;
    int ExpireCount = 10;

	// Use this for initialization
	void Start()
	{
		//no sound for bompoclypse
		if(MainController.GetComponent<CreateBoxes>().bombpocalypse_on == false)
		{
			//play sound and vary pitch
			UnivSource.PlayOneShot(MainController.GetComponent<OtherGameControls>().Sounds[3], Random.Range(0.1F, 0.6F));
			//0.6F);
		}

        rigidBody = transform.GetComponent<Rigidbody2D>();
        rigidBody.gravityScale = 0.4f;
        prevVelocity = rigidBody.velocity.y;
    }

    // Update is called once per frame
    void Update()
    {

        Velocity = rigidBody.velocity.y;

        // Note: Velocity and prevVelocity are both negative; Velocity > prevVelocity means it is travelling downwards at a slower speed (less negative)
        // A slower downwards speed means that the bomb was obstructed by something and thus triggers detonation
        if (Velocity > prevVelocity)
        {
            Boom();
        }

        prevVelocity = Velocity;

        if (Exploded && ExpireCount-- <= 0)
        {
            Destroy(this.gameObject);

            //Source.PlayOneShot (BoomSound);
        }

    }

    // Explosion
    void Boom()
    {
        // Univ Audio sound
        UnivSource.PlayOneShot(MainController.GetComponent<OtherGameControls>().Sounds[1], Random.Range(0.15f, 0.65f));


        rigidBody.isKinematic = true;
        transform.GetComponent<PolygonCollider2D>().isTrigger = true;

        transform.localScale = new Vector3(bombRadius, bombRadius, bombRadius);
        explosion.transform.position = transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y, -3);
        explosion.GetComponent<ParticleSystem>().Play();

        Color temp = GetComponent<SpriteRenderer>().material.color;
        temp.a = .5f;
        GetComponent<SpriteRenderer>().material.color = temp;

        Exploded = true;

        //Get all the colliders within bombRadius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, bombRadius);
        foreach (Collider2D col in hitColliders)
        {
            //Don't destroy the bomb or nonblocks
            if (col.tag != "NonBlock" && col.tag != "bomb")
            {
                //Check to see if the block is parented by a stuckblock
                if (col.transform.parent.name == "StuckBlock" || col.transform.parent.name == "RainStuck")
                {
                    GameObject theParent = col.transform.parent.gameObject;

                    //Set all the children of the stuck block to be loose blocks again
                    foreach (Transform child in theParent.GetComponentsInChildren<Transform>())
                    {
                        child.GetComponent<Transform>().SetParent(GameObject.Find("Boxes").transform);

                        if (child.gameObject.GetComponent<Rigidbody2D>() == null)
                        {
                            child.gameObject.AddComponent<Rigidbody2D>();
                        }
                        if (child.gameObject.GetComponent<Sticking>() == null)
                        {
                            child.gameObject.AddComponent<Sticking>();
                        }
                        if (child.gameObject.GetComponent<MaxVelocity>() == null)
                        {
                            child.gameObject.AddComponent<MaxVelocity>();
                        }

                        //child.gameObject.AddComponent<Rigidbody2D>();
                        child.gameObject.GetComponent<Rigidbody2D>().useAutoMass = true;

                        // Readd the stcking script to the children
                        //child.gameObject.AddComponent<Sticking>();
                        child.gameObject.GetComponent<Sticking>().MainController = this.MainController;
                        child.gameObject.GetComponent<Sticking>().UnivSource = this.UnivSource;

                        //child.gameObject.AddComponent<MaxVelocity>();
                        child.GetComponent<MaxVelocity>().OtherControls = MainController.GetComponent<OtherGameControls>();


                    }
                    Destroy(theParent);
                }
                Destroy(col.gameObject);
            }
        }
    }

    //void OnTriggerStay2D(Collider2D other)
    //{
    //    /*
    //    if (Exploded && other.tag != "NonBlock" && false)
    //    {
    //        if (other.GetComponentInParent<Rigidbody2D>() != null)
    //        {
    //            GameObject parent = other.GetComponentInParent<Transform>().gameObject;
    //            foreach (Transform child in parent.GetComponentsInChildren<Transform>())
    //            {
    //                child.SetParent(GameObject.Find("Boxes").transform);
    //                child.gameObject.AddComponent<Rigidbody2D>();
    //                child.gameObject.AddComponent<Sticking>();
    //                child.gameObject.AddComponent<MaxVelocity>();
    //                child.GetComponent<MaxVelocity>().maxVelocity = parent.GetComponent<MaxVelocity>().maxVelocity;
    //            }
    //            Destroy(parent);
    //        }
    //        Destroy(other.gameObject);
    //    }*/
    //}
}
