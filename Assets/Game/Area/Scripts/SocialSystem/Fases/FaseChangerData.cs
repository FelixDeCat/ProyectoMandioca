using UnityEngine;
[System.Serializable]
public struct FaseChangerData
{
    [SerializeField] int iDFase;
    [SerializeField] int indexFase;
    public int IDFase { get => iDFase; }
    public int IndexToChange { get => indexFase; }
}
