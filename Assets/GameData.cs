using UnityEngine.SceneManagement;

internal class GameData
{
    private class LevelData
    {
        public int sceneIndex = 0;
        public bool unlocked = false;
        public bool completed = false;
    }

    private static readonly LevelData[] levelDatas_ = new LevelData[] {
        new() { sceneIndex = 2, unlocked = true},
        new() { sceneIndex = 3 },
        new() { sceneIndex = 4 },
        new() { sceneIndex = 5 },
        new() { sceneIndex = 6 },
        new() { sceneIndex = 7 },
        new() { sceneIndex = 8 },
        new() { sceneIndex = 9 }
    };

    private static int currentLevelDataIndex_ = 0;

    internal static bool IsLevelCompleted(int level) { return levelDatas_[level - 1].completed; }

    internal static bool IsLevelUnlocked(int level) { return levelDatas_[level - 1].unlocked; }

    internal static void LoadMenu() { SceneManager.LoadScene(1); }

    internal static void LoadLevel(int level)
    {
        currentLevelDataIndex_ = level - 1;
        SceneManager.LoadScene(levelDatas_[currentLevelDataIndex_].sceneIndex);
    }

    internal static void CurrentLevelCompleted()
    {
        levelDatas_[currentLevelDataIndex_].completed = true;

        if (currentLevelDataIndex_ < levelDatas_.Length - 1)
            levelDatas_[currentLevelDataIndex_ + 1].unlocked = true;
    }
}
