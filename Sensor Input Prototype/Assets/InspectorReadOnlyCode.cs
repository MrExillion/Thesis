using UnityEditor;
using UnityEngine;



namespace SensorInputPrototype.InspectorReadOnlyCode {
#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(ShowOnlyAttribute))]
public class ShowOnlyDrawer: PropertyDrawer
{
    

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        string valueStr;
        
        switch (prop.propertyType)
        {
            case SerializedPropertyType.Integer:
                valueStr = prop.intValue.ToString();
                break;
            case SerializedPropertyType.Boolean:
                valueStr = prop.boolValue.ToString();
                break;
            case SerializedPropertyType.Float:
                valueStr = prop.floatValue.ToString("0.00000");
                break;
            case SerializedPropertyType.String:
                valueStr = prop.stringValue;
                break;
            case SerializedPropertyType.Quaternion:
                valueStr = prop.quaternionValue.ToString();
                break;
            //case SerializedPropertyType.ObjectReference:
            //        if (prop.objectReferenceValue.name == null)
            //            valueStr = "Empty";
            //        else
            //            valueStr = prop.objectReferenceValue.name;
            //    break;
            default:
                valueStr = "(not supported)";
            break;
        }

        //EditorGUI.LabelField(position, this.fieldInfo.GetValue(preferredLabel) + label.text, valueStr);
        EditorGUI.LabelField(position, label.text, valueStr);
    }
}

public class ShowOnlyAttribute : PropertyAttribute
{
    
    //public ShowOnlyAttribute(string v)
    //{
    //    V = v;
        
    //}
    

    //public string V { get; }
    
    

}
#endif
}
