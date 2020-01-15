using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering.HDPipeline;

public class DofController : MonoBehaviour
{
    DepthOfField dofComponent;
    [SerializeField] VolumeProfile volumeProfile;
    [SerializeField] float focusDist;
    [SerializeField] bool dofEnabled;

    // Start is called before the first frame update
    void Start()
    {
        DepthOfField tmp;
        if (volumeProfile.TryGet<DepthOfField>(out tmp))
        {
            dofComponent = tmp;
        }
    }

    // Update is called once per frames
    void Update()
    {
        if (dofEnabled) dofComponent.focusMode.SetValue(new DepthOfFieldModeParameter(DepthOfFieldMode.UsePhysicalCamera, true));
        else dofComponent.focusMode.SetValue(new DepthOfFieldModeParameter(DepthOfFieldMode.Off, true));
        dofComponent.focusDistance.SetValue(new MinFloatParameter(focusDist, 0.1f, true));
    }
}
