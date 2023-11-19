using UnityEngine;
using SensorInputPrototype.InspectorReadOnlyCode;

[RequireComponent(typeof(UniversalPanel))]
public class Templates : MonoBehaviour, ITemplate
{
    #if UNITY_EDITOR 
    [ShowOnly] 
    #endif
    [SerializeField] 
    private int readOnlyTemplateId = 0;
    #if UNITY_EDITOR
    [ShowOnly] 
    #endif
    [SerializeField]
    protected int templateId = 0;



    protected void SetTemplateId()
    {
        templateId = GetComponent<UniversalPanel>().PanelId;
        
    }

    private void Awake()
    {


        
    }


    private void Update()
    {
        readOnlyTemplateId = templateId;
    }
}