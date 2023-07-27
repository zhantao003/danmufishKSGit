using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class CRandomEventAttri : Attribute
{
    public int nID; // ÊÂ¼þ¼üÖµ

    public CRandomEventAttri(int value)
    {
        nID = value;
    }
}
