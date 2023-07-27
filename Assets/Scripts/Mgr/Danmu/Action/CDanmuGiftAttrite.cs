using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class CDanmuGiftAttrite : Attribute
{ 
    public string eventKey; // ÊÂ¼þ¼üÖµ

    public CDanmuGiftAttrite(string value)
    {
        eventKey = value;
    }
}
