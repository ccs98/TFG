using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class GameUtility
{
    public const float ResolutionDelayTime = 1;
    public const string SavePrefKey = "Game_Highscore_Value";
    public const string archivoXmlNombre = "Questions_Data";
    public static string archivoXmlPath
    {
        get
        {
            return Application.dataPath + "/";
        }
    }
}
[System.Serializable()]
public class Data
{
    public Question[] preguntas = new Question[0];
    public Data(){}
    public static void write(Data datos, string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Data));
        using (Stream stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, datos);
        }
    }
    public static Data Fetch(string Path, int number)
    {
        return Fetch(out bool resultado, Path, number);
    }
    public static Data Fetch(out bool resultado,string archivoPath, int number)
    {
        if(File.Exists("Questions_Data" + number.ToString() + ".xml"))
        {
            Debug.Log("entra en 1");
            resultado = false;
            return new Data();
        }
        XmlSerializer deserializer = new XmlSerializer(typeof(Data));
        using (Stream stream = new FileStream(archivoPath, FileMode.Open))
        {
            var data = (Data) deserializer.Deserialize(stream);
            resultado = true;
            return data;
        }
    }
}
