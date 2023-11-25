using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SensorInputPrototype.MixinInterfaces;
using UnityEngine;
using UnityEngine.InputSystem;

public static class ThesisAPI
{
    private static ConditionalWeakTable<MThesisAPI, Fields> table;
    static ThesisAPI()
    {
        table = new ConditionalWeakTable<MThesisAPI, Fields>();
        
    }

    private sealed class Fields : MonoBehaviour
    {


    }
    #region temporrary API function location 
    // needs its own library or location eventually.

    public static T GetComponentOrAdd<T>(this MThesisAPI map) where T : UnityEngine.Component
    {
        T component = table.GetOrCreateValue(map).gameObject.GetComponent<T>();
        if (component.GetComponent<T>() == null)
        {
            component = table.GetOrCreateValue(map).gameObject.AddComponent<T>() as T;

        }

        return component;
        
    }

    public static T GetComponentOrAdd_Overload1<T>(MThesisAPI host) where T : UnityEngine.Component
    {
                
        //T component = table.GetOrCreateValue(
            //(MThesisAPI)host.GetType().GetInterface("MThesisAPI")).gameObject.GetComponent<T>() as T;
            T component = table.GetOrCreateValue(host).GetComponent<T>() as T;

        if (component == null)
        {
            component = table.GetOrCreateValue(host).gameObject.AddComponent<T>() as T;

        }

        return component;

    }

    //public static KeyValuePair getInterfaceKey(this MThesisAPI map)
    //{
    //    System.Reflection.Assembly.
    //    //map.GetHashCode() == SensorInputPrototype.MixinInterfaces.MThesisAPI mThesisAPI
    //    //return KeyValuePair key: this MThesisAPI,MappingType.SimpleContent>;
    //}


    #endregion


}
