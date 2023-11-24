using UnityEngine;
using SensorInputPrototype.InspectorReadOnlyCode;
using SensorInputPrototype.MixinInterfaces;

//[RequireComponent(typeof(UniversalPanel))]
public class Templates : MonoBehaviour, ITemplate,MComicManager
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

    private int totalPanelNum = 0;

    protected void SetTemplateId()
    {

        if (GetComponent<UniversalPanel>() != null)
        {
            templateId = GetComponent<UniversalPanel>().PanelId;
        }
        else
        {
            if(templateId == 0)
            {
                templateId = totalPanelNum;
            }
            else
            {
                templateId = templateId + 1;
            }
            
        }
    }

    private void Awake()
    {

        totalPanelNum = this.GetNumberOfPanels(this, true);
        
    }


    private void Update()
    {
        readOnlyTemplateId = templateId;
    }
}