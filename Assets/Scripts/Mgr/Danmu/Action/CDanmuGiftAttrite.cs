using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class CDanmuGiftAttrite : Attribute
{ 
    public string eventKey; // �¼���ֵ

    public CDanmuGiftAttrite(string value)
    {
        eventKey = value;
    }
}
