using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FindNodesAndAdd : MonoBehaviour
{
    public bool add;
    public bool execute;
    public bool render;
    public ManagerNodes manager;

    bool canrender;

    float timer;

    public void OnEnable()
    {
       // execute = false;
    }

    private void Update()
    {
        if (!execute) return;

        if (timer < 0.2f)
        {
            timer = timer + 1 * Time.deltaTime;
        }
        else
        {
            Debug.Log("JJEJEJCUTANDO;");
            timer = 0;
            //manager.Initialize();
        }


        if (add)
        {
            add = false;

            var nodes = FindObjectsOfType<IA_Felix.Node>();

            foreach (var n in nodes)
            {
                n.transform.SetParent(this.transform);
            }
        }
        
        if (render)
        {
            render = false;
            canrender = !canrender;
            manager.Render(canrender);

        }
        
    }
}
