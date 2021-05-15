#if UNITY_2019_3_OR_NEWER
using UnityEngine.UIElements;

#else
using UnityEngine.Experimental.UIElements;
#endif

namespace VRC.Udon.Editor.ProgramSources.UdonGraphProgram.UI
{
#if UNITY_2019_3_OR_NEWER
    public class CharField : TextInputBaseField<char>
    {
        public CharField()
            : base(1, char.MinValue, null)
#else
    public class CharField : TextInputFieldBase<char>
    {
        public CharField()
            : base(1, char.MinValue)
#endif
        {
            this.AddToClassList("UdonValueField");
        }

        public override char value
        {
            get { return base.value; }
            set
            {
                base.value = value;
                this.text = value.ToString();
            }
        }

        protected override void ExecuteDefaultAction(EventBase evt)
        {
            base.ExecuteDefaultAction(evt);

            if (this.text.Length > 0)
            {
                this.value = this.text.ToCharArray()[0];
            }
        }
    }
}