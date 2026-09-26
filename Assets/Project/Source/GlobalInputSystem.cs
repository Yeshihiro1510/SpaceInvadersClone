using UnityEngine;

namespace Project.Source
{
    public static class GlobalInputSystem
    {
        public static InputSystem_Actions InputSystem { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            InputSystem = new InputSystem_Actions();
        }
    }
}