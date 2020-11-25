using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Linq.Expressions;

public class TentacleWall_controller : MonoBehaviour
{
    public List<TentacleWall> tentacles = new List<TentacleWall>();
    Transform characterT;

    float _count;
    [SerializeField] float intervalTimeTentacle = 12;

    bool randomTentaclesOn = false;
    CDModule timer = new CDModule();

    private void Start()
    {
        characterT = Main.instance.GetChar().Root;

        foreach (Transform item in transform)
        {
            var tentacle = item.GetComponent<TentacleWall>();

            if (tentacle != null)
                tentacles.Add(tentacle);
        }        
    }

    public void StartRandomTentacles() { randomTentaclesOn = true; }
    public void StopRandomTentacles() { randomTentaclesOn = false; _count = 0; }

    public void OpenTentacles()
    {
        for (int i = 0; i < tentacles.Count; i++)
        {
            tentacles[i].gameObject.SetActive(true);
            tentacles[i].On();
        }
    }

    public void CloseTentacles()
    {
        for (int i = 0; i < tentacles.Count; i++)
        {
            tentacles[i].CloseTentacles();
            tentacles[i].Off();
        }
    }

    public void OpenTentacles(TentacleWall[] col)
    {
        for (int i = 0; i < col.Length; i++)
        {
            col[i].gameObject.SetActive(true);
            col[i].On();
        }
    }

    public void CloseTentacles(TentacleWall[] col)
    {
        for (int i = 0; i < col.Length; i++)
        {
            col[i].CloseTentacles();
            col[i].Off();
        }
    }

    private void Update()
    {
        if (!randomTentaclesOn) return;

        timer.UpdateCD();

        _count += Time.deltaTime;

        if(_count >= intervalTimeTentacle)
        {
            _count = 0;
            var aux = tentacles.Where(t => Vector3.Distance(t.transform.position, characterT.position) <= 7).ToArray();
            OpenTentacles(aux);
            timer.AddCD("closeSelectedTentacles", () => CloseTentacles(aux), 5);
        }

    }

    public List<TentacleWall> GetAllTentacles => tentacles;

}
