using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpatialGrid_handler : MonoBehaviour
{
    private SpatialGrid _grid;
    private CharacterHead _hero;

    public float width = 15f;
    public float height = 30f;

    public IEnumerable<GridEntity> selected = new List<GridEntity>();

    void Start()
    {
        _grid = GetComponent<SpatialGrid>();
        _hero = Main.instance.GetChar();
        
    }

    private void Update()
    {

        if (Application.isPlaying)
        {
            selected = Query(_hero);
            var temp = FindObjectsOfType<GridEntity>().Where(x=>!selected.Contains(x));
            foreach (var item in temp)
            {
                if(item.gameObject.activeInHierarchy)
                    item.GetComponent<PlayObject>()?.Off();
                //item.onGrid = false;
            }
            foreach (var item in selected)
            {
                if(!item.gameObject.activeInHierarchy)
                    item.GetComponent<PlayObject>()?.On();
                //item.onGrid = true;
            }

        }
    }

    public IEnumerable<GridEntity> Query(PlayObject centerObject)
    {
        var h = height * 0.5f;
        var w = width * 0.5f;
        //posicion inicial --> esquina superior izquierda de la "caja"
        //posición final --> esquina inferior derecha de la "caja"
        //como funcion para filtrar le damos una que siempre devuelve true, para que no filtre nada.
        
        var q = _grid.Query(
            centerObject.transform.position + new Vector3(-w, 0, -h),
            centerObject.transform.position + new Vector3(w, 0, h),
            x => true);

        return q;
    }

    void OnDrawGizmos()
    {
        if (_grid == null)
            return;

        //Flatten the sphere we're going to draw
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(_hero.transform.position, new Vector3(width, 0, height));
    }
    
    
}
