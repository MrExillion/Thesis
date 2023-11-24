using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SensorInputPrototype.MixinInterfaces;
using UnityEngine;

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



    #endregion


}
