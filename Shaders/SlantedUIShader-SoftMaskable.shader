Shader "Hidden/Custom/SlantedUIShader (SoftMaskable)"
{
    Properties
    {
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}
        _WidthHeightSize ("WidthHeightSize", Vector) = (0, 0, 0, 0)
        _Top ("Top", Vector) = (0, 0, 0, 0)
        _Bottom ("Bottom", Vector) = (0, 0, 0, 0)
        _Corner ("Corner", Range(0, 100)) = 0
        _UseGrayscale ("UseGrayscale", Integer) = 0
        _GrayscaleFactor ("GrayscaleFactor", Range(-0.5, 0.5)) = 0
        _FillType ("FillType", Integer) = 0
        _FillColor ("FillColor", Color) = (1, 1, 1, 1)
        _GradientType ("GradientType", Integer) = 0
        _GradientScale ("GradientScale", Range(-1, 1)) = 0
        _GradientRotation ("GradientRotation", Range(0, 360)) = 0
        _GradientColorLength ("GradientColorLength", Integer) = 0
        _GradientAlphaLength ("GradientAlphaLength", Integer) = 0
        // -- Stroke --
        _StrokeType ("Stroke Type", Integer) = 0
        _StrokeColorType ("Stroke Type", Integer) = 0
        _StrokeWidth ("Stroke Width", Float) = 1
        _StrokeColor ("Stroke Color", Color) = (0, 0, 0, 1)
        _StrokeGradientType ("StrokeGradientType", Integer) = 0
        _StrokeGradientScale ("StrokeGradientScale", Range(-1, 1)) = 0
        _StrokeGradientRotation ("StrokeGradientRotation", Range(0, 360)) = 0
        _StrokeGradientColorLength ("StrokeGradientColorLength", Integer) = 0
        _StrokeGradientAlphaLength ("StrokeGradientAlphaLength", Integer) = 0

        // --- Mask support ---
        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
        [HideInInspector] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

        // --- Gradient ---
        [HideInInspector] _GradientColors_0 ("GradientColors_0", Color) = (0, 0, 0, 0)
        [HideInInspector] _GradientColors_1 ("GradientColors_1", Color) = (0, 0, 0, 0)
        [HideInInspector] _GradientColors_2 ("GradientColors_2", Color) = (0, 0, 0, 0)
        [HideInInspector] _GradientColors_3 ("GradientColors_3", Color) = (0, 0, 0, 0)
        [HideInInspector] _GradientColors_4 ("GradientColors_4", Color) = (0, 0, 0, 0)
        [HideInInspector] _GradientColors_5 ("GradientColors_5", Color) = (0, 0, 0, 0)
        [HideInInspector] _GradientColors_6 ("GradientColors_6", Color) = (0, 0, 0, 0)
        [HideInInspector] _GradientColors_7 ("GradientColors_7", Color) = (0, 0, 0, 0)
        [HideInInspector] _GradientAlphas_0 ("GradientAlphas_0", Color) = (0, 0, 0, 0)
        [HideInInspector] _GradientAlphas_1 ("GradientAlphas_1", Color) = (0, 0, 0, 0)
        [HideInInspector] _GradientAlphas_2 ("GradientAlphas_2", Color) = (0, 0, 0, 0)
        [HideInInspector] _GradientAlphas_3 ("GradientAlphas_3", Color) = (0, 0, 0, 0)
        [HideInInspector] _GradientAlphas_4 ("GradientAlphas_4", Color) = (0, 0, 0, 0)
        [HideInInspector] _GradientAlphas_5 ("GradientAlphas_5", Color) = (0, 0, 0, 0)
        [HideInInspector] _GradientAlphas_6 ("GradientAlphas_6", Color) = (0, 0, 0, 0)
        [HideInInspector] _GradientAlphas_7 ("GradientAlphas_7", Color) = (0, 0, 0, 0)

        [HideInInspector] _StrokeGradientColors_0 ("StrokeGradientColors_0", Color) = (0, 0, 0, 0)
        [HideInInspector] _StrokeGradientColors_1 ("StrokeGradientColors_1", Color) = (0, 0, 0, 0)
        [HideInInspector] _StrokeGradientColors_2 ("StrokeGradientColors_2", Color) = (0, 0, 0, 0)
        [HideInInspector] _StrokeGradientColors_3 ("StrokeGradientColors_3", Color) = (0, 0, 0, 0)
        [HideInInspector] _StrokeGradientColors_4 ("StrokeGradientColors_4", Color) = (0, 0, 0, 0)
        [HideInInspector] _StrokeGradientColors_5 ("StrokeGradientColors_5", Color) = (0, 0, 0, 0)
        [HideInInspector] _StrokeGradientColors_6 ("StrokeGradientColors_6", Color) = (0, 0, 0, 0)
        [HideInInspector] _StrokeGradientColors_7 ("StrokeGradientColors_7", Color) = (0, 0, 0, 0)
        [HideInInspector] _StrokeGradientAlphas_0 ("StrokeGradientAlphas_0", Color) = (0, 0, 0, 0)
        [HideInInspector] _StrokeGradientAlphas_1 ("StrokeGradientAlphas_1", Color) = (0, 0, 0, 0)
        [HideInInspector] _StrokeGradientAlphas_2 ("StrokeGradientAlphas_2", Color) = (0, 0, 0, 0)
        [HideInInspector] _StrokeGradientAlphas_3 ("StrokeGradientAlphas_3", Color) = (0, 0, 0, 0)
        [HideInInspector] _StrokeGradientAlphas_4 ("StrokeGradientAlphas_4", Color) = (0, 0, 0, 0)
        [HideInInspector] _StrokeGradientAlphas_5 ("StrokeGradientAlphas_5", Color) = (0, 0, 0, 0)
        [HideInInspector] _StrokeGradientAlphas_6 ("StrokeGradientAlphas_6", Color) = (0, 0, 0, 0)
        [HideInInspector] _StrokeGradientAlphas_7 ("StrokeGradientAlphas_7", Color) = (0, 0, 0, 0)

    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "PreviewType"="Plane"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        Cull Off
        Lighting Off
        ZTest [unity_GUIZTestMode]
        ColorMask [_ColorMask]
        // ---

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #define SLANT_SOFTMASK
            #include "SlantedCore.cginc"


            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP
            #pragma multi_compile_local _ SOFTMASK_EDITOR
            #pragma shader_feature_local _ SOFTMASKABLE // Add for soft mask

            ENDCG
        }
    }
}

