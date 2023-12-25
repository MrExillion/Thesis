using SensorInputPrototype.MixinInterfaces;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public static class LightSensorTransition 
    {
    private static ConditionalWeakTable<MLightSensorTransition, Fields> table;
        static LightSensorTransition()
        {

        table = new ConditionalWeakTable<MLightSensorTransition, Fields>();
        }
    private sealed class Fields : MonoBehaviour, MTransition
    {
        internal LightSensor lightSensor;
        internal float maxIntensity = 256f;
        internal float currentIntensity = 0f;
    }


    public static void SetupLightSensor(this MLightSensorTransition map)
    {
        //Get Senstor and assign to mixin map lightsensor
        table.GetOrCreateValue(map).lightSensor = InputSystem.GetDevice<LightSensor>();
        // Enable the lightsensor device linked to the mixin map.
        InputSystem.EnableDevice(table.GetOrCreateValue(map).lightSensor);
        // Get first reading for Lightsensor light Level
        table.GetOrCreateValue(map).currentIntensity = LightSensor.current.lightLevel.value;
    }
    public static void LightSensorUpdate(this MLightSensorTransition map)
    {
        //Uddate reading for Lightsensor light Level
        table.GetOrCreateValue(map).currentIntensity = LightSensor.current.lightLevel.value;
    }

    public static float LinearLightIntensityReadout(this MLightSensorTransition map)
    {
        return Mathf.Log10(table.GetOrCreateValue(map).currentIntensity) / 10;
    }

}
