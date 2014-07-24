using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.IO;
using System.Linq;
using System.Text;

/// <summary>
/// Each score entry
/// </summary>
public class ScoreEntry
{
    public int ScoreInt { get; set; }
    public DateTime Date { get; set; }
}

public class ScoreManager
{

    //High score table
    private List<ScoreEntry> HighScores;


    public ScoreManager()
    {
        HighScores = LoadScores();
    }

    private void SaveScores()
    {
        StringBuilder toSave = new StringBuilder();
        //order the scores and save the first 10
        HighScores = HighScores.OrderByDescending(x => x.ScoreInt).Take(10).ToList<ScoreEntry>();

        foreach (var item in HighScores)
        {
            toSave.AppendFormat("{0}-{1}-{2}-{3},", item.ScoreInt, item.Date.Year, item.Date.Month, item.Date.Day);
        }


        PlayerPrefs.SetString("HighScores", toSave.ToString());
    }

    public List<ScoreEntry> GetScores()
    {
        if (HighScores == null)
            LoadScores();
        return HighScores;
    }

    public void AddScore(ScoreEntry score)
    {
        HighScores.Add(score);
        SaveScores();
    }

    private List<ScoreEntry> LoadScores()
    {
        List<ScoreEntry> temp = new List<ScoreEntry>();
        //Get the data
        var data = PlayerPrefs.GetString("HighScores");
        //If not blank then load it
        if (!string.IsNullOrEmpty(data))
        {
            foreach (var item in data.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] info = item.Split(new char[] { '-' });
                temp.Add(new ScoreEntry()
                {
                    ScoreInt = int.Parse(info[0]),
                    Date = new DateTime(int.Parse(info[1]), int.Parse(info[2]), int.Parse(info[3]))
                });
            }
            return temp.OrderByDescending(x => x.ScoreInt).ToList<ScoreEntry>();
        }

        return temp;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.LoadLevel("StartScene");
    }

}
