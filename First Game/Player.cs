﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace FootballGame
{
  public class Player 
  {
    public static Form1 ParentForm;
    public static List<Player> players = new List<Player>();

    private int changeX;
    private int changeY;
    private int playerWidth;
    private int playerHeight;
    private int cap = 2;
    private int team;
    private int centerX;
    private int centerY;
    private bool hasBall = false;
    private int movingAroundBlocker = 0;
    private int top;
    private int left;
    private PictureBox pictureBox;

    public int Cap { get => cap; set => cap = value; }
    public int Team { get => team; set => team = value; }
    public int PlayerWidth { get => playerWidth; set => playerWidth = value; }
    public int PlayerHeight { get => playerHeight; set => playerHeight = value; }
    public int ChangeX { get => changeX; set => changeX = value; }
    public int ChangeY { get => changeY; set => changeY = value; }
    public int CenterX { get => centerX; set => centerX = value; }
    public int CenterY { get => centerY; set => centerY = value; }
    public bool HasBall { get => hasBall; set => hasBall = value; }
    public int MovingAroundBlocker { get => movingAroundBlocker; set => movingAroundBlocker = value; }

    public int Top
    {
      get => top;
      set
      {
        top = value;
        CenterY = top + PlayerHeight;
      }
    }
    public int Left
    {
      get => left;
      set
      {
        left = value;
        CenterX = left + PlayerWidth;
      }
    }

    public PictureBox PicBox
    {
      get { return pictureBox; }
      set
      {
        pictureBox = value;
        PlayerWidth = pictureBox.Width;
        PlayerHeight = pictureBox.Height;
      }
    }

    public Player()
    {

    }

    public virtual void Move()
    {
      if (Math.Abs(ChangeY) > Cap)
      {
        ChangeY = Cap * Math.Sign(ChangeY);
      }
      if (Math.Abs(ChangeX) > Cap)
      {
        ChangeX = Cap * Math.Sign(ChangeX);
      }
      CheckFormBoundries();
      MovePic(this);
      Application.DoEvents();
    }

    public virtual void MoveTowardsTarget(int Y, int X)
    {
    }


    public virtual void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      switch (collisionOrientation)
      {
        case CollisionOrientation.Above:
          this.Top -= 2;
          this.ChangeY = -8;
          break;
        case CollisionOrientation.Below:
          this.Top += 2;
          this.ChangeY = 8;
          break;
        case CollisionOrientation.ToLeft:
          this.Left -= 2;
          this.ChangeX = -8;
          break;
        case CollisionOrientation.ToRight:
          this.Left += 2;
          this.ChangeX = 8;
          break;
      }
    }

    public virtual void MoveAroundPlayer(CollisionOrientation collisionOrientation)
    {

    }

    private void CheckFormBoundries()
    {
      if (this.Left < 0)
      {
        this.ChangeX = 0;
        this.Left = 1;
      }
      if (this.Left > ParentForm.Width - this.PicBox.Width - 10)
      {
        this.ChangeX = 0;
        this.Left = ParentForm.Width - this.PicBox.Width - 10 - 1;
      }
      if (this.Top < 0)
      {
        this.ChangeY = 0;
        this.Top = 1;
      }
      if (this.Top > ParentForm.Height - this.PicBox.Height - 35)
      {
        this.ChangeY = 0;
        this.Top = ParentForm.Height - this.PicBox.Height - 35 - 1;
      }
    }

    public static void MovePic(Player player)
    {
      player.Top = player.Top + player.ChangeY/32;
      player.Left = player.Left + player.ChangeX/32;
      player.PicBox.Top = player.Top;
      player.PicBox.Left = player.Left;
    }
  }
}