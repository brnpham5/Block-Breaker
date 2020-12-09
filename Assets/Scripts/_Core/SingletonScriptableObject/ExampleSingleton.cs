using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{

    /// <summary>
    /// Example scriptable singleton
    /// Example of initialization on creation
    /// Needs to be referenced in a scene before becoming active
    /// </summary>
    //[CreateAssetMenu(fileName = "Example Singleton", menuName = "Core/Example Singleton")]
    public class ExampleSingleton : SingletonScriptableObject<ExampleSingleton>
    {
        private GameObject _capsule;
        public static GameObject Capsule
        {
            get { return Instance._capsule; }
            private set { Instance._capsule = value; }
        }

        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void firstInitialization()
        {
            Capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            Capsule.transform.position = Vector3.zero;
            DontDestroyOnLoad(Capsule);
        }
    }

}
