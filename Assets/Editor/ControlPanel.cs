using UnityEngine;
using UnityEditor;

public class ControlPanel : EditorWindow
{
    //public int someProp;
    //public string testString = "";
    //public string testString2 = "";

    [MenuItem("Window/ControlPanel")] //this attribute put this function in a given menu path


    //it's static as we have single instance of the class
    public static void Draw()
    {
        //return the last window opened on screen of this type(I dont understand)
        GetWindow<ControlPanel>(); //for now it crates a "new" window for us
    }

    private void OnEnable()
    {

    }

    //update when we interact with the window
    private void OnGUI()
    {

        //GUILayout.Label("Some label", EditorStyles.boldLabel);

        //testString = EditorGUILayout.TextField("Text", testString /*text to show in the field*/ );
        //someProp = int.Parse(EditorGUILayout.TextField("Int", someProp.ToString() /*text to show in the field*/ ));

        if (GUILayout.Button("DeleteData"))
        {
            GameData.Delete();
        }
    }

}
