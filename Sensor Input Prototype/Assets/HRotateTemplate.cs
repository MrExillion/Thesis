using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorInputPrototype.MixinInterfaces;
public class HRotateTemplate : InteractionType, MHRotate, MMixinTemplates
{

   
    private void Awake()
    {
        SetTemplateId();
    }
    

}