using UnityEngine;
using UnityEngine.UI;

namespace DevelopTools.UI
{
    public class DebugVisualElement : MonoBehaviour
    {
        [SerializeField] Text txt_name = null;
        [SerializeField] Text txt_value = null;
        public void Create(string _name, string initialValue) { txt_name.text = _name; txt_value.text = initialValue; }
        public void SetValue(string val) => txt_value.text = val;
    }
}