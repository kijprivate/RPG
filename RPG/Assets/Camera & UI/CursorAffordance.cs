using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorAffordance : MonoBehaviour {

    [SerializeField]Texture2D walkCursor = null;
    [SerializeField]Texture2D fightCursor = null;
    [SerializeField]Texture2D errorCursor = null;
    [SerializeField]Vector2 cursorHotspot = new Vector2(0, 0);

    [SerializeField] const int walkableLayerNumber = 8;
    [SerializeField] const int enemyLayerNumber = 9;

    private CameraRaycaster camRay;
	// Use this for initialization
	void Start () {
        camRay = GetComponentInChildren<CameraRaycaster>();
        camRay.notifyLayerChangeObservers += OnLayerChange;
    }

    // Update is called once per frame
    void OnLayerChange(int newLayer) {
        switch (newLayer)
        {
            case walkableLayerNumber:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                break;
            case enemyLayerNumber:
                Cursor.SetCursor(fightCursor, cursorHotspot, CursorMode.Auto);
                break;
            default:
                Debug.Log("Dont know whay the layer is");
                Cursor.SetCursor(errorCursor, cursorHotspot, CursorMode.Auto);
                return;
        }

    }

}
