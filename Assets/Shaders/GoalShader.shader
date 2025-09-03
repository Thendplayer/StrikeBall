Shader "Custom/GoalLimit"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _BorderColor ("Border Color", Color) = (0.2, 0.6, 1.0, 0.8)
        _WaveSpeed ("Wave Speed", Float) = 2.0
        _WaveFrequency ("Wave Frequency", Float) = 10.0
        _WaveAmplitude ("Wave Amplitude", Float) = 0.1
        _DistortionStrength ("Distortion Strength", Float) = 0.05
        _Transparency ("Transparency", Range(0, 1)) = 0.7
        _EdgeGlow ("Edge Glow", Float) = 2.0
        _NoiseScale ("Noise Scale", Float) = 5.0
    }
    
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent" 
            "Queue"="Transparent"
            "IgnoreProjector"="True"
        }
        
        LOD 100
        
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
                float3 normal : TEXCOORD3;
                UNITY_FOG_COORDS(4)
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _BorderColor;
            float _WaveSpeed;
            float _WaveFrequency;
            float _WaveAmplitude;
            float _DistortionStrength;
            float _Transparency;
            float _EdgeGlow;
            float _NoiseScale;
            
            // Simple noise function for procedural effects
            float noise(float2 pos)
            {
                return frac(sin(dot(pos, float2(12.9898, 78.233))) * 43758.5453);
            }
            
            // Fractional Brownian Motion for more complex noise
            float fbm(float2 pos)
            {
                float value = 0.0;
                float amplitude = 0.5;
                float frequency = 1.0;
                
                for(int i = 0; i < 4; i++)
                {
                    value += amplitude * noise(pos * frequency);
                    amplitude *= 0.5;
                    frequency *= 2.0;
                }
                
                return value;
            }
            
            v2f vert (appdata v)
            {
                v2f o;
                
                // Calculate world position
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.worldPos = worldPos.xyz;
                
                // Add wave distortion to vertex position
                float time = _Time.y * _WaveSpeed;
                float wave = sin(worldPos.x * _WaveFrequency + time) * 
                           cos(worldPos.z * _WaveFrequency * 0.7 + time * 0.8);
                
                // Apply vertical wave displacement
                worldPos.y += wave * _WaveAmplitude;
                
                // Add noise-based distortion
                float2 noisePos = worldPos.xz * _NoiseScale + time * 0.1;
                float noiseValue = fbm(noisePos) * 2.0 - 1.0;
                worldPos.xyz += v.normal * noiseValue * _DistortionStrength;
                
                o.vertex = mul(UNITY_MATRIX_VP, worldPos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = normalize(_WorldSpaceCameraPos - worldPos.xyz);
                
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float time = _Time.y * _WaveSpeed;
                
                // Sample main texture with animated UV distortion
                float2 distortedUV = i.uv;
                distortedUV += sin(i.worldPos.xz * _WaveFrequency + time) * _DistortionStrength * 0.1;
                
                fixed4 texColor = tex2D(_MainTex, distortedUV);
                
                // Create animated noise pattern
                float2 noiseUV = i.worldPos.xz * _NoiseScale + time * 0.5;
                float noisePattern = fbm(noiseUV);
                
                // Create edge glow effect based on viewing angle
                float fresnel = 1.0 - dot(i.normal, i.viewDir);
                fresnel = pow(fresnel, _EdgeGlow);
                
                // Animated wave pattern
                float wavePattern = sin(i.worldPos.x * _WaveFrequency + time) * 
                                  cos(i.worldPos.z * _WaveFrequency * 0.7 + time * 0.8) * 0.5 + 0.5;
                
                // Combine all effects
                float3 finalColor = _BorderColor.rgb;
                finalColor *= (1.0 + fresnel * 2.0); // Enhance with fresnel
                finalColor += wavePattern * 0.3; // Add wave brightness variation
                finalColor += noisePattern * 0.2; // Add noise variation
                finalColor *= texColor.rgb; // Multiply by texture
                
                // Calculate final alpha
                float alpha = _BorderColor.a * _Transparency;
                alpha *= (0.7 + fresnel * 0.3); // Vary alpha with viewing angle
                alpha *= (0.8 + wavePattern * 0.4); // Vary alpha with waves
                alpha *= (0.9 + noisePattern * 0.2); // Add noise to alpha
                
                fixed4 finalResult = fixed4(finalColor, alpha);
                
                UNITY_APPLY_FOG(i.fogCoord, finalResult);
                return finalResult;
            }
            ENDCG
        }
    }
    
    Fallback "Sprites/Default"
}