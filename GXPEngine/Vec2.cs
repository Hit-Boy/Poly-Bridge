using System;
using System.Diagnostics.SymbolStore;
using System.Drawing;
using System.Runtime.CompilerServices;
using GXPEngine;

public struct Vec2 
{
	public float x;
	public float y;

	public Vec2 (float pX = 0, float pY = 0) 
	{
		x = pX;
		y = pY;
	}

	public override string ToString () 
	{
		return String.Format ("({0},{1})", x, y);
	}

	public static Vec2 Unit() {
		return new Vec2(1, 0);
	}

	public void Print() {
		Console.WriteLine(this);
	}

	public void SetXY(float pX, float pY) 
	{
		x = pX;
		y = pY;
	}

	public float Length() {
		return Mathf.Sqrt (x * x + y * y);
	}

	public void Normalize() {
		float len = Length ();
		if (len > 0) {
			x /= len;
			y /= len;
		}
	}

	public Vec2 Normalized() {
		Vec2 result = new Vec2 (x, y);
		result.Normalize ();
		return result;
	}

	public static Vec2 operator +(Vec2 left, Vec2 right) {
		return new Vec2 (left.x + right.x, left.y + right.y);
	}

	public static Vec2 operator -(Vec2 left, Vec2 right) {
		return new Vec2 (left.x - right.x, left.y - right.y);
	}

	public static Vec2 operator *(Vec2 v, float scalar) {
		return new Vec2 (v.x * scalar, v.y * scalar);
	}

	public static Vec2 operator *(float scalar, Vec2 v) {
		return new Vec2 (v.x * scalar, v.y * scalar);
	}

	public static Vec2 operator /(Vec2 v, float scalar) {
		return new Vec2 (v.x / scalar, v.y / scalar);
	}

	public float RadToDeg(float angle) {
		return angle * 180 / Mathf.PI;
	}
	
	public float DegToRad(float angle) {
		return angle / 180 * Mathf.PI;
	}


	// set this vec to target angle
	public void SetAngleDeg(float angle) {
		float tmp = GetAngleDeg();
		TurnDeg(angle - tmp);
	}

	//get angle of this 
	public float GetAngleDeg() {
		float angle = 0;
		float tmp = Mathf.Atan2(y, x);
		angle = RadToDeg(tmp);
		if (angle < 0)
			angle = 360 + angle;

		return angle;
	}
	

	// rotates this by deg
	public void TurnDeg(float rotation) {
		float angle = DegToRad(rotation);
		Vec2 tmp = this;
		tmp.x = Mathf.Cos(angle) * x - Mathf.Sin(angle) * y;
		tmp.y = Mathf.Sin(angle) * x + Mathf.Cos(angle) * y;
		this = tmp;

	}
	
	//rotates this by rad
	public void TurnRad(float rotation) {
		Vec2 tmp = this;
		tmp.x = Mathf.Cos(rotation) * x - Mathf.Sin(rotation) * y;
		tmp.y = Mathf.Sin(rotation) * x + Mathf.Cos(rotation) * y;
		this = tmp;
	}
	
	// rotates this point around a point
	public void TurnDegAroundPoint(Vec2 point, float rotation) {
		float angle = DegToRad(rotation);
		Vec2 tmp = this;
		tmp.x = Mathf.Cos(angle) * (point.x - x) - Mathf.Sin(angle) * (y - point.y);
		tmp.y = Mathf.Sin(angle) * (point.x - x) + Mathf.Cos(angle) * (y - point.y);
		this = tmp;
	}

	//dot product of this and vec2
	public float DotProduct(Vec2 vec2) {
		float dot = 0;
		dot = x * vec2.x + y * vec2.y;
		return dot;
	}

	//returns point on the vec2 that lies on normal from this point to that vec
	public Vec2 PointProjection(Vec2 vec2) {
		Vec2 projection = new Vec2();
		Vec2 firstPoint = new Vec2(vec2.x, vec2.y);
		Vec2 secondPoint = new Vec2(vec2.x/2, vec2.y/2);
		projection = this + VecNormalToLine(firstPoint, secondPoint);
		return projection;
	}

	// projects this on vec2
	public Vec2 VecProjection(Vec2 vec2) {
		Vec2 projection = new Vec2();
		vec2 = vec2.Normalized();
		projection = vec2 * DotProduct(vec2);
		return projection;
	}

	// normal from this point to line, consisting 2 points
	public float PointNormalToLine(Vec2 firstPoint, Vec2 secondPoint) {
		float normalLength = -1;
		float numerator = Mathf.Abs((x - firstPoint.x) * (firstPoint.y - secondPoint.y) + (y - firstPoint.y) * (secondPoint.x - firstPoint.x));
		float denominator = Mathf.Sqrt(Mathf.Pow(firstPoint.y - secondPoint.y, 2) + Mathf.Pow(secondPoint.x - firstPoint.x, 2));
		normalLength = numerator / denominator;
		return normalLength;
	}

	// on which side of the line with 2 points lies this point
	// returns 1, -1 or 0
	public int PointWhichSide(Vec2 firstPoint, Vec2 secondPoint) {
		int side = - 2;
		side = Mathf.Sign((x - firstPoint.x) * (firstPoint.y - secondPoint.y) + (y - firstPoint.y) * (secondPoint.x - firstPoint.x));
		return side;
	}

	//returns vec from this point to line with 2 points
	public Vec2 VecNormalToLine(Vec2 firstPoint, Vec2 secondPoint) {
		Vec2 vecNormal = new Vec2(secondPoint.y - firstPoint.y, secondPoint.x - firstPoint.x);
		vecNormal.Normalize();
		switch (PointWhichSide(firstPoint, secondPoint)) {
			case -1:
				vecNormal.y *= -1;
				vecNormal *= -1 * PointNormalToLine(firstPoint, secondPoint);
				break;
			case 1:
				vecNormal.x *= -1;
				vecNormal *= -1 * PointNormalToLine(firstPoint, secondPoint);
				break;
			case 0:
				vecNormal = new Vec2(0, 0);
				Console.WriteLine("Point is on the line");
				break;
			default:
				break;
		}
		
		return vecNormal;
	}

	//let this bounce of vec2
	public Vec2 VecReflect(Vec2 vec2, float bounciness) {
		Print();
		Vec2 result = new Vec2();
		vec2.Print();
		VecProjection(vec2).Print();
		result = -1 * this + (1 + bounciness) * (VecProjection(vec2));
		
		return result;
	}
	
	
}