using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AStarState<Node>
{
    public HashSet<Node> open;
    public HashSet<Node> closed;
    public Dictionary<Node, float> gs;
    public Dictionary<Node, float> fs;
    public Dictionary<Node, Node> previous;
    public Node current;
    public bool finished;

    public AStarState()
    {
        open = new HashSet<Node>();
        closed = new HashSet<Node>();
        gs = new Dictionary<Node, float>();
        fs = new Dictionary<Node, float>();
        previous = new Dictionary<Node, Node>();
        current = default(Node);
        finished = false;
    }

    public AStarState(AStarState<Node> copy)
    {
        open = new HashSet<Node>(copy.open);
        closed = new HashSet<Node>(copy.closed);
        gs = new Dictionary<Node, float>(copy.gs);
        fs = new Dictionary<Node, float>(copy.fs);
        previous = new Dictionary<Node, Node>(copy.previous);
        current = copy.current;
        finished = copy.finished;
    }

    public AStarState<Node> Clone()
    {
        return new AStarState<Node>(this);
    }
}

