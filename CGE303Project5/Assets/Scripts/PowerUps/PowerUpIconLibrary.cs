using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerUpIconEntry
{
    public string name;
    public Sprite icon;
}

// Add power up icons in Assets/PowerUPIconLibrary
[CreateAssetMenu(fileName = "PowerUpIconLibrary", menuName = "PowerUps/Icon Library")]
public class PowerUpIconLibrary : ScriptableObject
{
    public List<PowerUpIconEntry> icons;

    private Dictionary<string, Sprite> iconDict;

    void OnEnable()
    {
        iconDict = new Dictionary<string, Sprite>();
        foreach (var entry in icons)
        {
            iconDict[entry.name] = entry.icon;
        }
    }

    public Sprite GetIcon(string name)
    {
        if (iconDict != null && iconDict.ContainsKey(name))
            return iconDict[name];
        return null;
    }
}
