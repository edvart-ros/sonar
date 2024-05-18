#define PI 3.14159265359
#define RAD2DEG 180.0/PI
#define DEG2RAD 1.0(/RAD2DEG)

Texture2D<float> depthTex;
Texture2D<float4> normalsTex;
RWTexture2D<float4> resultTex;
RWTexture2D<float4> debugTex;
RWTexture2D<float4> debugTex2;
RWTexture2D<float4> sonarTex;
float4x4 camMatrix;
float focalLength;
float sonarMaxRange;
float fov;
float2 res;

// ws - World Space
float3 BLws;
float3 TLws;
float3 TRws;
float3 BRws;

// vs - View Space (camera coordinates)
float3 BLvs;
float3 TLvs;
float3 TRvs;
float3 BRvs;