using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class SonarReplacement : MonoBehaviour
{
    private CustomPassVolume passVolume;
    private DrawRenderersCustomPass pass;
    private Camera cam;
    
    void Start()
    {
        cam = GetComponent<Camera>();
        passVolume = gameObject.AddComponent<CustomPassVolume>();
        passVolume.isGlobal = false;
        passVolume.targetCamera = cam;
        passVolume.injectionPoint = CustomPassInjectionPoint.BeforeTransparent;
        pass = new DrawRenderersCustomPass();
        pass.overrideMode = DrawRenderersCustomPass.OverrideMaterialMode.Shader;
        pass.overrideShader = Shader.Find("Unlit/SonarReplace");
        pass.clearFlags = ClearFlag.All;
        passVolume.customPasses.Add(pass);
    }

    void Update()
    {
        Shader.SetGlobalFloat("_SonarFOV", cam.fieldOfView);
    }
}
