using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class CDanmuCmdAttrite : Attribute
{
    public string eventKey; // ÊÂ¼ş¼üÖµ

    public CDanmuCmdAttrite(string value)
    {
        eventKey = value;
    }
}
