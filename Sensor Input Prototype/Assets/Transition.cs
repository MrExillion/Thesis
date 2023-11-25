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
    public delegate void TransitionCB();


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
            // just used to update canTransition
            canTransition = (GlobalReferenceManager.MixinPairs.Find(x => x.Item1 == GlobalReferenceManager.GetActiveComicTemplate().GetInstanceID()).Item2 as ComicManagerMixin).CanTransition(); // In theory it should look at the fields associated with the implementation inside the extension, but this might be incorrect.



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
                    
                    cB?.Invoke();
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

}

