using UnityEngine;
using System.Collections;
using System;
using System.Xml;
using System.Xml.Serialization;

public class SafetyNetEntryData
{

    [XmlElement("name")]
    public string entryName { get; set; }
    [XmlElement("description")]
    public string entryDescription { get; set; }
    [XmlElement("type")]
    public int entryType { get; set; }
    [XmlElement("importance")]
    public float entryImportance { get; set; }
    [XmlElement("position")]
    public Vector3 entryPosition { get; set; }

    public override int GetHashCode()
    {
        return base.GetHashCode() + entryName.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj == null || !(this.GetType().Equals(obj.GetType())))
        {
            return false;
        }
        SafetyNetEntryData compared = (SafetyNetEntryData) obj;
        if (entryName != compared.entryName || entryType != compared.entryType || entryPosition != compared.entryPosition)
            return false;

        if (!GetHashCode().Equals(compared.GetHashCode()))
            return false;

        return true;
    }
}