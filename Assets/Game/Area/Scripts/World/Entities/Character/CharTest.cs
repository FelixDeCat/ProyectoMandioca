using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharTest : MonoBehaviour
{

    public int offset = 20;

    void Start()
    {
        DevelopTools.UI.Debug_UI_Tools.instance.CreateToogle("speed fast", false, ChangeSpeed);
    }

    public string ChangeSpeed(bool active)
    {
        var character = GetComponentInChildren<CharacterHead>();

        if (active)character.SetFastSpeed();
        else character.SetNormalSpeed();
        
        return active ? "velocidad alta" : "velocidad normal";
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var character = GetComponentInChildren<CharacterHead>();
            var posaux = new Vector3(character.gameObject.transform.position.x, character.gameObject.transform.position.y + offset, character.gameObject.transform.position.z);
            character.gameObject.transform.position = posaux;
        }
    }


    #region Debuggin
    [System.Serializable]
    public class DebugOptions
    {
        
    }
    #endregion
}
