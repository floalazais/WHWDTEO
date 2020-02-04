using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    [SerializeField] private Material transparentMaterial;
    private Material[] _childrenMaterials;
    private Renderer[] _childrenRenderers;

    public void StartFade(float seconds)
    {
        Debug.Log("Start Fade");
        _childrenRenderers = GetComponentsInChildren<Renderer>();
        _childrenMaterials = new Material[_childrenRenderers.Length];
        for (int i = 0; i < _childrenRenderers.Length; i++)
        {
            _childrenMaterials[i] = _childrenRenderers[i].material;
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
            Debug.Log("Time: " + elapsedTime);
        }
    }
}
