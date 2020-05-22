using UnityEngine;
namespace DungeonGenerator.Core
{
    public class Constantes
    {
        public static readonly int[] dirX = { -1, 0, 1, 0 };
        public static readonly int[] dirZ = { 0, 1, 0, -1 };

        static int op;
        public static int GetOpuesto(int id)
        {
            if (id == 0) op = 2;
            if (id == 1) op = 3;
            if (id == 2) op = 0;
            if (id == 3) op = 1;
            return op;
        }

        public static readonly Vector3 empty = new Vector3(-1, -1, -1);
    }
}
