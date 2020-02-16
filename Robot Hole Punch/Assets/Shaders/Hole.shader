Shader "Custom/Hole" {
    Properties{
        _Color("Color", Color) = (0.0, 0.0, 0.0, 0.0)
        _MainTex("Texture", 2D) = "white" { }
    }
        SubShader{
            Tags { "RenderType" = "Opaque" "Queue" = "Geometry+2"}
            ColorMask RGBA
            Cull Off
            ZTest LEqual
            Stencil {
                Ref 1
                Comp notequal
            }

            CGPROGRAM
            #pragma surface surf Lambert finalcolor:mycolor
            sampler2D _MainTex;
            struct Input {
                float2 uv_MainTex;
            };
            float4 _Color;
            void mycolor(Input IN, SurfaceOutput o, inout fixed4 color)
            {
                color *= _Color;
            }
            void surf(Input IN, inout SurfaceOutput o) {
                o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
                o.Normal = half3(0,0,-1);
                o.Alpha = 1;
            }
            ENDCG
    }
        Fallback "Diffuse"
}