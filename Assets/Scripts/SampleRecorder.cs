using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SampleRecorder : MonoBehaviour {

    private struct SingleSample {
        public List<Vector3> trackersWorldPositions;
        //TODO: rotazioni
        public int tickIndex;
    }

    private float tickCadence = 0.05f;

    public List<Transform> trackersTransform;

    public GameObject prefabTrackerCube;

    private List<SingleSample> memoryMovments;

    public bool recordNow;

    public bool playbackNow;

    private int currentTick = 0;

    private float cooldownTick;

    public List<GameObject> trackersPreview;

    // Use this for initialization
    void Start () {
        memoryMovments = new List<SingleSample> ();
        recordNow = false;
        playbackNow = false;
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

        if(playbackNow && cooldownTick <= 0) {
            if (currentTick >= memoryMovments.Count)
                currentTick = 0;
            currentTick++;
            CreatePreviewTrackers();
            cooldownTick = tickCadence;
        }
    }
    private void CreatePreviewTrackers () {
        if (trackersPreview == null || trackersPreview.Count != trackersTransform.Count) {
            trackersPreview = new List<GameObject> ();
            for (int i = 0; i < trackersTransform.Count; i++) {
                trackersPreview.Add (Instantiate (prefabTrackerCube, transform));
              // trackersPreview[i].name = StaticTestList.ArtList[i];
                trackersPreview[i].GetComponent<MeshRenderer>().material.color = StaticTestList.ColorList[i];
            }
        }

        for (int i = 0; i < trackersPreview.Count; i++)
        {
            trackersPreview[i].transform.DOMove(memoryMovments[currentTick - 1].trackersWorldPositions[i], tickCadence);
        }
    }

    
    public void StartRecording () {
        recordNow = true;
        cooldownTick = 0;
        memoryMovments.Clear();
        currentTick = 0;
    }

    public void StopRecording () {
        recordNow = false;
    }

    public void StartPlayback()
    {
        currentTick = 0;
        playbackNow = true;
    }

    public void StopPlayback()
    {
        playbackNow = false;
    }

}