using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorInputPrototype.MixinInterfaces;
using System;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
using Unity.Android.Types;
using UnityEditor;
using System.ComponentModel;

public class GlobalRefManagerComponent : MonoBehaviour, IGlobalReferenceManager
{

    [SerializeField] public Array[] Comics = { };
    private List<GameObject> comics;

    public Array[][] Chapters = { };
    private List<GameObject> chapter;

    public Array[][] Pages = {};
    private List<GameObject> pages;

    public Array[][] Panels = { };
    private List<GameObject> panels;
    public GameObject[] hierarchy= {};
    //public List<GameObject[][][][]> hierachy2;


    //public List<List<GameObject>> hhh;
    public GameObject comicContainer;
    public static ComicManagerTemplate comicTemplate;
   
    //[SerializeField]public ComicList ComicList = new ComicList();
    private void Awake()
    {

        //I dont actually think these clases are needed for static classes with interfaces to be accessible?
    }

    private void OnEnable()
    {
        comicTemplate = GameObject.FindAnyObjectByType<ComicManagerTemplate>();
        comicContainer = comicContainer.gameObject;
        

        comics = comicTemplate.comicsList;
        foreach (GameObject go in comicTemplate.comicsList)
        {
            chapter = go.GetComponent<ChapterManagerTemplate>().chapterOrder;
            foreach (GameObject go2 in go.GetComponent<ChapterManagerTemplate>().chapterOrder)
            {
                pages = go2.GetComponent<PageManagerTemplate>().pageOrder;
                foreach (GameObject go3 in go2.GetComponent<PageManagerTemplate>().pageOrder)
                {
                    panels = go3.GetComponent<PanelManagerTemplate>().panelOrder;
                    foreach (GameObject go4 in panels)
                    {
                        //Comics.SetValue(go,Chapters.SetValue(go2,Pages.SetValue(go3,Panels.SetValue(go4, panels.IndexOf(go4)), pages.IndexOf(go3)), chapter.IndexOf(go2)), comics.IndexOf(go)));
                        //Panels.SetValue(go4, panels.IndexOf(go4));

                        //hirearchy[comics, comics.IndexOf(go)]SetValue(chapter,)[chapter.IndexOf(go2)][pages.IndexOf(go3)][panels.IndexOf(go4)] = go4;
                        hierarchy.SetValue(go4, (comics.IndexOf(go)) * (chapter.IndexOf(go2)) * (pages.IndexOf(go3)) * (panels.IndexOf(go4)));

                    }
                    
                    //Pages.SetValue(Panels, pages.IndexOf(go3));
                }
                //Chapters.SetValue(Pages, chapter.IndexOf(go2));
            }
            //Comics.SetValue(Chapters, comics.IndexOf(go));
            
        }
    }

}
