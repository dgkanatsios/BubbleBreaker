using UnityEngine;
using System.Collections;

public class StartScene : MonoBehaviour
{
  
    // Use this for initialization
    void Start()
    {
        Globals.GameScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    void OnGUI()
    {
        Helpers.AutoResize(Globals.Width, Globals.Height);
        
        GUILayout.BeginArea(new Rect(10, 30, Globals.Width - 20, Globals.Height));
        GUILayout.Box("Bubble Breaker");


        if (GUILayout.Button("Start Game"))
            Application.LoadLevel("bubbleBreakerGameScene");

        if (GUILayout.Button("High scores"))
            Application.LoadLevel("highScoresScene");

        SettingsManager.Sound = GUILayout.Toggle(SettingsManager.Sound, "Sound");
        

        GUILayout.EndArea();
    }


}
