using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using SensorInputPrototype.InspectorReadOnlyCode;
using SensorInputPrototype.MixinInterfaces;
using UnityEngine.UIElements;





public class TabTransitionTemplateMixin : MonoBehaviour, MTabTransition
    {

#if UNITY_EDITOR
        [ShowOnly]
#endif
        [SerializeField]
        private GameObject signalObject;
#if UNITY_EDITOR
        [ShowOnly]
#endif
        [SerializeField]
        int numTouches = 0;
        //int touchTransitionType = 0;
        private UniversalPanel universalPanel;
        private Ray ray;

        public List<GameObject> targetList;
        private void Awake()
        {

        }

        // Start is called before the first frame update
        private void Start()
        {
        //RadarTemplate.radar1.SetSignalObject(signalObject);
        //Console.WriteLine("Name {0}, Age = {1}", h.Name, h.GetAge());
        //Console.ReadKey();
        universalPanel = GetComponent<UniversalPanel>();
        }

    // Update is called once per frame
    void Update()
    {

        if (GlobalReferenceManager.GetCurrentUniversalPanel() != universalPanel)
        {
            return;
        }

        // touchCount is the amount of touches registered on the screen so 1 = 1 finger, 2 = 2 fingers etc.
        if (Input.touchCount > numTouches) // i need to add a cd timer
        {
            Touch touch = Input.GetTouch(0);

            if (Input.touchCount == 1) // i need to add a cd timer
            {
                RaycastHit hit;
                if (TouchPhase.Began == touch.phase && (touch.deltaTime >= 0.5f + Time.deltaTime || touch.deltaTime == 0))
                {
                    this.OneFingerTouchTab(touch.position.x, touch.position.y);

                    //numTouches += 1;
                    Debug.Log(touch.deltaTime + ", " + Time.deltaTime);

                    Vector3 rayOrigin = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x,touch.position.y,0f));
                    ray = new Ray(rayOrigin, Camera.main.transform.forward); // this could be a problem if it wasn't certain that ray wouldn't be null ever with a touch count at == 1;
                    if( (int)universalPanel.transitionType == 1)
                    {
                        GlobalReferenceManager.GetCurrentUniversalPanel().TriggerTransition();
                        this.OneFingerTouchTab(0, 0);
                        return;
                    }

                }
                if (TouchPhase.Moved == touch.phase && (int)universalPanel.transitionType == 3 &&
                    Vector2.Distance(touch.position, this.GetTouchCoords()) > Screen.currentResolution.width / 10 &&
                    this.GetTouchCoords() != Vector2.zero) // swipe distance travel > 1/10th screen width, and saved vector isn't a zero-vector.
                {
                    GlobalReferenceManager.GetCurrentUniversalPanel().TriggerTransition();
                    this.OneFingerTouchTab(0, 0);
                }
                else if (TouchPhase.Began == touch.phase && (int)universalPanel.transitionType == 4) //Drag object
                {
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (targetList.Contains(hit.collider.gameObject))
                        {
                            this.CursorItemSet(hit.collider.gameObject);
                        }
                    }

                }

                if (TouchPhase.Began == touch.phase && (int)universalPanel.transitionType == 2 &&
                    (touch.deltaTime >= 0.5f + Time.deltaTime || touch.deltaTime == 0)) // touch and hit object
                {
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (targetList.Contains(hit.collider.gameObject))
                        {
                            GlobalReferenceManager.GetCurrentUniversalPanel().TriggerTransition();
                            this.OneFingerTouchTab(0, 0);
                        }


                    }



                }
            }



            // Debug.Log("Number of touches registered" + Input.touchCount);

        }
        else
        {
            //this.ClearCursorItem(); // I think this one hurts more than it helps.
        }
    }
 
}
