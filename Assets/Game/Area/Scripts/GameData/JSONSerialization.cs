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
        StreamWriter file = File.CreateText(DirectoryDocuments(newPath) + ".json");
        string json = JsonUtility.ToJson(data, true);

        file.Write(json);
        file.Close();
    }

    public static void Deserialize<T>(string newPath, T data)
    {
        if(File.Exists(DirectoryDocuments(newPath) + ".json"))
        {
            string infoJSON = File.ReadAllText(DirectoryDocuments(newPath) + ".json");
            JsonUtility.FromJsonOverwrite(infoJSON, data);
        }
        else
        {
            Debug.LogError("No existe el archivo JSON");
        }
    }

    public static bool IsFileExist(string path) => File.Exists(DirectoryDocuments(path) + ".json");

    public static string DirectoryDocuments(string path)
    {
        string pathDocuments = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments).Replace("\\", "/") + "/Mandioca";

        if (!Directory.Exists(pathDocuments))
            Directory.CreateDirectory(pathDocuments);

        pathDocuments += "/" + path;

        return pathDocuments;
    }
}
