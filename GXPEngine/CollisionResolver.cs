using System;
using System.Diagnostics.Contracts;
using GXPEngine;
using GXPEngine.Core;

public class CollisionResolver {
    private VerletBody verletBody;
    private Mover mover;
    public bool collisionThisFrame;
    private float Cr = 0.95f; // coefficent of restitution
    private float angularMult = 0.8f;
    
    public CollisionResolver(VerletBody verletBody, Mover mover) {
        this.verletBody = verletBody;
        this.mover = mover;
    }

    public void VerletBoundaries() {
        foreach (VerletPoint p in verletBody.point) {
            if (p.position.y > 550) {
                p.y = 550; 
            }
            if (p.position.y < -550) {
                p.y = -550; 
            }

            if (p.x < 10) {
                p.x = 10; 
            }
            if (p.x > 790) {
                p.x = 790; 
            }
        }
    }

    public void CollisionCheck() {
        
        Vec2 moverToFirstPoint = new Vec2();
        Vec2 moverToSecondPoint = new Vec2();
        Vec2 moverNormalToPLatform;
        foreach (VerletConstraint constraint in verletBody.constraint) {
            moverToFirstPoint.SetXY(Mathf.Abs(mover.x - constraint.one.x), Mathf.Abs(mover.y - constraint.one.y));
            moverToSecondPoint.SetXY(Mathf.Abs(mover.x - constraint.two.x), Mathf.Abs(mover.y - constraint.two.y));
            moverNormalToPLatform = mover.position.VecNormalToLine(constraint.one.position, constraint.two.position);
            
            if (moverNormalToPLatform.Length() - mover.radius < float.Epsilon) {
                Vec2 POI = mover.position.VecNormalToLine(constraint.one.position, constraint.two.position) + mover.position;
                if(new Vec2(POI.x - constraint.one.x, POI.y - constraint.one.y).Length() + 
                   new Vec2(POI.x - constraint.two.x, POI.y - constraint.two.y).Length() <= 
                   new Vec2(constraint.one.x - constraint.two.x, constraint.one.y - constraint.two.y).Length()){
                   
                    Vec2 offset = new Vec2();
                    if (mover.oldPosition.PointWhichSide(constraint.one.position, constraint.two.position) ==
                        mover.position.PointWhichSide(constraint.one.position, constraint.two.position)) {
                        offset = mover.radius * moverNormalToPLatform.Normalized() - moverNormalToPLatform;

                    }
                    else {
                        offset = -mover.radius * moverNormalToPLatform.Normalized();
                    }

                    PlatformAndCircleCollision(offset, mover, constraint);
                }
            }
        }
    }


    private void PlatformAndCircleCollision(Vec2 offset, Mover mover, VerletConstraint constraint) {
        
        mover.position -= offset;

        if (!collisionThisFrame) {
            float vBallBefore = mover.velocity.Length();
            float vConstraintAfter;
            float vBallAfter;
            float vConstraintBefore = (Vec2.FindCenter(constraint.one.position, constraint.two.position) -
                                       Vec2.FindCenter(constraint.one.oldPosition, constraint.two.oldPosition)).Length();

            
            vBallAfter = (Cr * constraint.mass * (vBallBefore - vConstraintBefore) + constraint.mass * vConstraintBefore +
                         mover.mass * vBallBefore) / (constraint.mass + mover.mass);
            if(constraint.one._fixed && constraint.two._fixed)
                mover.velocity = mover.velocity.VecReflect(new Vec2(constraint.one.x - constraint.two.x, constraint.one.y - constraint.two.y), 1) * Cr;
            else {
                mover.velocity = mover.velocity.VecReflect(new Vec2(constraint.one.x - constraint.two.x, constraint.one.y - constraint.two.y), 1).Normalized()  * vBallAfter;
            }
            
            vConstraintAfter = (Cr * mover.mass * (vConstraintBefore - vBallBefore) + constraint.mass * vConstraintBefore +
                                mover.mass * vBallBefore) / (constraint.mass + mover.mass);
            
            if (!constraint.one._fixed && !constraint.two._fixed) {
                constraint.one.oldPosition -= offset.Normalized() * vConstraintAfter;
                constraint.two.oldPosition -= offset.Normalized() * vConstraintAfter;
            }
            else if(!constraint.one._fixed && constraint.two._fixed) {
                constraint.one.oldPosition -= offset.Normalized() * vConstraintAfter;
            }
            else if(constraint.one._fixed && !constraint.two._fixed) {
                constraint.two.oldPosition -= offset.Normalized() * vConstraintAfter;
            }
            
            Vec2 POIOnTheLine = mover.position + mover.radius * offset.Normalized();
            GiveAngularVelocity(constraint, POIOnTheLine, offset, vConstraintAfter);
                
            collisionThisFrame = true;
        }
    }

    private void GiveAngularVelocity(VerletConstraint constraint, Vec2 POI, Vec2 offset, float speedAfter) {
        float lengthToPointOne = new Vec2(POI.x - constraint.one.x, POI.y - constraint.one.y).Length();
        float lengthToPointTwo = new Vec2(POI.x - constraint.two.x, POI.y - constraint.two.y).Length();

        if (!constraint.one._fixed && !constraint.two._fixed) {
            if (lengthToPointOne > lengthToPointTwo) {
                constraint.one.oldPosition -=
                    offset.Normalized() * lengthToPointTwo / (lengthToPointOne + lengthToPointTwo) * speedAfter * angularMult;
                constraint.two.oldPosition +=
                    offset.Normalized() * lengthToPointOne / (lengthToPointOne + lengthToPointTwo) * speedAfter * angularMult;
            }
            else {
                constraint.one.oldPosition +=
                    offset.Normalized() * lengthToPointTwo / (lengthToPointOne + lengthToPointTwo) * speedAfter * angularMult;
                constraint.two.oldPosition -=
                    offset.Normalized() * lengthToPointOne / (lengthToPointOne + lengthToPointTwo) * speedAfter * angularMult;
            } 
        }
        else if(!constraint.one._fixed && constraint.two._fixed) {
            constraint.one.oldPosition += offset.Normalized() * speedAfter * angularMult;
        }
        else if(constraint.one._fixed && !constraint.two._fixed) {
            constraint.two.oldPosition += offset.Normalized() * speedAfter * angularMult;
        }

    }
}
