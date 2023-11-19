using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HRotateTemplate : InteractionType, MHRotate, MMixinTemplates
{

   
    private void Awake()
    {
        SetTemplateId();
    }
    

}