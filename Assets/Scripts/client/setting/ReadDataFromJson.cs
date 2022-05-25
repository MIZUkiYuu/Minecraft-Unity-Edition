using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

#region JsonFormatClass
public class JsonData 
{
    public List<JsonFileData> KeyBindings;
}

[Serializable]
public class JsonFileData
{
    public string name;
    public string key;
}
#endregion

public class ReadDataFromJson
{
    public List<JsonFileData> JsonData = new List<JsonFileData>();

    public string ReadFromJson(string jsonFileName)
    {
        if (File.Exists(Application.dataPath + "/Json/" + jsonFileName))
        {
            StreamReader sr = new StreamReader(Application.dataPath + "/Json/" + jsonFileName);
            string readJsonString = sr.ReadToEnd();
            sr.Close();
            return readJsonString;
        }
        else
        {
            Debug.Log("NOT FOUND FILE");
        }
        return "NO ANY DATA";
    }
}
