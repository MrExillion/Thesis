using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Linq;
using Toggle = UnityEngine.UIElements.Toggle;


public class EMM_UIScript : MonoBehaviour
{

    public UIDocument DEMMUIDoc;
    private bool selections = false;
    private bool[] bools = new bool[54];
    List<Toggle> choicesList = new List<Toggle>();

    private void OnEnable()
    {
        BindDEMMUI();
    }


    private IEnumerable<Object> BindDEMMUI()
    {
        var root = DEMMUIDoc.rootVisualElement;




        for (int i = 0; i < 54; i++)
        {
            choicesList.Add(root.Q<Toggle>("Toggle" + (i + 1)));
            //root.Q<Toggle>("Toggle" + (i+1), "CheckBoxCustom").(choicesList[i]);

        }
        var confirmBtn = root.Q<Button>("ConfirmChoices");

        if (confirmBtn != null)
        {
            confirmBtn.clickable.clicked += () =>
                {
                    if (selections)
                    {
                        string scenename = SceneManager.GetActiveScene().name;
                        foreach (var choice in choicesList)
                        {
                            if (choice.value == true)
                            {
                                if (scenename == "DisengagementScene")
                                {
                                    DataAcquisition.Singleton.disengagementReactionCards += choice.name + ", ";
                                }
                                else if (scenename == "EngagementScene")
                                {
                                    DataAcquisition.Singleton.engagementMappingReactionCompletionCards += choice.name + ", ";
                                }

                            }
                        }

                        if ((DataAcquisition.Singleton.timeAtClassicLoad >= 0 || DataAcquisition.Singleton.timeAtInteractiveLoad >= 0) && !(DataAcquisition.Singleton.timeAtClassicLoad >= 0 && DataAcquisition.Singleton.timeAtInteractiveLoad >= 0)) //XOR 
                        {
                            SceneManager.LoadScene("IntermediateScene");
                        }
                        else
                        {
                            SceneManager.LoadScene("FinalSurveyScene");
                        }

                        //
                        //
                    }
                };
        }
        return null;
    }

    private void Update()
    {
        int count = 0;
        int[] index = new int[5];

        for (int i = 0; i < choicesList.Count; i++)
        {
            if (count == 5)
            {

                for (int j = 0; j < choicesList.Count; j++)
                {
                    if (!index.Contains(j))
                    {
                        choicesList[j].SetEnabled(false);
                    }

                }
                break;


            }
            else
            {
                for (int j = 0; j < choicesList.Count; j++)
                {
                    if (!index.Contains(j))
                    {
                        choicesList[j].SetEnabled(true);
                        if (selections == true) // just here to prevent the call happening over and over... its actually pointless it might even be more expensive to compare five times than to access the heap.
                        {
                            selections = false;

                        }

                    }


                }
            }


            bools[i] = (choicesList[i].value);

            if (true == bools[i])
            {
                index[count] = i;
                count++;
            }



        }

        //foreach (bool bl in bools)
        //{

        //    if (bl)
        //    {
        //        count++;
        //       index[count-1] = bools.ToList().FindAll(x=> x.value)IndexOf(bl);
        //    }

        //}
        if (count == 5 && selections == false)
        {
            selections = true;
            string strout = "";
            for (int i = 0; i < 5; i++)
            {
                strout += (string)(i + choicesList[index[i]].text + "\n");
            }
            Debug.Log("5 selections:\n" + strout);
        }

    }

}


