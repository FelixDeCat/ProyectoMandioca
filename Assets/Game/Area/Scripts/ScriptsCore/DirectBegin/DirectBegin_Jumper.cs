/////////////////////////////////////////////////////////////
/// Este script funciona en conjunto con el DirectBegin_Scene
/// este script va a estar unicamente en la escena de Carga
/// cuando entra a la escena se va a fijar si ese script existe
/// si es asi, este avisa con un event y hace que se Saltee la
/// escena de Menu
/////////////////////////////////////////////////////////////
namespace Tools.Testing
{
    using UnityEngine;
    public class DirectBegin_Jumper : MonoBehaviour
    {
        public void Start()
        {
            var aux = FindObjectOfType<DirectBegin_Scene>();

            // si entra acá es porque alguna escena
            // me trajo hasta la escena de carga
            if (aux != null)
            {
                Scenes.Load(aux.data.SceneToJump);
            }
        }
    }
}


