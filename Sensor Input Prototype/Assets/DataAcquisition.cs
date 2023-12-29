using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataAcquisition : MonoBehaviour
{
    public float timeSinceStartUp = 0f;
    public float timeSinceLastTransition = 0f;
    public float timeAtClassicLoad = 0f;
    public float timeAtClassicEnd = 0f;
    public float durationForClassic = 0f;
    public float timeAtInteractiveLoad = 0f;
    public float timeAtInteractiveEnd = 0f;
    public float durationForInteractive = 0f;

    public float timeAtFFDP = 0f;

    public int numberOfTouchesTotal = 0;
    public int numberOfTouchInteractions = 0;
    public List<Touch> touchesList = new List<Touch>();
    public System.DateTime testParticipantID = System.DateTime.UtcNow;
    public int frameCountPerActiveGyro = 0;
    public int frameCountPerActiveMicrophone = 0;
    public int frameCountPerActiveLightSensor = 0;
    public static DataAcquisition Singleton;
    
    
    //variables used to calculate others
    public float transitionTime;



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
    }

    public void DumpData()
    {
        string stringToSave = "timeSinceStartUp, timeSinceLastTransition, timeAtClassicLoad, timeAtClassicEnd, durationForClassic, timeAtInteractiveLoad,  timeAtInteractiveEnd, durationForInteractive, timeAtFFDP, numberOfTouchesTotal, numberOfTouchInteractions, "; /*"column_1, column2, column3\r\naaa, bbb, ccc\r\n111, 222, 333"*/
        for(int i = 0; i < touchesList.Count; i++) 
        {
            stringToSave += "TouchesListIndex_"+i+", Phase_"+i+", TapCount_"+i+", Pressure_"+i+", FingerId_"+i+", MaxPossiblePressure_"+i+", ";
        }
        stringToSave += "testParticipantID, frameCountPerActiveGyro, frameCountPerActiveMicrophone, frameCountOerActiveLightSensor";
        stringToSave += "\r\n" + timeSinceStartUp + ", " + timeSinceLastTransition + ", " + timeAtClassicLoad + ", " + durationForClassic + ", " + timeAtInteractiveLoad + ", " + timeAtInteractiveEnd + ", " + durationForInteractive + ", " + timeAtFFDP + ", " + numberOfTouchesTotal + ", " + numberOfTouchInteractions+ ", ";
        for (int i = 0; i < touchesList.Count; i++)
        {
            stringToSave += i+","+touchesList[i].phase +","+touchesList[i].tapCount + "," + touchesList[i].pressure + "," + touchesList[i].fingerId + ","+touchesList[i].maximumPossiblePressure;
        }
        stringToSave += testParticipantID + ", " + frameCountPerActiveGyro + ", " + frameCountPerActiveMicrophone + ", " + frameCountPerActiveLightSensor;

        System.IO.File.WriteAllText(relPath + "DataAcquisition.csv", stringToSave);


    }




}
