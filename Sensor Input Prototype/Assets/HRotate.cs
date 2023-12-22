using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using SensorInputPrototype.InspectorReadOnlyCode;
using Unity.Mathematics;
using SensorInputPrototype.MixinInterfaces;
using Unity.VisualScripting;

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
    private sealed class Fields : MonoBehaviour, MTransition
    {
        internal float horizontalRotation = 0f; // this should match the cameras current rotation  --- and it should reset on transition complete
        internal float horizontalRotationEuler = 0;
        internal bool hasTransitioned = false;
        
        //[SerializeField] internal GameObject signalObject; // used in Radar to track game objects or instantiate them
        internal enum CallbackRegistry { GetCallbackRegistry = 0, UpdateRotation = 1, GetHRotation = 2, OnRotatePhone = 3, TryTransition = 4};
    }
    public static void UpdateRotation(this MHRotate map, float rotationToSave, float eulerAngleToSave)
    {
        table.GetOrCreateValue(map).horizontalRotation = rotationToSave;
        table.GetOrCreateValue(map).horizontalRotationEuler = eulerAngleToSave;
        //Debug.Log(table.GetOrCreateValue(map).horizontalRotation);
    }

    public static bool IsReset(this MHRotate map)
    {
        

        GameObject gameObject = (GlobalReferenceManager.GetActivePanelTemplate() as PanelManagerTemplate).panelOrder[(int)MathF.Abs( (float) Camera.main.GetComponent<CameraSequencer>().currentComicManagerMixin.previousPanel )];
        if(gameObject.GetComponent<UniversalPanel>().transitionType == (int)Transition.transitionTypes.HRotate)
        {
            if (((table.GetOrCreateValue(map).horizontalRotationEuler > -300.0f && table.GetOrCreateValue(map).horizontalRotationEuler < -60.0f) || (table.GetOrCreateValue(map).horizontalRotationEuler < 300.0f && table.GetOrCreateValue(map).horizontalRotationEuler > 60.0f)) && table.GetOrCreateValue(gameObject.GetComponent<HRotateTemplate>()).hasTransitioned)
            {
                table.GetOrCreateValue(map).hasTransitioned = table.GetOrCreateValue(gameObject.GetComponent<HRotateTemplate>()).hasTransitioned;
            }
            else if (!((table.GetOrCreateValue(map).horizontalRotationEuler > -300.0f && table.GetOrCreateValue(map).horizontalRotationEuler < -60.0f) || (table.GetOrCreateValue(map).horizontalRotationEuler < 300.0f && table.GetOrCreateValue(map).horizontalRotationEuler > 60.0f)) && table.GetOrCreateValue(gameObject.GetComponent<HRotateTemplate>()).hasTransitioned)
            {
                table.GetOrCreateValue(gameObject.GetComponent<HRotateTemplate>()).hasTransitioned = false;
                table.GetOrCreateValue(map).hasTransitioned = table.GetOrCreateValue(gameObject.GetComponent<HRotateTemplate>()).hasTransitioned;
            }


        }
        return table.GetOrCreateValue(map).hasTransitioned;
    }

    public static float GetHRotation(this MHRotate map)
    {
        return table.GetOrCreateValue(map).horizontalRotation;

    }
    public static void OnRotatePhone(this MHRotate map, Camera camera)
    {
        camera.gameObject.transform.rotation = new Quaternion(camera.transform.rotation.x, camera.transform.rotation.y, table.GetOrCreateValue(map).horizontalRotation, camera.transform.rotation.w);
        
    }
    public static bool IsTransitionConditionMet(this MHRotate map)
    {
        //Debug.Log("Horizontal Angle: " + table.GetOrCreateValue(map).horizontalRotationEuler);
        
            //if (((table.GetOrCreateValue(map).horizontalRotationEuler > -300.0f && table.GetOrCreateValue(map).horizontalRotationEuler < -60.0f) || (table.GetOrCreateValue(map).horizontalRotationEuler < 300.0f && table.GetOrCreateValue(map).horizontalRotationEuler > 60.0f)))
            //  table.GetOrCreateValue(map).hasTransitioned = true;
            
        
                //float angleZ = Camera.main.gameObject.transform.rotation.eulerAngles.z;
         return ((table.GetOrCreateValue(map).horizontalRotationEuler > -300.0f && table.GetOrCreateValue(map).horizontalRotationEuler < -60.0f) || (table.GetOrCreateValue(map).horizontalRotationEuler < 300.0f && table.GetOrCreateValue(map).horizontalRotationEuler > 60.0f));
        //return (MathF.Acos(table.GetOrCreateValue(map).horizontalRotation) < -60.0f || MathF.Acos(table.GetOrCreateValue(map).horizontalRotation) > 60.0f);

    }
    public static bool IsResetConditionMet(this MHRotate map)
    {
        //Debug.Log("Horizontal Angle: " + table.GetOrCreateValue(map).horizontalRotationEuler);

        //if (((table.GetOrCreateValue(map).horizontalRotationEuler > -300.0f && table.GetOrCreateValue(map).horizontalRotationEuler < -60.0f) || (table.GetOrCreateValue(map).horizontalRotationEuler < 300.0f && table.GetOrCreateValue(map).horizontalRotationEuler > 60.0f)))
        //  table.GetOrCreateValue(map).hasTransitioned = true;
        Debug.Log("Camera rotation delta around z: " + (Mathf.Abs(Mathf.DeltaAngle(table.GetOrCreateValue(map).horizontalRotationEuler, Camera.main.transform.rotation.eulerAngles.z)) < 10f));

        //float angleZ = Camera.main.gameObject.transform.rotation.eulerAngles.z;
        return (Mathf.Abs(Mathf.DeltaAngle(table.GetOrCreateValue(map).horizontalRotationEuler, Camera.main.transform.rotation.eulerAngles.z)) < 10f);
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