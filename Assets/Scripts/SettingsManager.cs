using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// A helper class to store our settings
/// </summary>
public static class SettingsManager
{


    public static bool Sound
    {
        get
        {
            return bool.Parse(PlayerPrefs.GetString("Sound", "true"));
        }
        set
        {
            PlayerPrefs.SetString("Sound", value.ToString());
        }
    }
}

