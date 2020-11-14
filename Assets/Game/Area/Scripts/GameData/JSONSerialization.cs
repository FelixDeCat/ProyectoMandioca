using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Json;

public static class JSONSerialization
{
    public static void Serialize<T>(string newPath, T data)
    {
        //Creamos el archivo de texto
        StreamWriter file = File.CreateText(Application.dataPath + "/" + newPath + ".json");
        string json = JsonUtility.ToJson(data, true);

        file.Write(json);
        file.Close();
    }

    public static void Deserialize<T>(string newPath, T data)
    {
        if(File.Exists(Application.dataPath + "/" + newPath + ".json"))
        {
            string infoJSON = File.ReadAllText(Application.dataPath + "/" + newPath + ".json");
            JsonUtility.FromJsonOverwrite(infoJSON, data);
        }
        else
        {
            Debug.LogError("No existe el archivo JSON");
        }
    }

    public static bool IsFileExist(string path) => File.Exists(Application.dataPath + "/" + path + ".json");
}
