using UnityEngine;
using System.Collections.Generic;

namespace DungeonGenerator.Components
{
    public class Wing : MonoBehaviour
    {

        bool isActive;

        public List<GameObject> toEnable;
        public List<GameObject> toDisable;

        public bool IsActive
        {
            get => isActive;
            set
            {
                isActive = value;
                if (value) Activar();
                else Desactivar();
            }
        }

        private void Awake()
        {
            IsActive = false;
        }

        void Activar()
        {
            toEnable.ForEach(x => x.SetActive(true));
            toDisable.ForEach(x => x.SetActive(false));
        }
        void Desactivar()
        {
            toEnable.ForEach(x => x.SetActive(false));
            toDisable.ForEach(x => x.SetActive(true));
        }
    }

}
