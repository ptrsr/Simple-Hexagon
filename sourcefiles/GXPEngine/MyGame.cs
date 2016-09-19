using System;
using GXPEngine;
using System.IO;
using System.Drawing;
using System.Collections.Generic;

public class MyGame : Game
{
    public static int width = 800;
    public static int height = 800;

    public static float rot = 0;
    private static float rotSpeed = 2f;
    private static bool menu = false;

    private static float interval = 30;
    private static float speed = 1.5f;

    public Hexagon hexagon;

	public MyGame () : base(width, height, false)
	{
        hexagon = new Hexagon(width, height);
        AddChild(hexagon);
        hexagon.Menu();
	}

	void Update ()
	{
        if (menu == true)
        {
            hexagon.Play();
            Rotate();
            SpeedUp();

            if (Input.GetKeyDown(Key.SPACE)) { menu = false; }
        }
        else
        {
            speed = 1.5f;
            interval = 30f;
            hexagon.Menu();
            if (Input.GetKeyDown(Key.SPACE)) { menu = true; }
        }

        if (hexagon.timer > 3600)
        {
            hexagon.DrawVictory();
            menu = false;

            if (Input.GetKeyDown(Key.SPACE)) { game.Destroy(); }
        }
	}

	static void Main() {
		new MyGame().Start();
	}

    public static void Rotate()
    {
        rot += rotSpeed;
    }

    static public void Play()
    {
        menu = true;
    }

    static public void Stop()
    {
        menu = false;
    }

    static public float GetInterval()
    {
        return interval;
    }
    static public float GetSpeed()
    {
        return speed;
    }
    static public float GetRotSpeed()
    {
        return rotSpeed;
    }
    static public void SetRotSpeed(float speed)
    {
        if (rotSpeed < 0) { rotSpeed = -speed; }
        else
        {
            rotSpeed = speed;
        }
    }
    static void SpeedUp()
    {
        speed *= 1.0005f;
        interval *= 0.9995f;
    }
}

