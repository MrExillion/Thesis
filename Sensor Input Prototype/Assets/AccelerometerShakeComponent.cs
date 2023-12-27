
using SensorInputPrototype.MixinInterfaces;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SensorInputPrototype.MixinInterfaces
{
    /// <summary>
    /// place THIS class <see cref="AccelerometerShakeComponent"/> on the GameObject as a Component where you whish to incorporate the Mixin. Ofc.You want to create one for each Mixin you whish to make. And best practice to use them with a Prefab with a Prefab Script Responsible for communicating with other mixins
    /// </summary>
    public class AccelerometerShakeComponent : MonoBehaviour, MAccelerometerShake
    {
        public LinearAccelerationSensor linearAccelerometerSensorReference; // you can implement this however you like, but needs to be public or have a public Get() function
        public UniversalPanel universalPanel;
        void Awake()
        {
            this.MixinClass_Initialized(gameObject);
        }

        // Use this for initialization
        void Start()
        {
            this.MixinClass_Start();
        }

        // Update is called once per frame
        void Update()
        {
            this.MixinClass_Update();
        }
        void FixedUpdate()
        {
            this.MixinClass_FixedUpdate();
        }
    
    
    }
}