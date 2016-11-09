using UnityEngine;
using System.Collections;

public class BombColor : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("ChangeColor", 0.25f, .02f);
    }
    float everySecond;
    float colorValue = 0;
    bool increasing = true;

    // Update is called once per frame
    void Update()
    {
    }

    void ChangeColor()
    {
        Color c = Color.HSVToRGB(0.0f, 1.0f, colorValue);
        GetComponent<SpriteRenderer>().color = c;
        if (increasing)
        {
            colorValue = colorValue + (.08f);
        }
        else
        {
            colorValue = colorValue - (.08f);
        }

        if (colorValue >= 1)
        {
            increasing = false;
        }
        else if (colorValue <= 0)
        {
            increasing = true;
        }
    }
}
