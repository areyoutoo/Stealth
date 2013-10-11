﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class UICamera : MonoBehaviour {
	public static UICamera main { get; protected set; }
	
	public int uiLayer     { get; protected set; }
	public int uiLayerMask { get; protected set; }
	
	public GameObject target { get; protected set; }
	
	GameObject mouseDownTarget;
	GameObject lastTarget;
	bool lastInputPress;
	
	UIClickable clickTarget;
	UIClickable lastClickTarget;
	
	protected virtual void Update() {
		uiLayer = gameObject.layer;
		uiLayerMask = 1 << uiLayer;
		
		Vector3 inputPos = Input.mousePosition;
		
		bool inputPress;
		#if UNITY_STANDALONE || UNITY_WEBPLAYER
		inputPress = Input.GetMouseButton(0);
		#else
		inputPress = Input.touchCount > 0;
		#endif
		
		RaycastHit hit;
		Ray ray = camera.ScreenPointToRay(inputPos);
		if (Physics.Raycast(ray, out hit, 100000f, uiLayerMask)) {
			target = hit.collider.gameObject;
			
			//has our mouse target changed? (in/out)
			if (target != lastTarget) {
				if (lastTarget != null) {
					if (lastClickTarget != null) {
						lastClickTarget.MouseOut();
					} else {
						lastTarget.SendMessage("MouseOut", SendMessageOptions.DontRequireReceiver);
					}
				}
				
				clickTarget = target.GetComponent<UIClickable>();
				if (clickTarget == null) {
					clickTarget.MouseIn();
				} else {
					target.SendMessage("MouseIn", SendMessageOptions.DontRequireReceiver);
				}
			}
			
			//has there been a change in click status? (up/down)
			if (inputPress != lastInputPress) {
				if (inputPress) {
					if (clickTarget != null) {
						clickTarget.MouseDown();
					} else {
						target.SendMessage("MouseDown", SendMessageOptions.DontRequireReceiver);
					}
					mouseDownTarget = target;
				} else {
					if (clickTarget != null) {
						clickTarget.MouseUp();
						if (mouseDownTarget == target) {
							clickTarget.MouseClick();
						}
					} else {
						target.SendMessage("MouseUp", SendMessageOptions.DontRequireReceiver);
						if (mouseDownTarget == target) {
							target.SendMessage("MouseClick", SendMessageOptions.DontRequireReceiver);
						}
					}
				}
			}
			
			//ongoing click target?
			if (inputPress && clickTarget != null) {
				clickTarget.UpdatePressed();
			}
		}
		
		lastTarget = target;
		lastClickTarget = clickTarget;
		lastInputPress = inputPress;
	}
	
	protected virtual void Awake() {
		main = this;
	}
	
	protected virtual void OnDestroy() {
		if (main == this) {
			main = null;
		}
	}
}
