using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class Sonar : MonoBehaviour
{
    public Shader SonarShader;
    private CustomPassVolume passVolume;
    private DrawRenderersCustomPass pass;
    private Camera cam;
    public float sonarRange = 30.0f;
    
    void Start()
    {
        cam = GetComponent<Camera>();
        passVolume = gameObject.AddComponent<CustomPassVolume>();
        passVolume.isGlobal = false;
        passVolume.targetCamera = cam;
        passVolume.injectionPoint = CustomPassInjectionPoint.AfterPostProcess;
        pass = new DrawRenderersCustomPass();
        pass.overrideMode = DrawRenderersCustomPass.OverrideMaterialMode.Shader;
        pass.overrideShader = Shader.Find("Custom/SonarShader");
        pass.clearFlags = ClearFlag.All;
        passVolume.customPasses.Add(pass);
    }

    void Update()
    {
        Shader.SetGlobalFloat("_SonarFOV", cam.fieldOfView);
        Shader.SetGlobalFloat("_SonarRange", sonarRange);
    }
}
