using SensorInputPrototype.MixinInterfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public static class Transition
{

    private static ConditionalWeakTable<MTransition, Fields> table;
    public delegate void TransitionCB(MTransition map);
    public delegate void TransitionCallBack(MTransition map);

    public delegate bool TransitionResetCondition(UniversalPanel panel);

    public static bool atEndOfPage = false;
    public static bool atEndOfChapter = false;
    public static bool atEndOfComic = false;
    public static bool canTransition = false;
    public static bool transitionInProgress = false;

    public enum transitionTypes { HRotate = 0, SingleTap = 1, CoverLightSensor = 2 };

    static Transition()
    {

        table = new ConditionalWeakTable<MTransition, Fields>();
        //TransitionCallBack callBack = CallBack;
        // TransitionCallBack callBack = CallBack;

    }
    private sealed class Fields : MonoBehaviour, MTransition
    {
        internal bool canTransition = false;
        internal bool callBackInProgress = false;
        internal List<TransitionCB> transitionCB = new List<TransitionCB>();
        public static readonly TransitionCallBack callBack = CallBack;
        public static readonly TransitionResetCondition transitionResetCondition = UniversalPanel.ResetConditions;

        void Update()
        {
            //    // just used to update canTransition
            //    canTransition = (GlobalReferenceManager.MixinPairs.Find(x => x.Item1 == GlobalReferenceManager.GetActiveComicTemplate().GetInstanceID()).Item2 as ComicManagerMixin).CanTransition(); // In theory it should look at the fields associated with the implementation inside the extension, but this might be incorrect.

            /*
                First check if the panel can transition or not.
            
                    then if it can, check if at the end of page
                        if yes then check if at the end of chapter
                            if yes then check if at the end of comic
                                if yes then transition to next comic,
                                else
                                    transition to next chapter
                            else
                                transition to next page
                       else
                            transition to next panel
                .... in this case we just want to set the bools.
            then the CanTransition will simply set the bools in the GlobalReferenceManager or something
                
             
             */




        }



    }

    public static bool CanTransition(this MTransition map)
    {
        return table.GetOrCreateValue(map).canTransition;
        //return Transition.canTransition;

    }
    public static void ResetCanTransition(this MTransition map)
    {
        table.GetOrCreateValue(map).canTransition = false;

    }

    private static void TransitionCallBackOldMethod(this MTransition map)
    {
        // original plan with this was something akin to hooksecurefunc in World Of Warcraft LUA scripting, I haven't tested if it could work like this.

        try
        {
            foreach (TransitionCB cB in table.GetOrCreateValue(map).transitionCB)
            {
                try
                {

                    cB?.Invoke(map);
                }
                catch
                {
                    Debug.LogAssertion("Failed Callback: " + nameof(cB));
                    continue;
                }

            }
        }
        catch
        {
            Debug.LogAssertion("Failed Callback all Transition Callbacks related to:", table.GetOrCreateValue(map));

        }
        finally
        {
            table.GetOrCreateValue(map).callBackInProgress = false;
        }


    }
    public static void Next(this MTransition map)
    {
        if (table.GetOrCreateValue(map).canTransition)
        {
            TransitionCallBackOldMethod(map);
        }


    }
    public static void AddTransitionCallBack(this MTransition map, TransitionCB delegateFunction)
    {
        table.GetOrCreateValue(map).transitionCB.Add(delegateFunction);

    }

    public static void TryTransition()
    {
        //GlobalReferenceManager.GetCurrentUniversalPanel().CheckTransitionType();
        
        Debug.Log("TryTansition() Executed");

    }

    public static void TriggerTransition(this MTransition map)
    {

        //Transition.TryTransition();
        
        bool testBool = table.Any(x => x.Value.callBackInProgress == true);
        Debug.Log("callBackInProgress evaluated to " + testBool);
        if (!(table.Any(x => x.Value.callBackInProgress == true)) ) // a double check to avoid multiple instances of a transition running at the same time.
        {
            table.GetOrCreateValue(map).canTransition = true;

            // this line below should prompt the table.Any to return true, which must return false for the line below to execute, thus checking for a Transition to already be in progress.
            table.GetOrCreateValue(map).callBackInProgress = true;
            Transition.transitionInProgress = true;


            OnTransitionTrigger(map);

            //UniversalPanel panel = GlobalReferenceManager.GetCurrentUniversalPanel();
            //GlobalReferenceManager.GetCurrentUniversalPanel().StartCoroutine(TransitionCooldown(GlobalReferenceManager.GetCurrentUniversalPanel(), map)); // blocks the transition until the trigger condition is unmade
            //while (Fields.transitionResetCondition.Invoke(panel) == false)
            //{
            ////Transition.transitionInProgress = true;

            //}


            ResetLocalTransitionState(map);
            Transition.Fields.callBack.Invoke(map); // making the callback single instance is full of IDK if this is possible, but this way is an attempt. Unlike the above i need to know this globally across all implementations, nut just in a specific instance. so i cannot use any to find the callback.

            
            
            
        }

    }

    public static void ResetLocalTransitionState(MTransition map)
    {
        //table.GetOrCreateValue(map).canTransition = false;
        Transition.canTransition = false;
        Transition.transitionInProgress = false;
        //Transition.transitionInProgress = false;
        //table.GetOrCreateValue(map).callBackInProgress = false;
        Debug.Log("ResetLocalTransitionState() Executed");

    }

    public static void OnTransitionTrigger(MTransition map)
    {
        Debug.Log("OnTransitionTrigger(), entered execution with map:" + map.ToString());
        bool canTransition = table.GetOrCreateValue(map).canTransition;
        Transition.canTransition = canTransition;
        Debug.Log("Transition.canTransition = " + canTransition.ToString() + ", table.GetOrCreateValue(map).canTransition is source.");
        if (canTransition)
        {
            Debug.Log("GlobalReferenceManager.GetCurrentUniversalPanel().PanelId == GlobalReferenceManager.GetActivePanelTemplate().gameObject.GetComponent<PanelManagerTemplate>().panelOrder.Count -1  :  " + (GlobalReferenceManager.GetCurrentUniversalPanel().PanelId == GlobalReferenceManager.GetActivePanelTemplate().gameObject.GetComponent<PanelManagerTemplate>().panelOrder.Count - 1));


            if (GlobalReferenceManager.GetCurrentUniversalPanel().PanelId == GlobalReferenceManager.GetActivePanelTemplate().gameObject.GetComponent<PanelManagerTemplate>().panelOrder.Count - 1)
            {

                Transition.atEndOfPage = true;
                Debug.Log("(GlobalReferenceManager.GetActivePanelTemplate() as PanelManagerTemplate).pageId == GlobalReferenceManager.GetActivePageTemplate().gameObject.GetComponent<PageManagerTemplate>().pageOrder.Count -1   :   " + ((GlobalReferenceManager.GetActivePanelTemplate() as PanelManagerTemplate).pageId == GlobalReferenceManager.GetActivePageTemplate().gameObject.GetComponent<PageManagerTemplate>().pageOrder.Count - 1));

                if ((GlobalReferenceManager.GetActivePanelTemplate() as PanelManagerTemplate).pageId == GlobalReferenceManager.GetActivePageTemplate().gameObject.GetComponent<PageManagerTemplate>().pageOrder.Count - 1)
                {
                    Transition.atEndOfChapter = true;
                    Debug.Log("(GlobalReferenceManager.GetActivePageTemplate() as PageManagerTemplate).chapterId == GlobalReferenceManager.GetActiveChapterTemplate().gameObject.GetComponent<ChapterManagerTemplate>().chapterOrder.Count -1   :    " + ((GlobalReferenceManager.GetActivePageTemplate() as PageManagerTemplate).chapterId == GlobalReferenceManager.GetActiveChapterTemplate().gameObject.GetComponent<ChapterManagerTemplate>().chapterOrder.Count - 1));
                    if ((GlobalReferenceManager.GetActivePageTemplate() as PageManagerTemplate).chapterId == GlobalReferenceManager.GetActiveChapterTemplate().gameObject.GetComponent<ChapterManagerTemplate>().chapterOrder.Count - 1)
                    {
                        Transition.atEndOfComic = true;
                    }
                    else
                    {
                        Transition.atEndOfComic = false;
                    }
                }
                else
                {
                    Transition.atEndOfChapter = false;
                }
            }
            else
            {
                Transition.atEndOfPage = false;
            }

        }

        Debug.Log("OnTransitionTrigger() executed");
    }
    public static bool CheckTransitionType(this MTransition map)
    {
        table.GetOrCreateValue(map).canTransition = true;
        return table.GetOrCreateValue(map).canTransition;
        //int nextInLineTransitionType = table.GetOrCreateValue(map).transitionToDo;

        int nextInLineTransitionType = (int)GlobalReferenceManager.GetCurrentUniversalPanel().transitionType;
        //int currentPanel = table.GetOrCreateValue(map).currentPanel;
        //Debug.Log("TransitionType: " + nextInLineTransitionType + ",\tgameObject: " + map + ",\tPanelId: " + currentPanel, table.GetOrCreateValue(map).gameObject);
        switch (nextInLineTransitionType)
        {
            case (int)Transition.transitionTypes.HRotate:
                //if(gameObject.GetComponent<HRotateMixin>() == null)
                //{
                //Debug.Assert(GetComponent<HRotateMixin>() == null, "Universal Panel, with ID: " + gameObject.GetComponent<UniversalPanel>().PanelId.ToString() + ", has evaluated: \"GetComponent<HRotateMixin>() == null\" ",gameObject.GetComponent<UniversalPanel>());
                // goto default;
                //}
                // else
                // {
                //Debug.Log("CanTransition:\t"+GetComponent<HRotateMixin>().canTransition+",\t"+this.PanelId);
                // try { //return map.GetAllOfSameType(map, false)[currentPanel].GetComponent<HRotateMixin>().canTransition; }
                //  catch
                //   {
                //Debug.LogError("Somthing bad happened", gameObject);
                //       return false;
                //   }
                // }

                table.GetOrCreateValue(map).canTransition = GlobalReferenceManager.GetCurrentUniversalPanel().GetComponent<HRotateMixin>().canTransition;
                Debug.Log("CheckTransitionType() Exectued returned: " + table.GetOrCreateValue(map).canTransition);
                return table.GetOrCreateValue(map).canTransition;

            case 1:


            case 2:

            case 3:

            case 4:

            default:
                Debug.Log("Transition not found, defaulting");
                return false;
        }


    }

    public static void CallBack(MTransition map)
    {
        //CallBacks for TransitionTrigger


        // Add functions below here to be invoked with the CallBack().
        

        // I Think i should add the Set Actives, and itteration updates here if not handled from CameraSequencer
        table.GetOrCreateValue(map).callBackInProgress = false;


    }
    public static IEnumerator TransitionCooldown(UniversalPanel panelParse, MTransition map)
    {
        while (Transition.transitionInProgress)
        {
            if (Fields.transitionResetCondition.Invoke(panelParse))
            {
                Transition.transitionInProgress = false;
                table.GetOrCreateValue(map).callBackInProgress = false;
                

            }
            else
            {
                table.GetOrCreateValue(map).canTransition = false;


            }
            Debug.Log("BeforeYield: " + table.GetOrCreateValue(map).canTransition);
            Debug.Log("BeforeYield Invoked: " + Fields.transitionResetCondition.Invoke(panelParse));

            yield return null;
            //yield return new WaitUntil(() => (bool)Fields.transitionResetCondition?.Invoke(panelParse) == true); // I am not sure i understand/remember this functionality anymore!

        }

        
        Debug.Log("AfterYield: " + table.GetOrCreateValue(map).canTransition);
        Debug.Log("AfterYield Invoked: " + Fields.transitionResetCondition.Invoke(panelParse));

    }






}

