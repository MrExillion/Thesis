using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public static DataAcquisition Singleton;
    
    
    //variables used to calculate others
    public float transitionTime;
    public bool endOfExperiment = false;
    public bool isEnding = false;


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
    }

    // Update is called once per frame
    private void Update()
    {
        timeSinceStartUp = Time.realtimeSinceStartup;
        timeSinceLastTransition = transitionTime - Time.realtimeSinceStartup;
        if (SceneManager.GetSceneByName("ComicBook").isLoaded)
        {
            numberOfTouchesTotal += Input.touchCount;
        }
        if (SceneManager.GetSceneByName("ClassicComicBook").isLoaded)
        {
            numberOfTouchesTotalClassic += Input.touchCount;
        }
        

    }

    public void EndInteractive()
    {
        durationForInteractive = timeAtInteractiveEnd - timeAtInteractiveLoad;
    }
    public void EndClassic()
    {
        durationForClassic = timeAtClassicEnd - timeAtClassicLoad;
    }
    public void EndExperiment()
    {
        durationForExperiment = durationForClassic+durationForInteractive;
        DumpData();
    }

    public void DumpData()
    {
        string stringToSave = "timeSinceStartUp; timeSinceLastTransition; timeAtClassicLoad; timeAtClassicEnd; durationForClassic; timeAtInteractiveLoad;  timeAtInteractiveEnd; durationForInteractive; timeAtFFDP; numberOfTouchesTotal; numberOfTouchInteractions; "; /*"column_1; column2; column3\r\naaa; bbb; ccc\r\n111; 222; 333"*/
        for(int i = 0; i < touchesList.Count; i++) 
        {
            stringToSave += "TouchesListIndex_"+i+"; Phase_"+i+"; TapCount_"+i+"; Pressure_"+i+"; FingerId_"+i+"; MaxPossiblePressure_"+i+"; ";
        }
        for (int i = 0; i < touchesListClassic.Count; i++)
        {
            stringToSave += "TouchesListIndexC_" + i + "; PhaseC_" + i + "; TapCountC_" + i + "; PressureC_" + i + "; FingerIdC_" + i + "; MaxPossiblePressureC_" + i + "; ";
        }

        stringToSave += "testParticipantID; frameCountPerActiveGyro; frameCountPerActiveMicrophone; frameCountOerActiveLightSensor; disengagementReactionCards; engagementMappingReactionCompletionCards; durationForExperiment; ";
        for(int i = 0; i < 20; i++)
        {
            stringToSave += "timeSpentOnPanel_" + i +"; ";
        }
        for (int i = 0; i < 20; i++)
        {
            stringToSave += "timeSpentLookingAtClassicPanel_" + i+"; ";
        }
        stringToSave += "\r\n" + timeSinceStartUp + "; " + timeSinceLastTransition + "; " + timeAtClassicLoad + "; "+ timeAtClassicEnd + "; " + durationForClassic + "; " + timeAtInteractiveLoad + "; " + timeAtInteractiveEnd + "; " + durationForInteractive + "; " + timeAtFFDP + "; " + numberOfTouchesTotal + "; " + numberOfTouchInteractions+ "; ";
        for (int i = 0; i < touchesList.Count; i++)
        {
            stringToSave += i+"; "+touchesList[i].phase +"; "+touchesList[i].tapCount + "; " + touchesList[i].pressure + "; " + touchesList[i].fingerId + "; "+touchesList[i].maximumPossiblePressure + "; ";
        }
        for (int i = 0; i < touchesListClassic.Count; i++)
        {
            stringToSave += i + "; " + touchesListClassic[i].phase + "; " + touchesListClassic[i].tapCount + "; " + touchesListClassic[i].pressure + "; " + touchesListClassic[i].fingerId + "; " + touchesListClassic[i].maximumPossiblePressure+ "; ";
        }
        stringToSave += testParticipantID + "; " + frameCountPerActiveGyro + "; " + frameCountPerActiveMicrophone + "; " + frameCountPerActiveLightSensor + "; " + disengagementReactionCards + "; " + engagementMappingReactionCompletionCards + "; " + durationForExperiment + "; ";
        for (int i = 0; i < 20; i++)
        {
            stringToSave += timeSpentOnPanel[i]+"; ";
        }
        for (int i = 0; i < 20; i++)
        {
            
            stringToSave += timeSpentLookingAtClassicPanel[i];
            if (i < 19)
            {
                stringToSave += "; ";
            }
        }
        
        System.IO.File.WriteAllText(relPath + "DataAcquisition"+System.IO.Directory.GetFiles(relPath, "*.csv").Length+".csv", stringToSave);


    }




}
