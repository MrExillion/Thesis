using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EndClassicComicUIBehaviour : MonoBehaviour
{

    public UIDocument endClassicComicUIBehaviour;
    public UniversalPanel finalPanel;

    private void Awake()
    {
        if(endClassicComicUIBehaviour == null)
        {
            endClassicComicUIBehaviour = GetComponent<UIDocument>();
        }

    }


    void OnEnable() 
    {

        BindEndClassicComicUIBehaviour();


    }
    private void Update()
    {
        if (GlobalRefManagerComponent.singleton.rayHitFinalPanel() == true)
        {
            endClassicComicUIBehaviour.rootVisualElement.Q<Button>("EndReadingBtn").visible = true;
            endClassicComicUIBehaviour.rootVisualElement.Q<Button>("EndReadingBtn").SetEnabled(true);
        }
    }
    private IEnumerator<Object> BindEndClassicComicUIBehaviour()
    {

        var root = endClassicComicUIBehaviour.rootVisualElement;

        var btn = root.Q<Button>("EndReadingBtn");

        if(btn != null)
        {

            btn.clickable.clicked += () => 
            {

                //finalPanel.TriggerTransition(); // this is never used by the camera sequencer, i should just load the next scene.
                DataAcquisition.Singleton.EndClassic();
                UnityEngine.SceneManagement.SceneManager.LoadScene("Choice");
            };

        }

        btn.visible = false;
        btn.SetEnabled(false);
        return null;
    }


}
