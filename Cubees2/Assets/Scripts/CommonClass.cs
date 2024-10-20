using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonClass : MonoBehaviour
{
    public struct MovingParameters {
        Vector3 next_position;
        Vector3 next_rotation;

        public void set(Vector3 _next_position, Vector3 _next_rotation){
            next_position = _next_position;
            next_rotation = _next_rotation;
        }

        public Vector3 getPos() { return next_position; }
        public Vector3 getRot() { return next_rotation; }

        public static bool operator ==( MovingParameters a,  MovingParameters b) { return a.next_position.Equals(b.next_position) && a.next_rotation.Equals(b.next_rotation); }
        public static bool operator !=( MovingParameters a,  MovingParameters b) { return !(a == b); }

        public override bool Equals(object obj)
        {
            if (!(obj is MovingParameters other)) return false;
            return next_position == other.next_position && next_rotation == other.next_rotation;
        }
        public override int GetHashCode()
        {
            return next_position.GetHashCode() ^ next_rotation.GetHashCode();
        }
    }

    public float GetHeight(AnimationCurve curve, float x){
        return Mathf.Sqrt(0.5f - Mathf.Pow(curve.Evaluate(x) - 0.5f, 2f)) - 0.5f;
    }
}
