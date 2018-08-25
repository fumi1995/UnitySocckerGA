using UnityEngine;

/* InitilaizationMonobehaviour
    Monobehabiourを継承した初期化関数の実装 
*/

namespace SubScripts.Base
{
    public class InitializationMonobehaviour : MonoBehaviour
    {
        // 自動初期化呼出しフラグ
        public bool IsAutoInitialized;

        // 初期化フラグ
        private bool _isInitialized;
        public bool IsInitialized
        {
            get
            {
                return _isInitialized;
            }
        }

        // 初期化
        public virtual void Initialization()
        {
            if(_isInitialized)return;

            _isInitialized = true;
        }

        // 自動呼出し
        public virtual void Start ()
        {
            if (IsAutoInitialized)
            {
                Initialization();
            }
        }

    }
}
