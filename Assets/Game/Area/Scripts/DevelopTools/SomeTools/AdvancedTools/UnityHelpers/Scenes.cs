using UnityEngine.SceneManagement;

public static class Scenes {

    public static void Load_0_Load() { Load("0_Load"); }
    public static void Load_Selector() { Load("Selector"); }
    public static void Load_Gym() { Load("Gym"); }
    public static void ReloadThisScene() { Load(SceneManager.GetActiveScene().name); }
    public static string GetActiveSceneName() { return SceneManager.GetActiveScene().name; }
    public static void Load(string s) { SceneManager.LoadScene(s); }
}
