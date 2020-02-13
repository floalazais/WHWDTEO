using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering.HDPipeline;

public class ExposureController : MonoBehaviour
{
    Exposure expCompobent;
    [SerializeField] VolumeProfile volumeProfile;
    [SerializeField] float exposureCompensation;
    //[SerializeField] bool dofEnabled;

    // Start is called before the first frame update
    void Start()
    {
        Exposure tmp;
        if (volumeProfile.TryGet<Exposure>(out tmp))
        {
            expCompobent = tmp;
        }
    }

    // Update is called once per frames
    void Update()
    {
        //if (dofEnabled) dofComponent.focusMode.SetValue(new DepthOfFieldModeParameter(DepthOfFieldMode.UsePhysicalCamera, true));
        //else dofComponent.focusMode.SetValue(new DepthOfFieldModeParameter(DepthOfFieldMode.Off, true));
        expCompobent.compensation.SetValue(new MinFloatParameter(exposureCompensation, 0.1f, true));
    }
}
