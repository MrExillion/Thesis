using SensorInputPrototype.MixinInterfaces;
using System.Collections;
using UnityEngine;



    public class DragAndDrop : MonoBehaviour, MTabTransition
{
    private UniversalPanel universalPanel;
    private TabTransitionTemplateMixin transitionTemplateMixin;
    public GameObject dropTarget;
   
        // Use this for initialization
        void Start()
    {
        transitionTemplateMixin = GetComponent<TabTransitionTemplateMixin>();
        universalPanel = GetComponent<UniversalPanel>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalReferenceManager.GetCurrentUniversalPanel() != universalPanel)
        {
            return;
        }


        if (Input.touchCount == 1 && transitionTemplateMixin.CursorHasItem())
        {
            Touch touch = Input.GetTouch(0);
            Vector3 tempV3pseudo = new Vector3(touch.position.x, touch.position.y, 0f); // Form Vector3 from Vector2 by adding z = 0, as z component.
            Vector3 tempV3 = Camera.main.ScreenToWorldPoint(tempV3pseudo); // Use the Pseudo Vector3 crated to specify that the distance in world space from the screen is 0, and get world coordiantes for the screen space under the touch point. 
            tempV3.z = transitionTemplateMixin.GetCursorItem().transform.position.z; // use the temporary vector which still has pseudo z as a component and then swap the pseudo 0 with the z component of the plane of item in the cursor. 
            transitionTemplateMixin.GetCursorItem().transform.position = tempV3; // use the temporary now REAL vector3 to move the cursor item in X and Y matching the Touch Point.
        }

        if (Input.touchCount == 0) // If last frame had 1 finger touch and now has ended. check for cursor item.
        {
            Debug.Log("Entered DragAndDrop Update() If Statement: TouchCount 0,touches.Len 1, touches[0].phase Ended   => True", gameObject);
            if (transitionTemplateMixin.CursorHasItem())
            {
                Vector2 vec2CI = new Vector2(transitionTemplateMixin.GetCursorItem().transform.position.x, transitionTemplateMixin.GetCursorItem().transform.position.y);
                Vector2 vec2DT = new Vector2(dropTarget.transform.position.x, dropTarget.transform.position.y);

                Debug.Log("Distance: " + Vector2.Distance(vec2CI, vec2DT) + ", CursorItem: " + vec2CI + " TargetItem: " + vec2DT);
                if (Vector2.Distance(vec2CI,vec2DT) <= 0.5) // If the distance between the 2D vectors i.e. ignoring the Z space component in world space, if said distance is less than or equals to 0.25 world units then TRUE... ==> This means that the condition to Trigger TriggerTransition(); is met. 0.25 is based on scale 1 means that 0.5 for both objects center if both are scale 1 should mean they touch. The selection 0.25 is to allow for sub 1 scaled items to get reasonably close to each other without the use of broadphase collision detection or any other physics as the objects physically exist in different depth planes, but are projected in Orthographic view, to appear to exist in 2D. This is a choice made for stillistic reasons and solving a compositing task in the engine, due to limitations of AI generated arts and consistency in terms of interactive props and characters from multiple angles and in interactive scenarios. DO NOT CHANGE THIS APPROACH, OR CIRCUMVENT WITH DEPTH BASED COLLISIONS! IT IS MORE EXPENSIVE, AND DOES NOT WORK WELL WITH UNITYS APPLIANCE OF 2 DIFFERENT PHYSICS ENGINES THAT ALSO CYCLES OUTSIDE THE NORMAL UPDATE DELEGATION USING FIXED UPDATE; YOU MAY THINK THIS WILL SAVE YOU TIME, THIS IS A MISTAKE I PERSONALLY MADE SEVERAL YEARS AGO OR RATHER WAS PART OF MAKING SEVERAL YEARS AGO WHEN MAKING THE GAME JAM GAME CRISPY CHRISTMAS YOU CAN CURRENTLY(DEC-23-2023) ON ZURICHO.DK/GAMES! HEED THIS WARNING! THIS IS THE EASIEST SOLUTION!
                {
                    //transitionTemplateMixin.GetCursorItem().transform.position = transitionTemplateMixin.GetCursorItemOrigin(); // handled by clearCursor now.
                    
                    GlobalReferenceManager.GetCurrentUniversalPanel().TriggerTransition();
                }
                //transitionTemplateMixin.GetCursorItem().transform.position = transitionTemplateMixin.GetCursorItemOrigin();  // handled by clearCursor now.
                transitionTemplateMixin.ClearCursorItem(); // Clears the cursor and restores the item to its original position. This can run outside a callback, as it already references a MixinInterfaceMap that persists and isn't relying on void Update from monobehaviour as a delegate.
            }
            
        }


    }

}
