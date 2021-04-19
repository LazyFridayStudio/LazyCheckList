using System.Collections.Generic;
using UnityEngine;

namespace LazyHelper.LazyCheckList.Scriptables
{
    public class LazyCheckListCategory : ScriptableObject
    {
        public List<CheckListItem> Items = new List<CheckListItem>();
    }
}