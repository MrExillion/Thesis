using UnityEngine.SceneManagement;
using UnityEngine;
using SensorInputPrototype.MixinInterfaces;
public class GlobalRefManagerComponent : MonoBehaviour, IGlobalReferenceManager
{
    // possibly redundant later... but for now do as tooltip says!
    [TooltipAttribute("REQUIRED: Set this to the first and or only comic in the hierarchy")] 
    public GameObject primaryComic;
    [HideInInspector] public static GlobalRefManagerComponent singleton;
    //[SerializeField] public Array[] Comics = { };
    //private List<GameObject> comics;

    //public Array[][] Chapters = { };
    //private List<GameObject> chapter;

    //public Array[][] Pages = {};
    //private List<GameObject> pages;

    //public Array[][] Panels = { };
    //private List<GameObject> panels;
    //public GameObject[] hierarchy= {};
    ////public List<GameObject[][][][]> hierachy2;


    ////public List<List<GameObject>> hhh;
    //public GameObject comicContainer;
    //public static ComicManagerTemplate comicTemplate;

    ////[SerializeField]public ComicList ComicList = new ComicList();
    //private void Awake()
    //{

    //    //I dont actually think these clases are needed for static classes with interfaces to be accessible?
    //}

    //private void OnEnable()
    //{
    //    comicTemplate = GameObject.FindAnyObjectByType<ComicManagerTemplate>();
    //    comicContainer = comicContainer.gameObject;


    //    comics = comicTemplate.comicsList;
    //    foreach (GameObject go in comicTemplate.comicsList)
    //    {
    //        chapter = go.GetComponent<ChapterManagerTemplate>().chapterOrder;
    //        foreach (GameObject go2 in go.GetComponent<ChapterManagerTemplate>().chapterOrder)
    //        {
    //            pages = go2.GetComponent<PageManagerTemplate>().pageOrder;
    //            foreach (GameObject go3 in go2.GetComponent<PageManagerTemplate>().pageOrder)
    //            {
    //                panels = go3.GetComponent<PanelManagerTemplate>().panelOrder;
    //                foreach (GameObject go4 in panels)
    //                {
    //                    //Comics.SetValue(go,Chapters.SetValue(go2,Pages.SetValue(go3,Panels.SetValue(go4, panels.IndexOf(go4)), pages.IndexOf(go3)), chapter.IndexOf(go2)), comics.IndexOf(go)));
    //                    //Panels.SetValue(go4, panels.IndexOf(go4));

    //                    //hirearchy[comics, comics.IndexOf(go)]SetValue(chapter,)[chapter.IndexOf(go2)][pages.IndexOf(go3)][panels.IndexOf(go4)] = go4;
    //                    hierarchy.SetValue(go4, (comics.IndexOf(go)) * (chapter.IndexOf(go2)) * (pages.IndexOf(go3)) * (panels.IndexOf(go4)));

    //                }

    //                //Pages.SetValue(Panels, pages.IndexOf(go3));
    //            }
    //            //Chapters.SetValue(Pages, chapter.IndexOf(go2));
    //        }
    //        //Comics.SetValue(Chapters, comics.IndexOf(go));

    //    }
    //}

    void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
        }
    }


    void Start()    // Initialize references -- MUST run before Camera Start();
    {
        this.GlobalReferenceManagerInit(gameObject);

        this.SetDefactorPanelIds(primaryComic.GetComponent<ComicManagerTemplate>());

        DataAcquisition.Singleton.isEnding = false;

    }
    
    
    

    void Update()
    {
        if (Input.touchCount == 5)
        {

            // If 5 fingerdeathpunch then kill the experiment but ask why:
            if (Input.GetTouch(4).phase == TouchPhase.Ended && !DataAcquisition.Singleton.isEnding)
            {
                DataAcquisition.Singleton.isEnding = true;
                //Debug.Log("Stopping the Experiment!");

                DataAcquisition.Singleton.timeAtFFDP = Time.realtimeSinceStartup;

                if (SceneManager.GetSceneByName("ComicBook").isLoaded)
                {
                    DataAcquisition.Singleton.EndInteractive();
                   
                }
                if (SceneManager.GetSceneByName("ClassicComicBook").isLoaded)
                {
                    DataAcquisition.Singleton.EndClassic();
                    
                    
                
                }



                //if (DataAcquisition.Singleton.endOfExperiment) Not interested in this here;
                //{

                //    SceneManager.LoadScene("FinalSurveyScene");
                //    //DataAcquisition.Singleton.EndExperiment();
                //}



                    //SceneManager.LoadScene("ComicBook");
                    SceneManager.LoadScene("DisengagementScene");

                
            }

        }
    }

}
