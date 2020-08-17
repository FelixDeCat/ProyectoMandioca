using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisionsDataBase : MonoBehaviour
{
    public static MisionsDataBase instance; private void Awake() => instance = this;
    public Mision[] misionsdatabase = new Mision[0];

    public Mision GetMision(int ID)
    {
        for (int i = 0; i < misionsdatabase.Length; i++)
        {
            if (ID == misionsdatabase[i].id_mision)
            {
                return misionsdatabase[i];
            }
        }
        return null;
    }
}
