#region NameSpaces

using System;
using System.Collections.Generic;
using LazyHelper.LazyCheckList.Scriptables;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

#endregion

//================================================================
//							IMPORTANT
//================================================================
//				Copyright LazyFridayStudio
//DO NOT SELL THIS CODE OR REDISTRIBUTE WITH INTENT TO SELL.
//
//Send an email to our support line for any questions or inquirys
//CONTACT: Lazyfridaystudio@gmail.com
//
//Alternatively join our Discord
//DISCORD: https://discord.gg/Z3tpMG
//
//Hope you enjoy the simple checklist
//designed and made by lazyFridayStudio
//================================================================




namespace LazyHelper.LazyCheckList
{        
    public enum CheckListType
    {
            Normal,
            Urgent,
            InProgress,
            Planned,
            Idea
    }
    
    [System.Serializable]
    public class CheckListItem
    {
        public string itemDescription;
        public CheckListType itemType;
    }

    public class LazyCheckListExplorer : EditorWindow
    {
        public static LazyCheckListExplorer window;
        public static LazyCheckListMaster masterChecklist;
        
        #region Editer General Values
        [MenuItem("Window/LazyHelper/Lazy Checklist")]
        public static void Init()
        {
            // Get existing open window or if none, make a new one:
            window = (LazyCheckListExplorer) GetWindow(typeof(LazyCheckListExplorer));
            window.titleContent.text = "Lazy Checklist Explorer";
            window.position = new Rect(0, 0, 600, 800);
            window.autoRepaintOnSceneChange = false;
        }
        #endregion
        #region Styles
        public GUIStyle mainTextStyle = new GUIStyle();
        public GUIStyle secondaryTextStyle = new GUIStyle();
        public GUIStyle generalTexStyle = new GUIStyle();
        public GUIStyle paddingStyle = new GUIStyle();

        public GUIStyle evenItemStyle = new GUIStyle();
        public GUIStyle oddItemStyle = new GUIStyle();
        #endregion
        #region Textures
        private Texture2D mainBackground;
        private Texture2D secondaryBackground;
        private Texture2D seperator;
        private Texture2D itemSeperator;
        private Texture2D oddBackground;
        private Texture2D evenBackground;
        #endregion
        #region Sections

        Rect headerSection;
        Rect subMenuSection;
        Rect itemSection; 
        Rect categorySection;

        #endregion
        #region Generation Functions
        private void OnHierarchyChange()
        {
            OnEnable();
            Repaint();
        }
        
        private void GenerateStyle()
        {
            string path = "Assets/Editor/LazyHelpers/Resources/HeaderFont.ttf";
            Font headerFont = EditorGUIUtility.Load(path) as Font;
            
            //Main Text
            mainTextStyle.normal.textColor = LazyEditorHelperUtils.LazyFridayMainColor;
            mainTextStyle.fontSize = 16;
            mainTextStyle.alignment = TextAnchor.LowerCenter;
            mainTextStyle.font = headerFont; 
            
            //Secondary Text
            secondaryTextStyle.normal.textColor = LazyEditorHelperUtils.LazyFridayMainColor;
            secondaryTextStyle.fontSize = 12;
            secondaryTextStyle.fontStyle = FontStyle.Bold;
            secondaryTextStyle.alignment = TextAnchor.MiddleCenter;
            secondaryTextStyle.wordWrap = true;
            
            //General Text
            generalTexStyle.normal.textColor = LazyEditorHelperUtils.LazyFridaySecondaryColor;
            generalTexStyle.fontSize = 12;
            generalTexStyle.alignment = TextAnchor.MiddleLeft;
            generalTexStyle.wordWrap = true;
            
            //Item Styles
            oddItemStyle.normal.background = oddBackground;
            oddItemStyle.padding = new RectOffset(3, 3, 3, 3);
            oddItemStyle.border = new RectOffset(0, 0, 5, 5);
            oddItemStyle.normal.textColor = LazyEditorHelperUtils.LazyFridaySecondaryColor;

            evenItemStyle.normal.background = evenBackground;
            evenItemStyle.border = new RectOffset(0, 0, 5, 5);
            evenItemStyle.padding = new RectOffset(3, 3, 3, 3);
            evenItemStyle.normal.textColor = LazyEditorHelperUtils.LazyFridaySecondaryColor;
            
            paddingStyle.margin = new RectOffset(2, 2, 4, 4);
        }
        
        private void GenerateTextures()
        {
            mainBackground = new Texture2D(1, 1);
            mainBackground.SetPixel(0, 0, LazyEditorHelperUtils.LazyFridayBackgroundColor);
            mainBackground.Apply();
            
            secondaryBackground = new Texture2D(1, 1);
            secondaryBackground.SetPixel(0, 0, new Color32(33, 33, 33, 255));
            secondaryBackground.Apply();

            seperator = new Texture2D(1, 1);
            seperator.SetPixel(0, 0, new Color32(242, 242, 242, 255));
            seperator.Apply();
            
            //Item areas
            evenBackground = new Texture2D(1, 1);
            evenBackground.SetPixel(0, 0, new Color32(44, 44, 44, 255));
            evenBackground.Apply();

            oddBackground = new Texture2D(1, 1);
            oddBackground.SetPixel(0, 0, new Color32(33, 33, 33, 255));
            oddBackground.Apply();
        }
        
        //Start Function
        private void OnEnable()
        {
            GenerateAndCheckFiles();
            GenerateStyle();
            GenerateTextures();
        }
        #endregion
        
        #region Drawing Functions

        private void OnGUI()
        {
            if (mainBackground == null)
            {
                OnEnable();
            }
            
            DrawLayout();
            DrawHeader();
            DrawSubHeading();
            DrawItemArea();
        }

        private void DrawLayout()
        {
            headerSection.x = 0;
            headerSection.y = 0;
            headerSection.width = Screen.width;
            headerSection.height = 25;

            subMenuSection.x = 0;
            subMenuSection.y = headerSection.height;
            subMenuSection.width = Screen.width;
            subMenuSection.height = 27;

            itemSection.x = 0;
            itemSection.y = headerSection.height + subMenuSection.height;
            itemSection.width = Screen.width;
            itemSection.height = Screen.height;
            
            categorySection.x = 0;
            categorySection.y = headerSection.height + subMenuSection.height;
            categorySection.width = itemSection.x;
            categorySection.height = Screen.height;

            GUI.DrawTexture(headerSection, mainBackground);
            GUI.DrawTexture(subMenuSection, secondaryBackground);
            GUI.DrawTexture(categorySection, secondaryBackground);
            GUI.DrawTexture(itemSection, mainBackground);

            //Draw Seperators
           // GUI.DrawTexture(new Rect(categorySection.width - 2, headerSection.height + subMenuSection.height, 2, categorySection.height), seperator);
            GUI.DrawTexture(new Rect(headerSection.x, headerSection.height - 2, headerSection.width, 2), seperator);
            GUI.DrawTexture(new Rect(subMenuSection.x, (subMenuSection.height + headerSection.height) - 2, subMenuSection.width, 2), seperator);
        }

        private void DrawHeader()
        {
            GUILayout.BeginArea(headerSection);
           // Rect centerRect = LazyEditorHelperUtils.CenterRect(headerSection, logoHeader);
           GUILayout.Space(7);
           GUILayout.BeginHorizontal();
           GUILayout.Label("LAZY FRIDAY STUDIO", mainTextStyle);
           GUILayout.EndHorizontal();
            //GUI.Label(new Rect(centerRect.x + 13, centerRect.y - 2, centerRect.width, centerRect.height), logoHeader);
            GUILayout.EndArea();
        }

        private void DrawSubHeading()
        {
            GUILayout.BeginArea(subMenuSection);
            GUILayout.BeginHorizontal(paddingStyle);

            if (masterChecklist.AllBundles.Count > 0)
            {
                if (GUILayout.Button("Create New Bundle or Edit Existing Bundles", GUILayout.MaxWidth(300)))
                {
                    LazyCheckListBundleCreator.Init();
                }
            }
            else
            {
                if (GUILayout.Button("Create New Bundle", GUILayout.MaxWidth(120)))
                {
                    LazyCheckListBundleCreator.Init();
                }
            }
            


            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button("Help", GUILayout.MaxWidth(100)))
            {
                Application.OpenURL("https://www.lazyfridaystudio.com/lazysceneloader");
            }
            
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void DrawItemArea()
        {
            GUILayout.BeginArea(itemSection);
            GUILayout.BeginVertical();

            if (masterChecklist.AllBundles.Count > 0)
            {
                for (int i = 0; i < masterChecklist.AllBundles.Count; i++)
                {
                    LazyCheckListBundle asset = masterChecklist.AllBundles[i];
                    bool isEven = i % 2 == 0;
                    GUIStyle itemStyle = new GUIStyle();
                    
                    if (isEven)
                    {
                        itemStyle = evenItemStyle;
                    }
                    else
                    {
                        itemStyle = oddItemStyle;
                    }
                    
                    float thumbnailSize = 75;
                    GUILayout.BeginHorizontal(itemStyle, GUILayout.MaxHeight(75), GUILayout.MinHeight(75));
                    GUILayout.Space(5);
                    if (asset.thumbnail != null)
                    {
                        GUILayout.BeginVertical(GUILayout.MaxWidth(75), GUILayout.MinWidth(75));
                        GUILayout.FlexibleSpace();
                        GUILayout.Label(asset.thumbnail,GUILayout.MaxWidth(75), GUILayout.MinWidth(75), GUILayout.MaxHeight(thumbnailSize), GUILayout.MaxWidth(thumbnailSize));
                        GUILayout.FlexibleSpace();
                        GUILayout.EndVertical();
                    }
                    else
                    {
                        GUILayout.BeginVertical(GUILayout.MaxWidth(75), GUILayout.MinWidth(75));
                        GUILayout.FlexibleSpace();
                        GUILayout.Label(asset.bundleName, secondaryTextStyle );
                        GUILayout.FlexibleSpace();
                        GUILayout.EndVertical();
                    }
                    
                    GUILayout.Space(5);

                    #region Seperator
                    GUILayout.BeginVertical(GUILayout.MaxWidth(3));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("|", generalTexStyle);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndVertical();
                    #endregion

                    GUILayout.Space(5);

                    #region  Description
                    GUILayout.BeginVertical(GUILayout.MaxWidth(450), GUILayout.MinWidth(450));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(asset.bundleDescription, generalTexStyle);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndVertical();
                    #endregion
                    
                    GUILayout.Space(5);
                    
                    #region Seperator
                    GUILayout.BeginVertical(GUILayout.MaxWidth(3));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("|", generalTexStyle);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndVertical();
                    #endregion
                    
                    GUILayout.Space(5);

                    #region Select Bundle
                    GUILayout.BeginVertical();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Select Bundle"))
                    {
                        LazyCheckList.Init(asset);

                        if (window != null)
                        {
                            window.Close();
                        }
                        else
                        {
                            window = (LazyCheckListExplorer) GetWindow(typeof(LazyCheckListExplorer));
                            window.Close();
                        }

                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndVertical();
                    #endregion
                    GUILayout.EndHorizontal();
                    

                }
            } else
            {
                EditorGUILayout.HelpBox("No Bundles Made, Create your first one by clicking the Create New Bundle button", MessageType.Info);
                //NO BUNDLES SO DRAW WARNING MESSAGE
            }
            
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        #endregion

        private void GenerateAndCheckFiles()
        {
            if (AssetDatabase.IsValidFolder("Assets/Editor/LazyHelpers/LazyCheckList/Resources"))
            {
                masterChecklist = AssetDatabase.LoadAssetAtPath("Assets/Editor/LazyHelpers/LazyCheckList/Resources/CheckListMaster.asset", typeof(LazyCheckListMaster)) as LazyCheckListMaster;
                if (masterChecklist == null)
                {
                    //Debug.Log("no asset file found, could not reload");	
                    masterChecklist = CreateInstance(typeof(LazyCheckListMaster)) as LazyCheckListMaster;
                    AssetDatabase.CreateAsset(masterChecklist, "Assets/Editor/LazyHelpers/LazyCheckList/Resources/CheckListMaster.asset");
                    GUI.changed = true;
                }
            }
            else
            {
                AssetDatabase.CreateFolder("Assets/Editor/LazyHelpers/LazyCheckList", "Resources");

                masterChecklist = AssetDatabase.LoadAssetAtPath("Assets/Editor/LazyHelpers/LazyCheckList/Resources/CheckListMaster.asset", typeof(LazyCheckListMaster)) as LazyCheckListMaster;
                if (masterChecklist == null)
                {
                    //Debug.Log("no asset file found, could not reload");	
                    masterChecklist = CreateInstance(typeof(LazyCheckListMaster)) as LazyCheckListMaster;
                    AssetDatabase.CreateAsset(masterChecklist, "Assets/Editor/LazyHelpers/LazyCheckList/Resources/CheckListMaster.asset");
                    GUI.changed = true;
                }
            }

            if (!AssetDatabase.IsValidFolder("Assets/Editor/LazyHelpers/LazyCheckList/Resources/Bundles"))
            {
                AssetDatabase.CreateFolder("Assets/Editor/LazyHelpers/LazyCheckList/Resources", "Bundles");
            }
        }
    }

    public class LazyCheckListBundleCreator : EditorWindow
    {
        public static LazyCheckListBundleCreator window;
        public LazyCheckListMaster masterChecklist;
        
        #region Editer General Values
        public static void Init()
        {
            // Get existing open window or if none, make a new one:
            window = (LazyCheckListBundleCreator) GetWindow(typeof(LazyCheckListBundleCreator));
            window.titleContent.text = "Lazy Checklist Creator";
            window.position = new Rect(window.position.x, window.position.y, 710, 350);
            window.autoRepaintOnSceneChange = false;
        }
        #endregion
        
        #region Styles
        public GUIStyle mainTextStyle = new GUIStyle();
        public GUIStyle secondaryTextStyle = new GUIStyle();
        public GUIStyle generalTexStyle = new GUIStyle();
        public GUIStyle paddingStyle = new GUIStyle();

        public GUIStyle evenItemStyle = new GUIStyle();
        public GUIStyle oddItemStyle = new GUIStyle();
        #endregion
        #region Textures
        private Texture2D mainBackground;
        private Texture2D secondaryBackground;
        private Texture2D seperator;
        private Texture2D oddBackground;
        private Texture2D evenBackground;
        #endregion
        #region Sections

        Rect headerSection;
        Rect subMenuSection;
        Rect itemSection; 
        Rect categorySection;

        #endregion
        #region Generation Functions
        private void OnHierarchyChange()
        {
            OnEnable();
            Repaint();
        }
        
        private void GenerateStyle()
        {
            string path = "Assets/Editor/LazyHelpers/Resources/HeaderFont.ttf";
            Font headerFont = EditorGUIUtility.Load(path) as Font;
            
            //Main Text
            mainTextStyle.normal.textColor = LazyEditorHelperUtils.LazyFridayMainColor;
            mainTextStyle.fontSize = 16;
            mainTextStyle.alignment = TextAnchor.LowerCenter;
            mainTextStyle.font = headerFont; 
            
            //Secondary Text
            secondaryTextStyle.normal.textColor = LazyEditorHelperUtils.LazyFridayMainColor;
            secondaryTextStyle.fontSize = 12;
            secondaryTextStyle.fontStyle = FontStyle.Bold;
            secondaryTextStyle.alignment = TextAnchor.MiddleCenter;

            //General Text
            generalTexStyle.normal.textColor = LazyEditorHelperUtils.LazyFridaySecondaryColor;
            generalTexStyle.fontSize = 12;
            generalTexStyle.alignment = TextAnchor.MiddleCenter;
            
            //Item Styles
            oddItemStyle.normal.background = oddBackground;
            oddItemStyle.padding = new RectOffset(3, 3, 3, 3);
            oddItemStyle.border = new RectOffset(0, 0, 5, 5);
            oddItemStyle.normal.textColor = LazyEditorHelperUtils.LazyFridaySecondaryColor;

            evenItemStyle.normal.background = evenBackground;
            evenItemStyle.border = new RectOffset(0, 0, 5, 5);
            evenItemStyle.padding = new RectOffset(3, 3, 3, 3);
            evenItemStyle.normal.textColor = LazyEditorHelperUtils.LazyFridaySecondaryColor;
            
            paddingStyle.margin = new RectOffset(2, 2, 4, 4);
        }
        
        private void GenerateTextures()
        {
            mainBackground = new Texture2D(1, 1);
            mainBackground.SetPixel(0, 0, LazyEditorHelperUtils.LazyFridayBackgroundColor);
            mainBackground.Apply();
            
            secondaryBackground = new Texture2D(1, 1);
            secondaryBackground.SetPixel(0, 0, new Color32(33, 33, 33, 255));
            secondaryBackground.Apply();

            seperator = new Texture2D(1, 1);
            seperator.SetPixel(0, 0, new Color32(242, 242, 242, 255));
            seperator.Apply();
            
            //Item areas
            evenBackground = new Texture2D(1, 1);
            evenBackground.SetPixel(0, 0, new Color32(44, 44, 44, 255));
            evenBackground.Apply();

            oddBackground = new Texture2D(1, 1);
            oddBackground.SetPixel(0, 0, new Color32(33, 33, 33, 255));
            oddBackground.Apply();
        }
        
        //Start Function
        private void OnEnable()
        {
            GenerateStyle();
            GenerateTextures();
        }
        #endregion

        private void OnDestroy()
        {
            AssetDatabase.SaveAssets();
        }

        private bool isCreatingNewBundle;
        
        #region Drawing Functions

        private void OnGUI()
        {
            
            if (masterChecklist == null)
            {
                OnEnable();
                masterChecklist = AssetDatabase.LoadAssetAtPath("Assets/Editor/LazyHelpers/LazyCheckList/Resources/CheckListMaster.asset", typeof(LazyCheckListMaster)) as LazyCheckListMaster;
            }

            if (window == null)
            {
                window = (LazyCheckListBundleCreator) GetWindow(typeof(LazyCheckListBundleCreator));
            }
            
            DrawLayout();
            DrawHeader();
            DrawSubHeading();
            DrawItemArea();
        }

        private void DrawLayout()
        {
            headerSection.x = 0;
            headerSection.y = 0;
            headerSection.width = Screen.width;
            headerSection.height = 25;

            subMenuSection.x = 0;
            subMenuSection.y = headerSection.height;
            subMenuSection.width = Screen.width;
            subMenuSection.height = 27;

            itemSection.x = 0;
            itemSection.y = headerSection.height + subMenuSection.height;
            itemSection.width = Screen.width;
            itemSection.height = Screen.height;
            
            categorySection.x = 0;
            categorySection.y = headerSection.height + subMenuSection.height;
            categorySection.width = itemSection.x;
            categorySection.height = Screen.height;

            GUI.DrawTexture(headerSection, mainBackground);
            GUI.DrawTexture(subMenuSection, secondaryBackground);
            GUI.DrawTexture(categorySection, secondaryBackground);
            GUI.DrawTexture(itemSection, mainBackground);

            //Draw Seperators
           // GUI.DrawTexture(new Rect(categorySection.width - 2, headerSection.height + subMenuSection.height, 2, categorySection.height), seperator);
            GUI.DrawTexture(new Rect(headerSection.x, headerSection.height - 2, headerSection.width, 2), seperator);
            GUI.DrawTexture(new Rect(subMenuSection.x, (subMenuSection.height + headerSection.height) - 2, subMenuSection.width, 2), seperator);
        }

        private void DrawHeader()
        {
            GUILayout.BeginArea(headerSection);
           // Rect centerRect = LazyEditorHelperUtils.CenterRect(headerSection, logoHeader);
           GUILayout.Space(7);
           GUILayout.BeginHorizontal();
           GUILayout.Label("Bundle Editor", mainTextStyle);
           GUILayout.EndHorizontal();
            //GUI.Label(new Rect(centerRect.x + 13, centerRect.y - 2, centerRect.width, centerRect.height), logoHeader);
            GUILayout.EndArea();
        }

        private void DrawSubHeading()
        {
            GUILayout.BeginArea(subMenuSection);
            GUILayout.BeginHorizontal(paddingStyle);
            
            if (GUILayout.Button("Create New Bundle"))
            {
                isCreatingNewBundle = true;
                window.position = new Rect(window.position.x, window.position.y, 400, 355);
            }

            if (masterChecklist.AllBundles.Count > 0)
            {
                if (GUILayout.Button("Edit Bundle"))
                {
                    isCreatingNewBundle = false;
                    window.position = new Rect(window.position.x, window.position.y, 710, 350);
                }
            }
            
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void DrawItemArea()
        {
            GUILayout.BeginArea(itemSection);
            GUILayout.BeginVertical();

            if (isCreatingNewBundle)
            {
                DrawCreatingNewBundle();
            }
            else
            {
                EditBundles();
            }
            
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        
        private bool useThumbnail = false;
        private string bundleName = "Alpha V1";
        private string bundleDescription = "Basic description of a bundle to be displayed in the bundle browser";
        private Object thumbnail;
        
        // = AssetDatabase.LoadAssetAtPath("Assets/Editor/LazyHelpers/LazyCheckList/Resources/Icons/MissingIcon.png", typeof(Texture));
        private void DrawCreatingNewBundle()
        {
            #region Thumbnail area
            GUILayout.BeginVertical(oddItemStyle);
            GUILayout.Label("Thumbnail", secondaryTextStyle);
            GUILayout.Label("thumbnail will be used in the bundle explorer instead of text", generalTexStyle);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            useThumbnail = GUILayout.Toggle(useThumbnail, "Use thumbnail?");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            if (useThumbnail == false)
            {
                thumbnail = null;
            }
            else
            {
                thumbnail = EditorGUILayout.ObjectField(thumbnail, typeof(Texture), false);
            }
            GUILayout.EndVertical();
            #endregion

            GUILayout.Space(5);

            #region Bundle Name area
            GUILayout.BeginVertical(evenItemStyle);
            GUILayout.Label("Bundle Name", secondaryTextStyle);
            GUILayout.Label("Name that will be displayed on the bundle brower button", generalTexStyle);
            bundleName = GUILayout.TextField(bundleName, 16);
            GUILayout.EndVertical();
            #endregion

            GUILayout.Space(5);
            
            #region Bundle Description Area
            //Text Area Name
            GUILayout.BeginVertical(oddItemStyle);
            GUILayout.Label("Bundle Description", secondaryTextStyle);
            GUILayout.Label("A small description of the category", generalTexStyle);
            bundleDescription = GUILayout.TextArea(bundleDescription,150);
            GUILayout.EndVertical();
            #endregion

            GUILayout.Space(5);
            
            #region Create new bundle
            GUILayout.BeginVertical(evenItemStyle);
            if (thumbnail == null && useThumbnail == true)
            {
                EditorGUILayout.HelpBox("Thumbnail cannot be null", MessageType.Error);
            }
            else
            {
                if (bundleName == string.Empty)
                {
                    EditorGUILayout.HelpBox("Name Cannot Be Empty", MessageType.Error);
                }
                else
                {
                    if (bundleDescription == string.Empty)
                    {
                        EditorGUILayout.HelpBox("Description Cannot Be Empty", MessageType.Error);
                    }
                    else
                    {
                        bool nameAlreadyTaken = false;
                        for (int i = 0; i < masterChecklist.AllBundles.Count; i++)
                        {
                            if (masterChecklist.AllBundles[i].bundleName == bundleName)
                            {
                                nameAlreadyTaken = true;
                            }
                        }

                        if (nameAlreadyTaken)
                        {
                            EditorGUILayout.HelpBox("Name Already Taken Please Choose A Different Name", MessageType.Error);
                        }
                        else
                        {
                            if (GUILayout.Button("Create Catagory"))
                            {
                                CreateBundle(bundleName, bundleDescription, thumbnail);
                            } 
                        }
                    }
                }
            }
            GUILayout.EndVertical();

            #endregion
            
        }

        private void CreateBundle(string _bundleName , string _BundleDescription, Object _thumbnail)
        {
            if (masterChecklist == null)
            { 
                masterChecklist = AssetDatabase.LoadAssetAtPath("Assets/Editor/LazyHelpers/LazyCheckList/Resources/CheckListMaster.asset", typeof(LazyCheckListMaster)) as LazyCheckListMaster;
            }
            
            EditorUtility.SetDirty(masterChecklist);
            LazyCheckListBundle tempItem = CreateInstance(typeof(LazyCheckListBundle)) as LazyCheckListBundle;
            tempItem.thumbnail = (Texture)_thumbnail;
            tempItem.bundleName = _bundleName;
            tempItem.bundleDescription = _BundleDescription;

            AssetDatabase.CreateAsset(tempItem, "Assets/Editor/LazyHelpers/LazyCheckList/Resources/Bundles/"+ _bundleName +".asset");
            masterChecklist.AllBundles.Add(tempItem);
            AssetDatabase.SaveAssets();
        }

        private void EditBundles()
        {
            for (int i = 0; i < masterChecklist.AllBundles.Count; i++)
            {
                string thumbnailBeforeEdit = String.Empty;
                string beforeEdit = masterChecklist.AllBundles[i].bundleName;
                Object thumbnail = null;
                
                if (masterChecklist.AllBundles[i].thumbnail != null)
                {
                    thumbnail = masterChecklist.AllBundles[i].thumbnail;
                    thumbnailBeforeEdit = AssetDatabase.GetAssetPath(thumbnail);
                }
                
                bool isEven = i % 2 == 0;
                GUIStyle itemStyle = new GUIStyle();

                if (isEven)
                {
                    itemStyle = evenItemStyle;
                }
                else
                {
                    itemStyle = oddItemStyle;
                }

                #region Name
                GUILayout.BeginHorizontal(itemStyle);
                masterChecklist.AllBundles[i].bundleName = GUILayout.TextField(masterChecklist.AllBundles[i].bundleName, 16, GUILayout.MaxWidth(120),GUILayout.MinWidth(120));
                if (!beforeEdit.Equals(masterChecklist.AllBundles[i].bundleName))
                {
                    EditorUtility.SetDirty(masterChecklist);
                    AssetDatabase.SaveAssets();
                }
                #endregion
              
                #region Description
                masterChecklist.AllBundles[i].bundleDescription = GUILayout.TextField(masterChecklist.AllBundles[i].bundleDescription, 150, GUILayout.MinWidth(250));
                #endregion
                
                #region Thumbnail
                thumbnail = EditorGUILayout.ObjectField(masterChecklist.AllBundles[i].thumbnail, typeof(Texture), false);
                
                if (!thumbnailBeforeEdit.Equals(AssetDatabase.GetAssetPath(thumbnail)))
                {
                    EditorUtility.SetDirty(masterChecklist);
                    masterChecklist.AllBundles[i].thumbnail = (Texture) thumbnail;
                    AssetDatabase.SaveAssets();
                }
                #endregion
                

                #region Delete
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Delete Bundle"))
                {
                    EditorUtility.SetDirty(masterChecklist);
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(masterChecklist.AllBundles[i]));
                    masterChecklist.AllBundles.RemoveAt(i);
                    AssetDatabase.SaveAssets();
                }
                GUILayout.EndHorizontal();
                #endregion
            }
        }
        
        #endregion
    }
    
    public class LazyCheckList : EditorWindow
    {
        #region Editer General Values

        public static LazyCheckList window;

        #endregion
        public static LazyCheckListBundle currentBundle;
        
        public static void Init(LazyCheckListBundle _currentBundle)
        {
            currentBundle = _currentBundle;
            
            // Get existing open window or if none, make a new one:
            window = (LazyCheckList) GetWindow(typeof(LazyCheckList));
            window.titleContent.text = "Bundle Explorer: " + currentBundle.bundleName;
            window.position = new Rect(0, 0, 600, 800);
            window.autoRepaintOnSceneChange = false;

        }

        public static LazyCheckListBundle checkListItem;

        #region Styles
        public GUIStyle mainTextStyle = new GUIStyle();
        public GUIStyle secondaryTextStyle = new GUIStyle();
        public GUIStyle generalTexStyle = new GUIStyle();

        public GUIStyle evenItemStyle;
        public GUIStyle oddItemStyle;
        #endregion
        #region Textures
        private Texture2D mainBackground;
        private Texture2D secondaryBackground;
        private Texture2D seperator;
        private Texture2D oddBackground;
        private Texture2D evenBackground;
        #endregion
        #region Generation Functions

        private void OnHierarchyChange()
        {
            OnEnable();
            Repaint();
        }

        private void GenerateStyle()
        {
            string path = "Assets/Editor/LazyHelpers/Resources/HeaderFont.ttf";
            Font headerFont = EditorGUIUtility.Load(path) as Font;

            //Main Text
            mainTextStyle.normal.textColor = LazyEditorHelperUtils.LazyFridayMainColor;
            mainTextStyle.fontSize = 16;
            mainTextStyle.alignment = TextAnchor.LowerCenter;
            mainTextStyle.font = headerFont;

            //Secondary Text
            secondaryTextStyle.normal.textColor = LazyEditorHelperUtils.LazyFridayMainColor;
            secondaryTextStyle.fontSize = 12;
            secondaryTextStyle.fontStyle = FontStyle.Bold;
            secondaryTextStyle.alignment = TextAnchor.MiddleCenter;

            //General Text
            generalTexStyle.normal.textColor = LazyEditorHelperUtils.LazyFridaySecondaryColor;
            generalTexStyle.fontSize = 12;
            generalTexStyle.alignment = TextAnchor.MiddleLeft;

            //Item Styles
            oddItemStyle.normal.background = oddBackground;
            oddItemStyle.padding = new RectOffset(3, 3, 3, 3);
            oddItemStyle.border = new RectOffset(0, 0, 5, 5);
            oddItemStyle.normal.textColor = LazyEditorHelperUtils.LazyFridaySecondaryColor;

            evenItemStyle.normal.background = evenBackground;
            evenItemStyle.border = new RectOffset(0, 0, 5, 5);
            evenItemStyle.padding = new RectOffset(3, 3, 3, 3);
            evenItemStyle.normal.textColor = LazyEditorHelperUtils.LazyFridaySecondaryColor;
        }

        private void GenerateTextures()
        {
            mainBackground = new Texture2D(1, 1);
            mainBackground.SetPixel(0, 0, LazyEditorHelperUtils.LazyFridayBackgroundColor);
            mainBackground.Apply();

            secondaryBackground = new Texture2D(1, 1);
            secondaryBackground.SetPixel(0, 0, new Color32(33, 33, 33, 255));
            secondaryBackground.Apply();

            seperator = new Texture2D(1, 1);
            seperator.SetPixel(0, 0, new Color32(242, 242, 242, 255));
            seperator.Apply();

            //Item areas
            evenBackground = new Texture2D(1, 1);
            evenBackground.SetPixel(0, 0, new Color32(44, 44, 44, 255));
            evenBackground.Apply();

            oddBackground = new Texture2D(1, 1);
            oddBackground.SetPixel(0, 0, new Color32(33, 33, 33, 255));
            oddBackground.Apply();
        }

        //Start Function
        private void OnEnable()
        {
            GenerateTextures();
            GenerateStyle();
        }
        #endregion
    }
}

