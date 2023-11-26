using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class ListTuple<V> where V : ComicList
{
    List<Dictionary<int,V>> keyValuePairs;
    public ListTuple(int key, ChapterList comicList)
    {
        keyValuePairs = new List<Dictionary<int,V>>();
        keyValuePairs.ToDictionary(key => comicList);
        
        //keyValuePairs.Add(Dictionary.KeyCollection key,values)
            //(x=> key, y=> values))>();
    }
}



[System.Serializable]
public class ComicList
{
    [HeaderAttribute("Comics")]
    ListTuple<ComicList> comics;
    public ComicList(int index, ChapterList chapterList)
    {
        
        comics = new ListTuple<ComicList>(index,chapterList);
    }
}
[System.Serializable]
public class ChapterList
{
    [HeaderAttribute("Chapters")]
    ComicList chapters;
    //ListTuple<K>
    
    public ChapterList(ComicList ComicList, GameObject gameObject)
    {
        
        
        //chapters = new ComicList(ComicList gameObject: gameObject);
    }

}

[System.Serializable]
public class PageList
{
    [HeaderAttribute("Pages")]
    ChapterList pages;
    public PageList(PanelList panelList, GameObject gameObject)
    {
        
    PageList pagelList = new PageList(panelList,gameObject);
        //pages = new ChapterList(pagelList,gameObject: gameObject);
    }

}
[System.Serializable]
public class PanelList
{
    List<Tuple<PageList,GameObject>> panels;

    public PanelList()
    {
        panels = new List<Tuple<PageList,GameObject>>();
    }

}