using UnityEngine;
using System.Collections;

public class BackgroundChangeRandomColor : MonoBehaviour
{
    Color newColor, oldColor;
    public float rate = 0.1f;
    float tLerp = 0;
    // Use this for initialization
    void Start()
    {
        //set a random color
        newColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1);
        //cache the current color
        oldColor = GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {

        tLerp += Time.deltaTime * rate; //speed
        Color color = Color.Lerp(oldColor, newColor, tLerp);
        GetComponent<Renderer>().material.color = color;
        
        if (tLerp >= 1)//we reached the random color, so create a new one and reset the lerp variable
        {
            oldColor = GetComponent<Renderer>().material.color;
            newColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1);
            tLerp = 0;
        }
    }


}
