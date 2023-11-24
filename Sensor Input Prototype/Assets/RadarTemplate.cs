using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorInputPrototype.InspectorReadOnlyCode;
using SensorInputPrototype.MixinInterfaces;
public class RadarTemplate : InteractionType, MRadar
{

    public static RadarTemplate radar1;

    private void Awake()
    {
        if (radar1 == null)
        {
                  radar1 = this;
        }
        else
        { 
            return;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
