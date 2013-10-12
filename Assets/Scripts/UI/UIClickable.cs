using UnityEngine;
using System.Collections;

public abstract class UIClickable : MonoBehaviour {
	
	/// <summary>
	/// Called during each frame when the mouse is pressed over this object.
	/// </summary>
	public virtual void UpdatePressed() {
	}
	
	/// <summary>
	/// Called once when the mouse enters this object.
	/// </summary>
	public virtual void MouseIn() {
	}
	
	/// <summary>
	/// Called once when the mouse exits this object.
	/// </summary>
	public virtual void MouseOut() {
	}
	
	/// <summary>
	/// Called once when the mouse is released over this object, or exits this object while pressed.
	/// </summary>
	public virtual void MouseUp() {
	}
	
	/// <summary>
	/// Called once when the mouse is pressed over this object.
	/// </summary>
	public virtual void MouseDown() {
	}
	
	/// <summary>
	/// Called once when the mouse is pressed and then released over this object (calls on release).
	/// </summary>
	public virtual void MouseClick() {
	}
}
