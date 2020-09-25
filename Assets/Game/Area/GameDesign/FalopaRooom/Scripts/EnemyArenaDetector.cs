using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArenaDetector : MonoBehaviour
{
    bool activateDetector;
    [SerializeField] Transform _pivot = null;
    [SerializeField] float _cooldown = 3;
    [SerializeField] float _delayActivation = 3;
    [SerializeField] float _sizeOfSphere = 20;
    [SerializeField] LayerMask _mask = 0;
    List<Collider> colliders = new List<Collider>();
    public List<GameObject> walls = new List<GameObject>();

    void Start()
    {
        var _collider = GetComponents<Collider>();
        for (int i = 0; i < walls.Count; i++)
        {
            walls[i].SetActive(false);
        }
        for (int i = 0; i < _collider.Length; i++)
        {
            colliders.Add(_collider[i]);
        }
        StartCoroutine(DetectorOfEnemys());
    }

    IEnumerator DetectorOfEnemys()
    {
        while (true)
        {
            if (activateDetector)
            {
                yield return new WaitForSeconds(_cooldown);
                var radar = Physics.OverlapSphere(_pivot.position, _sizeOfSphere, _mask);
                if (radar.Length == 0)
                {
                    for (int i = 0; i < walls.Count; i++)
                    {
                        walls[i].SetActive(false);
                    }
                    activateDetector = false;
                }
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    IEnumerator ActivateDoors()
    {
        yield return new WaitForSeconds(_delayActivation);
        for (int i = 0; i < colliders.Count; i++)
        {
            colliders[i].enabled = false;
        }
        for (int i = 0; i < walls.Count; i++)
        {
            walls[i].SetActive(true);
        }
        activateDetector = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterHead>())
        {
            StartCoroutine(ActivateDoors());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_pivot.position, _sizeOfSphere);
    }

}
