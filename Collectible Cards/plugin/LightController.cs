using UnityEngine;

namespace CollectibleCards2
{
    public static class LightController
    {
        public static GameObject LightObj { get; set; } = null;
        public static void SetupPictureLight()
        {
            LightObj = new("Light");
            LightObj.transform.position = new Vector3(13.73f, 16.33f, 17.79f);
            LightObj.transform.eulerAngles = new Vector3(39.4763f, 209.4147f, 227.5626f);
            Light lightComp = LightObj.AddComponent<Light>();
            lightComp.intensity = 1.125f;
            lightComp.shadowBias = 0.5f;
            lightComp.shadowResolution = UnityEngine.Rendering.LightShadowResolution.High;
            lightComp.type = LightType.Directional;
        }
        public static void Cleanup()
        {
            UnityEngine.Object.Destroy(LightObj);
            LightObj = null;
        }
    }
}
