using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using SensorInputPrototype.MixinInterfaces;
using Unity.VisualScripting;

public static class MicrophoneFifoAmp
{



    private static ConditionalWeakTable<MMicrophoneFifoAmp, Fields> table;

    static MicrophoneFifoAmp()
    {
        table = new ConditionalWeakTable<MMicrophoneFifoAmp, Fields>();
    }
    private sealed class Fields : MonoBehaviour, MTransition
    {
        internal Queue<float> samplesQueue = new Queue<float>();
    }

    public static void Enqueue(this MMicrophoneFifoAmp map, float soundSample)
    {
        table.GetOrCreateValue(map).samplesQueue.Enqueue(soundSample);

        
    }

    public static void Dequeue(this MMicrophoneFifoAmp map)
    {
        table.GetOrCreateValue(map).samplesQueue.Dequeue();

    }

}
