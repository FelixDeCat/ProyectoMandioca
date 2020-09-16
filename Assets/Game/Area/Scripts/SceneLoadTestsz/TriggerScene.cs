using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SM {

    public class TriggerScene : MonoBehaviour {
        [SerializeField] UnityEvent OnTriggerEnterEvent = null;
        [SerializeField] UnityEvent OnTriggerExitEvent = null;

        public void OnTriggerEnter( Collider other ) {
            OnTriggerEnterEvent.Invoke();
            SceneManager._sceneManager.LoadScene();

        }
        public void OnTriggerExit( Collider other ) {
            OnTriggerExitEvent.Invoke();
        }
    }

}
