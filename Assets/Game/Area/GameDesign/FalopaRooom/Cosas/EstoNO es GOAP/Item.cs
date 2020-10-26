using UnityEngine;
using System.Collections;

namespace GOAP
{
    public enum ItemType
    {
       hero,
       skill
    }

    public class Item : MonoBehaviour
    {
        public ItemType type;
        public Waypoint _wp;

        

        public string itemName_id;

        private void Start()
        {
            GOAP_Skills_Base skill = GetComponent<GOAP_Skills_Base>();
            if (skill)
                itemName_id = skill.skillName;
            else
            {
                itemName_id = "Hero";
            }
        }

        public bool interactuable = true;

        private void Initialize()
        {
            if(Navigation.instance != null)
            {
                _wp = Navigation.instance.NearestTo(transform.position);
                _wp.nearbyItems.Add(this);
            }
            
        }

        //private void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.K))
        //        Initialize();
        //}

        //private void OnDestroy()
        //{
        //    _wp.nearbyItems.Remove(this);
        //}

        //public void Press()
        //{

        //}
    }

}

