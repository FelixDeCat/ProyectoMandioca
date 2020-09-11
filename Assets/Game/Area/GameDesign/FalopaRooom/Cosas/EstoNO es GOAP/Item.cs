using UnityEngine;
using System.Collections;

namespace GOAP
{
    public enum ItemType
    {
        Invalid,
        Key,
        Door,
        Entity,
        Mace,
        PastaFrola,
        Button,
        Hp,
        Coin,
        CoinDoor,
        PressurePlate,
        KeyDoor,
        PatrolPoint,
        Dude,
        HideSpot
    }

    public class Item : MonoBehaviour
    {
        public ItemType type;
        public Waypoint _wp;

        public bool interactuable = true;

        private void Start()
        {
            _wp = Navigation.instance.NearestTo(transform.position);   
            _wp.nearbyItems.Add(this);
        }

        //private void OnDestroy()
        //{
        //    _wp.nearbyItems.Remove(this);
        //}

        public void Press()
        {

        }
    }

}

