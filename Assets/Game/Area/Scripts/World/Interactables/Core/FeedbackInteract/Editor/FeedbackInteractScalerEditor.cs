//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(FeedbackInteract_Scaler))]
//public class FeedbackInteractScalerEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        //base.OnInspectorGUI();

//        FeedbackInteract_Scaler scaler = (FeedbackInteract_Scaler)target;

//        scaler.ownTransform = EditorGUILayout.Toggle("Own Transform", scaler.ownTransform);

//        if (!scaler.ownTransform == true)
//        {
//            scaler.toscale =  EditorGUILayout.ObjectField("to scale", scaler.toscale, typeof(Transform), true) as Transform;
//        }
//    }
//}


//public class CameraMovementEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        //base.OnInspectorGUI();

//        CameraMovement cam = (CameraMovement)target;

//        cam.edgePan = EditorGUILayout.Toggle("Edge Pan", cam.edgePan);

//        if (cam.edgePan == true)
//        {
//            // Enable distFromEdge field
//            // Enable panSpeed field
//        }
//    }
//}
