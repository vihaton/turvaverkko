using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System;

[XmlRoot("SafetyNetCollection")]
public class SaveDataContainer
{
    [XmlElement("Timestamp")]
    public string timeStamp;

    [XmlArray("SafetyNetArray"), XmlArrayItem("SafetyNet")]
    public SafetyNetData[] SafetyNetArray;
    
    public void Save(string path)
    {
        Debug.Log("SavePath: " + path);
        timeStamp = DateTime.Now.ToString();
        var serializer = new XmlSerializer(typeof(SaveDataContainer));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public SaveDataContainer Load(string path)
    {
        Debug.Log("LoadPath " + path);
        var serializer = new XmlSerializer(typeof(SaveDataContainer));

        if (!File.Exists(path))
        {
            Debug.Log(path + ": file not found, lets make new one");
            Save(path);
        }

        SaveDataContainer SDC = null;

        for (int i = 0; i < 3; i++)
        {
            SDC = ExtractSaveDataContainer(path, serializer);
            if (SDC != null) {
                return SDC;
            } else
            {
                Save(path);
            }
        }

        return SDC;
    }

    private static SaveDataContainer ExtractSaveDataContainer(string path, XmlSerializer serializer)
    {
        using (var stream = new FileStream(path, FileMode.Open))
        {
            try
            {
                return serializer.Deserialize(stream) as SaveDataContainer;
            } catch (Exception e)
            {
                Debug.Log("Could not retrieve SaveDataContainer, exception: " + e.ToString());
                return null;
            }
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static SaveDataContainer LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(SaveDataContainer));
        return serializer.Deserialize(new StringReader(text)) as SaveDataContainer;
    }
}