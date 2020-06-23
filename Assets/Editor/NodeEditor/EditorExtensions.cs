using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.EditorExtensions
{
    public static class EditorExtensions
    {
        public static bool MouseOverNode(Rect nodeRect, Event mouseEvent)
        {
            return nodeRect.Contains(mouseEvent.mousePosition);
        }
        
    }    

}

