#if UNITY_ANDROID
#define OCULUS_QUEST
#endif

using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace ureishi.UKeyboard.Udon.Demo
{
    // Keyboard.cs を使うデモです
    // 伝言にパスワードやコマンドが表示されてしまうと困るので、
    // PasswordEvent.OnEndEdit, Command.OnEndEditを
    // 経由してChat.OnEndEditを呼ぶ。
    public class Chat : UdonSharpBehaviour
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
        private Color
            _joinColor = new Color(0x4f, 0xf3, 0x66, 0xff) / 0xff,
            _leaveColor = new Color(0xef, 0x3a, 0x43, 0xff) / 0xff;

        [SerializeField]
        private AudioSource
            audioSource;

        [SerializeField]
        private AudioClip
            audioClip;

        private readonly string
            CTagEnd = "</color>";

        [SerializeField]
        private Text
            _textPrefab;

        [SerializeField]
        private int
            _maxTexts = 100;

        [SerializeField]
        private InputField
            _inputField;

        [SerializeField]
        private Text
            _inputFieldText;

        [SerializeField]
        private ScrollRect
            _scrollRect;

        [UdonSynced]
        private string
            _sendString = string.Empty;

        private string
            _recieveString = string.Empty;

        private void Start()
        {
#if !OCULUS_QUEST
            _inputField.placeholder.gameObject.SetActive(true);
            _inputField.enabled = true;
            _inputField.text = string.Empty;
#endif
        }

        public void OnEndEdit()
        {
            // このメソッド内を変更する
            if (string.IsNullOrEmpty(keyboard.text))
                return;

            if (!Networking.IsOwner(gameObject))
                Networking.SetOwner(Networking.LocalPlayer, gameObject);

#if !UNITY_EDITOR
            var tempText = $"[{DateTime.Now:HH:mm:ss}][{Networking.LocalPlayer.displayName}] {keyboard.text}";
#else
            var tempText = $"[{DateTime.Now:HH:mm:ss}][{"player"}] {keyboard.text}";
#endif
            var byteCount = 0;
            var charCount = 0;

            for (var i = 0; i < tempText.Length; i++)
            {
                if (tempText[i] < 128)
                    byteCount += 1;
                else
                    byteCount += 3;

                if (byteCount < 128)
                    charCount++;
                else
                    break;
            }

            text = tempText.Substring(0, charCount);

            WriteUiText(text);

#if !OCULUS_QUEST
            _inputField.text = text;
#else
            _inputFieldText.text = text;
#endif

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
                    Debug.LogWarning($"[{nameof(Chat)}] Target Udon Sharp Behaviour is null.", this);
                }
            }
        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            WriteUiText($"[{DateTime.Now:HH:mm:ss}]{CTag(_joinColor)}[System]{CTagEnd} {player.displayName} joined.");
        }

        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            WriteUiText($"[{DateTime.Now:HH:mm:ss}]{CTag(_leaveColor)}[System]{CTagEnd} {player.displayName} left.");
        }

        public override void OnPreSerialization()
        {
            if (!string.IsNullOrEmpty(text) && _sendString != text)
                _sendString = text;
        }

        public override void OnDeserialization()
        {
            if (!string.IsNullOrEmpty(_sendString) && _sendString != text && _sendString != _recieveString)
            {
                _recieveString = _sendString;

                text = _recieveString;
                WriteUiText(text);

#if !OCULUS_QUEST
                _inputField.text = text;
#else
                _inputFieldText.text = text;
#endif
            }
        }

        public void WriteUiText(string text)
        {
            GameObject instance = null;

            for (var i = 0; i <= _textPrefab.transform.parent.childCount - _maxTexts; i++)
            {
                if (_textPrefab.transform.parent.GetChild(i) != _textPrefab.transform)
                    instance = _textPrefab.transform.parent.GetChild(i).gameObject;
            }

            if (!instance)
                instance = VRCInstantiate(_textPrefab.gameObject);

            instance.transform.SetParent(_textPrefab.transform.parent);
            instance.transform.localPosition = _textPrefab.transform.localPosition;
            instance.transform.localRotation = _textPrefab.transform.localRotation;
            instance.transform.localScale = _textPrefab.transform.localScale;
            instance.transform.SetAsLastSibling();
            instance.GetComponent<Text>().text = text;
            instance.SetActive(true);

            if (audioSource && !audioSource.isPlaying)
                audioSource.PlayOneShot(audioClip);
        }

        public void ClearUiTexts()
        {
            foreach (Transform child in _textPrefab.transform.parent)
            {
                if (child != _textPrefab.transform)
                    Destroy(child.gameObject);
            }
        }

        private string CTag(Color c)
        {
            return $"<color=\"#{ToHtmlStringRGB(c)}\">";
        }

        private string ToHtmlStringRGB(Color c)
        {
            c *= 0xff;
            return $"{Mathf.RoundToInt(c.r):x2}{Mathf.RoundToInt(c.g):x2}{Mathf.RoundToInt(c.b):x2}";
        }
    }
}
