#if UNITY_2019_3_OR_NEWER
using UnityEngine.UIElements;
#else
using UnityEngine.Experimental.UIElements;
#endif
using System;
using UnityEngine;

namespace VRC.Udon.Editor.ProgramSources.UdonGraphProgram.UI
{
#if UNITY_2019_3_OR_NEWER
    public class ByteField : TextInputBaseField<byte>
    {
        public ByteField()
            : base(3, char.MinValue, null)
#else
    public class ByteField : TextInputFieldBase<byte>
    {
        public ByteField()
            : base(3, char.MinValue)
#endif
        {
            this.AddToClassList("UdonValueField");
            this.isDelayed = true;
        }

        public override byte value
        {
            get
            {
                return base.value;
            }
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
                try
                {
                    
                    var byteValue = Convert.ToByte(this.text);
                    this.value = byteValue;
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
            
        }

    }
}