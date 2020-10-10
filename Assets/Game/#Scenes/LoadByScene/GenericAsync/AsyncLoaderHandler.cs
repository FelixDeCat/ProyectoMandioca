using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class AsyncLoaderHandler : GenericAsyncLocalScene
{
    public bool find_components = true;
    public LoadComponent[] components = new LoadComponent[0];
    
    private void Update()
    {
        if (find_components)
        {
            find_components = false;
            components = GetComponentsInChildren<LoadComponent>();
            HashSet<LoadComponent> comps = new HashSet<LoadComponent>(components);
            comps.Remove(this);
            components = comps.ToArray();
        }
    }

    protected override IEnumerator AsyncLoad()
    {
        for (int i = 0; i < components.Length; i++)
        {
            yield return components[i]?.Load();
        }
        yield return null;
    }

    protected override void AsyncLoadEnded()
    {
       
    }

    protected override void OnEnter()
    {
        
    }

    protected override void OnExit()
    {
       
    }
}
