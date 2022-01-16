using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace ureishi.UKeyboard.Udon.Demo
{
    // Keyboard.cs を使うデモです
    public class Command : UdonSharpBehaviour
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
        private Chat
            chat;

        public void OnEndEdit()
        {
            // このメソッド内を変更する
            text = keyboard.text;

            if (text == "/version" || text == "/v")
            {
#if !UNITY_EDITOR
                chat.WriteUiText($"[{DateTime.Now:HH:mm:ss}][{Networking.LocalPlayer.displayName}] {text}");
#else
                chat.WriteUiText($"[{DateTime.Now:HH:mm:ss}][player] {text}");
#endif
                chat.WriteUiText($"> {nameof(UKeyboard)} {keyboard.version} - Ureishi");
            }
            else if (text == "/help" || text == "/?")
            {
#if !UNITY_EDITOR
                chat.WriteUiText($"[{DateTime.Now:HH:mm:ss}][{Networking.LocalPlayer.displayName}] {text}");
#else
                chat.WriteUiText($"[{DateTime.Now:HH:mm:ss}][player] {text}");
#endif
                chat.WriteUiText($"> /version, /v");
                chat.WriteUiText($"> /help, /?");
                chat.WriteUiText($"> /date");
                chat.WriteUiText($"> /finger");
                chat.WriteUiText($"> /clear");
            }
            else if (text == "/date")
            {
#if !UNITY_EDITOR
                chat.WriteUiText($"[{DateTime.Now:HH:mm:ss}][{Networking.LocalPlayer.displayName}] {text}");
#else
                chat.WriteUiText($"[{DateTime.Now:HH:mm:ss}][player] {text}");
#endif
                chat.WriteUiText($"> {DateTime.Now:yyyy/MM/dd HH:mm:ss}");
            }
            else if (text == "/finger")
            {
#if !UNITY_EDITOR
                chat.WriteUiText($"[{DateTime.Now:HH:mm:ss}][{Networking.LocalPlayer.displayName}] {text}");
#else
                chat.WriteUiText($"[{DateTime.Now:HH:mm:ss}][player] {text}");
#endif
                foreach (var player in VRCPlayerApi.GetPlayers(new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()]))
                    chat.WriteUiText($"> {player.playerId:d2} {player.displayName}{(player.isMaster ? " (Master)" : string.Empty)}");
            }
            else if (text == "/clear")
            {
                chat.ClearUiTexts();
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
                        Debug.LogWarning($"[{nameof(Command)}] Target Udon Sharp Behaviour is null.", this);
                    }
                }
            }
        }
    }
}
