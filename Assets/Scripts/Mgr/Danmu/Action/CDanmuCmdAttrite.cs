using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class CDanmuCmdAttrite : Attribute
{
    public string eventKey; // �¼���ֵ

    public CDanmuCmdAttrite(string value)
    {
        eventKey = value;
    }
}
