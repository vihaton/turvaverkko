using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class SafetyNetData
{

    [XmlElement("id")]
    public int id { get; set; }
    [XmlArray("SafetyNetEntryData"), XmlArrayItem("Entry")]
    public SafetyNetEntryData[] SafetyNetArray;
    
}
