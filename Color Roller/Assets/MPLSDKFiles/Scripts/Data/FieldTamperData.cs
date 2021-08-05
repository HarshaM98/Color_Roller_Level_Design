using System;

[Serializable]
public class FieldTamperData
{
    public double FieldStoredInSDK;
    public double PreviousField;

    public FieldTamperData(double fieldStoredInSdk, double prevField)
    {
        FieldStoredInSDK = fieldStoredInSdk;
        PreviousField = prevField;
    }

    public override string ToString()
    {
        return ("{" + FieldStoredInSDK + ":" + PreviousField + "}");
    }
}

[Serializable]
public class Field
{
    public string FieldName;
    public string FieldSequence;

    public Field(string fieldName, string fieldSequence)
    {
        FieldName = fieldName;
        FieldSequence = fieldSequence;
    }

    public override string ToString()
    {
        return ("{" + FieldName + ":" + FieldSequence + "}");
    }
}
