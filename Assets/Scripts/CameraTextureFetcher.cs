using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using System.Reflection;

class DepthNormalsGetter : CustomPass
{
    public Camera           bakingCamera = null;
    public RenderTexture    depthTexture = null;
    public RenderTexture    normalTexture = null;
    public RenderTexture    tangentTexture = null;
    public bool             render = true;

    protected override bool executeInSceneView => false;

    protected override void Execute(CustomPassContext ctx)
    {
        if (!render || bakingCamera == null){
            return;
        }
        if (depthTexture == null && normalTexture == null && tangentTexture == null){
            return;
        }

        if (depthTexture != null)
            bakingCamera.aspect = Mathf.Max(bakingCamera.aspect, depthTexture.width / (float)depthTexture.height);
        if (normalTexture != null)
            bakingCamera.aspect = Mathf.Max(bakingCamera.aspect, normalTexture.width / (float)normalTexture.height);
        if (tangentTexture != null)
            bakingCamera.aspect = Mathf.Max(bakingCamera.aspect, tangentTexture.width / (float)tangentTexture.height);
        bakingCamera.TryGetCullingParameters(out var cullingParams);
        cullingParams.cullingOptions = CullingOptions.None;

        // Assign the custom culling result to the context
        ctx.cullingResults = ctx.renderContext.Cull(ref cullingParams);
        var overrideDepthTest = new RenderStateBlock(RenderStateMask.Depth) { depthState = new DepthState(true, CompareFunction.LessEqual) };

        if (depthTexture != null)
            CustomPassUtils.RenderDepthFromCamera(ctx, bakingCamera, depthTexture, ClearFlag.All, bakingCamera.cullingMask, overrideRenderState: overrideDepthTest);
        if (normalTexture != null)
            CustomPassUtils.RenderNormalFromCamera(ctx, bakingCamera, normalTexture, ClearFlag.All, bakingCamera.cullingMask, overrideRenderState: overrideDepthTest);
        if (tangentTexture != null)
            CustomPassUtils.RenderTangentFromCamera(ctx, bakingCamera, tangentTexture, ClearFlag.All, bakingCamera.cullingMask, overrideRenderState: overrideDepthTest);
    }
}