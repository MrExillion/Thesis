using SensorInputPrototype.MixinInterfaces;
using System.Collections;
using System.Collections.Generic;
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
            
            //internal int exampleInt = 1; // You can do this with any data type, field or class accesible to your scope, as long as its parsed in, is declared in preproccessor "using" or is native to C#.
            internal float lastFrameReading = 1.0f;
            //internal string intensityLastFrame = "string";
            //internal enum enumItems { item = 0, item1 = 1 };
            internal float currentMaxMagnitude;
            internal float currentMinMagnitude;

            internal Queue<float> accelerationMagnitudeBuffer = new(); // or new List<string>(); depends on your style ofc.
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
            table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor = table.GetOrCreateValue(mMixinInterface).accelerometerShakeComponent.linearAccelerometerSensorReference; /* Obviously this must be implemented*/

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
            // called in void Start();
            table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor.samplingFrequency = Mathf.Min(Mathf.Round(Time.deltaTime * 1), 50.0f); //Either match framerate or 50Hz, doesn't go beyond 50FPS.
            table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor.MakeCurrent();
            if (GlobalReferenceManager.GetCurrentUniversalPanel().transitionType != 8 && table.GetOrCreateValue(mMixinInterface).universalPanel != GlobalReferenceManager.GetCurrentUniversalPanel())
            {
                table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor.samplingFrequency = 1f; // reduce calls to once per second 1Hz when not in use.
            }
            table.GetOrCreateValue(mMixinInterface).lastFrameReading = table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor.acceleration.value.magnitude;
            table.GetOrCreateValue(mMixinInterface).accelerationMagnitudeBuffer.Enqueue(table.GetOrCreateValue(mMixinInterface).lastFrameReading);
            table.GetOrCreateValue(mMixinInterface).currentMaxMagnitude = table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor.acceleration.value.magnitude;
            table.GetOrCreateValue(mMixinInterface).currentMinMagnitude = table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor.acceleration.value.magnitude;
        }
        /// <summary>
        /// Called from void <see cref="AccelerometerShakeComponent.Update"/>
        /// <br> Modify behaviour in <seealso cref="AccelerometerShake"/> NOT in the Component</br>
        /// </summary>
        /// <param name="mMixinInterface"> use 'this' it refers to the AccelerometerShackeComponent's implementation of MAccelerometerShake interface</param>
        public static void MixinClass_Update(this MAccelerometerShake mMixinInterface)
        {

            if (table.GetOrCreateValue(mMixinInterface).universalPanel != GlobalReferenceManager.GetCurrentUniversalPanel())
            {
                return;
            }
            Vector3 linearAcceleration = table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor.acceleration.value;
            if(table.GetOrCreateValue(mMixinInterface).accelerationMagnitudeBuffer.Count < 8 && table.GetOrCreateValue(mMixinInterface).accelerationMagnitudeBuffer.Count > 1)
            {
                table.GetOrCreateValue(mMixinInterface).accelerationMagnitudeBuffer.Enqueue(linearAcceleration.magnitude);
                
                if(table.GetOrCreateValue(mMixinInterface).accelerationMagnitudeBuffer.Count != 8)
                {
                    return;
                }
                


            }
            else if (table.GetOrCreateValue(mMixinInterface).accelerationMagnitudeBuffer.Count == 0)
            {
                mMixinInterface.MixinClass_OnFocusGained();
                return;
            }
            
                table.GetOrCreateValue(mMixinInterface).accelerationMagnitudeBuffer.Dequeue();
                table.GetOrCreateValue(mMixinInterface).accelerationMagnitudeBuffer.Enqueue(linearAcceleration.magnitude);
            



            // Once the buffer is filled we start looking at it:
            float[] bufferArr = new float[8];
            table.GetOrCreateValue(mMixinInterface).accelerationMagnitudeBuffer.CopyTo(bufferArr, 0);
            float positiveBufferReadings = 0;
            float negativeBufferReadings = 0;
            float bufferReadingsDelta = 0;
            float minMaxDelta = 0;
            foreach (var reading in bufferArr)
            {
                table.GetOrCreateValue(mMixinInterface).currentMaxMagnitude = Mathf.Max(table.GetOrCreateValue(mMixinInterface).currentMaxMagnitude, reading);
                table.GetOrCreateValue(mMixinInterface).currentMinMagnitude = Mathf.Min(table.GetOrCreateValue(mMixinInterface).currentMinMagnitude, reading);

                // add all the positive and negative magnitudes separately one negative pile and one positive pile, if this delta averaged is within 20% of the Max-Min delta then trigger.
                if( reading < 0)
                {
                    negativeBufferReadings -= reading;
                }
                else // 0 readings included in maximum, maybe i should subtract these from the avg count?
                {
                    positiveBufferReadings += reading;
                }
            
            }
            bufferReadingsDelta = Mathf.Abs((positiveBufferReadings - negativeBufferReadings) / 8f);
            minMaxDelta = Mathf.Abs(table.GetOrCreateValue(mMixinInterface).currentMaxMagnitude - table.GetOrCreateValue(mMixinInterface).currentMinMagnitude);


            
            if(bufferReadingsDelta == 0)
            {
                //We dont want to divide by 0   >B^(
                return;
            }
            if(Mathf.Abs(minMaxDelta/bufferReadingsDelta) < 0.25f && bufferReadingsDelta > 9.81f/3)
            {
                table.GetOrCreateValue(mMixinInterface).universalPanel.TriggerTransition();
            
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
            table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor.samplingFrequency = Mathf.Min(Mathf.Round(Time.deltaTime * 1), 50.0f); //Either match framerate or 50Hz, doesn't go beyond 50FPS.
            table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor.MakeCurrent();

            
            table.GetOrCreateValue(mMixinInterface).accelerationMagnitudeBuffer.Enqueue(table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor.magnitude);
            
        }
        public static void MixinClass_BufferFlush(this MAccelerometerShake mMixinInterface)
        {
            table.GetOrCreateValue(mMixinInterface).accelerationMagnitudeBuffer.Clear();
            table.GetOrCreateValue(mMixinInterface).lastFrameReading = 0f;
            if(GlobalReferenceManager.GetCurrentUniversalPanel().transitionType != 8 && table.GetOrCreateValue(mMixinInterface).universalPanel != GlobalReferenceManager.GetCurrentUniversalPanel())
            {
                table.GetOrCreateValue(mMixinInterface).linearAccelerationSensor.samplingFrequency = 1f; // reduce calls to once per second 1Hz when not in use.
            }
        }

    }
}