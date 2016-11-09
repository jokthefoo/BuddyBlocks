using UnityEngine;
using System.Collections;

public class BlocksFall : MonoBehaviour {

    public Transform playerPlatform;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag != "NonBlock")
        {
            //if(col.tag != "bomb")
            //{
            //    GameObject temp = col.transform.parent.gameObject;
            //    if (temp.tag != "Boxes")
            //    {
            //        Destroy(temp);
            //    }
            //}

            Destroy(col.gameObject);
        }     
    }
}
