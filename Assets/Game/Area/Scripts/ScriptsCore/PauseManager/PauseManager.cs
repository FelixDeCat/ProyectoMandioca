using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;

public class PauseManager : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            var myGameCores = FindObjectsOfType<DontDestroy>().Where(x => x.transform != transform.parent).ToArray();

            for (int i = 0; i < myGameCores.Length; i++)
                Destroy(myGameCores[i].gameObject);
            SceneManager.LoadScene(0);
            Destroy(transform.parent.gameObject);
        }
    }
}
