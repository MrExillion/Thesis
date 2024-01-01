using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class FinalSurveyUIBehaviour : MonoBehaviour
{
    public UIDocument finalSurveyUI;

    private void Awake()
    {
        if (finalSurveyUI == null)
        {
            finalSurveyUI = GetComponent<UIDocument>();
        }

    }

    private void OnEnable()
    {


        BindFinalSurveyBehaviour();

    }

    private IEnumerator<Object> BindFinalSurveyBehaviour()
    {
        string newtext = "\r\n\r\n\r\nTak for din evaluering af Del 2.\r\nInden vi er helt færdige er der lige et sidste ting, nemlig at lukke appen det kan du gøre ved at klikke \"Afslut Experiment\". Hvis en anden skal teste efter dig skal du klikke på \"Klar til Næste\"";

        var root = finalSurveyUI.rootVisualElement;
        var infoText = root.Q<Label>("InfoTekst");
        var startClassicComicBtn = root.Q<Button>("GoToClassicComicButton");
        var startInteractiveComicBtn = root.Q<Button>("GoToInteractiveComicButton");
        startClassicComicBtn.text = "Aflust Experiment";
        startInteractiveComicBtn.text = "Klar til Næste";
        var nextTextBtn = root.Q<Button>("NaesteTekstKnap");
        if (nextTextBtn != null)
        {
            nextTextBtn.clickable.clicked += () => {
                //var infoText = root.Q<Label>("InfoTekst");
                infoText.text += newtext;
                nextTextBtn.SetEnabled(false);
                nextTextBtn.visible = false;
                var btn = root.Q<Button>("GoToInteractiveComicButton");
                if (btn != null)
                {
                    if (btn.visible == true)
                    {
                        btn.SetEnabled(true);
                    }
                }
                var btn2 = root.Q<Button>("GoToClassicComicButton");
                if (btn2 != null)
                {
                    if (btn2.visible == true)
                    {
                        btn2.SetEnabled(true);
                    }
                }



            };

        }


        //if (startInteractiveComicBtn != null && DataAcquisition.Singleton.timeAtClassicLoad > 0)
        //{
            //startClassicComicBtn.SetEnabled(false);
            //startClassicComicBtn.visible = false;

            startInteractiveComicBtn.clickable.clicked += () =>
            {

                DataAcquisition.Singleton.EndExperiment();
                DataAcquisition.Singleton.DumpData();
                GameObject.Destroy(DataAcquisition.Singleton.gameObject);
                if(DataAcquisition.Singleton != null) {
                    DataAcquisition.Singleton = null;
                }
                
                SceneManager.LoadScene("MainMenu");

            };
            //startInteractiveComicBtn.SetEnabled(false);
        //}
        //else if (startClassicComicBtn != null && DataAcquisition.Singleton.timeAtInteractiveLoad > 0)
        //{
            //startInteractiveComicBtn.SetEnabled(false);
            //startInteractiveComicBtn.visible = false;

            startClassicComicBtn.clickable.clicked += () =>
            {
                DataAcquisition.Singleton.EndExperiment();
                DataAcquisition.Singleton.DumpData();
                Application.Quit();

            };
            //startClassicComicBtn.SetEnabled(false);
        //}

        var bugfixinstructions = root.Q<Label>("BugFixInstruksioner");

        if (bugfixinstructions != null && DataAcquisition.Singleton.bugsfixed)
        {
            bugfixinstructions.visible = false;

        }

        var header = root.Q<Label>("IntermediateMenuHeaderText");
        if (header != null)
        {
            if (DataAcquisition.Singleton.timeAtClassicLoad > 0 && DataAcquisition.Singleton.timeAtInteractiveLoad > 0)
            {
                header.text += " " + (3 + DataAcquisition.Singleton.numberOfPreviousRespondents);
            }
            else if (DataAcquisition.Singleton.timeAtInteractiveLoad > 0)
            {
                header.text += "1";
            }
            else if (DataAcquisition.Singleton.timeAtClassicLoad > 0)
            {
                header.text += "2";
            }

        }

        return null;
    }

}
