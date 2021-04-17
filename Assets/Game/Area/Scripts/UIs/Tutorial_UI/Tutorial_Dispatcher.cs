using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Dispatcher : MonoBehaviour
{
    [SerializeField] TutorialSettings settings = null;

    public void DispatchTutorial()
    {
        Tutorial_UIController.instance.SetNewTutorial(settings);
        enabled = false;
    }
}
