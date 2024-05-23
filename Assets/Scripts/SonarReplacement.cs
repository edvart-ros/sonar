using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class Sonar : MonoBehaviour
{
    public RenderTexture rawSonarShaderTex; // input to compute shader
    public RenderTexture resultSonarTex; // texture to render into
    public Shader SonarShader;
    public ComputeShader SonarCompute;
    [Range(0.001f, 200f)]
    public float sonarRange = 30.0f;
    
    private CustomPassVolume passVolume;
    private DrawRenderersCustomPass pass;
    private Camera cam;
    private int[] res = new int[2];
    private int clearKernelID;
    private int mainKernelID;
    
    void Start()
    {
        cam = GetComponent<Camera>();
        clearKernelID = SonarCompute.FindKernel("CSClear");
        mainKernelID = SonarCompute.FindKernel("CSMain");
        
        // set up custom pass volume for DrawRenderers with sonar replacement shader
        passVolume = gameObject.AddComponent<CustomPassVolume>();
        passVolume.isGlobal = false;
        passVolume.targetCamera = cam;
        passVolume.injectionPoint = CustomPassInjectionPoint.AfterPostProcess;
        // set up the DrawRenderersCustomPass
        pass = new DrawRenderersCustomPass();
        pass.overrideMode = DrawRenderersCustomPass.OverrideMaterialMode.Shader;
        pass.overrideShader = SonarShader;  
        pass.clearFlags = ClearFlag.All;
        passVolume.customPasses.Add(pass);
        
        // set up compute shader to produce final sonar image
        if (rawSonarShaderTex.width == resultSonarTex.width && rawSonarShaderTex.height == resultSonarTex.height)
        {
            res[0] = rawSonarShaderTex.width;
            res[1] = rawSonarShaderTex.height;
            SonarCompute.SetTexture(0, "inputTex", rawSonarShaderTex);
            SonarCompute.SetTexture(0, "resultTex", resultSonarTex);
            SonarCompute.SetInts("res", res);
        }
        else
        {
            Debug.LogError("input and output texture resolutions are not equal");
        }
        SonarCompute.SetTexture(0, "inputTex", rawSonarShaderTex);
        SonarCompute.SetTexture(0, "resultTex", resultSonarTex);
        SonarCompute.SetTexture(1, "inputTex", rawSonarShaderTex);
        SonarCompute.SetTexture(1, "resultTex", resultSonarTex);
    }

    void Update()
    {
        Shader.SetGlobalFloat("_SonarFOV", cam.fieldOfView);
        res[0] = rawSonarShaderTex.width;
        res[1] = rawSonarShaderTex.height;
        SonarCompute.SetInts("res", res);
        SonarCompute.SetFloat("MaxRange", sonarRange);
        SonarCompute.Dispatch(clearKernelID, res[0]/32, res[1]/32, 1);
        SonarCompute.Dispatch(mainKernelID, res[0]/32, res[1]/32, 1);
    }
}
