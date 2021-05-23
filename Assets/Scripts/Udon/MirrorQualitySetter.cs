using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class MirrorQualitySetter : UdonSharpBehaviour
{
    [SerializeField] private LayerMask reflectLayers;

    public LayerMask GetReflectLayers()
    {
        return reflectLayers;
    }
    
    public void SetQuality(VRC_MirrorReflection targetMirror)
    {
        if (!targetMirror)
        {
            return;
        }

        targetMirror.m_ReflectLayers = reflectLayers;
    }
}
