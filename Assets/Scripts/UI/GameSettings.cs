using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    private static string filePath;

    public float soundVolume = 1.0f;
    public float musicVolume = 1.0f;

    private void OnEnable()
    {
        filePath = Path.Combine(Application.persistentDataPath, "gameSettings.json");
    }

    public void LoadSettings()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }

    public void SaveSettings()
    {
        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(filePath, json);
    }
}
