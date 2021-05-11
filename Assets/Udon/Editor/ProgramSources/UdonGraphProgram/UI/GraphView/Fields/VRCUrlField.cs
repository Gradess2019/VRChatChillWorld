#if UNITY_2019_3_OR_NEWER
using UnityEngine.UIElements;
#else
using UnityEngine.Experimental.UIElements;
#endif
using VRC.SDKBase;

namespace VRC.Udon.Editor.ProgramSources.UdonGraphProgram.UI
{
#if UNITY_2019_3_OR_NEWER
    public class VRCUrlField : TextInputBaseField<VRCUrl>
    {
        public VRCUrlField()
            : base(-1, char.MinValue, null)
#else
    public class VRCUrlField : TextInputFieldBase<VRCUrl>
    {
        public VRCUrlField()
            : base(-1, char.MinValue)
#endif
        {
            AddToClassList("UdonValueField");
            RegisterCallback<BlurEvent>(OnBlur);
        }

        private void OnBlur(BlurEvent evt)
        {
            base.value = new VRCUrl(text);
        }

        public override VRCUrl value
        {
            get => base.value;
            set
            {
                base.value = value;
                text = value?.Get() ?? string.Empty;
            }
        }

        ~VRCUrlField()
        {
            UnregisterCallback<BlurEvent>(OnBlur);
        }
    }
}
