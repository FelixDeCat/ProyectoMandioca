using UnityEngine.SceneManagement;
using Tools.Nomenclature;

public static class Scenes {

    public static void Load_Load() { Load(Nomenclature.SCENE_NAME_FIRST_SCENE_FOR_LOADING); }
    public static void Load_Menu() { Load(Nomenclature.SCENE_NAME_FIRST_SCENE_FOR_LOADING); }
    public static void Load_Gym() { Load(Nomenclature.SCENE_NAME_GYM); }
    public static void ReloadThisScene() { Load(SceneManager.GetActiveScene().name); }
    public static void UnloadThisScene() { UnLoad(SceneManager.GetActiveScene().name); }
    public static string GetActiveSceneName() { return SceneManager.GetActiveScene().name; }
    public static void Load(string s) { SceneManager.LoadScene(s); }
    public static void UnLoad(string s) { SceneManager.UnloadSceneAsync(s); }
}
