using SensorInputPrototype.MixinInterfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SensorInputPrototype.MixinInterfaces
{
    public static class AccelerometerShake
    {
        private static ConditionalWeakTable<MAccelerometerShake, Fields> table;
        
        static AccelerometerShake()
        {

            table = new ConditionalWeakTable<MAccelerometerShake, Fields>();

        }
        private sealed class Fields /* : MonoBehaviour, MTransition*/  // Inheritance Mainly for Interfaces, try to avoid it. Update and Start don't work in here anyway parse gameObject to table in Inititialization phase. 
        {
            internal GameObject _gameObject; // gameObject where Mixin is Implemented
            internal UniversalPanel universalPanel; // TemplateComponent Connection/Adapter/Interface/Manifold use this to communicate between instances and multiple mixins. Think of it as Containing sockets that can convert USB to RJ45 or something else, like a driver.
            internal AccelerometerShakeComponent accelerometerShakeComponent; // use this to communicate with the properties of the implementation specific to this instance.
            internal LinearAccelerationSensor linearAccelerationSensor; // You can use any built in class you like as long as its marked as internal and thus accessible from table.GetOrCreateValue(Interface parsed by extension).accelerometerSensor etc.       
            internal int frameCount = 0; // You can do this with any data type, field or class accesible to your scope, as long as its parsed in, is declared in preproccessor "using" or is native to C#.
            internal Vector3 lastFrameReading = Vector3.one;
            internal int sampleRate = 64;
            internal Vector3 currentMaxMagnitude;
            internal Vector3 currentMinMagnitude;
            internal bool cooldownIP = false;
            internal Queue<Vector3> accelerationVector3Buffer = new(); // or new List<string>(); depends on your style ofc.
        }
        /// <summary>
        /// This function should be called in a <see cref="AccelerometerShakeComponent"/> : <see cref="MonoBehaviour"/>, <seealso cref="MAccelerometerShake"/>  <code cref="MonoBehaviour"> void Awake(){ this.MixinClass_Initialized(gameObject);}</code>
        /// <br> Modify behaviour in <seealso cref="AccelerometerShake"/> NOT in the Component</br>
        /// </summary>
        /// <param name="mMixinInterface"></param>
        /// <param name="arg_gameObject"></param>
        public static void MixinClass_Initialized(this MAccelerometerShake mMixinInterface, GameObject arg_gameObject)
        {
            table.GetOrCreateValue(mMixinInterface)._gameObject = arg_gameObject;
            table.GetOrCreateValue(mMixinInterface).accelerometerShakeComponent = table.GetOrCreateValue(mMixinInterface)._gameObject.GetComponent<AccelerometerShakeComponent>();
            table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor = table.GetOrCreateValue(mMixinInterface).accelerometerShakeComponent.linearAccelerationSensorReference; /* Obviously this must be implemented*/
            table.GetOrCreateValue(mMixinInterface).universalPanel = table.GetOrCreateValue(mMixinInterface)._gameObject.GetComponent<UniversalPanel>();
            /*
            You can fill the initialization as you need.

            */


        }

        /*
        Add Functions as you need But do keep 1 call per monobehaviour function.

        */
        /// <summary>
        /// Called from void <see cref="AccelerometerShakeComponent.Start"/>
        /// <br> Modify behaviour in <seealso cref="AccelerometerShake"/> NOT in the Component</br> 
        /// </summary>
        /// <param name="mMixinInterface"> use 'this' it refers to the AccelerometerShackeComponent's implementation of MAccelerometerShake interface</param>
        public static void MixinClass_Start(this MAccelerometerShake mMixinInterface)
        {
            InputSystem.EnableDevice(table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor);
            // called in void Start();
            table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor.samplingFrequency = Mathf.Max(Mathf.Min(Mathf.Round(1/Time.deltaTime), 64.0f),16f); //Either match framerate or 64Hz, yet no less than 16Hz, doesn't go beyond 64FPS.
            _Fields(mMixinInterface).sampleRate = Mathf.RoundToInt(_Fields(mMixinInterface).linearAccelerationSensor.samplingFrequency);
            table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor.MakeCurrent();
            if (GlobalReferenceManager.GetCurrentUniversalPanel().transitionType != 8 && table.GetOrCreateValue(mMixinInterface).universalPanel != GlobalReferenceManager.GetCurrentUniversalPanel())
            {
                InputSystem.DisableDevice(table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor);
                //table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor.samplingFrequency = 1f; // reduce calls to once per second 1Hz when not in use.
            }
            table.GetOrCreateValue(mMixinInterface).lastFrameReading = table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor.acceleration.value;
            table.GetOrCreateValue(mMixinInterface).accelerationVector3Buffer.Enqueue(table.GetOrCreateValue(mMixinInterface).lastFrameReading);
            table.GetOrCreateValue(mMixinInterface).currentMaxMagnitude = table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor.acceleration.value;
            table.GetOrCreateValue(mMixinInterface).currentMinMagnitude = table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor.acceleration.value;
        }
        /// <summary>
        /// Called from void <see cref="AccelerometerShakeComponent.Update"/>
        /// <br> Modify behaviour in <seealso cref="AccelerometerShake"/> NOT in the Component</br>
        /// </summary>
        /// <param name="mMixinInterface"> use 'this' it refers to the AccelerometerShackeComponent's implementation of MAccelerometerShake interface</param>
        public static void MixinClass_Update(this MAccelerometerShake mMixinInterface)
        {
            MAccelerometerShake m = mMixinInterface;
            if (table.GetOrCreateValue(mMixinInterface).universalPanel != GlobalReferenceManager.GetCurrentUniversalPanel())
            {
                m.MixinClass_BufferFlush();
                return;
            }
            Vector3 linearAcceleration = table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor.acceleration.value;
            //Debug.Log(linearAcceleration);
            if(table.GetOrCreateValue(mMixinInterface).accelerationVector3Buffer.Count < _Fields(m).sampleRate && table.GetOrCreateValue(mMixinInterface).accelerationVector3Buffer.Count > 1)
            {
                table.GetOrCreateValue(mMixinInterface).accelerationVector3Buffer.Enqueue(linearAcceleration);
                
                if(table.GetOrCreateValue(mMixinInterface).accelerationVector3Buffer.Count != _Fields(m).sampleRate)
                {
                    return;
                }
                


            }
            else if (table.GetOrCreateValue(mMixinInterface).accelerationVector3Buffer.Count == 0)
            {
                //Debug.Log("FocusGained");
                mMixinInterface.MixinClass_OnFocusGained();
                return;
            }
            
                table.GetOrCreateValue(mMixinInterface).accelerationVector3Buffer.Dequeue();
                table.GetOrCreateValue(mMixinInterface).accelerationVector3Buffer.Enqueue(linearAcceleration);
            



            // Once the buffer is filled we start looking at it:
            Vector3[] bufferArr = new Vector3[_Fields(m).accelerationVector3Buffer.Count];
            table.GetOrCreateValue(mMixinInterface).accelerationVector3Buffer.CopyTo(bufferArr, 0);
            Vector3 positiveBufferReadings = Vector3.zero;
            Vector3 negativeBufferReadings = Vector3.zero;
            Vector3 bufferReadingsDelta = Vector3.zero;
            Vector3 minMaxDelta = Vector3.zero;
            if (!bufferArr.Contains(_Fields(m).currentMaxMagnitude)) // the logic is not reallly needed, resetting them would be usefull anyaway, but this way keeps them persistent also. But it has 0 uses nor effect.
            {
                _Fields(m).currentMaxMagnitude = Vector3.zero;
            }
            if (!bufferArr.Contains(_Fields(m).currentMinMagnitude))
            {
                _Fields(m).currentMinMagnitude = Vector3.zero;
            }
            foreach (var reading in bufferArr)
            {
                table.GetOrCreateValue(mMixinInterface).currentMaxMagnitude.x = Mathf.Max(table.GetOrCreateValue(mMixinInterface).currentMaxMagnitude.x, reading.x);
                table.GetOrCreateValue(mMixinInterface).currentMaxMagnitude.y = Mathf.Max(table.GetOrCreateValue(mMixinInterface).currentMaxMagnitude.y, reading.y);
                table.GetOrCreateValue(mMixinInterface).currentMaxMagnitude.z = Mathf.Max(table.GetOrCreateValue(mMixinInterface).currentMaxMagnitude.z, reading.z);
                table.GetOrCreateValue(mMixinInterface).currentMinMagnitude.x = Mathf.Abs(Mathf.Min(table.GetOrCreateValue(mMixinInterface).currentMinMagnitude.x, reading.x));
                table.GetOrCreateValue(mMixinInterface).currentMinMagnitude.y = Mathf.Abs(Mathf.Min(table.GetOrCreateValue(mMixinInterface).currentMinMagnitude.y, reading.y));
                table.GetOrCreateValue(mMixinInterface).currentMinMagnitude.z = Mathf.Abs(Mathf.Min(table.GetOrCreateValue(mMixinInterface).currentMinMagnitude.z, reading.z));
                // add all the positive and negative magnitudes separately one negative pile and one positive pile, if this delta averaged is within 20% of the Max-Min delta then trigger.
                if ( reading.x < 0)
                {
                    negativeBufferReadings.x += reading.x *-1;
                }
                else // 0 readings included in maximum, maybe i should subtract these from the avg count?
                {
                    positiveBufferReadings.x += reading.y;
                }
                if (reading.y < 0)
                {
                    negativeBufferReadings.y += reading.y * -1;
                }
                else // 0 readings included in maximum, maybe i should subtract these from the avg count?
                {
                    positiveBufferReadings.y += reading.y;
                }
                if (reading.z < 0)
                {
                    negativeBufferReadings.z += reading.z * -1;
                }
                else // 0 readings included in maximum, maybe i should subtract these from the avg count?
                {
                    positiveBufferReadings.z += reading.z;
                }
            }
            bufferReadingsDelta.x = Mathf.Abs((positiveBufferReadings.x + negativeBufferReadings.x) / bufferArr.Length);
            bufferReadingsDelta.y = Mathf.Abs((positiveBufferReadings.y + negativeBufferReadings.y) / bufferArr.Length);
            bufferReadingsDelta.z = Mathf.Abs((positiveBufferReadings.z + negativeBufferReadings.z) / bufferArr.Length);
            minMaxDelta.x = Mathf.Abs(table.GetOrCreateValue(mMixinInterface).currentMaxMagnitude.x + table.GetOrCreateValue(mMixinInterface).currentMinMagnitude.x);
            minMaxDelta.y = Mathf.Abs(table.GetOrCreateValue(mMixinInterface).currentMaxMagnitude.y + table.GetOrCreateValue(mMixinInterface).currentMinMagnitude.y);
            minMaxDelta.z = Mathf.Abs(table.GetOrCreateValue(mMixinInterface).currentMaxMagnitude.z + table.GetOrCreateValue(mMixinInterface).currentMinMagnitude.z);

            //Debug.Log("BufferReadingsDelta: " + bufferReadingsDelta + ", Min-MaxDelta: " + minMaxDelta + ", +SigmaBuffer Values: " + positiveBufferReadings + ", -SigmaBuffer Values, \n" + negativeBufferReadings + ", CurrentMaxMagnitude in Buffer: " + table.GetOrCreateValue(mMixinInterface).currentMaxMagnitude + ",CurrentMinMagnitude in Buffer: " + table.GetOrCreateValue(mMixinInterface).currentMinMagnitude);

            if (minMaxDelta.x != 0)
            {

                //We dont want to divide by 0   >B^(

                //Debug.Log("TriggerTransition() if true: " + (Mathf.Abs(bufferReadingsDelta.x / minMaxDelta.x) > (1f - 0.25f) && minMaxDelta.x * 2 > 9.81f / 3) + "\n |bufferReadingsDelta.x / MinMaxDelta.x| > (1-0.25) \n && |MinMaxDelta.x*2| > 0.33G ===> (" + (9.81f / 3) + ")");
                if (Mathf.Abs(bufferReadingsDelta.x / minMaxDelta.x) > (1f - 0.25f) && minMaxDelta.x * 2 > 9.81f / 3) 
                {
                    table.GetOrCreateValue(mMixinInterface).frameCount++;
                    table.GetOrCreateValue(mMixinInterface).universalPanel.TriggerTransition();
                    //Debug.Log("Transition.TriggerTransition() was called by: UniversalPanel: " + table.GetOrCreateValue(mMixinInterface).universalPanel.name + ", ID: " + table.GetOrCreateValue(mMixinInterface).universalPanel.PanelId + ", InstanceID: " + table.GetOrCreateValue(mMixinInterface).universalPanel.GetInstanceID() + ", TransitionType: " + table.GetOrCreateValue(mMixinInterface).universalPanel.transitionType + ", MixinInterface: " + nameof(mMixinInterface) + ", Line: " + (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber() + ", frame count:" + table.GetOrCreateValue(mMixinInterface).frameCount);
                }
                
            }
            if (minMaxDelta.y != 0)
            {
                //We dont want to divide by 0   >B^(
                //Debug.Log("TriggerTransition() if true: " + (Mathf.Abs(bufferReadingsDelta.y / minMaxDelta.y) > (1f - 0.25f) && minMaxDelta.y * 2 > 9.81f / 3) + "\n |bufferReadingsDelta.y / MinMaxDelta.y| > (1-0.25) \n && |MinMaxDelta.y*2| > 0.33G ===> (" + (9.81f / 3) + ")");

                if (Mathf.Abs(bufferReadingsDelta.y / minMaxDelta.y) > (1f - 0.25f) && minMaxDelta.y * 2 > 9.81f / 3)
                {
                    table.GetOrCreateValue(mMixinInterface).frameCount++;
                    table.GetOrCreateValue(mMixinInterface).universalPanel.TriggerTransition();
                    //Debug.Log("Transition.TriggerTransition() was called by: UniversalPanel: " + table.GetOrCreateValue(mMixinInterface).universalPanel.name + ", ID: " + table.GetOrCreateValue(mMixinInterface).universalPanel.PanelId + ", InstanceID: " + table.GetOrCreateValue(mMixinInterface).universalPanel.GetInstanceID() + ", TransitionType: " + table.GetOrCreateValue(mMixinInterface).universalPanel.transitionType + ", MixinInterface: " + nameof(mMixinInterface) + ", Line: " + (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber() + ", frame count:" + table.GetOrCreateValue(mMixinInterface).frameCount);
                }
            }
            if (minMaxDelta.z != 0)
            {
                //We dont want to divide by 0   >B^(
                //Debug.Log("TriggerTransition() if true: " + (Mathf.Abs(bufferReadingsDelta.z / minMaxDelta.z) > (1f - 0.25f) && minMaxDelta.z * 2 > 9.81f / 3) + "\n |bufferReadingsDelta.z / MinMaxDelta.z| > (1-0.25) \n && |MinMaxDelta.z*2| > 0.33G ===> (" + (9.81f / 3) + ")");

                if (Mathf.Abs(bufferReadingsDelta.z / minMaxDelta.z) < (1f - 0.25f) && minMaxDelta.z * 2 > 9.81f / 3)
                {
                    table.GetOrCreateValue(mMixinInterface).frameCount++;
                    table.GetOrCreateValue(mMixinInterface).universalPanel.TriggerTransition();
                    //Debug.Log("Transition.TriggerTransition() was called by: UniversalPanel: " + table.GetOrCreateValue(mMixinInterface).universalPanel.name + ", ID: " + table.GetOrCreateValue(mMixinInterface).universalPanel.PanelId + ", InstanceID: " + table.GetOrCreateValue(mMixinInterface).universalPanel.GetInstanceID() + ", TransitionType: " + table.GetOrCreateValue(mMixinInterface).universalPanel.transitionType + ", MixinInterface: " + nameof(mMixinInterface) + ", Line: " + (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber() + ", frame count:" + table.GetOrCreateValue(mMixinInterface).frameCount);
                }

            }



            if(Mathf.RoundToInt(Time.realtimeSinceStartup) % 60 == 0 && !_Fields(m).cooldownIP)
            {


               
                
                //Debug.Log("Test of Fields as local parse variable implicit pointer or not:\n\t" +
                    //_Fields(m)._gameObject.GetInstanceID() + "\nNeeds to be the same as:\n\t" +
                    //table.GetOrCreateValue(mMixinInterface)._gameObject.GetInstanceID());
                _Fields(m).cooldownIP = true;

                
            }
            if (_Fields(m).cooldownIP)
            {
                _Fields(m).cooldownIP = (Mathf.RoundToInt(Time.realtimeSinceStartup) % 60 == 0);
            }



        }
        /// <summary>
        /// Called from void <see cref="AccelerometerShakeComponent.FixedUpdate"/>
        /// <br> Modify behaviour in <seealso cref="AccelerometerShake"/> NOT in the Component</br>
        /// </summary>
        /// <param name="mMixinInterface"> use 'this' it refers to the AccelerometerShackeComponent's implementation of MAccelerometerShake interface</param>
        public static void MixinClass_FixedUpdate(this MAccelerometerShake mMixinInterface) { }
        /*
        You can also add Delgegates and so on... BUT YOU CANNOT USE COROUTINES! They don't yield properly. I haven't researched why, but its possible they are not threadsafe due to deadlocks or some underlying problem with static functions. However if you use Mixin's properly you shouldn't need coroutines and in worst case scenario write a separate class for those in the traditional sense for unity and use them with caution.


        For best use add the mixins as their own components in prefabs, such that a prefab can turn the components on and off, or implement its own. Think of prefabs as the Templates, and the TemplateComponents as Component Mixins. 
        */

        public static void MixinClass_OnFocusGained(this MAccelerometerShake mMixinInterface)
        {
            InputSystem.EnableDevice(table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor);
            table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor.samplingFrequency = Mathf.Min(Mathf.Round(Time.deltaTime * 1), 50.0f); //Either match framerate or 50Hz, doesn't go beyond 50FPS.
            table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor.MakeCurrent();

            
            table.GetOrCreateValue(mMixinInterface).accelerationVector3Buffer.Enqueue(table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor.acceleration.value);
            
        }
        public static void MixinClass_BufferFlush(this MAccelerometerShake mMixinInterface)
        {
            table.GetOrCreateValue(mMixinInterface).accelerationVector3Buffer.Clear();
            table.GetOrCreateValue(mMixinInterface).lastFrameReading = Vector3.zero;
            if(GlobalReferenceManager.GetCurrentUniversalPanel().transitionType != 8 && table.GetOrCreateValue(mMixinInterface).universalPanel != GlobalReferenceManager.GetCurrentUniversalPanel())
            {
                InputSystem.DisableDevice(table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor);
                //table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor.samplingFrequency = 1f; // reduce calls to once per second 1Hz when not in use.
            }
        }

        private static Fields _Fields(this MAccelerometerShake m)
        {
            return table.GetOrCreateValue(m);

        }




    }
}