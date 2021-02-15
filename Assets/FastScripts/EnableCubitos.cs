using UnityEngine;
public class EnableCubitos : MonoBehaviour
{
    public GameObject[] gos;
    public void EnableCubitosBoool(bool _val) { foreach (var g in gos) g.GetComponent<Renderer>().enabled = _val; }
}
