using UdonSharp;
using UdonSharp.Video;
using UnityEngine;

namespace Udon
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
    public class AudioController : UdonSharpBehaviour
    {
        [SerializeField] private VolumeController volumeController;
        [SerializeField] private AudioSource source;

        public void OnSetVolume()
        {
            source.volume = volumeController.slider.value;
            volumeController.UpdateVolumeIcon();
        }

        public void SetMuted(bool mute)
        {
            source.mute = mute;
            volumeController.SetMuted(source.mute);
        }

        public void ToggleMute()
        {
            var mute = source.mute;
            mute = !mute;
        
            source.mute = mute;
            volumeController.SetMuted(mute);
        }
    }
}
