using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace SensorInputPrototype.Ui
{
    public class ChoiceUIBehaviour : MonoBehaviour
    {

        public UIDocument choiceUI;

        private void Awake()
        {
            if (choiceUI == null)
            {
                choiceUI = GetComponent<UIDocument>();
            }

        }

        private void OnEnable()
        {
            BindChoiceUIBehaviour();
        }


        private IEnumerator<Object> BindChoiceUIBehaviour()
        {
            var root = choiceUI.rootVisualElement;
            var noBtn = root.Q<Button>("NejKnap");
            var yesBtn = root.Q<Button>("JaKnap");

            if(noBtn != null)
            {
                noBtn.clickable.clicked += () => {
                    SceneManager.LoadScene("DisengagementScene");
                
                };
            }
            if (yesBtn != null)
            {
                yesBtn.clickable.clicked += () => {
                    SceneManager.LoadScene("EngagementScene");

                };
            }


            return null;
        }



    }



}   




