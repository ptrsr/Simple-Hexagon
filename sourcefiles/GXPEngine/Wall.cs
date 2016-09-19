using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;

namespace GXPEngine
{
    class Wall : GameObject
    {
        public int side;
        public PointF[] points;
        public float dist = 700;



        protected float thick = 47;
        protected float speed = MyGame.GetSpeed();

        protected Vec2 vector0;
        protected Vec2 vector5;

        protected Vec2 vector1;
        protected Vec2 vector2;
        protected Vec2 vector3;
        protected Vec2 vector4;

        protected Vec2 closePoint;
        protected Vec2 farPoint;

        protected Vec2 midPos = new Vec2(MyGame.width / 2, MyGame.height / 2);

        public Wall(int _side)
        {
            side = _side;

            vector0 = new Vec2((Mathf.Cos((side * 60) * (Mathf.PI / 180))), (Mathf.Sin((side * 60) * (Mathf.PI / 180)))).Rotate(MyGame.rot - MyGame.GetRotSpeed() );
            vector5 = vector0.Clone().Rotate(60);
            UpdateWall();
        }

        public void UpdateWall()
        {
            dist -= speed;
            closePoint = new Vec2(dist, dist);
            farPoint = new Vec2(dist + thick, dist + thick);

            vector0 = vector0.Rotate(MyGame.GetRotSpeed());
            vector5 = vector5.Rotate(MyGame.GetRotSpeed());

            vector1 = vector0.Clone().Scale(farPoint).Add(midPos);
            vector2 = vector0.Clone().Scale(closePoint).Add(midPos);
            vector3 = vector5.Clone().Scale(closePoint).Add(midPos);
            vector4 = vector5.Clone().Scale(farPoint).Add(midPos);

            points = new PointF[]
            {
                Vec2.zero.ReturnPoint(vector1),
                Vec2.zero.ReturnPoint(vector2),
                Vec2.zero.ReturnPoint(vector3),
                Vec2.zero.ReturnPoint(vector4),
            };
        }

        public void StopWall()
        {
            if (speed > 0.2) { speed *= 0.9f; }
            else if (speed > 0 & speed < 0.2f) { speed = -speed; }
            else if (speed < 0) { speed *= 1.1f; }
        }

        public float Dist()
        {
            Vec2 Dif = Hexagon.ReturnPlayerPos().Clone().Substract(vector2);
            Vec2 lineNormal = vector2.Clone().Substract(vector3).Normal();
            float Dist = -Dif.Dot(lineNormal);
            return(Dist);
        }
    }
}

