using SensorInputPrototype.MixinInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ChapterManagerMixin : MonoBehaviour, MChapterManager, MTransition, IGlobalReferenceManager
{
    public static ChapterManagerTemplate template;

}
