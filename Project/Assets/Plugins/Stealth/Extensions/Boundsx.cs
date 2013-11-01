using UnityEngine;
using System.Collections;

public static class Boundsx {
	public static Bounds EncapsulateAll(Bounds[] bounds) {
		Bounds b;
		if (bounds.Length > 0) {
			b = new Bounds(bounds[0].center, bounds[0].size);
			for (int i=1; i<bounds.Length; i++) {
				b.Encapsulate(bounds[i]);
			}
		} else {
			b = new Bounds(Vector3.zero, Vector3.zero);
		}
		
		return b;
	}
	
	public static Bounds EncapsulateAll(Renderer[] renderers) {
		return EncapsulateAll<Renderer>(renderers, r => r.bounds);
	}
	
	public static Bounds EncapsulateAll(Collider[] colliders) {
		return EncapsulateAll<Collider>(colliders, c => c.bounds);
	}
	
	static Bounds EncapsulateAll<T>(T[] args, System.Func<T,Bounds> boundsFunc) {
		Bounds b;
		if (args.Length > 0) {
			Bounds first = boundsFunc(args[0]);
			b = new Bounds(first.center, first.size);
			for (int i=1; i<args.Length; i++) {
				b.Encapsulate(boundsFunc(args[i]));
			}
		} else {
			b = new Bounds(Vector3.zero, Vector3.zero);
		}
		
		return b;
	}
}
