using GXPEngine;

public class Mover{

    public float x {
        get {
            return position.x;
        }
        set {
            position.x = value;
        }
    }
    public float y {
        get {
            return position.y;
        }
        set {
            position.y = value;
        }
    }


    public Vec2 velocity;
    public Vec2 position;
    public Vec2 acceleration;
    public Vec2 oldPosition;
    public float radius = 20f;
    public float mass = 1f;

    public Mover(Vec2 newPosition) {
        position = newPosition;
        velocity = new Vec2();
        acceleration = new Vec2();
    }

    public Mover(float px, float py) : this(new Vec2(px, py)) {}

    public void MoveMover() {
        velocity += acceleration;
        position += velocity;

        oldPosition = position;

        acceleration.SetXY (0, 0);
    }

    public void AddAcceleration(Vec2 pacceleration) {
        acceleration += pacceleration;
    }
    
    

}