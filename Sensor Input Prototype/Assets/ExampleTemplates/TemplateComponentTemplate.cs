
using UnityEngine;
using System.Runtime.CompilerServices;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using SensorInputPrototype.ExampleTemplates;

namespace SensorInputPrototype.ExampleTemplates
{
    /// <summary>
    /// place THIS class <see cref="TemplateComponentTemplate"/> on the GameObject as a Component where you whish to incorporate the Mixin. Ofc.You want to create one for each Mixin you whish to make. And best practice to use them with a Prefab with a Prefab Script Responsible for communicating with other mixins
    /// </summary>
    public class TemplateComponentTemplate : MonoBehaviour, MMixinInterfaceTemplate
    {
        public LightSensor lightSensorReference; // you can implement this however you like, but needs to be public or have a public Get() function
        public PrefabMixinManagerTemplate prefabMixinManager;
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
    }
}