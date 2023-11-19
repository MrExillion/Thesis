using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using SensorInputPrototype.InspectorReadOnlyCode;
using Unity.Mathematics;

public static class HRotate
{
#if UNITY_EDITOR
    [ShowOnly]
#endif
    [SerializeField]
    private static ConditionalWeakTable<MHRotate, Fields> table;
    static HRotate()
    {

        table = new ConditionalWeakTable<MHRotate, Fields>();
    }
    private sealed class Fields
    {
        internal float horizontalRotation = 0f; // this should match the cameras current rotation  --- and it should reset on transition complete
        internal float horizontalRotationEuler = 0;
        //[SerializeField] internal GameObject signalObject; // used in Radar to track game objects or instantiate them
        internal enum CallbackRegistry { GetCallbackRegistry = 0, UpdateRotation = 1, GetHRotation = 2, OnRotatePhone = 3, TryTransition = 4};
    }
    public static void UpdateRotation(this MHRotate map, float rotationToSave, float eulerAngleToSave)
    {
        table.GetOrCreateValue(map).horizontalRotation = rotationToSave;
        table.GetOrCreateValue(map).horizontalRotationEuler = eulerAngleToSave;
        //Debug.Log(table.GetOrCreateValue(map).horizontalRotation);
    }

    public static float GetHRotation(this MHRotate map)
    {
        return table.GetOrCreateValue(map).horizontalRotation;

    }
    public static void OnRotatePhone(this MHRotate map, Camera camera)
    {
        camera.gameObject.transform.rotation = new Quaternion(camera.transform.rotation.x, camera.transform.rotation.y, table.GetOrCreateValue(map).horizontalRotation, camera.transform.rotation.w);
        
    }
    public static bool TryTransition(this MHRotate map)
    {
        //Debug.Log("Horizontal Angle: " + table.GetOrCreateValue(map).horizontalRotationEuler);

        //float angleZ = Camera.main.gameObject.transform.rotation.eulerAngles.z;
        return ((table.GetOrCreateValue(map).horizontalRotationEuler > -300.0f && table.GetOrCreateValue(map).horizontalRotationEuler < -60.0f) || (table.GetOrCreateValue(map).horizontalRotationEuler < 300.0f && table.GetOrCreateValue(map).horizontalRotationEuler > 60.0f) );
        //return (MathF.Acos(table.GetOrCreateValue(map).horizontalRotation) < -60.0f || MathF.Acos(table.GetOrCreateValue(map).horizontalRotation) > 60.0f);

    }

    //private static Fields.CallbackRegistry GetCallBackRegistry(this MHRotate map)
    //{

    //    //return table.CallbackRegistry.GetOrCreateValue(map)
    //    return Fields.CallbackRegistry;
    //}
/*
    private static object CallbackExecute(this MHRotate map, string functionNameToHookToCallback, float arg1, Func<Debug> func)
    {
        object payload = null;
        
        
        switch (functionNameToHookToCallback)
        {
            case nameof(Fields.CallbackRegistry.GetCallbackRegistry) :
                    {
                    //table.GetOrCreateValue(map).GetCallbackRegistry(arg1,arg2);
                    throw new ArgumentException("Function Not Implemented", functionNameToHookToCallback);
                    
                    
                    }

            case nameof(Fields.CallbackRegistry.UpdateRotation):
                {
                    map.UpdateRotation(arg1);
                    payload = func;
                    break;
                }
            case nameof(Fields.CallbackRegistry.GetHRotation):
                {
                    map.GetHRotation();
                    payload = func;
                    break;
                }
            case nameof(Fields.CallbackRegistry.OnRotatePhone):
                {

                    throw new ArgumentException("Incorrect param type", "arg1: found float expected Camera");
                }
            case nameof(Fields.CallbackRegistry.TryTransition):
                {
                    map.TryTransition();
                    payload = func;
                    break;
                }
            default:
                {
                    throw new ArgumentException("Function Not Implemented", functionNameToHookToCallback);
                    
                }
        }
        
        

        return payload;
    }
    private static object CallbackExecute(this MHRotate map, string functionNameToHookToCallback, Camera arg1, Func<Debug> func)
    {
        object payload = null;


        switch (functionNameToHookToCallback)
        {
            case nameof(Fields.CallbackRegistry.GetCallbackRegistry):
                {
                    throw new ArgumentException("Function Not Implemented", functionNameToHookToCallback);
                }

            case nameof(Fields.CallbackRegistry.UpdateRotation):
                {
                                          
                    
                    throw new ArgumentException("Incorrect param type", "arg1: found Camera expected float");
                    
                }
            case nameof(Fields.CallbackRegistry.GetHRotation):
                {
                    map.GetHRotation();
                    payload = func;
                    break;
                }
            case nameof(Fields.CallbackRegistry.OnRotatePhone):
                {

                    map.OnRotatePhone(arg1);
                    payload = func;
                    break;
                }
            case nameof(Fields.CallbackRegistry.TryTransition):
                {
                    map.TryTransition();
                    payload = func;
                    break;
                }
            default:
                {
                    throw new ArgumentException("Function Not Implemented", functionNameToHookToCallback);
                }
        }



        return payload;
    }

    private static object CallbackExecute(this MHRotate map, string functionNameToHookToCallback, Func<UnityEngine.Debug> func)
    {
        object payload = null;


        switch (functionNameToHookToCallback)
        {
            case nameof(Fields.CallbackRegistry.GetCallbackRegistry):
                {
                    throw new ArgumentException("Function Not Implemented", functionNameToHookToCallback);
                }

            case nameof(Fields.CallbackRegistry.UpdateRotation):
                {


                    throw new ArgumentException("Incorrect param type", "arg1: missing argument expected float");

                }
            case nameof(Fields.CallbackRegistry.GetHRotation):
                {
                    func();
                    payload = map.GetHRotation();
                    break;
                }
            case nameof(Fields.CallbackRegistry.OnRotatePhone):
                {

                    throw new ArgumentException("Incorrect param type", "arg1: missing argument expected a Camera");
                }
            case nameof(Fields.CallbackRegistry.TryTransition):
                {
                    func();
                    payload = map.TryTransition(); ;
                    break;
                }
            default:
                {
                    throw new ArgumentException("Function Not Implemented", functionNameToHookToCallback);
                }
        }



        return payload;
    }

    */



}