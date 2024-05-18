using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable] // This doesn't serialize the RenderTexture but helps with Unity's inspector and organization
public class RenderTextures
{
    public RenderTexture depth;
    public RenderTexture normals;
    public RenderTexture result;
    public RenderTexture debug;
    public RenderTexture debug2;
    public RenderTexture sonar;
}

public class Sonar : MonoBehaviour
{
    private Camera cam;
    public float sonarMaxRange = 20.0f;
    public ComputeShader shader;
    public RenderTextures renderTextures;
    
    private Vector3[] frustumCornersVS = new Vector3[4];
    private Vector3[] frustumCornersWS = new Vector3[4];
    private int kernelMain;
    private int kernelClear;
    private float[] resolution = new float[2];
    void Start()
    {
        cam = GetComponent<Camera>();
        kernelMain = shader.FindKernel("CSMain");
        kernelClear = shader.FindKernel("CSClear");
        shader.SetTexture(kernelMain, "depthTex", renderTextures.depth);
        shader.SetTexture(kernelMain, "normalsTex", renderTextures.normals);
        shader.SetTexture(kernelMain, "debugTex", renderTextures.debug);
        shader.SetTexture(kernelMain, "debugTex2", renderTextures.debug2);
        shader.SetTexture(kernelMain, "resultTex", renderTextures.result);
        shader.SetTexture(kernelMain, "sonarTex", renderTextures.sonar);
        shader.SetTexture(kernelClear, "sonarTex", renderTextures.sonar);
    }

    void Update()
    {
        cam.CalculateFrustumCorners(new Rect(0, 0, 1, 1), cam.farClipPlane, Camera.MonoOrStereoscopicEye.Mono, frustumCornersVS);
        for (int i = 0; i < 4; i++)
        {
            frustumCornersWS[i] = cam.transform.TransformDirection(frustumCornersVS[i]);
        }

        resolution[0] = renderTextures.depth.width;
        resolution[1] = renderTextures.depth.height;

        shader.SetFloats("res", resolution);
        shader.SetFloat("focalLength", cam.focalLength);
        shader.SetFloat("sonarMaxRange", sonarMaxRange);
        shader.SetFloat("fov", cam.fieldOfView);
        shader.SetMatrix("camMatrix", cam.worldToCameraMatrix);
        
        shader.SetVector("BLws", frustumCornersWS[0]);
        shader.SetVector("TLws", frustumCornersWS[1]);
        shader.SetVector("TRws", frustumCornersWS[2]);
        shader.SetVector("BRws", frustumCornersWS[3]);
        
        shader.SetVector("BLvs", frustumCornersVS[0]);
        shader.SetVector("TLvs", frustumCornersVS[1]);
        shader.SetVector("TRvs", frustumCornersVS[2]);
        shader.SetVector("BRvs", frustumCornersVS[3]);
        
        shader.Dispatch(kernelClear, 32, 32, 1);
        shader.Dispatch(kernelMain, 32, 32, 1);
        
    }
}
