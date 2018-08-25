using UnityEngine;

/* SingletonMonobehabiour
    Monobehabiourを継承したシングルトン 
*/

namespace SubScripts.Base
{
    public class SingletonMonobehaviour<T> : InitializationMonobehaviour where T: SingletonMonobehaviour<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                }

                // Hierarchy上のSingletonがなかったら生成する
                if (_instance == null)
                {
                    var o = new GameObject();

                    _instance = o.AddComponent<T>();

                    o.name = "(singleton)" + typeof(T);

                    Debug.LogWarning("[Singleton] Singletonを生成しました");
                }

                if (_instance == null)
                {
                    Debug.LogError("[Singleton] Singletonがありません");
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = (T) this;

                return;
            }

            if (_instance == this)
            {
                return;
            }

            // 複数存在する場合はここで削除
            Destroy(this);
        }
    }
}
