//using SensorInputPrototype.MixinInterfaces;
//using System.Collections;
//using System.Runtime.CompilerServices;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using System.Collections.Generic;
//using SensorInputPrototype.ExampleTemplates;

//namespace SensorInputPrototype.ExampleTemplates
//{
    ///// <summary>
    ///// place THIS class <see cref="TemplateComponent"/> on the GameObject as a Component where you whish to incorporate the Mixin. Ofc.You want to create one for each Mixin you whish to make. And best practice to use them with a Prefab with a Prefab Script Responsible for communicating with other mixins
    ///// </summary>
    //public class TemplateComponent : MonoBehaviour, MMixinInterfaceTemplate
    //{
    //    public LightSensor lightSensorReference; // you can implement this however you like, but needs to be public or have a public Get() function
    //    public PrefabMixinManager prefabMixinManager;
    //    void Awake()
    //    {
    //        this.MixinClass_Initialized(gameObject);
    //    }

    //    // Use this for initialization
    //    void Start()
    //    {
    //        this.MixinClass_Start();
    //    }

    //    // Update is called once per frame
    //    void Update()
    //    {
    //        this.MixinClass_Update();
    //    }
    //}
//}


//public interface MMixinInterfaceTemplate /* interface name */
//{
//    // Reference only functions set in static Class with ConditionalWeakTable<MMmixinInterfaceMap, ClassNamedFields> table. table used with extension methods when the interface is implemented making the pattern Mixin as using Component and Decoupling pattern principles.

//}
//public static class MixinClass
//{
//    private static ConditionalWeakTable<MMixinInterfaceTemplate, Fields> table;
//    static MixinClass()
//    {

//        table = new ConditionalWeakTable<MMixinInterfaceTemplate, Fields>();
//    }
//    private sealed class Fields /* : MonoBehaviour, MTransition*/  // Inheritance Mainly for Interfaces, try to avoid it. Update and Start don't work in here anyway parse gameObject to table in Inititialization phase. 
//    {
//        internal GameObject _gameObject; // gameObject where Mixin is Implemented
//        internal PrefabMixinManagerTemplate prefabMixinManager; // TemplateComponent Connection/Adapter/Interface/Manifold use this to communicate between instances and multiple mixins. Think of it as Containing sockets that can convert USB to RJ45 or something else, like a driver.
//        internal TemplateComponentTemplate accelerometerShakeComponent; // use this to communicate with the properties of the implementation specific to this instance.
//        internal LightSensor accelerometerSensor; // You can use any built in class you like as long as its marked as internal and thus accessible from table.GetOrCreateValue(Interface parsed by extension).accelerometerSensor etc.       
//        internal int exampleInt = 1; // You can do this with any data type, field or class accesible to your scope, as long as its parsed in, is declared in preproccessor "using" or is native to C#.
//        internal float exampleFloat = 1.0f;
//        internal string intensityLastFrame = "string";
//        internal enum enumItems { item = 0, item1 = 1 };
//        internal List<string> stringList = new(); // or new List<string>(); depends on your style ofc.
//    }
//    /// <summary>
//    /// This function should be called in a <see cref="TemplateComponentTemplate"/> : <see cref="MonoBehaviour"/>, <seealso cref="MMixinInterfaceTemplate"/>  <code cref="MonoBehaviour"> void Awake(){ this.MixinClass_Initialized(gameObject);}</code>
//    /// </summary>
//    /// <param name="mMixinInterface"></param>
//    /// <param name="arg_gameObject"></param>
//    public static void MixinClass_Initialized(this MMixinInterfaceTemplate mMixinInterface, GameObject arg_gameObject)
//    {
//        table.GetOrCreateValue(mMixinInterface)._gameObject = arg_gameObject;
//        table.GetOrCreateValue(mMixinInterface).accelerometerShakeComponent = table.GetOrCreateValue(mMixinInterface)._gameObject.GetComponent<TemplateComponentTemplate>();
//        table.GetOrCreateValue(mMixinInterface).accelerometerSensor = table.GetOrCreateValue(mMixinInterface).accelerometerShakeComponent.lightSensorReference; /* Obviously this must be implemented*/
//        /*
//        You can fill the initialization as you need.
        
//        */
//    }

//    /*
//    Add Functions as you need But do keep 1 call per monobehaviour function.
    
//    */
//    public static void MixinClass_Start(this MMixinInterfaceTemplate mMixinInterface) { }
//    public static void MixinClass_Update(this MMixinInterfaceTemplate mMixinInterface) { }
//    public static void MixinClass_FixedUpdate(this MMixinInterfaceTemplate mMixinInterface) { }
//    /*
//    You can also add Delgegates and so on... BUT YOU CANNOT USE COROUTINES! They don't yield properly. I haven't researched why, but its possible they are not threadsafe due to deadlocks or some underlying problem with static functions. However if you use Mixin's properly you shouldn't need coroutines and in worst case scenario write a separate class for those in the traditional sense for unity and use them with caution.


//    For best use add the mixins as their own components in prefabs, such that a prefab can turn the components on and off, or implement its own. Think of prefabs as the Templates, and the TemplateComponents as Component Mixins. 
//    */
//}

//public class PrefabMixinManager : MonoBehaviour
//{
//    /*
//    Properties for managing mixin components on this gameobject used as a prefab, This Prefab can Inherit from Other Prefabs as long as it holds nothing important. Instead use Prefabs that has prefabs that use this class for each nested prefab, this way you inherit the GameObject Properties, but no inheritance brittleness is kept.
    
//    primary function of this is to be used as GetComponent<PrefabMixinManager>().TryGetComponentInChildren etc. or a common property like an ID that mixins require to determine how to behave, yet isn't required for the functioning of the code at compiletime, while also allowing for mixins to be removed or added with no need for them to know about each other.

//    */

//}