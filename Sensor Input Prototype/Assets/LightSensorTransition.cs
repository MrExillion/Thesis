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
        internal GameObject _gameObject; // gameObject where Mixin is Implemented
        internal UniversalPanel universalPanel;
        internal LightSensor lightSensor;
        internal float maxIntensity = 256f;
        internal float currentIntensity = 0f;
        internal float intensityLastFrame = 0f;
        internal float decreasingLightTimerStartTime = 0f;
        internal float intensityChangeTolerance = 0.1f;
    }


    public static void SetupLightSensor(this MLightSensorTransition map,GameObject arg_GameObject)
    {
        // Link GameObject To KeyValuePair Mapping
        table.GetOrCreateValue(map)._gameObject = arg_GameObject;
        //Link UniversalPanel Associated with this mixin. And Throw errors when null
        table.GetOrCreateValue(map).universalPanel = arg_GameObject.GetComponent<UniversalPanel>();
        //Get Senstor and assign to mixin map lightsensor
        table.GetOrCreateValue(map).lightSensor = InputSystem.GetDevice<LightSensor>();
        // Enable the lightsensor device linked to the mixin map.
        InputSystem.EnableDevice(table.GetOrCreateValue(map).lightSensor);
        table.GetOrCreateValue(map).lightSensor.MakeCurrent();
        // Get first reading for Lightsensor light Level
        table.GetOrCreateValue(map).currentIntensity = LightSensor.current.lightLevel.value;
        // Set last frame also to avoid first frame triggers
        table.GetOrCreateValue(map).intensityLastFrame = LightSensor.current.lightLevel.value;
    }
    public static void LightSensorUpdate(this MLightSensorTransition map)
    {

        table.GetOrCreateValue(map).lightSensor.MakeCurrent();
        //Update last frame Lightsensor reading of light level to this frame before updating this frame.
        table.GetOrCreateValue(map).intensityLastFrame = table.GetOrCreateValue(map).currentIntensity;
        //Uddate reading for Lightsensor light Level
        table.GetOrCreateValue(map).currentIntensity = LightSensor.current.lightLevel.value;
    }

    public static float LinearLightIntensityReadout(this MLightSensorTransition map)
    {
        return Mathf.Log10(table.GetOrCreateValue(map).currentIntensity) / 10;
    }
    public static float RawLightIntensityReadout(this MLightSensorTransition map)
    {
        return table.GetOrCreateValue(map).currentIntensity;
    }
    public static void LightSensorReadOutDump(this MLightSensorTransition map)
    {
        Debug.Log("LinearReadOut: " + map.LinearLightIntensityReadout() + ", Raw: " + map.RawLightIntensityReadout() + ", Previous Frame RawReadout: " + table.GetOrCreateValue(map).intensityLastFrame + "\n cur-last < -tolerance: "+ (table.GetOrCreateValue(map).currentIntensity - table.GetOrCreateValue(map).intensityLastFrame < -table.GetOrCreateValue(map).intensityChangeTolerance).ToString());
    }


    public static void OnLightSensorCovered(this MLightSensorTransition map, float durationExpression)
    {

            if((table.GetOrCreateValue(map).currentIntensity < table.GetOrCreateValue(map).intensityLastFrame) && table.GetOrCreateValue(map).decreasingLightTimerStartTime < Time.realtimeSinceStartup )
            {
                table.GetOrCreateValue(map).decreasingLightTimerStartTime = Time.realtimeSinceStartup + durationExpression;
                table.GetOrCreateValue(map).maxIntensity = table.GetOrCreateValue(map).currentIntensity;
            }
            else if (((table.GetOrCreateValue(map).currentIntensity - table.GetOrCreateValue(map).intensityLastFrame <= -table.GetOrCreateValue(map).intensityChangeTolerance) ||
                    (table.GetOrCreateValue(map).maxIntensity/4 > (table.GetOrCreateValue(map).currentIntensity)))
                    
                    && table.GetOrCreateValue(map).decreasingLightTimerStartTime <= Time.realtimeSinceStartup) 
            {
            // Must have been blocked for the duration if the following logic is implemented. Either light is approx 0 while the timer has reached its target, or light was lower than last frame while target duration was reached, under the condition that the timer gets "reset" if the Sensor is reexposed in the related logic.
                if (table.GetOrCreateValue(map).universalPanel.transitionType == 6)
                {
                    table.GetOrCreateValue(map).universalPanel.TriggerTransition();
                }
                if (table.GetOrCreateValue(map).universalPanel.transitionType == 7)
                 {
                    //if (table.GetOrCreateValue(map)._gameObject.GetComponent<MicrophoneBlowAirTrigger>().canTransition) // kept inside the == 7 due to requiring MicrophoneBlowAirTrigger != null
                    //{
                    //    table.GetOrCreateValue(map).universalPanel.TriggerTransition();
                    //}
                }
            }
            else if (table.GetOrCreateValue(map).currentIntensity - table.GetOrCreateValue(map).intensityLastFrame >= table.GetOrCreateValue(map).intensityChangeTolerance)
            {
                // Lightsensor must have been re-exposed.
                table.GetOrCreateValue(map).decreasingLightTimerStartTime = Time.realtimeSinceStartup;

            }
        
    }
}
