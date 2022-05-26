// Basic constraint: two points should be at a given distance from each other:

using System;
using GXPEngine;

public class VerletConstraint : Sprite{
	public VerletPoint one;
	public VerletPoint two;
	public Vec2 midPoint;
	public readonly float length;
	public float mass = 0.5f;

	public VerletConstraint(VerletPoint pOne, VerletPoint pTwo) : base("Platform.png", (pOne.position - pTwo.position).Length()/384, 0.5f, false){
		one = pOne;
		two = pTwo;
		length = (one.position - two.position).Length ();
		midPoint.x = (one.x + two.x) / 2;
		midPoint.y = (one.y + two.y) / 2;
		SetOrigin(0, height/2);
		UpdateConstraintSprite();
	}

	public void Apply() {
		Vec2 diff = two.position - one.position;
		float currentlength = diff.Length();
		diff.Normalize ();
		diff = (length - currentlength) * diff;
		
		if (!one._fixed && !two._fixed) {
			one.position -= diff * 0.5f;
			two.position += diff * 0.5f;
		} else if (!one._fixed && two._fixed) {
			one.position -= diff;
		} else if (one._fixed && !two._fixed) {
			two.position += diff;
		}
	}

	public void UpdateConstraintSprite() {
		SetXY(one.x, one.y);
		rotation = new Vec2(two.x - one.x, two.y - one.y).GetAngleDeg();
	}


}