using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutRoom : MonoBehaviour
{
    [SerializeField] private Material transparentMaterial;
    private Material[] _childrenMaterials;
    private Renderer[] _childrenRenderers;

    public void StartFade(float seconds)
    {
        _childrenRenderers = GetComponentsInChildren<Renderer>();
        _childrenMaterials = new Material[_childrenRenderers.Length];
        for (int i = 0; i < _childrenRenderers.Length; i++)
        {
            _childrenMaterials[i] = Material.Instantiate(_childrenRenderers[i].material);
            _childrenMaterials[i].SetFloat("_Mode", 2);
            _childrenMaterials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            _childrenMaterials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            _childrenMaterials[i].SetInt("_ZWrite", 0);
            _childrenMaterials[i].DisableKeyword("_ALPHATEST_ON");
            _childrenMaterials[i].EnableKeyword("_ALPHABLEND_ON");
            _childrenMaterials[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
            _childrenMaterials[i].renderQueue = 3000;
            //_childrenMaterials[i] = _childrenRenderers[i].material;
        }

        StartCoroutine(FadeIn(seconds));
    }


    IEnumerator FadeIn(float fadeTime)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            //float lerp = Mathf.PingPong(Time.time, fadeTime) / fadeTime;
            for (int i = 0; i < _childrenRenderers.Length; i++)
            {
                _childrenRenderers[i].material.Lerp(_childrenMaterials[i], transparentMaterial, elapsedTime);
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
