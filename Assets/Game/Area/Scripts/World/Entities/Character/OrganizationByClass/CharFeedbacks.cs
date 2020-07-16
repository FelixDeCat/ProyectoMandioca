using UnityEngine;

[System.Serializable]
public class CharFeedbacks
{
    public CharSounds sounds;
    public CharParticles particles;
    public CustomCamera customCam;

    public void Initialize()
    {
        sounds.Initialize();
        particles.Initialize();
    }
}
