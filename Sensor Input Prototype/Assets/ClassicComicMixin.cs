using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
//using UnityEngine.InputSystem.EnhancedTouch;


namespace SensorInputPrototype.MixinInterfaces
{
    public static class ClassicComicMixin
    {
        private static ConditionalWeakTable<MClassicComicMixin, Fields> table;
        static ClassicComicMixin()
        {

            table = new ConditionalWeakTable<MClassicComicMixin, Fields>();
        }
        private sealed class Fields /* : MonoBehaviour, MTransition*/  // Inheritance Mainly for Interfaces, try to avoid it. Update and Start don't work in here anyway parse gameObject to table in Inititialization phase. 
        {
            internal GameObject _gameObject; // gameObject where Mixin is Implemented
            internal Camera camera; // TemplateComponent Connection/Adapter/Interface/Manifold use this to communicate between instances and multiple mixins. Think of it as Containing sockets that can convert USB to RJ45 or something else, like a driver.
            internal CameraSequencer cameraSequencer; // use this to communicate with the properties of the implementation specific to this instance.
            //internal LightSensor lightSensor; // You can use any built in class you like as long as its marked as internal and thus accessible from table.GetOrCreateValue(Interface parsed by extension).lightSensor etc.       
            internal int exampleInt = 1; // You can do this with any data type, field or class accesible to your scope, as long as its parsed in, is declared in preproccessor "using" or is native to C#.
            internal float zoom = 1.0f;
            internal string intensityLastFrame = "string";
            internal Touch touch;
            internal Touch touch2;
            internal Touch touchLastFrame;
            internal Touch touch2LastFrame;
            internal int panelFocus;
            internal int touchCountLastFrame = 0;
            internal enum enumItems { item = 0, item1 = 1 };
            internal List<string> stringList = new(); // or new List<string>(); depends on your style ofc.
        }
        /// <summary>
        /// This function should be called in a <see cref="CameraSequencer"/> : <see cref="MonoBehaviour"/>, <seealso cref="MClassicComicMixin"/>  <code cref="MonoBehaviour"> void Awake(){ this.ClassicMixin_Initialized(gameObject);}</code>
        /// </summary>
        /// <param name="mMixinInterface"></param>
        /// <param name="arg_gameObject"></param>
        public static void ClassicMixin_Initialized(this MClassicComicMixin mMixinInterface, GameObject arg_gameObject)
        {
            MClassicComicMixin M = mMixinInterface;
            µ(M)._gameObject = arg_gameObject;
            µ(M).cameraSequencer = µ(M)._gameObject.GetComponent<CameraSequencer>();
            µ(M).camera = Camera.main;
            //table.GetOrCreateValue(mMixinInterface).lightSensor = table.GetOrCreateValue(mMixinInterface).cameraSequencer.lightSensorReference; /* Obviously this must be implemented*/
            /*
            You can fill the initialization as you need.

            */
            //µ(mMixinInterface).touch = Input.GetTouch(0);
            


        }

        /*
        Add Functions as you need But do keep 1 call per monobehaviour function.

        */
        public static void ClassicMixin_Start(this MClassicComicMixin mMixinInterface)
        {
            MClassicComicMixin M = mMixinInterface;
            //µ(M).touch = Input.GetTouch(0);
        }

        public static void ClassicMixin_CurrentPanelFocus(this MClassicComicMixin mMixinInterface)
        {
            MClassicComicMixin M = mMixinInterface;
            RaycastHit hit;
            Vector3 rayOrigin = Camera.main.transform.position;


            Ray ray = new Ray(rayOrigin, Camera.main.transform.forward);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.TryGetComponent(out UniversalPanel component))
                {
                    µ(M).panelFocus = GlobalRefManagerComponent.singleton.GetDefactoPanelId(component);
                }
            }

            DataAcquisition.Singleton.timeSpentLookingAtClassicPanel[table.GetOrCreateValue(M).panelFocus] += Time.unscaledDeltaTime;

        }



        public static void ClassicMixin_Update(this MClassicComicMixin mMixinInterface)
        {
            MClassicComicMixin M = mMixinInterface;
            bool changeRegistered = false;
            foreach (Touch touch in Input.touches)
            {
                if (Input.touches[touch.fingerId].deltaPosition.magnitude > 0.05f)
                {
                    changeRegistered = true;
                }
            }

            if ((Input.touchCount != µ(M).touchCountLastFrame) || changeRegistered)
            {
                Touch[] touchesToDump = new Touch[Input.touches.Length];
                if (touchesToDump.Length >= 1)
                {
                    Input.touches.CopyTo(touchesToDump, 0);
                    for (int i = 0; i < touchesToDump.Length; i++)
                    {
                        DataAcquisition.Singleton.touchesListClassic.Add(touchesToDump[i]);
                    }

                }
            }
            µ(M).touchCountLastFrame = Input.touchCount;
            M.ClassicMixin_CurrentPanelFocus();


            if (Input.touchCount == 1) 
            {
                µ(M).touch = Input.GetTouch(0);
                if (µ(M).touch.phase == TouchPhase.Began)
                {
                    DataAcquisition.Singleton.numberOfTouchInteractionsClassic += 1;
                }
                if (µ(M).touch.phase == TouchPhase.Moved)
                {
                    Vector3 tmp1v3 = Camera.main.ScreenToWorldPoint(µ(M).touch.position);
                    Vector3 tmp2v3 = Camera.main.ScreenToWorldPoint(µ(M).touchLastFrame.position);
                    Vector3 vec3 = new(tmp1v3.x - tmp2v3.x, tmp1v3.y - tmp2v3.y, 0);
                    Vector3 shV3 = µ(M).cameraSequencer.comicManager.gameObject.transform.position;
                    µ(M).cameraSequencer.comicManager.gameObject.transform.position = new(shV3.x+vec3.x,shV3.y+vec3.y,shV3.z);

                    //µ(M)._gameObject.transform.Translate(Vector3.Normalize(vec3)*);
                    

                    //µ(M).touchPosLastFrame = µ(M).touch.position;
                }
                if (µ(M).touch.phase == TouchPhase.Ended)
                {

                }
                µ(M).touchLastFrame = µ(M).touch;

            }
            if (Input.touchCount == 2)
            {
                µ(M).touch = Input.GetTouch(0);
                µ(M).touch2 = Input.GetTouch(1);
                if (µ(M).touch.phase == TouchPhase.Began)
                {
                    //µ(M).touch2LastFrame = µ(M).touch2;
                    //µ(M).touchLastFrame = µ(M).touch;
                    DataAcquisition.Singleton.numberOfTouchInteractionsClassic += 1;
                }
                if (µ(M).touch.phase == TouchPhase.Moved)
                {

                    float zoommultiplier = 0.25f;
                    if(Vector2.Distance(µ(M).touch.position,µ(M).touch2.position)*Time.deltaTime > Vector2.Distance(µ(M).touchLastFrame.position, µ(M).touch2LastFrame.position)*Time.deltaTime)
                    {
                        µ(M).zoom = Mathf.Max(0.001f, µ(M).zoom + zoommultiplier*Time.deltaTime * (Vector2.Distance(µ(M).touch.position, µ(M).touch2.position) - Vector2.Distance(µ(M).touchLastFrame.position, µ(M).touch2LastFrame.position)));
                        //    / Mathf.Sqrt(Mathf.Abs(µ(M).camera.scaledPixelHeight * µ(M).camera.scaledPixelWidth)));
                        µ(M).zoom = Mathf.Min(10f, µ(M).zoom);

                    }
                    if (Vector2.Distance(µ(M).touch.position, µ(M).touch2.position) < Vector2.Distance(µ(M).touchLastFrame.position, µ(M).touch2LastFrame.position))
                    {
                        µ(M).zoom = Mathf.Max(0.001f, µ(M).zoom + zoommultiplier*Time.deltaTime*(Vector2.Distance(µ(M).touch.position, µ(M).touch2.position) - Vector2.Distance(µ(M).touchLastFrame.position, µ(M).touch2LastFrame.position)));
                            
                            
                            
                            
                            // / Mathf.Sqrt(Mathf.Abs(µ(M).camera.scaledPixelHeight * µ(M).camera.scaledPixelWidth)));


                        µ(M).zoom = Mathf.Min(10f, µ(M).zoom);
                    }
                    GlobalReferenceManager.GetActiveComicTemplate().gameObject.transform.localScale = Vector3.one * µ(M).zoom;
                }
                if (µ(M).touch.phase == TouchPhase.Ended)
                {
                
                }
                µ(M).touchLastFrame = µ(M).touch;
                µ(M).touch2LastFrame = µ(M).touch2;

            }
            if (Input.touchCount == 3)
            {
                DataAcquisition.Singleton.numberOfTouchInteractionsClassic += 1;
                µ(M).zoom = 1;
                µ(M).cameraSequencer.comicManager.gameObject.transform.position = Vector3.zero;
                GlobalReferenceManager.GetActiveComicTemplate().gameObject.transform.localScale = Vector3.one * µ(M).zoom;

            }



            }
        public static void ClassicMixin_FixedUpdate(this MClassicComicMixin mMixinInterface) { }
        /*
        You can also add Delgegates and so on... BUT YOU CANNOT USE COROUTINES! They don't yield properly. I haven't researched why, but its possible they are not threadsafe due to deadlocks or some underlying problem with static functions. However if you use Mixin's properly you shouldn't need coroutines and in worst case scenario write a separate class for those in the traditional sense for unity and use them with caution.


        For best use add the mixins as their own components in prefabs, such that a prefab can turn the components on and off, or implement its own. Think of prefabs as the Templates, and the TemplateComponents as Component Mixins. 
        */

        private static Fields µ(MClassicComicMixin map)
        {
            return table.GetOrCreateValue(map);

        }

    }
}