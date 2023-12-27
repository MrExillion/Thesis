
using UnityEngine;
using SensorInputPrototype.MixinInterfaces;
using Unity.VisualScripting;
using System.Linq;

public class MicrophoneBlowAirTrigger : MonoBehaviour, MMicrophoneFifoAmp
{
    private UniversalPanel universalPanel;
    private AudioSource audioSource;
    public bool canTransition = false;
    private AudioClip clip;
    private int[] microphoneTransitions = new int[2];
    void Awake()
    {
        if(audioSource == null)
        {
            audioSource = Camera.main.gameObject.GetOrAddComponent<AudioSource>();

        }
        


    }


    // Start is called before the first frame update
    void Start()
    {
  
        universalPanel = GetComponent<UniversalPanel>();
        this.MicrophonoeFifoAmpInitializer(audioSource, 8, 512, 1);
        microphoneTransitions[0] = 5;
        microphoneTransitions[1] = 7;
    }

    // Update is called once per frame
    void Update()
    {

        //foreach(var dev in Microphone.devices)
        //{
            
        //    Debug.Log(dev);
        //}

        if (universalPanel != GlobalReferenceManager.GetCurrentUniversalPanel())
        {
            if(Microphone.devices.Length != 0)
            {

                if (Microphone.IsRecording(Microphone.devices[0]) && !microphoneTransitions.Contains(GlobalReferenceManager.GetCurrentUniversalPanel().transitionType))
                {
                    Microphone.End(Microphone.devices[0]);
                }

            }
            this.BufferFlush();
            canTransition = false;
            return; 
        
        }

        

        if (Microphone.IsRecording(Microphone.devices[0]))
        {

        }
        else
        {

            clip = Microphone.Start(Microphone.devices[0], true, 1, 44100);
                     

            
        
        }
        if (clip != null)
        {
            if (clip.length == 1f)
            { 
                this.BufferUpdate(clip, 0);
                //clip = null;
            }
        }

        if (universalPanel.transitionType == 5)
        {
            
            float lowerShelf = 0;
            float upperShelf = 0;
            int threshold = this.GetAvgQueueFrequencyDistribution().Length - Mathf.RoundToInt(this.GetAvgQueueFrequencyDistribution().Length / 3);
            Debug.Log("1: " + this.GetAvgQueueFrequencyDistribution()[0] +" ,2: " + this.GetAvgQueueFrequencyDistribution()[1] + " ,3: " + this.GetAvgQueueFrequencyDistribution()[2] + " ,4: " + this.GetAvgQueueFrequencyDistribution()[3] + " ,5: " + this.GetAvgQueueFrequencyDistribution()[4] + " ,6: " + this.GetAvgQueueFrequencyDistribution()[5] + " ,7: " + this.GetAvgQueueFrequencyDistribution()[6] + " ,8: " + this.GetAvgQueueFrequencyDistribution()[7] + " ,Amplitude: " + this.GetAvgQueueAmplitude());

            for (int i = 0; i < this.GetAvgQueueFrequencyDistribution().Length;i++)
            {
                if (i < this.GetAvgQueueFrequencyDistribution().Length - threshold)
                {
                    lowerShelf += this.GetAvgQueueFrequencyDistribution()[i];
                    
                }
                else
                {
                    upperShelf += this.GetAvgQueueFrequencyDistribution()[i];
                }

            }
            lowerShelf /= (this.GetAvgQueueFrequencyDistribution().Length - threshold);
            upperShelf /= threshold;
            bool isAirInput = (lowerShelf > upperShelf);
            Debug.Log("LS: "+lowerShelf + " ,US: " + upperShelf + " isAirInput: " + isAirInput);
            if (this.GetAvgQueueAmplitude()*10 > 0.6f && isAirInput)
            {
                canTransition = true;
            }
        }
        /*if (universalPanel.transitionType == 7)
        {

            float lowerShelf = 0;
            float upperShelf = 0;
            int threshold = this.GetAvgQueueFrequencyDistribution().Length - Mathf.RoundToInt(this.GetAvgQueueFrequencyDistribution().Length / 6);
            //Debug.Log("1: " + this.GetAvgQueueFrequencyDistribution()[0] +" ,2: " + this.GetAvgQueueFrequencyDistribution()[1] + " ,3: " + this.GetAvgQueueFrequencyDistribution()[2] + " ,4: " + this.GetAvgQueueFrequencyDistribution()[3] + " ,5: " + this.GetAvgQueueFrequencyDistribution()[4] + " ,6: " + this.GetAvgQueueFrequencyDistribution()[5] + " ,7: " + this.GetAvgQueueFrequencyDistribution()[6] + " ,8: " + this.GetAvgQueueFrequencyDistribution()[7] + " ,Amplitude: " + this.GetAvgQueueAmplitude());

            for (int i = 0; i < this.GetAvgQueueFrequencyDistribution().Length; i++)
            {
                if (i < this.GetAvgQueueFrequencyDistribution().Length - threshold)
                {
                    lowerShelf += this.GetAvgQueueFrequencyDistribution()[i];

                }
                else
                {
                    upperShelf += this.GetAvgQueueFrequencyDistribution()[i];
                }

            }
            lowerShelf /= (this.GetAvgQueueFrequencyDistribution().Length - threshold);
            upperShelf /= threshold;
            bool isAirInput = (lowerShelf > upperShelf);
            //Debug.Log("LS: "+lowerShelf + " ,US: " + upperShelf + " isAirInput: " + isAirInput);
            if (this.GetAvgQueueAmplitude() * 10 > 0.6f && isAirInput)
            {
                canTransition = true;
            }
        }*/




        //if (canTransition && universalPanel.transitionType == 5)
        //{
        if (canTransition)
        {
            universalPanel.TriggerTransition();
            canTransition = false;
        }

    }
}
