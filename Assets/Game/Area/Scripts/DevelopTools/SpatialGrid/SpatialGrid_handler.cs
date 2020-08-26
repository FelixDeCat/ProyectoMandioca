using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpatialGrid_handler : MonoBehaviour
{
    public static SpatialGrid_handler instance;
    private SpatialGrid _grid;

    public float width = 15f;
    public float height = 30f;

    private void Awake() => instance = this;

    public IEnumerable<GridEntity> selected = new List<GridEntity>();

    //void Start()
    //{
    //    _grid = GetComponent<SpatialGrid>();
    //    _hero = Main.instance.GetChar();
    //    StartCoroutine(CheckGrid());
    //}

    public void SetCurrentSpatial(SpatialGrid _current)
    {
        _grid = _current;
        StartCoroutine(CheckGrid());
    }

    public void ResumeCheck()
    {
        StartCoroutine(CheckGrid());
    }

    public void StopSpatialGrid()
    {
        //_grid = null;
        StopAllCoroutines();
    }

    private void OnDisable() => StopAllCoroutines();
    private void OnDestroy() => StopAllCoroutines();

    private IEnumerator CheckGrid()
    {
        while (true)
        {
            if (Application.isPlaying)
            {
                selected = Query(Main.instance.GetChar());
                var temp = FindObjectsOfType<GridEntity>().Where(x => !selected.Contains(x));
                foreach (var item in temp)
                {
                    if (item.gameObject.activeInHierarchy)
                    {
                        item.Initialize();
                        item.Off();
                    }
                }
                foreach (var item in selected)
                {
                    if (item.gameObject.activeInHierarchy)
                    {
                        item.Initialize();
                        item.On();
                    }
                }
            }
            yield return new WaitForSeconds(.2f);
        }
    }

    public IEnumerable<GridEntity> Query(PlayObject centerObject)
    {
        var h = height * 0.5f;
        var w = width * 0.5f;
        //posicion inicial --> esquina superior izquierda de la "caja"
        //posición final --> esquina inferior derecha de la "caja"
        //como funcion para filtrar le damos una que siempre devuelve true, para que no filtre nada.

        if (centerObject == null)
            Debug.LogError("el centerObject es Null");

        if (_grid == null) Debug.LogError("La grid es nula");

        var q = _grid.Query(
            centerObject.transform.position + new Vector3(-w, 0, -h),
            centerObject.transform.position + new Vector3(w, 0, h),
            x => true);

        return q;
    }

    void OnDrawGizmos()
    {
        if (_grid == null || Main.instance.GetChar() == null)
            return;

        //Flatten the sphere we're going to draw
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(Main.instance.GetChar().transform.position, new Vector3(width, 0, height));
    }


}
