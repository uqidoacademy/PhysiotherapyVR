using UnityEngine;

// Starting in 0 seconds
// the function will be launched every x seconds (repeat rate)

public class TimingSample : MonoBehaviour
{
  [SerializeField] float RepeatRate = 0.1f;

  void Start()
  {
    InvokeRepeating("TestFunc", 0.0f, RepeatRate);
  }

  void TestFunc()
  {
    Debug.Log("repeat every " + RepeatRate);
  }
}
