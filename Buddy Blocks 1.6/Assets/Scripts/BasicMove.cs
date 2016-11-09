using UnityEngine;
using System.Collections;

public class BasicMove : MonoBehaviour
{

    public GameObject MainController;

    float thrust = 850;
    float maxVelocity = 4.5f;


    Rigidbody2D rigidBody;

    private float dist;
    private Transform toDrag;
    private Vector2 offset;

    private bool left;
    private bool right;
    private Vector2 touchCache;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        thrust = MainController.GetComponent<OtherGameControls>().PlatformThrust;
        maxVelocity = MainController.GetComponent<OtherGameControls>().PlatformMaxSpeed;
    }
    
    void Update()
    {
        //If running game in editor
#if UNITY_EDITOR
        //If mouse button 0 is down
        if (Input.GetMouseButton(0))
        {
            //Cache mouse position
            Vector2 mouseCache = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //If mouse x position is less than or equal to a fraction of the screen width
            if (mouseCache.x < transform.position.x)
            {
                left = true;
            }
            else if(mouseCache.x > transform.position.x)
            {
                right = true;
            }
        }
#elif (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8)
        //If a touch is detected
        if (Input.touchCount >= 1)
        {
            //For each touch
            foreach (Touch touch in Input.touches)
            {
                //Cache touch position
                touchCache = Camera.main.ScreenToWorldPoint(touch.position);
                //If touch x position is less than or equal to a fraction of the screen width
                if (touchCache.x < transform.position.x)
                {
                    left = true;
                }
                //If mouse x position is greater than or equal to a fraction of the screen width
                if (touchCache.x > transform.position.x)
                {
                    right = true;
                }
            }
        }
#endif
    }


    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        /*
        if (Input.GetMouseButtonDown(0))
        {
#if UNITY_EDITOR
            offset = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, 0));
            //for touch device
#elif (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8)
            offset = Camera.main.ScreenToWorldPoint(new Vector2(Input.GetTouch(0).position.x, 0));
#endif
        }
        else if (Input.GetMouseButton(0))
        {
            if (offset.x < transform.position.x)
            {
                rigidBody.AddForce(transform.right * -thrust);
            }
            else if(offset.x > transform.position.x)
            {
                rigidBody.AddForce(transform.right * thrust);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
        }
        // Accelerate right if 'd' is pressed and left if 'a' is pressed; slow down if neither
        else */
        
        if (Input.GetKey("a") || Input.GetKey("left") || left)
        {
            rigidBody.AddForce(transform.right * -thrust);
            left = false;
            right = false;
            // Velocity model is too jerky; large momentum shifts make stacking impossible
            //rigidBody.velocity = new Vector2(-velocity, 0);
        }
        else if (Input.GetKey("d") || Input.GetKey("right") || right)
        {
            rigidBody.AddForce(transform.right * thrust);
            right = false;
            left = false;
            //rigidBody.velocity = new Vector2(velocity, 0);
        }
        else
        {
            if (rigidBody.velocity.x != 0)
            {
                // Determine direction of motion
                float sign = rigidBody.velocity.x / Mathf.Abs(rigidBody.velocity.x);

                // Apply an opposing force to come to a gentle rest
                rigidBody.AddForce(transform.right * (-sign * thrust));
            }

            if (Mathf.Abs(rigidBody.velocity.x) < 0.25)
            {
                rigidBody.velocity = new Vector2(0, 0);
            }
        }

        // Keep the velocity within the max velocity
        float temp = Mathf.Clamp(rigidBody.velocity.x, -maxVelocity, maxVelocity);
        rigidBody.velocity = new Vector2(temp, 0);

        if(transform.position.x < -18f)
        {
            rigidBody.AddForce(transform.right * thrust * 15);
        }
        else if (transform.position.x > 18f)
        {
            rigidBody.AddForce(transform.right * -thrust * 15);
        }

        //float x_pos = Mathf.Clamp(transform.position.x, -18f, 18f);
        //transform.position = new Vector3(x_pos, transform.position.y, transform.position.z);

    }
}
