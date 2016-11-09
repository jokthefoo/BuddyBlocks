using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class MaxVelocity : MonoBehaviour
{
    public OtherGameControls OtherControls;

    Rigidbody2D rigidBody;
    Transform thePlatform;
    float maxVelocity;

    // Use this for initialization
    void Start()
    {
   
        maxVelocity = OtherControls.MaxVel_Blocks;
        thePlatform = OtherControls.level_handler.Platform;

        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rigidBody != null)
        {
            if (Mathf.Abs(rigidBody.velocity.y) >= maxVelocity)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, -maxVelocity);
            }
        }

        if (transform.position.y < thePlatform.position.y - 30)
        {
            Destroy(transform.gameObject);
        }
    }
}
