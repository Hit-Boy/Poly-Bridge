using System.Collections.Generic;
using System;
using GXPEngine;

public class VerletBody: GameObject {
	public List<VerletPoint> point;
	public List<VerletConstraint> constraint;

	public VerletBody() {
		point = new List<VerletPoint> ();
		constraint = new List<VerletConstraint> ();
	}

	public void AddPoint(Vec2 Position, bool pFixed) {
		VerletPoint p = new VerletPoint (Position, pFixed);
		point.Add(p);
		AddChild(p);
	}

	public void AddConstraint(int p1, int p2) {
		VerletConstraint c = new VerletConstraint (point[p1], point[p2]);
		constraint.Add (c);
		AddChildAt(c, 0);
	}

	public void AddAcceleration(Vec2 acceleration) {
		foreach (VerletPoint p in point) {
			p.acceleration += acceleration;
		}
	}

	public void UpdateVerlet() {
		foreach (VerletPoint p in point) {
			p.Step ();
		}
	}

	public void UpdateConstraints() {
		foreach (VerletConstraint c in constraint) {
			c.Apply ();
		}
	}
	
	public void RemoveLastPoint()
	{
		point.RemoveAt(point.Count - 1);
	}
}

