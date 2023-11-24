using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using SensorInputPrototype.InspectorReadOnlyCode;
using SensorInputPrototype.MixinInterfaces;
public static class Radar
{
    #if UNITY_EDITOR
    [ShowOnly] 
    #endif
    [SerializeField]
    private static ConditionalWeakTable<MRadar, Fields> table;
    static Radar()
    {

        table = new ConditionalWeakTable<MRadar, Fields>();
    }
    private sealed class Fields
    {
        internal float timeOfReturn = 0f; // I have my suspicions as to whether or not this should be internal, really.
        internal float timeOfBroadcast = 0f; //
        internal float directionOfView = 0f; //
        internal float directionOfReturn = 0f; //
        [SerializeField] internal GameObject signalObject;
    }
    public static float GetAngleToPing(this MRadar map)
    {
        float angle = 0f;
        float aOut = table.GetOrCreateValue(map).directionOfView; // should be set to view at the time of broadcast? or is this maybe unnessecary.
        float aIn = table.GetOrCreateValue(map).directionOfReturn;
        angle = aOut - aIn;
        return angle;
    }
    public static void SetSignalObject(this MRadar map, GameObject signalObject)
    {
        table.GetOrCreateValue(map).signalObject = signalObject;
        
    }
    public static void SignalBroadcast(this MRadar map, float tSignalOut)
    {
        table.GetOrCreateValue(map).timeOfBroadcast = tSignalOut;

    }
    public static void SignalReturnRecieved(this MRadar map, float tSignalIn, Transform transformSelf, Transform transformPing)
    {
        table.GetOrCreateValue(map).timeOfReturn = tSignalIn;
        Vector3 pingVectorDir = Vector3.Normalize(transformPing.position - transformSelf.position);

        table.GetOrCreateValue(map).directionOfReturn = Vector3.SignedAngle(pingVectorDir, transformSelf.forward, transformSelf.up);
        //(transformPing.position - transformSelf.position)


    }
    public static void SendBroadcast(this MRadar map, GameObject goCaster) // can be hitscan or projectile, projectile is smarter i think.
    {
        GameObject.Instantiate(table.GetOrCreateValue(map).signalObject, goCaster.transform.position, goCaster.transform.rotation); // instantiate the pulsewave although its possible it should reflect for a guaranteed return registration? NO idk.
    }

}