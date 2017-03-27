using UnityEngine;
using System.Collections;
using System;
using System.Xml;
using System.Xml.Serialization;

public class SafetyNetEntryData
{

    [XmlElement("id")]
    public int id { get; set; }
    [XmlElement("type")]
    public int type { get; set; }
    [XmlElement("position")]
    public Vector3 position { get; set; }

    public override int GetHashCode()
    {
        return base.GetHashCode() + id.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj == null || !(this.GetType().Equals(obj.GetType())))
        {
            return false;
        }
        SafetyNetEntryData compared = (SafetyNetEntryData) obj;
        if (id != compared.id)
            return false;
        return true;
    }
}