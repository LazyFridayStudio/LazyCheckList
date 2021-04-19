using System.Collections.Generic;
using UnityEngine;

public class LazyScene : ScriptableObject
{
	public List<LazySceneCategorys> activeCategorys = new List<LazySceneCategorys>();
	public List<LazySceneCategorys> allCategorys = new List<LazySceneCategorys>();
}
