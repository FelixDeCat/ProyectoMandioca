using System.Diagnostics;
using System.IO;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public static class BinarySerialization
{
    public static void Serialize<T>(string name, T data)
    {
        FileStream file = File.Create(DirectoryDocuments(name) + ".bin");
        var binary = new BinaryFormatter();

        binary.Serialize(file, data);

        file.Close();
    }

    public static T Deserialize<T>(string name)
    {
        if (File.Exists(DirectoryDocuments(name) + ".bin"))
        {

            FileStream file = File.Open(DirectoryDocuments(name) + ".bin", FileMode.Open);
            var binary = new BinaryFormatter();

            T data = (T)binary.Deserialize(file);

            file.Close();

            return data;
        }
        else
        {
            UnityEngine.Debug.LogError("No hay archivo creado");
            return default(T);
        }
    }

    public static bool IsFileExist(string path) => File.Exists(DirectoryDocuments(path) + ".bin");

    public static string DirectoryDocuments(string path)
    {
        string pathDocuments = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments).Replace("\\", "/") + "/Mandioca";

        if (!Directory.Exists(pathDocuments))
            Directory.CreateDirectory(pathDocuments);

        pathDocuments += "/" + path;

        return pathDocuments;
    }
}
