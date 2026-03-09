using UnityEngine;

public class Test_ScaleFromAudio : MonoBehaviour
{
    [Header("AudioSetup")]
    public AudioSource source;

    public Vector3 minScale;
    public Vector3 maxScale;
    public FrequencyDetection detector;

    public float loudnessSensibility = 100;
    public float threshold = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;

        if (loudness < threshold)
            threshold = 0;

        //Lerp value from minimum to maximum scale
        transform.localScale = Vector3.Lerp(minScale, maxScale, loudness);
    }
}