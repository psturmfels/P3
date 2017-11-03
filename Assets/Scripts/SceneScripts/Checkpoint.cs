using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour {
    public static GameObject bridgeMellow;
    public static GameObject stiltMellow;
    public static CameraMovement cm;

    private static Vector3 CheckpointPos;
    private static PlayerActions controllerActions;
    private static PlayerActions keyboardActions;

    private SpriteRenderer sr;
    private Color activeColor = new Color(0.3f, 1f, 0.3f, 1.0f);
    private bool bridgeAtCheckpoint = false;
    private bool stiltAtCheckpoint = false;

    // Use this for initialization
    void Start () {
        sr = GetComponent<SpriteRenderer>();

        bridgeMellow = GameObject.Find("BridgeMellowMove");
        stiltMellow = GameObject.Find("StiltMellowMove");
        CheckpointPos = bridgeMellow.transform.position + Vector3.left;
        cm = Camera.main.gameObject.GetComponent<CameraMovement>();

        controllerActions = PlayerActions.CreateWithControllerBindings();
        keyboardActions = PlayerActions.CreateWithKeyboardBindings();

        if (bridgeMellow.GetComponent<MellowCrushed>() != null) {
            bridgeMellow.GetComponent<MellowCrushed>().Respawn += ResetBridgeToCheckpoint;
            bridgeMellow.GetComponent<MellowCrushed>().Remove += RegisterMellowRemoved;
        }
        if (stiltMellow.GetComponent<MellowCrushed>() != null) {
            stiltMellow.GetComponent<MellowCrushed>().Respawn += ResetStiltToCheckpoint;
            stiltMellow.GetComponent<MellowCrushed>().Remove += RegisterMellowRemoved;
        }
    }
    // Update is called once per frame
    void Update() {
        if (controllerActions.ResetCheckpoint.WasPressed || keyboardActions.ResetCheckpoint.WasPressed) {
            ResetStiltToCheckpoint();
            ResetBridgeToCheckpoint();
        }

        if (controllerActions.ResetLevel.WasPressed || keyboardActions.ResetLevel.WasPressed) {
            ResetToBeginning();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!bridgeAtCheckpoint && collision.gameObject == bridgeMellow) {
            bridgeAtCheckpoint = true;
            if (stiltAtCheckpoint) {
                SetCheckpoint(transform.position);
            }
        }
        else if (!stiltAtCheckpoint && collision.gameObject == stiltMellow) {
            stiltAtCheckpoint = true;
            if (bridgeAtCheckpoint) {
                SetCheckpoint(transform.position);
            }
        }
    }

    public void SetCheckpoint(Vector3 pos) {
        CheckpointPos = pos;
        sr.color = activeColor;
    }

    void RegisterMellowRemoved() {
        cm.RegisterMellowRemoved(CheckpointPos);
    }

    void ResetStiltToCheckpoint() {
        stiltMellow.transform.position = CheckpointPos - new Vector3(1, 0, 0);
    }

    void ResetBridgeToCheckpoint() {
        bridgeMellow.transform.position = CheckpointPos + new Vector3(1, 0, 0);
    }

    public void ResetToBeginning() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
