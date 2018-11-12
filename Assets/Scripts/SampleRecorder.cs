using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleRecorder : MonoBehaviour {

    private struct SingleSample {
        public List<Vector3> trackersWorldPositions;
        //TODO: rotazioni
        public int tickIndex;
    }

    public float tickCadence;

    public List<Transform> trackersTransform;

    public GameObject prefabTrackerCube;

    private List<SingleSample> memoryMovments;

    private bool recordNow;

    private int currentTick = 0;

    private float cooldownTick;

    private List<GameObject> trackersPreview;

    // Use this for initialization
    void Start () {
        memoryMovments = new List<SingleSample> ();
        recordNow = false;
    }

    void Update () {
        cooldownTick -= Time.deltaTime;

        if (recordNow && cooldownTick <= 0) {
            SingleSample sample = new SingleSample () { };
            sample.trackersWorldPositions = new List<Vector3>();

            for (int i = 0; i < trackersTransform.Count; i++) {
                sample.trackersWorldPositions.Add (trackersTransform[i].position);
            }
            sample.tickIndex = currentTick;
            memoryMovments.Add(sample);
            currentTick++;

            cooldownTick = tickCadence;

            CreatePreviewTrackers ();
        }
    }

    private void CreatePreviewTrackers () {
        if (trackersPreview == null) {
            trackersPreview = new List<GameObject> ();
            for (int i = 0; i < trackersTransform.Count; i++) {
                trackersPreview.Add (Instantiate (prefabTrackerCube, transform));

                trackersPreview[i].GetComponent<MeshRenderer>().material.color = StaticTestList.ColorList[i];
            }
        }

        for (int i = 0; i < trackersTransform.Count; i++) {
            trackersPreview[i].transform.position = memoryMovments[memoryMovments.Count-1].trackersWorldPositions[i];
        }
    }

    
    public void StartRecording () {
        recordNow = true;
        cooldownTick = 0;
    }

    public void StopRecording () {
        recordNow = false;
    }


}