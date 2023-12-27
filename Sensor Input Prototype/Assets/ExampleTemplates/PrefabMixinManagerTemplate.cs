using System.Collections;
using UnityEngine;
using SensorInputPrototype.ExampleTemplates;
namespace SensorInputPrototype.ExampleTemplates
{
    public class PrefabMixinManagerTemplate : MonoBehaviour
    {
        /*
        Properties for managing mixin components on this gameobject used as a prefab, This Prefab can Inherit from Other Prefabs as long as it holds nothing important. Instead use Prefabs that has prefabs that use this class for each nested prefab, this way you inherit the GameObject Properties, but no inheritance brittleness is kept.

        primary function of this is to be used as GetComponent<PrefabMixinManager>().TryGetComponentInChildren etc. or a common property like an ID that mixins require to determine how to behave, yet isn't required for the functioning of the code at compiletime, while also allowing for mixins to be removed or added with no need for them to know about each other.

        */

    }
}