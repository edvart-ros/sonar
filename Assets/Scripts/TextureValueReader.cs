using UnityEngine;

public class TextureValueReader : MonoBehaviour
{
    public ComputeShader computeShader;
    public RenderTexture inputTexture;
    private ComputeBuffer outputBuffer;
    private Vector4[] textureData;
    private int kernelHandle;
    void Start()
    {
        // Set up the ComputeBuffer
        int numPixels = inputTexture.width * inputTexture.height;
        outputBuffer = new ComputeBuffer(numPixels, sizeof(float) * 4);
        textureData = new Vector4[numPixels];

        // Set shader parameters
        kernelHandle = computeShader.FindKernel("CSMain");
        computeShader.SetTexture(kernelHandle, "inTex", inputTexture);
        computeShader.SetBuffer(kernelHandle, "outputBuffer", outputBuffer);
        computeShader.SetVector("res", new Vector2(inputTexture.width, inputTexture.height));

        computeShader.Dispatch(kernelHandle, inputTexture.width / 32, inputTexture.height / 32, 1);
        outputBuffer.GetData(textureData);
        for (int i = 0; i < textureData.Length; i++)
        {
            Debug.Log(textureData[i]);
        }
        outputBuffer.Release();
    }

    void Update(){
        // Set up the ComputeBuffer
        int numPixels = inputTexture.width * inputTexture.height;
        outputBuffer = new ComputeBuffer(numPixels, sizeof(float) * 4);
        textureData = new Vector4[numPixels];

        // Set shader parameters
        kernelHandle = computeShader.FindKernel("CSMain");
        computeShader.SetTexture(kernelHandle, "inTex", inputTexture);
        computeShader.SetBuffer(kernelHandle, "outputBuffer", outputBuffer);
        computeShader.SetVector("res", new Vector2(inputTexture.width, inputTexture.height));

        computeShader.Dispatch(kernelHandle, inputTexture.width / 32, inputTexture.height / 32, 1);
        outputBuffer.GetData(textureData);
        for (int i = 0; i < textureData.Length; i++)
        {
            Debug.Log(textureData[i]);
        }
        outputBuffer.Release();
    }
}
