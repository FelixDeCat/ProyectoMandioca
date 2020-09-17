using UnityEngine;
using System.Collections.Generic;

namespace PixelCrushers.SceneStreamer
{

    /// <summary>
    /// This trigger handler tells SceneStreamer about a neighboring scene. You'll generally
    /// add it to a trigger collider at the edge of a scene. When the player enters the edge,
    /// for example when entering the edge coming from a neighboring scene, the edge's scene
    /// is made the current scene.
    /// </summary>
    [AddComponentMenu("Scene Streamer/Scene Edge")]
    public class SceneEdge : MonoBehaviour
    {
        public GameObject currentSceneRoot;
        public string nextSceneName;
        public void OnTriggerEnter(Collider other)
        {
            if (currentSceneRoot && other.tag == "Player")
                SceneStreamer.SetCurrentScene(currentSceneRoot.name);
        }
    }

}