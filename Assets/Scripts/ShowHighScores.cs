using UnityEngine;
using System.Collections;
using System.Linq;

public class ShowHighScores : MonoBehaviour
{

    
    // Use this for initialization
    void Start()
    {

    }

    public ScoreManager manager;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.LoadLevel("StartScene");
    }

    void Awake()
    {
        manager = new ScoreManager();
    }

    void OnGUI()
    {
        Helpers.AutoResize(Globals.Width, Globals.Height);

        GUILayout.BeginArea(new Rect(10, 30, Globals.Width - 20, Globals.Height));

        if (Globals.GameScore != 0)
            GUILayout.Label("Your score: " + Globals.GameScore);

        GUILayout.Box("High scores");
        GUILayout.Box("Press Esc to go back");
        foreach (var score in manager.GetScores())
        {
            GUILayout.Label(string.Format("{0} - {1}", score.Date.ToShortDateString(), score.ScoreInt));
        }
        GUILayout.EndArea();
    }

}
