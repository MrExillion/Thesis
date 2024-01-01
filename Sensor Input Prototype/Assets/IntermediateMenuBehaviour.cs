using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class IntermediateMenuBehaviour : MonoBehaviour
{
    public UIDocument interMediateMenu;

    private void Awake()
    {
        if(interMediateMenu == null)
        {
            interMediateMenu = GetComponent<UIDocument>();
        }

    }

    private void OnEnable()
    {


        BindIntermediateBehaviour();

    }

    private IEnumerator<Object> BindIntermediateBehaviour()
    {
        string newtext = "\r\n\r\n\r\nTak for Deres evaluering af første halvdel.\r\nInden De fortsætter minder jeg Dem om følgende: De til enhver tid kan stoppe næste halvdel, hvis De mister interessen således, De under normale omstændigheder ville have lagt appen fra Dem, dog bør De se bort fra Deres generelle interesse i tegneserier.\r\n\r\nDe Kan gøre dette ved at lægge 5 fingre på skærmen og fjerne dem igen.\r\n\r\nAppen vil hefter Spørge Dem om hvorfor De har valgt at stoppe, hvor du blot skal vælge 5 ting på skærmen.\r\nDet det vil ligne det de har lige har set. Vær opmærksom på: om De svarer på årsagen til at stoppe eller fortsætte.\r\n";
        
        var root = interMediateMenu.rootVisualElement;
        var infoText = root.Q<Label>("InfoText");
        var startClassicComicBtn = root.Q<Button>("GoToClassicComicButton");
        var startInteractiveComicBtn = root.Q<Button>("GoToInteractiveComicButton");
        var nextTextBtn = root.Q<Button>("NaesteTekstKnap");
        if(nextTextBtn != null)
        {
            nextTextBtn.clickable.clicked += () => {

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


        if (startInteractiveComicBtn != null && DataAcquisition.Singleton.timeAtClassicLoad > 0)
        {
            startClassicComicBtn.SetEnabled(false);
            startClassicComicBtn.visible = false;

            startInteractiveComicBtn.clickable.clicked += () =>
            {
                SceneManager.LoadScene("ComicBook");
                DataAcquisition.Singleton.timeAtInteractiveLoad = Time.realtimeSinceStartup;
            };
            startInteractiveComicBtn.SetEnabled(false);
        }
        else if (startClassicComicBtn != null && DataAcquisition.Singleton.timeAtInteractiveLoad > 0)
        {
            startInteractiveComicBtn.SetEnabled(false);
            startInteractiveComicBtn.visible = false;

            startClassicComicBtn.clickable.clicked += () =>
            {
                SceneManager.LoadScene("ClassicComicBook");
                DataAcquisition.Singleton.timeAtClassicLoad = Time.realtimeSinceStartup;

            };
            startClassicComicBtn.SetEnabled(false);
        }

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
                header.text += " "+(3+ DataAcquisition.Singleton.numberOfPreviousRespondents);
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
