using SensorInputPrototype.MixinInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ChapterManagerMixin : MonoBehaviour, MChapterManager, MTransition
{
    public static ChapterManagerTemplate template;

}
