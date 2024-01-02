using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorInputPrototype.InspectorReadOnlyCode;

using static System.TimeZoneInfo;
using System.Net.Security;
using SensorInputPrototype.MixinInterfaces;
public class PanelManagerMixin : MonoBehaviour, MPanelManager, IGlobalReferenceManager
{


    //public GameObject[] panelOrder;
    //public int currentPanel = 1;
    //public int nextPanel = 2;
    //public static PanelManagerMixin panelManager;
    //public static PanelManagerTemplate mixin;
    public bool canTransition = false;
    //private static int transitionToDo = 0;

    //private void Awake()
    //{
    //    if (panelManager == null)
    //        panelManager = this;
    //    else
    //        return;
    //}
    //public static ComicManagerTemplate mixin;

    /*
     PanelManagerTemplate Has the following Fields:
        TemplateId (int) inherited from Template.cs > PanelSystemTemplateType.cs accessible as protected in PanelMangerTemplate
            
    public abstract class PanelSystemTemplateType : Templates
    {
        protected int panelId;
            constructor => panelId = base.templateId. This is an incorrect use of templateId.
    }
     
     
     
     */
    

    private void Start()
    {

        



    }

    public void Update()
    {

        //transitionToDo = panelOrder[currentPanel].GetComponent<UniversalPanel>().GetTransitionType(); // this should be a return type function from an interface that handles both panels and pages



         //CheckForTransition();


    }
    //public bool CheckForTransition()
    //{
    //    Debug.Log("TransitionType: " + transitionToDo + ",\tgameObject: " + this.gameObject + ",\tPanelId: " + currentPanel, this);
    //    switch (transitionToDo)
    //    {
    //        case 0:
    //            //if(gameObject.GetComponent<HRotateMixin>() == null)
    //            //{
    //            //Debug.Assert(GetComponent<HRotateMixin>() == null, "Universal Panel, with ID: " + gameObject.GetComponent<UniversalPanel>().PanelId.ToString() + ", has evaluated: \"GetComponent<HRotateMixin>() == null\" ",gameObject.GetComponent<UniversalPanel>());
    //            // goto default;
    //            //}
    //            // else
    //            // {
    //            //Debug.Log("CanTransition:\t"+GetComponent<HRotateMixin>().canTransition+",\t"+this.PanelId);
    //            try { return panelOrder[currentPanel].GetComponent<HRotateMixin>().canTransition; }
    //            catch
    //            {
    //                Debug.LogError("Somthing bad happened", gameObject);
    //                return false;
    //            }
    //        // }


    //        case 1:


    //        case 2:

    //        case 3:

    //        case 4:

    //        default:
    //            Debug.Log("Transition not found, defaulting");
    //            return false;
    //    }
    //}


    //public bool CanTransition()
    //{

    //    return canTransition;
    //}
    public float NextPanelAnchor(string axisLetter)
    {

        //if (Camera.main.GetComponent<CameraSequencer>().GetPanelFocus() + 1 < this.GetNumberOfPanels(this, false))
        if (!Transition.atEndOfPage)
        {


            if (axisLetter == "y")
            {
                float outValue = GetComponent<PanelManagerTemplate>().panelOrder[Camera.main.GetComponent<CameraSequencer>().GetPanelFocus()].transform.position.y;
                (GlobalReferenceManager.MixinPairs.Find(x => x.Item1 == GlobalReferenceManager.GetActiveComicTemplate().GetComponent<ComicManagerMixin>().GetInstanceID()).Item2 as ComicManagerMixin).previousPanel = (GlobalReferenceManager.MixinPairs.Find(x => x.Item1 == GlobalReferenceManager.GetActiveComicTemplate().GetComponent<ComicManagerMixin>().GetInstanceID()).Item2 as ComicManagerMixin).currentPanel;

                (GlobalReferenceManager.MixinPairs.Find(x => x.Item1 == GlobalReferenceManager.GetActiveComicTemplate().GetComponent<ComicManagerMixin>().GetInstanceID()).Item2 as ComicManagerMixin).currentPanel = (GlobalReferenceManager.MixinPairs.Find(x => x.Item1 == GlobalReferenceManager.GetActiveComicTemplate().GetComponent<ComicManagerMixin>().GetInstanceID()).Item2 as ComicManagerMixin).nextPanel;
                if((GlobalReferenceManager.MixinPairs.Find(x => x.Item1 == GlobalReferenceManager.GetActiveComicTemplate().GetComponent<ComicManagerMixin>().GetInstanceID()).Item2 as ComicManagerMixin).nextPanel == GetComponent<PanelManagerTemplate>().panelOrder.Count - 1)
                {
                    (GlobalReferenceManager.MixinPairs.Find(x => x.Item1 == GlobalReferenceManager.GetActiveComicTemplate().GetComponent<ComicManagerMixin>().GetInstanceID()).Item2 as ComicManagerMixin).nextPanel = 0;
                }
                else
                {
                    (GlobalReferenceManager.MixinPairs.Find(x => x.Item1 == GlobalReferenceManager.GetActiveComicTemplate().GetComponent<ComicManagerMixin>().GetInstanceID()).Item2 as ComicManagerMixin).nextPanel += 1;
                }
                



                return outValue;


            }
            else
            {
                Debug.Log("Focus Gained on: "+(Camera.main.GetComponent<CameraSequencer>().GetPanelFocus()));
                float outValue = GetComponent<PanelManagerTemplate>().panelOrder[Camera.main.GetComponent<CameraSequencer>().GetPanelFocus()].transform.position.x;
                
                return outValue;
            }
            //should throw exception here if bad.
        }
        else if (!Transition.atEndOfChapter && !Transition.atEndOfComic)
        {
            if (axisLetter == "x")
            {
                Debug.Log("HELP!");
                GlobalReferenceManager.SetActivePanelContainer((GlobalReferenceManager.GetActivePageTemplate() as PageManagerTemplate).pageOrder[this.GetComponent<PanelManagerTemplate>().pageId + 1].GetComponent<PanelManagerTemplate>());
                Transition.atEndOfPage = false;
            }
                
            return GlobalReferenceManager.GetActivePanelTemplate().GetComponent<PanelManagerMixin>().NextPanelAnchor(axisLetter);
        }
        else if (!Transition.atEndOfComic)
        {
            if (axisLetter == "x")
            {
                GlobalReferenceManager.SetActivePageContainer((GlobalReferenceManager.GetActiveChapterTemplate() as ChapterManagerTemplate).chapterOrder[(GlobalReferenceManager.GetActivePageTemplate() as PageManagerTemplate).chapterId + 1].GetComponent<PageManagerTemplate>());
                GlobalReferenceManager.SetActivePanelContainer((GlobalReferenceManager.GetActivePageTemplate() as PageManagerTemplate).pageOrder[0].GetComponent<PanelManagerTemplate>());
                Transition.atEndOfChapter = false; Transition.atEndOfPage = false;
            }
            
            return GlobalReferenceManager.GetActivePanelTemplate().GetComponent<PanelManagerMixin>().NextPanelAnchor(axisLetter);
        }
        else
        {

            if (DataAcquisition.Singleton.endOfExperiment)
            {
                if (Camera.main.scene.name == "ComicBook")
                {
                    DataAcquisition.Singleton.EndInteractive();
                }
                UnityEngine.SceneManagement.SceneManager.LoadScene("FinalSurveyScene");
                //DataAcquisition.Singleton.EndExperiment();
            }
            else
            {
                if(Camera.main.scene.name == "ComicBook")
                {
                    DataAcquisition.Singleton.EndInteractive();
                }

                UnityEngine.SceneManagement.SceneManager.LoadScene("Choice");
            }


            /*This above will end all comic looping below*/
            if (axisLetter == "x")
            {
                GlobalReferenceManager.SetActiveChapterContainer((GlobalReferenceManager.GetActiveComicTemplate() as ComicManagerTemplate).comicsList[0].GetComponent<ChapterManagerTemplate>());
                GlobalReferenceManager.SetActivePageContainer((GlobalReferenceManager.GetActiveChapterTemplate() as ChapterManagerTemplate).chapterOrder[0].GetComponent<PageManagerTemplate>());
                GlobalReferenceManager.SetActivePanelContainer((GlobalReferenceManager.GetActivePageTemplate() as PageManagerTemplate).pageOrder[0].GetComponent<PanelManagerTemplate>());
                Transition.atEndOfPage = false; Transition.atEndOfChapter = false; Transition.atEndOfComic = false;


            }
            return GlobalReferenceManager.GetActivePanelTemplate().GetComponent<PanelManagerMixin>().NextPanelAnchor(axisLetter);

            //if (axisLetter == "y")
            //{
            //    return GetComponent<PanelManagerTemplate>().panelOrder[0].transform.position.y;
            //}
            //else
            //{
            //    Debug.Log(0);
            //    return GetComponent<PanelManagerTemplate>().panelOrder[0].transform.position.x;
            //}

        }
        Debug.Log((Transition.atEndOfPage && Transition.atEndOfChapter && Transition.atEndOfComic) + ",  ERROR Code should be Unreachable, but was executed: Defaulting Transition to panel 0 in the current context.", this);
        return GetComponent<PanelManagerTemplate>().panelOrder[0].transform.position.x;

    }



}
