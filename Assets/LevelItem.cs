using UnityEngine;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour
{
    public int Level;
    public TMPro.TMP_Text Title;
    public Image Star;
    public Image Lock;

    void Start()
    {
        Title.text = $"Level {Level:D3}";
        Star.enabled = GameData.IsLevelCompleted(Level);
        Lock.enabled = !GameData.IsLevelUnlocked(Level);
    }

    public void OnClicked()
    {
        if (GameData.IsLevelUnlocked(Level))
            GameData.LoadLevel(Level);
    }
}
