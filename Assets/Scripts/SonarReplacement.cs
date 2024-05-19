using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class Sonar : MonoBehaviour
{
    private CustomPassVolume passVolume;
    private DrawRenderersCustomPass pass;
    private Camera cam;
    public float sonarRange = 30.0f;
    public RenderTexture rt;
    
    void Start()
    {
        rt = new RenderTexture(32, 32, 32, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
        cam = GetComponent<Camera>();
        cam.targetTexture = rt;
        GetComponent<TextureValueReader>().inputTexture = rt;
        passVolume = gameObject.AddComponent<CustomPassVolume>();
        passVolume.isGlobal = false;
        passVolume.targetCamera = cam;
        passVolume.injectionPoint = CustomPassInjectionPoint.BeforeTransparent;
        pass = new DrawRenderersCustomPass();
        pass.overrideMode = DrawRenderersCustomPass.OverrideMaterialMode.Shader;
        pass.overrideShader = Shader.Find("Unlit/SonarShader");
        pass.clearFlags = ClearFlag.All;
        passVolume.customPasses.Add(pass);
    }

    void Update()
    {
        Shader.SetGlobalFloat("_SonarFOV", cam.fieldOfView);
        Shader.SetGlobalFloat("_SonarRange", sonarRange);
    }
}
