using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevelopTools.UI
{

    public class DebugVisualSpace : MonoBehaviour
    {
        private Dictionary<string, DebugVisualElement> mychilds = new Dictionary<string, DebugVisualElement>();
        [SerializeField] Text myNameSpace = null;
        [SerializeField] GameObject model = null;
        [SerializeField] Transform parent = null;
        bool isHide = false;
        public void SetNameSpace(string _myNameSpace) => myNameSpace.text = _myNameSpace;
        public void SetValueChild(string _nameChild, string _value)
        {
            if (isHide) return;
            if (!mychilds.ContainsKey(_nameChild))
            {
                GameObject go = GameObject.Instantiate(model);
                go.transform.SetParent(parent);
                go.transform.position = parent.position;
                go.transform.localScale = new Vector3(1, 1, 1);
                var aux = go.GetComponent<DebugVisualElement>();
                aux.Create(_nameChild, _value);
                mychilds.Add(_nameChild, aux);
            }
            else mychilds[_nameChild].SetValue(_value);
        }
        public void BUTTON_ToogleHide() { isHide = !isHide; parent.gameObject.SetActive(!isHide); }


    }
}
