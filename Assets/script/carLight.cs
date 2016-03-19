using UnityEngine;
using System.Collections;

public class carLight : MonoBehaviour {
    private GameObject _child;

    GameObject spotLight;
    bool lightEnable = true;

    // Use this for initialization
    void Start () {
        //_child = transform.FindChild("Child").gameObject;
        spotLight = transform.FindChild("Spotlight").gameObject;
    }
	
	// Update is called once per frame
	void Update () {

	}
}
