using UnityEngine;

namespace Project.Source
{
    public static class GlobalServices
    {
        public static InputSystem_Actions InputSystem { get; private set; }
        public static AudioSource MusicSource { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            InputSystem = new InputSystem_Actions();
            MusicSource = new GameObject("[MusicSource]").AddComponent<AudioSource>();
            Object.DontDestroyOnLoad(MusicSource);
            MusicSource.clip = Resources.Load<AudioClip>("LunaticEyes_audio");
            MusicSource.volume = 0.2f;
            MusicSource.Play();
        }
    }
}