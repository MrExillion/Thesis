using SensorInputPrototype.InspectorReadOnlyCode;
using SensorInputPrototype.MixinInterfaces;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public static class TabTransition
{
#if UNITY_EDITOR
    [ShowOnly]
#endif
    [SerializeField]
    private static ConditionalWeakTable<MTabTransition, Fields> table;
    static TabTransition()
    {

        table = new ConditionalWeakTable<MTabTransition, Fields>();
    }
    private sealed class Fields : MonoBehaviour
    {
        internal float timeOfReturn = 0f; // I have my suspicions as to whether or not this should be internal, really.
        internal float timeOfBroadcast = 0f; //
        internal float directionOfView = 0f; //
        internal float directionOfReturn = 0f; //
        internal Vector2 screenCoordinates = new Vector2(0, 0);
        internal GameObject cursorItem;
        internal Vector3 cursorItemOrigin = new Vector3(0,0,0);
        //[SerializeField] internal GameObject signalObject;
    }
    public static float GetAngleToPing(this MTabTransition map)
    {
        float angle = 0f;
        float aOut = table.GetOrCreateValue(map).directionOfView; // should be set to view at the time of broadcast? or is this maybe unnessecary.
        float aIn = table.GetOrCreateValue(map).directionOfReturn;
        angle = aOut - aIn;
        return angle;
    }
    public static void OneFingerTouchTab(this MTabTransition map, float x, float y)
    {
        table.GetOrCreateValue(map).screenCoordinates.x = x;
        table.GetOrCreateValue(map).screenCoordinates.y = y;


    }
  
    public static Vector2 GetTouchCoords(this MTabTransition map)
    {


        return table.GetOrCreateValue(map).screenCoordinates;
    }
    public static void ClearCursorItem(this MTabTransition map)
    {
        table.GetOrCreateValue(map).cursorItem.transform.position = table.GetOrCreateValue(map).cursorItemOrigin;
        table.GetOrCreateValue(map).cursorItem = null;
        table.GetOrCreateValue(map).cursorItemOrigin = Vector3.zero;
        //Not really needed the below its just a precaution to kill off anything that lingers on.
        if (table.Any(x => x.Value.cursorItem != null))
        {
            foreach (var keyValuePair in table)
            {
                table.GetOrCreateValue(keyValuePair.Key).cursorItem = null;
                table.GetOrCreateValue(keyValuePair.Key).cursorItemOrigin = Vector3.zero;
            }
        }
    }
    public static void CursorItemSet(this MTabTransition map, GameObject item)
    {
        table.GetOrCreateValue(map).cursorItem = item;
        table.GetOrCreateValue(map).cursorItemOrigin = item.transform.position;
    }
    public static bool CursorHasItem(this MTabTransition map)
    {

        return table.GetOrCreateValue(map).cursorItem != null;
    }
    public static Vector3 GetCursorItemOrigin(this MTabTransition map)
    {
        return table.GetOrCreateValue(map).cursorItemOrigin;
    }

    public static GameObject GetCursorItem()
    {
        GameObject outval;
        outval = null;
        if (table.Any(x => x.Value.cursorItem != null))
        {
            foreach (var keyValuePair in table)
            {
                outval = table.GetOrCreateValue(keyValuePair.Key).cursorItem;
                break;
            }
        }

        try
        {
            if (outval != null)
            {
                throw new NullReferenceException("Outval in GetCursorItem failed, tried to get a cursoritem that does not exist.");


            }
            else
            {
                return outval;
            }



        }
        catch (NullReferenceException ex)
        {
            Debug.LogException(ex, table.First(x => x.Key == x.Key).Value.gameObject); // dunno how this works if at all...
            throw;
        
        }
    
    
    } // THIS IS CURSED!

    public static GameObject GetCursorItem(this MTabTransition map)
    {
        
        return table.GetOrCreateValue(map).cursorItem;
                
    }

    //public static void SetSignalObject(this MTabTransition map, GameObject signalObject)
    //{
    //    table.GetOrCreateValue(map).signalObject = signalObject;

    //}
    //public static void SignalBroadcast(this MTabTransition map, float tSignalOut)
    //{
    //    table.GetOrCreateValue(map).timeOfBroadcast = tSignalOut;

    //}
    //public static void SignalReturnRecieved(this MTabTransition map, float tSignalIn, Transform transformSelf, Transform transformPing)
    //{
    //    table.GetOrCreateValue(map).timeOfReturn = tSignalIn;
    //    Vector3 pingVectorDir = Vector3.Normalize(transformPing.position - transformSelf.position);

    //    table.GetOrCreateValue(map).directionOfReturn = Vector3.SignedAngle(pingVectorDir, transformSelf.forward, transformSelf.up);
    //    //(transformPing.position - transformSelf.position)


    //}
    //public static void SendBroadcast(this MRadar map, GameObject goCaster) // can be hitscan or projectile, projectile is smarter i think.
    //{
    //    GameObject.Instantiate(table.GetOrCreateValue(map).signalObject, goCaster.transform.position, goCaster.transform.rotation); // instantiate the pulsewave although its possible it should reflect for a guaranteed return registration? NO idk.
    //}

}