using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace ureishi.UKeyboard.Udon.Demo
{
    // Keyboard.cs を使うデモです
    // パスワードに該当しなければ、他のUdonのOnEndEditを呼び出します。
    public class PasswordEvent : UdonSharpBehaviour
    {
        [NonSerialized]
        public UKeyboard
            keyboard;

        [SerializeField]
        private UdonSharpBehaviour[]
            _udonSharpBehaviours;

        private string
            text = string.Empty;

        [SerializeField]
        private string
            _pass1,
            _pass2,
            _pass3;
        [SerializeField]
        private GameObject
            _go1,
            _go2,
            _go3,
            _go4;

        public void OnEndEdit()
        {
            // このメソッド内を変更する
            text = keyboard.text;

            if (text == _pass1)
            {
                _go1.SetActive(!_go1.activeSelf);
            }
            else if (text == _pass2)
            {
                _go2.SetActive(!_go2.activeSelf);
            }
            else if (text == _pass3)
            {
                _go3.SetActive(!_go3.activeSelf);
            }
#if !UNITY_EDITOR
            else if (text == Networking.LocalPlayer.displayName)
#else
            else if (text == "player")
#endif
            {
                _go4.SetActive(!_go4.activeSelf);
            }
            else
            {
                // Relay Event OnEndEdit()
                foreach (var udonSharpBehaviour in _udonSharpBehaviours)
                {
                    if (udonSharpBehaviour)
                    {
                        udonSharpBehaviour.SetProgramVariable(nameof(keyboard), keyboard);
                        udonSharpBehaviour.SendCustomEvent(nameof(OnEndEdit));
                    }
                    else
                    {
                        Debug.LogWarning($"[{nameof(PasswordEvent)}] Target Udon Sharp Behaviour is null.", this);
                    }
                }
            }
        }
    }
}
