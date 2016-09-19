using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class Hexagon : Canvas
    {
        private static Vec2 playerPos;
        public int playerSide;
        public int oldSide;

        protected bool first = true;
        protected int _stop;
        protected float _time = 0;
        protected int _playerRot = -150;
        protected int textAlpha = 0;
        protected int wave = 0;
        public int timer = 0;

        protected Vec2 midPos = new Vec2(MyGame.width / 2, MyGame.height / 2);

        List<Wall> list = new List<Wall>();

        public Hexagon(int width, int height)
            : base(width, height)
        {
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Waves.LoadWave();

        }

        public void Menu()
        {
            if (_stop == 1 || (60 - ((MyGame.rot + 14.8f) % 60)) > 57)
            {
                MyGame.SetRotSpeed(MyGame.GetRotSpeed() * (60 - ((MyGame.rot + 14.8f) % 60)) / 60);
                MyGame.Rotate();

                _stop = 1;


                if (60 - ((MyGame.rot + 15) % 60) < 45)
                {
                    _stop = 2;
                    MyGame.SetRotSpeed(0);
                }
            }
            else
            {
                MyGame.Rotate();
            }

            DrawBackground();
            DrawHexagon();
            DrawPlayer();
            DrawWalls();
            DrawText();
            DrawTimer();
            first = true;
        }

        public void Play()
        {
            _stop = 0;
            MyGame.SetRotSpeed(2);
            timer++;

            if (first == true) { timer = 0; }
            first = false;

            graphics.Clear(Color.Transparent);
            CheckSpawn();

            DrawBackground();
            DrawHexagon();
            DrawPlayer();
            DrawWalls();
            CheckCollision();
            DrawTimer();
        }

        void CheckSpawn()
        {
            if (_time <= 0)
            {
                _time = MyGame.GetInterval();
                for (int i = 0; i < 6; i++)
                {
                    if (Waves.ReturnWalls()[i, wave] == 1) { SpawnWall(i); }
                }
                if (wave == Waves.ReturnLength())
                {
                    wave = 0;
                    Waves.LoadWave();
                }
                else { wave++; }
            }
            _time--;
        }

        void SpawnWall(int side)
        {
            Wall wall = new Wall(side);
            list.Add(wall);
        }

        void DrawWalls()
        {
            if (list.Count > 0)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    Wall wall = list[i];
                    graphics.FillPolygon(Brushes.White, wall.points);
                    wall.UpdateWall();

                    if (_stop == 1 || _stop == 2)
                    {
                        wall.StopWall();
                        if (wall.dist > 1000) { list.Remove(wall); }
                    }

                    if (wall.dist < -5) { list.Remove(wall); }

                }
            }
        }

        void DrawBackground()
        {
            for (int i = 0; i <= 5; i++)
            {
                if (i % 2 == 0)
                {
                    graphics.FillPie(Brushes.DarkBlue, -width, -height, width * 3, height * 3, i * 60 + MyGame.rot, 60);
                }
                else
                {
                    graphics.FillPie(Brushes.Blue, -width, -height, width * 3, height * 3, i * 60 + MyGame.rot, 60);
                }
            }
        }

        public void DrawHexagon()
        {
            Vec2 vector = new Vec2(Mathf.Cos(MyGame.rot * Mathf.PI / 180), Mathf.Sin(MyGame.rot * Mathf.PI / 180));
            PointF[] points = {
                                  Vec2.zero.ReturnPoint(vector.Clone().Scale(new Vec2(40,40)).Add(midPos)),
                                  Vec2.zero.ReturnPoint(vector.Clone().Rotate(60).Scale(new Vec2(40,40)).Add(midPos)),
                                  Vec2.zero.ReturnPoint(vector.Clone().Rotate(120).Scale(new Vec2(40,40)).Add(midPos)),
                                  Vec2.zero.ReturnPoint(vector.Clone().Rotate(180).Scale(new Vec2(40,40)).Add(midPos)),
                                  Vec2.zero.ReturnPoint(vector.Clone().Rotate(240).Scale(new Vec2(40,40)).Add(midPos)),
                                  Vec2.zero.ReturnPoint(vector.Clone().Rotate(300).Scale(new Vec2(40,40)).Add(midPos))
                              };

            graphics.FillPolygon(Brushes.White, points);
        }

        public void DrawPlayer()
        {
            if (Input.GetKey(Key.D)) { _playerRot += 5; }
            if (Input.GetKey(Key.A)) { _playerRot -= 5; }

            Vec2 vector = new Vec2(Mathf.Cos((MyGame.rot + _playerRot) * Mathf.PI / 180), Mathf.Sin((MyGame.rot + _playerRot) * Mathf.PI / 180));
            PointF[] points = {
                                  Vec2.zero.ReturnPoint(vector.Clone().Scale(new Vec2(60,60)).Add(midPos)),
                                  Vec2.zero.ReturnPoint(vector.Clone().Rotate(-10).Scale(new Vec2(50,50)).Add(midPos)),
                                  Vec2.zero.ReturnPoint(vector.Clone().Rotate(10).Scale(new Vec2(50,50)).Add(midPos)),
                              };

            playerPos = vector.Clone().Scale(new Vec2(60, 60)).Add(midPos);
            if (_playerRot < 0) { _playerRot += 360; }

            graphics.FillPolygon(Brushes.LightBlue, points);
        }

        public void DrawText()
        {
            if (_stop == 2)
            {
                if (textAlpha < 255)
                {
                    textAlpha += 5;
                }

                var brush = new SolidBrush(Color.FromArgb(textAlpha, 255, 255, 255));

                var fonts = new PrivateFontCollection();
                fonts.AddFontFile("Bump.ttf");

                var myFont = new Font((FontFamily)fonts.Families[0], 33);
                graphics.DrawString("Simple", myFont, brush, new PointF(250, 103));

                myFont = new Font((FontFamily)fonts.Families[0], 25);
                graphics.DrawString("Hexagon", myFont, brush, new PointF(270, 150));

                myFont = new Font((FontFamily)fonts.Families[0], 17);
                graphics.DrawString("Press", myFont, brush, new PointF(335, 505));

                myFont = new Font((FontFamily)fonts.Families[0], 13);
                graphics.DrawString("Spacebar", myFont, brush, new PointF(322, 530));
            }
            else
            {
                textAlpha = 0;
            }
        }

        public void DrawTimer()
        {
            int seconds = Mathf.Floor(timer / 60f);
            int mili = timer % 60;
            string time = seconds.ToString() + ":" + mili.ToString();

            var brush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));

            var fonts = new PrivateFontCollection();
            fonts.AddFontFile("Bump.ttf");

            var myFont = new Font((FontFamily)fonts.Families[0], 20);
            graphics.DrawString(time, myFont, brush, new PointF(600, 20));

        }

        public void CheckCollision()
        {
            int currentSide = (_playerRot / 60) % 6;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].side == currentSide && list[i].Dist() < 0)
                {
                    if (currentSide != oldSide)
                    {
                        int sideDif = currentSide - oldSide;
                        if (sideDif == 0) { _playerRot -= 10; }
                        else if (sideDif == 5) { _playerRot += 10; }
                        else if (sideDif == -1) { _playerRot += 10; }
                        else { _playerRot -= 10; }

                    }
                    else
                    {
                        MyGame.Stop();
                    }
                }
            }
            oldSide = currentSide;
        }

        public void DrawVictory()
        {
            var brush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));

            var fonts = new PrivateFontCollection();
            fonts.AddFontFile("Bump.ttf");

            var myFont = new Font((FontFamily)fonts.Families[0], 50);
            graphics.DrawString("You Won!", myFont, brush, new PointF(150, 260));
        }

        public static Vec2 ReturnPlayerPos() {
            return playerPos;
        }
    }
}
