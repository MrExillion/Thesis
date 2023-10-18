using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RadarMixin : MonoBehaviour, MRadar
{
    [SerializeField] private GameObject signalObject;
    private int numTouches = 0;
    private void Awake()
    {
       
    }

    // Start is called before the first frame update
    private void Start()
    {
        RadarTemplate.radar1.SetSignalObject(signalObject);
        //Console.WriteLine("Name {0}, Age = {1}", h.Name, h.GetAge());
        //Console.ReadKey();

    }

    // Update is called once per frame
    void Update()
    {

       // touchCount is the amount of touches registered on the screen so 1 = 1 finger, 2 = 2 fingers etc.
        if(Input.touchCount > numTouches ) // i need to add a cd timer
        {
            Touch touch = Input.GetTouch(0);
            if (TouchPhase.Began == touch.phase && (touch.deltaTime >= 0.5f + Time.deltaTime || touch.deltaTime == 0))
            {
                RadarTemplate.radar1.SendBroadcast(gameObject);
                //numTouches += 1;
                Debug.Log(touch.deltaTime + ", " + Time.deltaTime);
            }
        }
       // Debug.Log("Number of touches registered" + Input.touchCount);

    }
}
