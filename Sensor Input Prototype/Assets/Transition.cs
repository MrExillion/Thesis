using SensorInputPrototype.MixinInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Transition
{

    private static ConditionalWeakTable<MTransition, Fields> table;
    public delegate void TransitionCB(MTransition map);
    public static bool atEndOfPage = false;
    public static bool atEndOfChapter = false;
    public static bool atEndOfComic = false;
   

    static Transition()
    {

        table = new ConditionalWeakTable<MTransition, Fields>();

    }
    private sealed class Fields : MonoBehaviour, MTransition
    {
        internal bool canTransition = false;
        internal bool callBackInProgress = false;
        internal List<TransitionCB> transitionCB = new List<TransitionCB>();

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

            if (canTransition)
            {
                
                if (GlobalReferenceManager.GetActivePanelTemplate().gameObject.GetComponent<UniversalPanel>().PanelId == GlobalReferenceManager.GetActivePanelTemplate().gameObject.GetComponent<PanelManagerTemplate>().panelOrder.Count)
                {
                    Transition.atEndOfPage = canTransition;

                    if (GlobalReferenceManager.GetActivePageTemplate().gameObject.GetComponent<PanelManagerTemplate>().panelId == GlobalReferenceManager.GetActivePageTemplate().gameObject.GetComponent<PageManagerTemplate>().pageOrder.Count)
                    {
                        Transition.atEndOfChapter = canTransition;
                        if (GlobalReferenceManager.GetActiveChapterTemplate().gameObject.GetComponent<PageManagerTemplate>().chapterId == GlobalReferenceManager.GetActivePageTemplate().gameObject.GetComponent<ChapterManagerTemplate>().chapterOrder.Count)
                        {
                            Transition.atEndOfComic = canTransition;
                        }
                    }
                }
            }
            
        }



    }

    public static bool CanTransition(this MTransition map)
    {
        return table.GetOrCreateValue(map).canTransition;

    }
    private static void TransitionCallBack(this MTransition map)
    {
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
            TransitionCallBack(map);
        }


    }
    public static void AddTransitionCallBack(this MTransition map,TransitionCB delegateFunction)
    {
        table.GetOrCreateValue(map).transitionCB.Add(delegateFunction);

    }

    public static void TryTransition(this MTransition map)
    {
        table.GetOrCreateValue(map).canTransition = true;

    }

    public static void TriggerTransition(this MTransition map)
    {
        
        //AddTransitionCallBack(thi)
        table.GetOrCreateValue(map).AddTransitionCallBack(ResetLocalTransitionState);
        //table.GetOrCreateValue(map).canTransition = true;
        map.TryTransition();
    }

    public static void ResetLocalTransitionState(this MTransition map)
    {
        table.GetOrCreateValue(map).canTransition = false;
    }


}

