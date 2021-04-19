using System.Collections.Generic;
using UnityEngine;

namespace LazyHelper.LazyCheckList.Scriptables
{
    public class LazyCheckListBundle : ScriptableObject
    {
        public Texture thumbnail;
        public string bundleName;
        public string bundleDescription;
        
        public List<LazyCheckListCategory> activeDisplayCategories = new List<LazyCheckListCategory>();
        public List<LazyCheckListCategory> allCategories = new List<LazyCheckListCategory>();
    } 
}

