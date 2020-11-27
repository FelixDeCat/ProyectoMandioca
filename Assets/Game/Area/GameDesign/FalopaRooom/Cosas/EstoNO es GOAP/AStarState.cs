using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AStarState<T>
{
    public HashSet<T> open;
    public HashSet<T> closed;
    public Dictionary<T, float> costs;
    public Dictionary<T, float> fitnesses;
    public Dictionary<T, T> previous;
    public T current;
    public bool finished;

    public AStarState()
    {
        open = new HashSet<T>();
        closed = new HashSet<T>();
        costs = new Dictionary<T, float>();
        fitnesses = new Dictionary<T, float>();
        previous = new Dictionary<T, T>();
        current = default(T);
        finished = false;
    }

    public AStarState(AStarState<T> copy)
    {
        open = new HashSet<T>(copy.open);
        closed = new HashSet<T>(copy.closed);
        costs = new Dictionary<T, float>(copy.costs);
        fitnesses = new Dictionary<T, float>(copy.fitnesses);
        previous = new Dictionary<T, T>(copy.previous);
        current = copy.current;
        finished = copy.finished;
    }

    public AStarState<T> Clone()
    {
        return new AStarState<T>(this);
    }
}

