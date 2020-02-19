using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutRoom : MonoBehaviour
{
    //[SerializeField] private Material transparentMaterial;
    private Material[] _childrenMaterials;
    private Material[] _childrenMaterialsAlpha;
    private Renderer[] _childrenRenderers;

    public void StartFade(float seconds)
    {
        _childrenRenderers = GetComponentsInChildren<Renderer>();
        _childrenMaterials = new Material[_childrenRenderers.Length];
        _childrenMaterialsAlpha = new Material[_childrenRenderers.Length];

        for (int i = 0; i < _childrenRenderers.Length; i++)
        {
            //Duplicate children materials into transparent ones
            _childrenMaterials[i] = Material.Instantiate(_childrenRenderers[i].material);
            _childrenMaterials[i].SetFloat("_Mode", 2);
            _childrenMaterials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            _childrenMaterials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            _childrenMaterials[i].SetInt("_ZWrite", 0);
            _childrenMaterials[i].DisableKeyword("_ALPHATEST_ON");
            _childrenMaterials[i].EnableKeyword("_ALPHABLEND_ON");
            _childrenMaterials[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
            _childrenMaterials[i].renderQueue = 3000;

            //Asign transparent materials to children renderers
            _childrenRenderers[i].material = _childrenMaterials[i];

            //Duplicate transparent materials into ones with Alpha 0
            _childrenMaterialsAlpha[i] = Material.Instantiate(_childrenMaterials[i]);
            _childrenMaterialsAlpha[i].SetColor("_Color", new Color(255, 255, 255, 0));
        }

        //StartCoroutine(FadeOut(seconds));
    }


    IEnumerator FadeOut(float fadeTime)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;

            //Transition between transparent materials and alpha 0 materials
            for (int i = 0; i < _childrenRenderers.Length; i++)
            {
                _childrenRenderers[i].material.Lerp(_childrenMaterials[i], _childrenMaterialsAlpha[i], elapsedTime);
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public void DisableEverything()
    {
        _childrenRenderers = GetComponentsInChildren<Renderer>();
        foreach(Renderer rend in _childrenRenderers)
        {
            rend.gameObject.SetActive(false);
        }
    }

    public void EnableEverything()
    {
        _childrenRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in _childrenRenderers)
        {
            rend.gameObject.SetActive(true);
        }
    }
}
