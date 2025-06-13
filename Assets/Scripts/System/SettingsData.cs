using UnityEngine;
using System.IO;

[System.Serializable]
public class SettingsData
{
    public bool musicOn = true;
    public bool sfxOn = true;

    private static string path => Application.persistentDataPath + "/settings.json";

    public static void Save(bool music, bool sfx)
    {
        SettingsData data = new SettingsData { musicOn = music, sfxOn = sfx };
        File.WriteAllText(path, JsonUtility.ToJson(data));
    }

    public static SettingsData Load()
    {
        if (!File.Exists(path))
            return new SettingsData(); // default ON

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<SettingsData>(json);
    }
}
