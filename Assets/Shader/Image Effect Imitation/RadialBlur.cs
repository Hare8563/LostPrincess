using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RadialBlur : MonoBehaviour 
{
    /// <summary>
    /// ぼかし度合い
    /// </summary>
    public int iterations = 3;
    /// <summary>
    /// 放射度
    /// </summary>
    public float blurSpread = 0.6f;
    /// <summary>
    /// ブラーシェーダー
    /// </summary>
    public Shader radialBlurShader = null;
    /// <summary>
    /// マテリアル
    /// </summary>
    static Material m_Material = null;
    /// <summary>
    /// 蓄積レンダーテクスチャ
    /// </summary>
    private RenderTexture accumTexture;

    /// <summary>
    /// マテリアル情報のプロパティ
    /// </summary>
    protected Material material
    {
        get
        {
            if (m_Material == null)
            {
                m_Material = new Material(radialBlurShader);
                m_Material.hideFlags = HideFlags.DontSave;
            }
            return m_Material;
        }
    }

    /// <summary>
    /// マテリアル無効化
    /// </summary>
    protected void OnDisable()
    {
        if (m_Material)
        {
            DestroyImmediate(m_Material);
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    protected void Start()
    {
        ////イメージエフェクトをサポートしていない場合は無効化
        //if (!SystemInfo.supportsImageEffects)
        //{
        //    enabled = false;
        //    return;
        //}
        ////PCがシェーダーを実行できない場合は無効化
        //if (!radialBlurShader || !material.shader.isSupported)
        //{
        //    enabled = false;
        //    return;
        //}
    }

    /// <summary>
    /// レンダリング
    /// </summary>
    /// <param name="source">レンダーテクスチャ</param>
    /// <param name="destination"></param>
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // 蓄積レンダーテクスチャ作成
        if (accumTexture == null || accumTexture.width != source.width || accumTexture.height != source.height)
        {
            DestroyImmediate(accumTexture);
            accumTexture = new RenderTexture(source.width, source.height, 0);
            accumTexture.hideFlags = HideFlags.HideAndDontSave;
            //RenderTexture から2DTextureへの変換
            Graphics.Blit(source, accumTexture);
        }
        //ブラートレイルを徐々に短くする
        blurSpread = Mathf.Clamp(blurSpread, 0.0f, 0.92f);
        //反映
        material.SetTexture("_MainTex", accumTexture);
        material.SetFloat("_AccumOrig", 1.0F - blurSpread);
    }

    //// Called by the camera to apply the image effect
    //void OnRenderImage(RenderTexture source, RenderTexture destination)
    //{
    //    int rtW = source.width / 4;
    //    int rtH = source.height / 4;
    //    RenderTexture buffer = RenderTexture.GetTemporary(rtW, rtH, 0);

    //    // Copy source to the 4x4 smaller texture.
    //    DownSample4x(source, buffer);

    //    // Blur the small texture
    //    for (int i = 0; i < iterations; i++)
    //    {
    //        RenderTexture buffer2 = RenderTexture.GetTemporary(rtW, rtH, 0);
    //        FourTapCone(buffer, buffer2, i);
    //        RenderTexture.ReleaseTemporary(buffer);
    //        buffer = buffer2;
    //    }
    //    Graphics.Blit(buffer, destination);

    //    RenderTexture.ReleaseTemporary(buffer);
    //}
}
