using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionHole : MonoBehaviour
{
    CharacterHead character;
    [SerializeField] float attractionForce = 500;
    [SerializeField] ParticleSystem holeParticle = null;
    [SerializeField] Transform centerPoint = null;
    [SerializeField] ForceMode mode = ForceMode.Acceleration;
    [SerializeField, Range(-1, 1)] int forceDirection = 1;

    [SerializeField] ParticleSystem attFeedback = null;
    bool on;
    bool isZero;

    protected void Awake()
    {
        on = true;
        var go = Instantiate(attFeedback);
        go.gameObject.SetActive(false);
        attFeedback = go;
    }

    private void Update()
    {
        if (!on) return;

        if (character)
        {
            Vector3 att = (centerPoint.position - character.transform.position) * forceDirection;
            att.y = 0.5f;

            if (att.x > -0.3f && att.x < 0.3f && att.z > -0.3f && att.z < 0.3f)
            {
                if(forceDirection == 1 && !isZero)
                {
                    character.GetCharMove().StopForce();
                    isZero = true;
                }
                else if (forceDirection == -1)
                {
                    att = Vector3.right;
                }
            }
            else
                isZero = false;

            if(!isZero)
                character.GetCharMove().MovementAddForce(att.normalized, attractionForce, mode);
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
        {
            character = other.GetComponent<CharacterHead>();
            if (attFeedback)
            {
                attFeedback.gameObject.SetActive(true);
                attFeedback.transform.position = character.transform.position;
                attFeedback.transform.SetParent(character.transform);
                attFeedback.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterHead>())
        {
            character?.GetCharMove().StopForceBool();
            character = null;
            isZero = false;

            if (attFeedback)
            {
                attFeedback.Stop();
                attFeedback.gameObject.SetActive(false);
            }
        }
    }
}
