using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class CRandomEventAttri : Attribute
{
    public int nID; // �¼���ֵ

    public CRandomEventAttri(int value)
    {
        nID = value;
    }
}
