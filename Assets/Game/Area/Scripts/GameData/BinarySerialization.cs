using System.Diagnostics;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class BinarySerialization
{
    public static void Serialize<T>(string name, T data)
    {
        FileStream file = File.Create(Application.dataPath + "/" + name + ".bin");
        var binary = new BinaryFormatter();

        binary.Serialize(file, data);

        file.Close();
    }

    public static T Deserialize<T>(string name)
    {
        if (File.Exists(Application.dataPath + "/" + name + ".bin"))
        {
            FileStream file = File.Open(Application.dataPath + "/" + name + ".bin", FileMode.Open);
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

    public static bool IsFileExist(string path) => File.Exists(Application.dataPath + "/" + path + ".bin");
}
