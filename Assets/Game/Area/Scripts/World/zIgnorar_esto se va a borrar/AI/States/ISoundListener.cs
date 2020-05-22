using UnityEngine;

public interface ISoundListener {
    void OnSoundListen(Inspectable inspec);
    void OnSoundListen(Vector3 position);
}