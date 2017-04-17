using UnityEngine;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("AnswerDataCollection")]
public class SaveDataContainer
{
    [XmlElement("Timestamp")]
    public string timeStamp;

    [XmlArray("EntryDataArray"), XmlArrayItem("Entry")]
    public SafetyNetEntryData[] SaveDataArray;
    
    public void Save(string path)
    {
        Debug.Log("SavePath: " + path);
        timeStamp = System.DateTime.Now.ToString();
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
            Save(path);
            Debug.Log(path + ": file not found, lets make new one");
        }

        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as SaveDataContainer;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static SaveDataContainer LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(SaveDataContainer));
        return serializer.Deserialize(new StringReader(text)) as SaveDataContainer;
    }
}