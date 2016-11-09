using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BombCountdown : MonoBehaviour
{
    public GameObject MainController;

    Color textColor;
    int textSize;
    Vector3 textPosition;

    // Use this for initialization
    void Start()
    {
        
        InvokeRepeating("TextAnimation", 0.25f, .02f);
    }

    float everySecond;
    float colorValue = 0;
    bool increasing = true;

    // Update is called once per frame
    void Update()
    {
    }

    void TextAnimation()
    {

        if (MainController.GetComponent<OtherGameControls>().level_handler.timer < 10)
        {
            textSize = 100;
            textPosition = new Vector3(-175, -50, 0);
            textColor = Color.HSVToRGB(1.0f, 1.0f, colorValue);

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

        else
        {
            textSize = 75;
            textPosition = new Vector3(-150, -40, 0);
            textColor = Color.HSVToRGB(0.114f, 1.0f, 1.0f);
        }
        GetComponent<Text>().color = textColor;
        //GetComponent<Text>().fontSize = textSize;
        //GetComponent<RectTransform>().position = textPosition;
    }
}
