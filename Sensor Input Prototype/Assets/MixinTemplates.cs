using System.Runtime.CompilerServices;
using UnityEngine;
using System.Reflection;

using SensorInputPrototype.InspectorReadOnlyCode;

public static class MixinTemplates
{
    #if UNITY_EDITOR 
    [ShowOnly] 
    #endif
    [SerializeField]
    private static ConditionalWeakTable<MMixinTemplates, Fields> table;
    static MixinTemplates()
    {

        table = new ConditionalWeakTable<MMixinTemplates, Fields>();
    }
    private sealed class Fields
    {
        internal int templateID;

    }





}