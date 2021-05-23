using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class MirrorToggler : UdonSharpBehaviour
{
    [SerializeField] private VRC_MirrorReflection mirror;
    [SerializeField] private MirrorQualitySetter qualitySetter;
    
    public override void Interact()
    {
        if (!mirror)
        {
            return;
        }

        if (mirror.m_ReflectLayers.Equals(qualitySetter.GetReflectLayers()))
        {
            mirror.gameObject.SetActive(!mirror.gameObject.activeSelf);
        }
        else
        {
            qualitySetter.SetQuality(mirror);
            mirror.gameObject.SetActive(true);
        }
    }
}
