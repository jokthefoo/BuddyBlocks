using UnityEngine;
using System.Collections;

public class RainbowColor : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("ChangeColor", 0.25f, .01f);
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
        Color c = Color.HSVToRGB(colorValue, 1.0f, 1.0f);
        GetComponent<SpriteRenderer>().color = c;
        if (increasing)
        {
            colorValue = colorValue + (.01f);
        }
        else
        {
            colorValue = colorValue - (.01f);
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
