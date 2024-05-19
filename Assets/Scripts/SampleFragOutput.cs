using UnityEngine;
using UnityEngine.Rendering;

public class SampleFragOutput : MonoBehaviour
{
    public ComputeShader shader;
    public RenderTexture inTex;
    public RenderTexture outTex;
    private int kernelMain;
    
    void Start()
    {
        kernelMain = shader.FindKernel("CSMain");
        shader.SetTexture(kernelMain, "inTex", inTex);
        shader.SetTexture(kernelMain, "outTex", outTex);
    }

    void Update()
    {
        shader.Dispatch(kernelMain, 32, 32, 1);
    }
}
