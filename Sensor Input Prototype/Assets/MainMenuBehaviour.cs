using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace SensorInputPrototype.Ui
{ 

    public class MainMenuBehaviour : MonoBehaviour
    {
        public UIDocument MainMenu;

        private void OnEnable()
        {
            BindMainMenu();
        }


        private IEnumerable<Object> BindMainMenu()
        {
            var root = MainMenu.rootVisualElement;
            var startClassicComicBtn = root.Q<Button>("GoToClassicComicButton");
            var startInteractiveComicBtn = root.Q<Button>("GoToInteractiveComicButton");
            if(startInteractiveComicBtn != null)
            {
                startInteractiveComicBtn.clickable.clicked += () =>
                {
                    SceneManager.LoadScene("ComicBook");
                    DataAcquisition.Singleton.timeAtInteractiveLoad = Time.realtimeSinceStartup;
                };
            }
            if(startClassicComicBtn != null)
            {
                startClassicComicBtn.clickable.clicked += () =>
                {
                    SceneManager.LoadScene("ClassicComicBook");
                    DataAcquisition.Singleton.timeAtClassicLoad = Time.realtimeSinceStartup;

                };
            }
            return null;
        }
    }




   // public class MainMenuBehaviour : VisualElement
   // {
   //     [UnityEngine.Scripting.Preserve]
   //     public new class UxmlFactory : UxmlFactory<MainMenuBehaviour> { }

   //     //private const string 
   //     Button InteractiveComicButton;
   //     public MainMenuBehaviour()
   //     {
            
   //     }


   //}
   //public class MainMenuButton : Button
   // {
   //     [Preserve]
   //     public new class UxmlFactory : UxmlFactory<MainMenuButton> { }

   //     public MainMenuButton()
   //     {
   //         this.clicked += this.MainMenuButton_onClick; 
   //         //GoToInteractiveComicButton

   //     }

   //     private void MainMenuButton_onClick(this MainMenuButton b)
   //     {
   //        if(b.ElementAt(IndexOf(UxmlFactory<Button>("GoToInteractiveComicButton")))
   //     }
   // }



}