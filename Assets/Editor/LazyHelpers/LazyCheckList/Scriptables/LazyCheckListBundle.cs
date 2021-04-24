using System.Collections.Generic;
using UnityEngine;

namespace LazyHelper.LazyCheckList.Scriptables
{
    public class LazyCheckListBundle : ScriptableObject
    {
        public Texture thumbnail;
        public string bundleName;
        public string orginalName;
        public string bundleDescription;
        
        public List<LazyCheckListCategory> activeDisplayCategories = new List<LazyCheckListCategory>();
        public LazyCheckListCategory generalCategory;
        public LazyCheckListCategory urgentCategory;
        public LazyCheckListCategory wIPCategory;
        public LazyCheckListCategory bugCategory;
        public LazyCheckListCategory ideaCategory;
        public LazyCheckListCategory doneCategory;
    } 
}

