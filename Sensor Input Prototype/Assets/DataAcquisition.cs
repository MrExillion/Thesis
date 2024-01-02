using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataAcquisition : MonoBehaviour
{
    
    public float timeSinceStartUp = 0f; // Implemented //possibly duplicated
    public float timeSinceLastTransition = 0f; // Implemented
    public float timeAtClassicLoad = 0f; // Implemented
    public float timeAtClassicEnd = 0f; // Implemented
    public float durationForClassic = 0f; // Implemented
    public float timeAtInteractiveLoad = 0f; // Implemented
    public float timeAtInteractiveEnd = 0f; // Implemented
    public float durationForInteractive = 0f; // Implemented
    public float durationForExperiment = 0f; // Implemented
    public float timeAtFFDP = 0f; // Implemented

    public int numberOfTouchesTotal = 0; // Implemented
    public int numberOfTouchInteractions = 0; // Implemented

    public int numberOfTouchInteractionsClassic = 0; // Implemented
    public int numberOfTouchesTotalClassic = 0; // Implemented

    public List<Touch> touchesList = new List<Touch>(); // Implemented
    public List<Touch> touchesListClassic = new List<Touch>(); // Implemented
    public System.DateTime testParticipantID = System.DateTime.UtcNow; // Implemented <---
    public int frameCountPerActiveGyro = 0; // NotImplemented
    public int frameCountPerActiveMicrophone = 0; // NotImplemented
    public int frameCountPerActiveLightSensor = 0; // NotImplemented

    public string disengagementReactionCards = "N/A";
    public string engagementMappingReactionCompletionCards = "N/A";

    public float[] timeSpentOnPanel = new float[20]; // Implemented
    public float[] timeSpentLookingAtClassicPanel = new float[20]; //
    public int numberOfPreviousRespondents = 0;
    
    public static DataAcquisition Singleton;
    
    
    //variables used to calculate others
    public float transitionTime;
    public bool endOfExperiment = false;
    public bool isEnding = false;


    public bool bugsfixed = true;

    string relPath = "";
    




    private void Awake()
    {
      if(Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
            relPath = Application.persistentDataPath + "/";
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(testParticipantID.ToString());
        DataAcquisition.Singleton.numberOfPreviousRespondents = System.IO.Directory.GetFiles(relPath, "*.csv").Length;

    }
    void OnEnable() // this is called each load even though this is on dont destroy on load in the menu, because its and callback delegate.
    { 
        SceneManager.sceneLoaded += SceneManager_sceneLoaded; 


    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "ComicBook" && loadSceneMode == LoadSceneMode.Single)
        {

            timeAtInteractiveLoad = Time.realtimeSinceStartup;
            DataAcquisition.Singleton.timeSinceLastTransition = timeAtInteractiveLoad;
            DataAcquisition.Singleton.endOfExperiment = (timeAtClassicLoad > 0);

        }
        if (scene.name == "ClassicComicBook" && loadSceneMode == LoadSceneMode.Single)
        {

            timeAtClassicLoad = Time.realtimeSinceStartup;
            DataAcquisition.Singleton.endOfExperiment = (timeAtInteractiveLoad > 0);
        }

    }



    int touchCountLastFrame = 0;
    // Update is called once per frame
    private void Update()
    {
        timeSinceStartUp = Time.realtimeSinceStartup;
        //timeSinceLastTransition = transitionTime - Time.realtimeSinceStartup;
        if (SceneManager.GetSceneByName("ComicBook").isLoaded)
        {
            bool changeRegistered = false;
            foreach(Touch touch in Input.touches)
            {
                if(Input.touches[touch.fingerId].deltaPosition.magnitude > 0.05f)
                {
                    changeRegistered = true;   
                }
            }
            if((Input.touchCount != touchCountLastFrame) || changeRegistered)
            {
                numberOfTouchesTotal += Input.touchCount;
            }

        }
        if (SceneManager.GetSceneByName("ClassicComicBook").isLoaded)
        {
            bool changeRegistered = false;
            foreach (Touch touch in Input.touches)
            {
                if (Input.touches[touch.fingerId].deltaPosition.magnitude > 0.05f)
                {
                    changeRegistered = true;
                }
            }
            if ((Input.touchCount != touchCountLastFrame) || changeRegistered)
            {
                numberOfTouchesTotalClassic += Input.touchCount;
            }
            
        }
        touchCountLastFrame = Input.touchCount;
        

    }

    public void EndInteractive()
    {
        timeAtInteractiveEnd = Time.realtimeSinceStartup;
        durationForInteractive = timeAtInteractiveEnd - timeAtInteractiveLoad;
    }
    public void EndClassic()
    {
        timeAtClassicEnd = Time.realtimeSinceStartup;
        durationForClassic = timeAtClassicEnd - timeAtClassicLoad;
    }
    public void EndExperiment()
    {
        durationForExperiment = durationForClassic+durationForInteractive;
        DumpData();
    }

    public void DumpData()
    {
        string stringToSave = "timeSinceStartUp\r\n timeSinceLastTransition\r\n timeAtClassicLoad\r\n timeAtClassicEnd\r\n durationForClassic\r\n timeAtInteractiveLoad\r\n  timeAtInteractiveEnd\r\n durationForInteractive\r\n timeAtFFDP\r\n numberOfTouchesTotal\r\n numberOfTouchInteractions\r\n "; /*"column_1\r\n column2\r\n column3\r\naaa\r\n bbb\r\n ccc\r\n111\r\n 222\r\n 333"*/
        for (int i = 0; i < touchesList.Count; i++) 
        {
            stringToSave += "TouchesListIndex_" + i + "\r\n Phase_" + i + "\r\n TapCount_" + i + "\r\n Pressure_" + i + "\r\n FingerId_" + i + "\r\n MaxPossiblePressure_" + i + "\r\n ";
        }
        for (int i = 0; i < touchesListClassic.Count; i++)
        {
            stringToSave += "TouchesListIndexC_" + i + "\r\n PhaseC_" + i + "\r\n TapCountC_" + i + "\r\n PressureC_" + i + "\r\n FingerIdC_" + i + "\r\n MaxPossiblePressureC_" + i + "\r\n ";
        }

        stringToSave += "testParticipantID\r\n frameCountPerActiveGyro\r\n frameCountPerActiveMicrophone\r\n frameCountOerActiveLightSensor\r\n disengagementReactionCards\r\n engagementMappingReactionCompletionCards\r\n durationForExperiment\r\n ";
        for (int i = 0; i < 20; i++)
        {
            stringToSave += "timeSpentOnPanel_" + i + "\r\n ";
        }
        for (int i = 0; i < 20; i++)
        {
            stringToSave += "timeSpentLookingAtClassicPanel_" + i + "\r\n ";
        }
        stringToSave += ";" + timeSinceStartUp + "\r\n " + timeSinceLastTransition + "\r\n " + timeAtClassicLoad + "\r\n " + timeAtClassicEnd + "\r\n " + durationForClassic + "\r\n " + timeAtInteractiveLoad + "\r\n " + timeAtInteractiveEnd + "\r\n " + durationForInteractive + "\r\n " + timeAtFFDP + "\r\n " + numberOfTouchesTotal + "\r\n " + numberOfTouchInteractions + "\r\n ";
        for (int i = 0; i < touchesList.Count; i++)
        {
            stringToSave += i + "\r\n " + touchesList[i].phase + "\r\n " + touchesList[i].tapCount + "\r\n " + touchesList[i].pressure + "\r\n " + touchesList[i].fingerId + "\r\n " + touchesList[i].maximumPossiblePressure + "\r\n ";
        }
        for (int i = 0; i < touchesListClassic.Count; i++)
        {
            stringToSave += i + "\r\n " + touchesListClassic[i].phase + "\r\n " + touchesListClassic[i].tapCount + "\r\n " + touchesListClassic[i].pressure + "\r\n " + touchesListClassic[i].fingerId + "\r\n " + touchesListClassic[i].maximumPossiblePressure + "\r\n ";
        }
        stringToSave += testParticipantID + "\r\n " + frameCountPerActiveGyro + "\r\n " + frameCountPerActiveMicrophone + "\r\n " + frameCountPerActiveLightSensor + "\r\n " + disengagementReactionCards + "\r\n " + engagementMappingReactionCompletionCards + "\r\n " + durationForExperiment + "\r\n ";
        for (int i = 0; i < 20; i++)
        {
            stringToSave += Singleton.timeSpentOnPanel[i] + "\r\n ";
        }
        for (int i = 0; i < 20; i++)
        {

            stringToSave += timeSpentLookingAtClassicPanel[i];
            if (i < 19)
            {
                stringToSave += "\r\n ";
            }
        }
        
        System.IO.File.WriteAllText(relPath + "DataAcquisition"+Singleton.numberOfPreviousRespondents+".csv", stringToSave);


    }




}
