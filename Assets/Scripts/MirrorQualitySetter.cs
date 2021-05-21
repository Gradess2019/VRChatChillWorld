
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class MirrorQualitySetter : UdonSharpBehaviour
{
    [SerializeField] private VRC_MirrorReflection mirror;
    [SerializeField] private LayerMask reflectLayers;
    public override void Interact()
    {
        if (!mirror)
        {
            return;
        }

        mirror.m_ReflectLayers = reflectLayers;
    }
}
