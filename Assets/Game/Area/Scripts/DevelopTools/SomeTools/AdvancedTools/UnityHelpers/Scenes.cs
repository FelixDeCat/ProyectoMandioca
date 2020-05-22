using UnityEngine.SceneManagement;

public static class Scenes {

    public static void Load_Ville() { Load("Ville"); }
    public static void Load_Dungeon() { Load("Dungeon"); }
    public static void Load_DungeonTuto() { Load("DungeonTuto"); }
    public static void Load_Game() { Load("AsteroidGame"); }
    public static void Load_0_LoadScene() { Load("Load"); }
    public static void Load_MainMenu() { Load("Menu"); }
    public static void Load_Options() { Load("Options"); }
    public static void Load_Credits() { Load("Credits"); }
    public static void Load_LevelSelector() { Load("LevelSelector"); }
    public static void Load_languageSelector() { Load("LanguajeSelector"); }
    public static void Load_DungeonGenerator() { Load("Generator Test"); }
    public static void Load_MainDungeon() { Load("Dungeon01"); }
    public static void Load_LoseEnergy() { Load("LoseEnergy"); }
    public static void Load_Message() { Load("MessageScene"); }
    public static void ReloadThisScene() { Load(SceneManager.GetActiveScene().name); }

    public static string GetActiveSceneName() { return SceneManager.GetActiveScene().name; }

    public static void LoadDungeon(int num)
    {
        Load("Dungeon_" + num.ToString());
    }

    public static void Load(string s) { SceneManager.LoadScene(s); }
}
