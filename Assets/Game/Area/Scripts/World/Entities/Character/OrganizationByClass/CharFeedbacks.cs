using UnityEngine;

[System.Serializable]
public class CharFeedbacks
{

    //esto recien lo hice y ya tengo ganas de cambiarlo
    // quiero que esto herede de LoadComponent y se encargue de inizializar
    // todos los feedbacks, y que estos tambien sean LoadComponent

    public CharSounds sounds;
    public CharParticles particles;
    public CustomCamera customCam;

    public void Initialize()
    {
        sounds.Initialize();
        particles.Initialize();
    }
}
