using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRandomTextureRenderer : MonoBehaviour
{
    public Renderer pRenderer;
    public Texture[] arrText;
    public string[] arrTag;

    // Start is called before the first frame update
    void Start()
    {
        if (pRenderer == null) return;

        int nIdx = Random.Range(0, arrText.Length);
        for(int i=0; i<arrTag.Length; i++)
        {
            pRenderer.material.SetTexture(arrTag[i], arrText[nIdx]);
        }
    }
}
