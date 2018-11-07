using UnityEditor;
using UnityEngine;

public class PlayerPrefsReset : EditorWindow {
     
    [MenuItem("Edit/Reset Playerprefs")]
     
    public static void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
