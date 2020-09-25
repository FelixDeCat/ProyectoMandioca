using UnityEngine;
[System.Serializable]
public struct FaseChangerData
{
    [SerializeField] private int iDFase;
    [SerializeField] private int indexFase;
    public int IDFase { get => iDFase; }
    public int IndexToChange { get => indexFase; }
}
