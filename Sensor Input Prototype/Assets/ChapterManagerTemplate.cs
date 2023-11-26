using SensorInputPrototype.MixinInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ChapterManagerTemplate : PanelSystemTemplateType, MChapterManager, IGlobalReferenceManager
    {
    public List<GameObject> chapterOrder;
    [SerializeField] public int comicId;


    public override int panelId
    {
        get { return panelId; }
        set { panelId = comicId; }
    }


    private void Awake()
    {
        //panelId = comicId;
        //this.InitializeChapterManager();
        this.AddNewMixin<ChapterManagerMixin>(gameObject);
        


    }

    private void Start()
    {
        this.InitializeComicStructure_chapters(gameObject, this, comicId, this.TrackChapters(chapterOrder));
    }
}

