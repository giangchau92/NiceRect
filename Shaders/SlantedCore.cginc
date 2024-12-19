#ifndef SLANT_CORE_CG_INCLUDED
#define SLANT_CORE_CG_INCLUDED

#include "UnityCG.cginc"
#include "UnityUI.cginc"
#ifdef SLANT_SOFTMASK
#include "Packages/com.coffee.softmask-for-ugui/Shaders/SoftMask.cginc"
#endif

#define FILL_TYPE_SOLID 0
#define FILL_TYPE_GRADIENT 1

#define GRADIENT_TYPE_LINER 0
#define GRADIENT_TYPE_RADIAL 1

#define STROKE_TYPE_NONE 0
#define STROKE_TYPE_INNER 1

#define STROKE_COLOR_TYPE_SOLID 0
#define STROKE_COLOR_TYPE_GRADIENT 1

sampler2D _MainTex;
float4 _MainTex_ST;

struct appdata
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
    float4 color : COLOR;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f
{
    float2 uv : TEXCOORD0;
    float4 vertex : SV_POSITION;
    float4 color : COLOR;
    float4 worldPosition : TEXCOORD1;
    UNITY_VERTEX_OUTPUT_STEREO
};


float4 _ClipRect;
float4 _Top;
float4 _Bottom;
float _Corner;
float4 _WidthHeightSize;
int _UseGrayscale;
float _GrayscaleFactor;
int _FillType;
float4 _FillColor;
float4 _GradientColors[8];
float4 _GradientColors_0;
float4 _GradientColors_1;
float4 _GradientColors_2;
float4 _GradientColors_3;
float4 _GradientColors_4;
float4 _GradientColors_5;
float4 _GradientColors_6;
float4 _GradientColors_7;
float4 _GradientAlphas[8];
float4 _GradientAlphas_0;
float4 _GradientAlphas_1;
float4 _GradientAlphas_2;
float4 _GradientAlphas_3;
float4 _GradientAlphas_4;
float4 _GradientAlphas_5;
float4 _GradientAlphas_6;
float4 _GradientAlphas_7;
int _GradientType = 0;
float _GradientScale = 1;
float _GradientRotation;
int _GradientColorLength;
int _GradientAlphaLength;
int _StrokeType;
int _StrokeColorType;
float _StrokeWidth;
float4 _StrokeColor;
float4 _StrokeGradientColors[8];
float4 _StrokeGradientColors_0;
float4 _StrokeGradientColors_1;
float4 _StrokeGradientColors_2;
float4 _StrokeGradientColors_3;
float4 _StrokeGradientColors_4;
float4 _StrokeGradientColors_5;
float4 _StrokeGradientColors_6;
float4 _StrokeGradientColors_7;
float4 _StrokeGradientAlphas[8];
float4 _StrokeGradientAlphas_0;
float4 _StrokeGradientAlphas_1;
float4 _StrokeGradientAlphas_2;
float4 _StrokeGradientAlphas_3;
float4 _StrokeGradientAlphas_4;
float4 _StrokeGradientAlphas_5;
float4 _StrokeGradientAlphas_6;
float4 _StrokeGradientAlphas_7;
int _StrokeGradientType = 0;
float _StrokeGradientScale = 1;
float _StrokeGradientRotation;
int _StrokeGradientColorLength;
int _StrokeGradientAlphaLength;


v2f vert (appdata v)
{
    v2f o;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_OUTPUT(v2f, o);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    o.worldPosition = v.vertex;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
    o.color = v.color;
    return o;
}

inline float cross2D(float2 a, float2 b)
{
    return a.x * b.y - a.y * b.x;
}

inline float line_segment(in float2 p, in float2 a, in float2 b) {
    float2 ba = b - a;
    float2 pa = p - a;
    float h = clamp(dot(pa, ba) / dot(ba, ba), 0.0, 1.0);
    return length(pa - h * ba);
}

inline fixed4 grayscale(fixed4 color, float grayscaleFactor)
{
    float gray = saturate(dot(color.rgb, float3(0.299, 0.587, 0.114)) + grayscaleFactor);
    return fixed4(gray, gray, gray, color.a);
}

float4 blendStrokeColors(float4 fillColor, float4 strokeColor)
{
    float3 Rf = fillColor.rgb;
    float Af = fillColor.a;
    float3 Rs = strokeColor.rgb;
    float As = strokeColor.a;

    float Ac = As + Af * (1.0 - As);

    if (Af == 0.0)
    {
        return strokeColor;
    }

    float3 Rc = (Rs * As + Rf * Af * (1.0 - As)) / Ac;

    return float4(Rc, Ac);
}


inline half4 mixAlpha(half4 mainTexColor, half4 fillColor, half4 strokeColor, float sdfAlpha, int useGrayscale, float grayscaleFactor)
{
    half4 col = mainTexColor * fillColor;
    col = blendStrokeColors(col, strokeColor);

    col = useGrayscale == 1 ? grayscale(col, grayscaleFactor) : col;
    col.a = min(col.a, sdfAlpha);
    return col;
}

inline float4 SampleGradient(float Time, float4 colors[8], int colorLength, float4 alphas[8], int alphaLength)
{
    float _GradientInterpolationType = 0;

    float3 color = colors[0].rgb;
    [unroll]
    for (int c = 1; c < 8; c ++)
    {
        float colorPos = saturate((Time - colors[c - 1].w) / (colors[c].w - colors[c - 1].w)) * step(c, colorLength - 1);
        color = lerp(color, colors[c].rgb, lerp(colorPos, step(0.01, colorPos), _GradientInterpolationType));
    }

    float alpha = alphas[0].x;
    [unroll]
    for (int a = 1; a < 8; a ++)
    {
        float alphaPos = saturate((Time - alphas[a - 1].y) / (alphas[a].y - alphas[a - 1].y)) * step(a, alphaLength - 1);
        alpha = lerp(alpha, alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), _GradientInterpolationType));
    }
    return float4(color, alpha);
}

#ifdef SLANT_MASK
float4 _ColorMask;
#endif


void SetupFillGradientData()
{
    _GradientColors[0] = _GradientColors_0;
    _GradientColors[1] = _GradientColors_1;
    _GradientColors[2] = _GradientColors_2;
    _GradientColors[3] = _GradientColors_3;
    _GradientColors[4] = _GradientColors_4;
    _GradientColors[5] = _GradientColors_5;
    _GradientColors[6] = _GradientColors_6;
    _GradientColors[7] = _GradientColors_7;
    _GradientAlphas[0] = _GradientAlphas_0;
    _GradientAlphas[1] = _GradientAlphas_1;
    _GradientAlphas[2] = _GradientAlphas_2;
    _GradientAlphas[3] = _GradientAlphas_3;
    _GradientAlphas[4] = _GradientAlphas_4;
    _GradientAlphas[5] = _GradientAlphas_5;
    _GradientAlphas[6] = _GradientAlphas_6;
    _GradientAlphas[7] = _GradientAlphas_7;
}

void SetupStrokeGradientData()
{
    _StrokeGradientColors[0] = _StrokeGradientColors_0;
    _StrokeGradientColors[1] = _StrokeGradientColors_1;
    _StrokeGradientColors[2] = _StrokeGradientColors_2;
    _StrokeGradientColors[3] = _StrokeGradientColors_3;
    _StrokeGradientColors[4] = _StrokeGradientColors_4;
    _StrokeGradientColors[5] = _StrokeGradientColors_5;
    _StrokeGradientColors[6] = _StrokeGradientColors_6;
    _StrokeGradientColors[7] = _StrokeGradientColors_7;
    _StrokeGradientAlphas[0] = _StrokeGradientAlphas_0;
    _StrokeGradientAlphas[1] = _StrokeGradientAlphas_1;
    _StrokeGradientAlphas[2] = _StrokeGradientAlphas_2;
    _StrokeGradientAlphas[3] = _StrokeGradientAlphas_3;
    _StrokeGradientAlphas[4] = _StrokeGradientAlphas_4;
    _StrokeGradientAlphas[5] = _StrokeGradientAlphas_5;
    _StrokeGradientAlphas[6] = _StrokeGradientAlphas_6;
    _StrokeGradientAlphas[7] = _StrokeGradientAlphas_7;
}

float2 GetCornerDistant(float2 vA, float2 vB, float corner)
{
    float a = acos(dot(vA, vB) / (length(vA) * length(vB))) / 2.0;
    float2 vC = normalize(normalize(vA) + normalize(vB));
    float d = corner / sin(a);
    return vC * d;
}

half4 frag (v2f i) : SV_Target
{
    half4 texColor = tex2D(_MainTex, i.uv);

    float2 A = float2(0, 1) * _WidthHeightSize.xy + float2(_Top.x, -_Top.y);
    float2 B = float2(1, 1) * _WidthHeightSize.xy - _Top.zw;
    float2 C = float2(1, 0) * _WidthHeightSize.xy + float2(- _Bottom.z, _Bottom.w);
    float2 D = float2(0, 0) * _WidthHeightSize.xy + _Bottom.xy;

    float2 cA = GetCornerDistant(B - A, D - A, _Corner);
    float2 cB = GetCornerDistant(C - B, A - B, _Corner);
    float2 cC = GetCornerDistant(D - C, B - C, _Corner);
    float2 cD = GetCornerDistant(A - D, C - D, _Corner);

    A = A + cA;
    B = B + cB;
    C = C + cC;
    D = D + cD;

    B.x = max(A.x, B.x);
    C.x = max(C.x, D.x);
    A.y = max(A.y, D.y);
    B.y = max(B.y, C.y);

    float2 uv = i.uv * _WidthHeightSize;
    float crossAB = cross2D(B - A, uv - A);
    float crossBC = cross2D(C - B, uv - B);
    float crossCD = cross2D(D - C, uv - C);
    float crossDA = cross2D(A - D, uv - D);

    float sdfAB = line_segment(uv, A, B);
    float sdfBC = line_segment(uv, B, C);
    float sdfCD = line_segment(uv, C, D);
    float sdfDA = line_segment(uv, D, A);

    float sdf = min(min(min(sdfAB, sdfBC), sdfCD), sdfDA);

    bool insideRect = crossAB < 0 && crossBC < 0 && crossCD < 0 && crossDA < 0;

    sdf = insideRect ? -sdf : sdf;

    float dpX = abs(ddx(sdf));
    float dpY = abs(ddy(sdf));
    float vX = (sdf - (_Corner - dpX)) / dpX;
    float vY = (sdf - (_Corner - dpY)) / dpY;
    float mask = saturate(1 - max(vX, vY));

    fixed4 fillColor = _FillColor;
    if (_FillType == FILL_TYPE_GRADIENT)
    {
        SetupFillGradientData();
        half t = 0;
        if (_GradientType == GRADIENT_TYPE_LINER)
        {
            half gradientRotation = radians(_GradientRotation);
            float gradientScale = (- _GradientScale + 1.0);
            t = cos(gradientRotation) * (i.uv.x - 0.5) * gradientScale +
                         sin(gradientRotation) * (i.uv.y - 0.5) * gradientScale + 0.5;
        }
        else
        {
            half fac = saturate(length(i.uv - float2(.5, .5)) * (2 - _GradientScale));
            t = clamp(fac, 0, 1);
        }
        fillColor = SampleGradient(t, _GradientColors, _GradientColorLength, _GradientAlphas, _GradientAlphaLength);
    }

    half4 strokeColor = 0;
    if (_StrokeType == STROKE_TYPE_INNER)
    {
        int strokeMask = sdf < _Corner && sdf > _Corner - _StrokeWidth;
        if (_StrokeColorType == STROKE_COLOR_TYPE_GRADIENT)
        {
            SetupStrokeGradientData();
            half t = 0;
            if (_StrokeGradientType == GRADIENT_TYPE_LINER)
            {
                half gradientRotation = radians(_StrokeGradientRotation);
                float gradientScale = (- _StrokeGradientScale + 1.0);
                t = cos(gradientRotation) * (i.uv.x - 0.5) * gradientScale +
                             sin(gradientRotation) * (i.uv.y - 0.5) * gradientScale + 0.5;
            }
            else
            {
                half fac = saturate(length(i.uv - float2(.5, .5)) * (2 - _StrokeGradientScale));
                t = clamp(fac, 0, 1);
            }
            strokeColor = SampleGradient(t, _StrokeGradientColors, _StrokeGradientColorLength, _StrokeGradientAlphas, _StrokeGradientAlphaLength);
        }
        else
        {
            strokeColor = _StrokeColor;
        }

        vX = -(sdf - (_Corner - _StrokeWidth + dpX)) / dpX;
        vY = -(sdf - (_Corner - _StrokeWidth + dpY)) / dpY;
        float innerMask = saturate(1 - max(vX, vY));

        strokeColor *= strokeMask;
        strokeColor.a *= innerMask;
    }


    fixed4 color = mixAlpha(texColor, fillColor, strokeColor, mask, _UseGrayscale, _GrayscaleFactor) * i.color;
    #ifdef SLANT_MASK
    color =  _ColorMask * mask * fillColor.a;
    #endif

    #ifdef SLANT_SOFTMASK
    color.a *= SoftMask(i.vertex, i.worldPosition, color.a);
    #endif

    #ifdef UNITY_UI_CLIP_RECT
    color.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
    #endif

    #ifdef UNITY_UI_ALPHACLIP
    clip(color.a - 0.001);
    #endif

    return color;
}

#endif