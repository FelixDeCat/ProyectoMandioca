using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraCinematic))]
public class CameraCinematicEditor : Editor
{
}
/*
    CameraCinematic _cinematic;
    public GameObject previewCam;
    void OnEnable()
    {
        _cinematic = (CameraCinematic)target;
        SceneView.duringSceneGui += OnSceneGUI;

    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
    void OnSceneGUI(SceneView sv)
    {
        if (previewCam == null) //Check if any preview objects have been removed
        {
            //If so, remove all the rest of them
            //Removed for size
            //Create all the preview objects again.
            //Create Camera
            previewCam = new GameObject();
            previewCam.hideFlags = HideFlags.HideAndDontSave;
            previewCam.name = "PreviewCamera";
            Camera CamComp = previewCam.AddComponent<Camera>();
            CamComp.depth = -20;
            previewCam.transform.position = _cinematic.transform.position;

            Rect PrevRect = EditorGUILayout.GetControlRect(GUILayout.Width(200), GUILayout.Height(200));
            Handles.BeginGUI();
            Handles.SetCamera(CamComp);
            Handles.DrawCamera(PrevRect, CamComp); //Render the camera and draw it in place (The problem zone)
            Handles.EndGUI();

        }
    }
}
/*
    void OnGUI()
    {
        previewCam = (Camera)EditorGUILayout.ObjectField(previewCam, typeof(Camera), true);
        if (previewCam)
        {
            Handles.DrawCamera(new Rect(10, 10, 200, 200), previewCam, DrawCameraMode.Normal);
        }
    }
/*

public class ShaderSandwich : EditorWindow
{
    public ShaderSandwich windowG;
    static public ShaderSandwich windowS;
    [MenuItem("Window/Shader Sandwich")]
    public static void Init()
    {
        //Normal window initialisation stuff (WindowS is a static version of WindowG, for simplicity in accessing it from other classes)
        windowS = (ShaderSandwich)EditorWindow.GetWindow(typeof(ShaderSandwich));
        windowS.windowG = windowS;
        windowS.wantsMouseMove = true;
        windowS.position = new Rect(100, 100, 1000, 700);
        windowS.title = "Shader Sandwich";
        windowS.windowG.OnEnable();
    }
    void OnGUI()
    {
        windowS = windowG;
        BeginWindows();
        //Draw the window which the preview is in
        Rect PreviewRect = new Rect(0, window.position.height - 210, 300, 210);
        GUILayout.Window(5, PreviewRect, WindowPreview, "Shader Preview");

        EndWindows();
        Repaint();//Repaint afterwards for dynamically updating sliders and such...
    }
    //Preview objects
    public GameObject previewCam;
    public GameObject previewObject;
    public Mesh previewMesh;
    public Material previewMat;
    void WindowPreview(int a)
    {
        ShaderSandwich window = windowG;
        if (previewCam == null || previewObject == null) //Check if any preview objects have been removed
        {
            //If so, remove all the rest of them
            //Removed for size
            //Create all the preview objects again.
            //Create Camera
            previewCam = new GameObject();
            previewCam.hideFlags = HideFlags.HideAndDontSave;
            previewCam.name = "PreviewCamera";
            Camera CamComp = previewCam.AddComponent("Camera") as Camera;
            CamComp.depth = -20;
            previewCam.AddComponent("PreviewCameraSet");
            previewCam.transform.position = new Vector3(0, 0, -2.175257f);
            //Create Material
            previewMat = new Material(Shader.Find(OpenShader.ShaderName + "Temp"));
            //Create Object
            //Removed for size
        }
        previewObject.renderer.material = previewMat;
        UpdatePreviewMaterial(); // Just fills the material with all sorts of parameters(Not put here because its a bit long and pointless)

        //Draw some controls for the preview, Background Color and the Mesh drawn.
        GUILayout.BeginHorizontal();
        previewCam.camera.backgroundColor = EditorGUILayout.ColorField("Background", previewCam.camera.backgroundColor);
        previewMesh = (Mesh)EditorGUILayout.ObjectField(previewMesh, typeof(Mesh), false);
        (previewObject.GetComponent(typeof(MeshFilter)) as MeshFilter).mesh = previewMesh;
        GUILayout.EndHorizontal();

        Rect PrevRect = EditorGUILayout.GetControlRect(GUILayout.Width(200), GUILayout.Height(200));
        /////////////////////////////////////////////////////Problem Zone!
        Handles.BeginGUI();
        Handles.DrawCamera(PrevRect, previewCam.camera); //Render the camera and draw it in place (The problem zone)
        Handles.EndGUI();
        /////////////////////////////////////////////////////Problem Zone!
    }
}
*/