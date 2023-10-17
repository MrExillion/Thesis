using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RadarMixin : MonoBehaviour, MRadar
{
    [SerializeField] private GameObject signalObject;
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

        Touch touch = Input.GetTouch(0);
        if(Input.touchCount > 0)
        {

            RadarTemplate.radar1.SendBroadcast(gameObject);
        }


    }
}
