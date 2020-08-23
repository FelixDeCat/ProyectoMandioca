using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Tools.Extensions;

public class SpatialGrid : MonoBehaviour
{
    #region Variables
    //punto de inicio de la grilla en X
    public float x;
    //punto de inicio de la grilla en Z
    public float z;
    //ancho de las celdas
    public float cellWidth;
    //alto de las celdas
    public float cellHeight;
    //cantidad de columnas (el "ancho" de la grilla)
    public int width;
    //cantidad de filas (el "alto" de la grilla)
    public int height;

    //ultimas posiciones conocidas de los elementos, guardadas para comparación.
    private Dictionary<GridEntity, Tuple<int, int>> lastPositions;
    //los "contenedores"
    private HashSet<GridEntity>[,] buckets;

    //el valor de posicion que tienen los elementos cuando no estan en la zona de la grilla.
    /*
     Const es implicitamente statica
     const tengo que ponerle el valor apenas la declaro, readonly puedo hacerlo en el constructor.
     Const solo sirve para tipos de dato primitivos.
     */
    readonly public Tuple<int, int> Outside = Tuple.Create(-1, -1);

    //Una colección vacía a devolver en las queries si no hay nada que devolver
    readonly public GridEntity[] Empty = new GridEntity[0];
    #endregion

    #region FUNCIONES
    private void Awake()
    {
        lastPositions = new Dictionary<GridEntity, Tuple<int, int>>();
        buckets = new HashSet<GridEntity>[width, height];

        //creamos todos los hashsets
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                buckets[i, j] = new HashSet<GridEntity>();

        //P/alumnos: por que no puedo usar OfType<>() despues del RecursiveWalker() aca?
        var ents = RecursiveWalker(transform)
            .Select(x => x.GetComponent<GridEntity>())
            .Where(x => x != null);

        foreach (var e in ents)
        {
            UpdateEntity(e);
        }

        SpatialGrid_handler.instance.SetCurrentSpatial(this);
    }

    private void RefreshGrid()
    {
        //P/alumnos: por que no puedo usar OfType<>() despues del RecursiveWalker() aca?
        var ents = RecursiveWalker(transform)
            .Select(x => x.GetComponent<GridEntity>())
            .Where(x => x != null);

        foreach (var e in ents)
        {
            UpdateEntity(e);
        }
    }

    public void UpdateEntity(GridEntity entity)
    {
        
        var lastPos = lastPositions.ContainsKey(entity) ? lastPositions[entity] : Outside;
        var currentPos = GetPositionInGrid(entity.gameObject.transform.position);

        //si la ultima posicion almacenada es igual a la nueva
        //no hago nada
        if (lastPos.Equals(currentPos))
            return;

        //ahora, si me da una posicion nueva hay que actualizar las cosas
        
        //si la almacenada esta dentro de la grilla
        //la quito del bucket
        if (IsInsideGrid(lastPos))
            buckets[lastPos.Item1, lastPos.Item2].Remove(entity);

        //entonces...

        //una vez quitada la vieja
        //metemos la nueva al bucket
        //y piso la almacenada con la nueva
        if (IsInsideGrid(currentPos))
        {
            
            buckets[currentPos.Item1, currentPos.Item2].Add(entity);
            lastPositions[entity] = currentPos;
        }
        else //si la nueva esta fuera de la grid, directamente quito del registro la entidad
            lastPositions.Remove(entity);
        
    }

    public IEnumerable<GridEntity> Query(Vector3 aabbFrom, Vector3 aabbTo, Func<Vector3, bool> filterByPosition)
    {
        //como no sabemos cual de las dos posiciones es cada esquina,
        //obtengo con los min y max cual el es From y cual el To
        var from = new Vector3(Mathf.Min(aabbFrom.x, aabbTo.x), 0, Mathf.Min(aabbFrom.z, aabbTo.z));
        var to = new Vector3(Mathf.Max(aabbFrom.x, aabbTo.x), 0, Mathf.Max(aabbFrom.z, aabbTo.z));

        //traduzco el from y to a posicion de grilla
        var fromCoord = GetPositionInGrid(from);
        var toCoord = GetPositionInGrid(to);

        //¡Ojo que clampea a 0,0 el Outside! TODO: Checkear cuando descartar el query si estan del mismo lado
        fromCoord = Tuple.Create(Utility.Clampi(fromCoord.Item1, 0, width), Utility.Clampi(fromCoord.Item2, 0, height));
        toCoord = Tuple.Create(Utility.Clampi(toCoord.Item1, 0, width), Utility.Clampi(toCoord.Item2, 0, height));

        if (!IsInsideGrid(fromCoord) && !IsInsideGrid(toCoord))
            return Empty;
        
        // Creamos tuplas de cada celda
        var cols = Generate(fromCoord.Item1, x => x + 1)
            .TakeWhile(x => x < width && x <= toCoord.Item1);

        var rows = Generate(fromCoord.Item2, y => y + 1)
            .TakeWhile(y => y < height && y <= toCoord.Item2);

        var cells = cols.SelectMany(
            col => rows.Select(
                row => Tuple.Create(col, row)
            )
        );

        var entities = cells.SelectMany(cell => buckets[cell.Item1, cell.Item2])
            .Where(e => e.gameObject != null);

        Debug.Log(entities.Count() + "entes");

        // Iteramos las que queden dentro del criterio
        var c = entities
            .Where(e =>
                from.x <= e.transform.position.x && e.transform.position.x <= to.x &&
                from.z <= e.transform.position.z && e.transform.position.z <= to.z
            ).Where(x => filterByPosition(x.transform.position));
        
        return c;
    }

    public Tuple<int, int> GetPositionInGrid(Vector3 pos)
    {
        //quita la diferencia, divide segun las celdas y floorea
        return Tuple.Create(Mathf.FloorToInt((pos.x - x) / cellWidth),
                            Mathf.FloorToInt((pos.z - z) / cellHeight));
    }

    public bool IsInsideGrid(Tuple<int, int> position)
    {
        //si es menor a 0 o mayor a width o height, no esta dentro de la grilla
        return 0 <= position.Item1 && position.Item1 < width &&
            0 <= position.Item2 && position.Item2 < height;
    }

    void OnDestroy()
    {
        var ents = RecursiveWalker(transform).Select(x => x.GetComponent<GridEntity>()).Where(x => x != null);

        SpatialGrid_handler.instance.StopSpatialGrid();
        //foreach (var e in ents)
        //e.OnMove -= UpdateEntity;
    }

    #region GENERATORS
    private static IEnumerable<Transform> RecursiveWalker(Transform parent)
    {
        foreach (Transform child in parent)
        {
            foreach (Transform grandchild in RecursiveWalker(child))
                yield return grandchild;
            yield return child;
        }
    }

    IEnumerable<T> Generate<T>(T seed, Func<T, T> mutate)
    {
        T accum = seed;
        while (true)
        {
            yield return accum;
            accum = mutate(accum);
        }
    }
    #endregion

    #endregion

    #region GRAPHIC REPRESENTATION
    public bool AreGizmosShutDown;
    public bool activatedGrid;
    public bool showLogs = true;
    private void OnDrawGizmos()
    {
        var rows = Generate(z, curr => curr + cellHeight)
                .Select(row => Tuple.Create(new Vector3(x, 0, row),
                                            new Vector3(x + cellWidth * width, 0, row)));

        //equivalente de rows
        /*for (int i = 0; i <= height; i++)
        {
            Gizmos.DrawLine(new Vector3(x, 0, z + cellHeight * i), new Vector3(x + cellWidth * width,0, z + cellHeight * i));
        }*/

        var cols = Generate(x, curr => curr + cellWidth)
                   .Select(col => Tuple.Create(new Vector3(col, 0, z), new Vector3(col, 0, z + cellHeight * height)));

        var allLines = rows.Take(width + 1).Concat(cols.Take(height + 1));

        foreach (var elem in allLines)
        {
            Gizmos.DrawLine(elem.Item1, elem.Item2);
        }

        if (buckets == null || AreGizmosShutDown) return;

//        var originalCol = GUI.color;
//        GUI.color = Color.red;
//        if (!activatedGrid)
//        {
//            IEnumerable<GridEntity> allElems = Enumerable.Empty<GridEntity>();
//            foreach(var elem in buckets)
//                allElems = allElems.Concat(elem);
//
//            int connections = 0;
//            foreach (var ent in allElems)
//            {
//                foreach(var neighbour in allElems.Where(x => x != ent))
//                {
//                    Gizmos.DrawLine(ent.transform.position, neighbour.transform.position);
//                    connections++;
//                }
//                if(showLogs)
//                    Debug.Log("tengo " + connections + " conexiones por individuo");
//                connections = 0;
//            }
//        }
//        else
//        {
//            int connections = 0;
//            foreach (var elem in buckets)
//            {
//                foreach(var ent in elem)
//                {
//                    foreach (var n in elem.Where(x => x != ent))
//                    {
//                        Gizmos.DrawLine(ent.transform.position, n.transform.position);
//                        connections++;
//                    }
//                    if(showLogs)
//                        Debug.Log("tengo " + connections + " conexiones por individuo");
//                    connections = 0;
//                }
//            }
//        }
//
//        GUI.color = originalCol;
//        showLogs = false;
    }
    #endregion
}
