using UnityEngine.Audio;

namespace ToolsMandioca.Sound
{
    using UnityEngine;
    using ToolsMandioca.Extensions;

    public static class ASourceCreator
    {
        public static AudioSource Create2DSource(AudioClip ac, string name, AudioMixerGroup mixerGroup, bool loop = false, bool playOnAwake = false)
        {
            Transform cam = Camera.main.transform;

            var source = cam
                .gameObject
                .CreateDefaultSubObject<AudioSource>("SOURCE-> " + name);

            source.outputAudioMixerGroup = mixerGroup;
            source.clip = ac;
            source.loop = loop;
            source.spatialBlend = 0;
            source.playOnAwake = playOnAwake;
            if (playOnAwake) source.Play();

            return source;
        }
        public static AudioSource Create3DSource(AudioClip ac,string name, Transform parent, bool loop = false, bool playOnAwake = false)
        {
            var source = parent
                .gameObject
                .CreateDefaultSubObject<AudioSource>("SOURCE-> " + name);

            source.clip = ac;
            source.loop = loop;
            source.spatialBlend = 1;
            source.playOnAwake = playOnAwake;
            if (playOnAwake) source.Play();

            return source;
        }

        public static void PlayIfNotPlaying(this AudioSource ac)
        {
            if (!ac.isPlaying) ac.Play();
        }
    }
}