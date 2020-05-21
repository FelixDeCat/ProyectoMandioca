using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Investigator : MonoBehaviour, ISoundListener
{

    public Vector3 pos;
    public RigidbodyPathFinder pathfinder;
    public LineOfSightOfType<RigInspectable> line_of_Sight_Behaviour;

    //public Follow follow;

    public List<Inspectable> inspectablesPriority = new List<Inspectable>();

    public List<Search> searchs = new List<Search>();
    bool active_search;
    Search current_search;

    public Inspectable current;
    RigInspectable currentRig;

    Action Nothing;
    Action Find;

    [Header("For Line of Sight")]
    public LayerMask layermask;

    
    
    Enem enem;

    float timer;

    bool search;
    bool investigate;
    bool run_investigator;
    bool isvectorbranch;
    bool findinmyzone;
    bool begin;

    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    #region Initialization
    public void Initialize(Enem enem)
    {
        //line_of_Sight_Behaviour = new LineOfSightOfType<RigInspectable>();

        //this.enem = enem;

        //line_of_Sight_Behaviour.Configurate(enem.rb.transform,
        //                                    ObjectFinded,
        //                                    Condition,
        //                                    layermask);

        //follow.Configure(enem.rb, enem.speed / 2, PersecutionBehavioursEnded);
        
    }

    #endregion
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    #region State Conection
    public void RunInvestigator(Action _callbackNothing, Action callbackFind)
    {
        Find = callbackFind;
        Nothing = _callbackNothing;
        run_investigator = true;
    }
    public void StopInvestigator()
    {
        pathfinder.Cancel();
        Nothing = delegate { };
        Find = delegate { };
       // enem.icon.Deactivate();
        timer = 0;
        begin = false;
        run_investigator = false;
        findinmyzone = false;
        investigate = false;
        pathfinder.RemoveCallbackEnd();
    }
    public void RefreshInvestigator()
    {
        if (!run_investigator) return;
        FindNewsSearchs();

        if (!active_search) return;
        RefreshTheSearch();
        RefreshInvestigation();
    }
    #endregion
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    #region New entries
    //INTERNOS
    public void FindNewsSearchs()
    {
        line_of_Sight_Behaviour.FindNews();
    }
    private bool Condition(RigInspectable arg)
    {
        if (!inspectablesPriority.Contains(arg.referencia_de_MiInspectable))
        {
            if (arg.referencia_de_MiInspectable.something_wrong_with_this)
            {
                if (arg.referencia_de_MiInspectable.CanInspect())
                {
                    return true;
                }
            }
        }
        return false;
    }
    private void ObjectFinded(RigInspectable obj) => AddNewSearch(obj.referencia_de_MiInspectable, Vector3.zero, true);
    //EXTERNOS
    public void OnSoundListen(Inspectable insp) => AddNewSearch(insp, Vector3.zero, true);
    public void OnSoundListen(Vector3 position) => AddNewSearch(null, position, true);

    public void AddNewSearch(Inspectable insp, Vector3 v3, bool forinspectable)
    {
        Find.Invoke();
        
        Search new_search = new Search();
        if (forinspectable)
        {
            new_search.priority = insp.level_priority;
            new_search.inspectable = insp;
            new_search.isvector = false;
            new_search.pos = insp.posToInspect.position;
            new_search.rig = insp.GetComponentInChildren<RigInspectable>();
        }
        else
        {
            new_search.priority = 1;
            new_search.inspectable = null;
            new_search.isvector = true;
            new_search.pos = v3;
            new_search.rig = null;
        }
        if (IsRepeated(new_search)) return;
        searchs.Add(new_search);
        StartCheck();
    }
    public bool IsRepeated(Search search)
    {
        if (!search.isvector)
        {
            foreach (var s in searchs)
                if (s.inspectable.Equals(search.inspectable))
                    return true;
        }
        else
        {
            foreach (var s in searchs)
                if (Vector3.Distance(s.pos, search.pos) < 1)
                    return true;
        }
        return false;
    }
    [System.Serializable]
    public class Search
    {
        public int priority = 0;
        public bool isvector;
        public Vector3 pos;
        public Inspectable inspectable;
        public RigInspectable rig;
    }

    #endregion
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    #region Checking
    public void StartCheck()
    {
        //if (enem.ISeeAEnemy) return;

        //var best_search = GetBestSearch();
        //if (best_search.priority == -1)
        //{
        //    Debug.Log("Saliendo de la biusqueda");
        //    Nothing.Invoke();
        //    return;
        //}


        //AssignAsCurrent(best_search);
        //SetValuesToBegin();
    }
    void AssignAsCurrent(Search _search)
    {
        current_search = _search;
        active_search = true;
    }
    void RemoveSearch()
    {
        searchs.Remove(current_search);
        current_search.inspectable.MarkAsInspected();
        current_search.inspectable.UnMarkSomethingWrong();
        current_search = null;
        active_search = false;
        StartCheck();
    }

    public Search GetBestSearch()
    {
        int level_priority = int.MinValue;
        Search best = new Search();
        best.priority = -1;
        foreach (var i in searchs)
        {
            if (i.priority > level_priority)
            {
                level_priority = i.priority;
                best = i;
                return i;
            }
        }
        return best;
    }
    #endregion
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    #region Searching
    
    public void SetValuesToBegin()
    {
        isvectorbranch = current_search.isvector;
        pathfinder.AddCallbackEnd(PersecutionBehavioursEnded);

        if (isvectorbranch)
        {
            pathfinder.Execute(current_search.pos);
            search = true;
        }
        else
        {
            //follow.SetTarget(current_search.pos);

            if (line_of_Sight_Behaviour.OnSight(current_search.rig))
            {
                search = false;
            }
            else
            {
                pathfinder.Execute(current_search.pos);
                search = true;
            }
        }

        begin = true;
    }
    void RefreshTheSearch()
    {
        if (!begin) return;

        //if (search) pathfinder.Refresh();
        //else follow.Execute();
    }

    void PersecutionBehavioursEnded()
    {
        if (isvectorbranch)
        {
            findinmyzone = true;
        }
        else
        {
            findinmyzone = false;
            current_search.inspectable.Configure(EndInspect, ValueInspect, AlreadyInspected, Message);
            current_search.inspectable.Inspect();

            //if (line_of_Sight_Behaviour.OnSight(current_search.rig))
            //{
            //    Debug.Log("EMPEZANDO A INSPECCIONAR");
            //    findinmyzone = false;
            //    current_search.inspectable.Configure(EndInspect, ValueInspect, AlreadyInspected);
            //    current_search.inspectable.Inspect();
            //}
            //else findinmyzone = true;
        }
        investigate = true;
    }
    void RefreshInvestigation()
    {
        if (!begin) return;
        if (!investigate) return;
        if (current_search.inspectable != null) current_search.inspectable.RefreshInspect();
        if (findinmyzone) { RefreshFindInMyZone(); }
    }
 
    public void RefreshFindInMyZone()
    {
        if (!begin) return;
        if (!investigate) return;
        if (!findinmyzone) return;

        if (timer < 3f)
        {
        //    timer = timer + 1 * Time.deltaTime;
         //   enem.rb.transform.Rotate(0, 10, 0);
        }
        else
        {
            begin = false;
            timer = 0;
            findinmyzone = false;
            Exit();
            return;
        }

        if (!isvectorbranch)
        {
            if (line_of_Sight_Behaviour.OnSight(current_search.rig))
            {
                PersecutionBehavioursEnded();
                findinmyzone = false;
                timer = 0;
                return;
            }
        }
    }

    void Exit()
    {
        Debug.LogWarning("SALIENDO");
        begin = false;
        RemoveSearch();
    }

    #endregion

    /////////////////////////////////////////////////////
    /// FUNCTIONS RETURNED BY INSPECTABLE
    /////////////////////////////////////////////////////
    public void EndInspect()
    {
        Exit();
    }
    public void Message(string message)
    {
        Debug.LogWarning("Message: " + message);
    }
    public void ValueInspect(float max, float current)
    {
     //   enem.icon.Set(enem.rb.transform.position, UI_World_Feedback.icon.inspect, max, current);
    }
    public void AlreadyInspected()
    {
        Exit();
    }
}


