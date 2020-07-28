using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionHole : MonoBehaviour
{
    CharacterHead character;
    [SerializeField] float attractionForce = 500;
    [SerializeField] ParticleSystem holeParticle = null;
    [SerializeField] Transform centerPoint = null;
    bool on;
    bool isZero;

    protected void Awake()
    {
        on = true;
    }

    private void Update()
    {
        if (!on) return;

        if (character)
        {
            Vector3 att = centerPoint.position - character.transform.position;

            if (att.x > -0.3f && att.x < 0.3f && att.z > -0.3f && att.z < 0.3f)
            {
                if (!isZero)
                {
                    character.GetCharMove().StopForce();
                    isZero = true;
                }
            }
            else
                isZero = false;

            if(!isZero)
                character.GetCharMove().MovementAddForce(att.normalized, attractionForce);
        }
    }

    public void OnOffTrap(bool b)
    {
        on = b;

        if (b) holeParticle?.Play();
        else holeParticle?.Stop();

        if (!b) character?.GetCharMove().StopForceBool();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterHead>())
            character = other.GetComponent<CharacterHead>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterHead>())
        {
            character?.GetCharMove().StopForceBool();
            character = null;
            isZero = false;
        }
    }
}
