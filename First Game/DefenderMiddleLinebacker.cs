﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  class DefenderMiddleLinebacker : Defender
  {
    public override void Initialize()
    {
      this.SpeedCap = 124;
      this.Intelligence = 13;
      this.TargetPlayer = Game.ControllablePlayer;
      base.Initialize();
    }

    public override void Move()
    {
      if (Game.IsThrowing)
        this.ChangeX += 20;
 
      if (TargetPlayer != Game.ControllablePlayer)
        TargetPlayer = Game.ControllablePlayer;

      if (MovingAroundBlocker > 0)
      {
        MovingAroundBlocker--;
        base.Move(); 
        return;
      }

      Random random = new Random();
      if(this.Intelligence > random.Next(0,15))
      {

        int calculatedTargetY = TargetPlayer.Top; 
        int calculatedTargetX = 0;
        bool closeToTackle = false;

        int diffX = 200;
        if (TargetPlayer.Left > 160 && Math.Abs(TargetPlayer.Top - this.Top) < 100)
          diffX = 20;

        if (Math.Abs(this.CenterX - TargetPlayer.CenterX) < TargetPlayer.PlayerWidth + 60 && Math.Abs(this.CenterY - TargetPlayer.CenterY) < TargetPlayer.PlayerHeight + 60)
          closeToTackle = true;

        // Go right towards target if close to target or are blitzing
        if (closeToTackle || this.DefensiveMode == DefensiveMode.Blitz)
        {
          calculatedTargetX = TargetPlayer.Left + (TargetPlayer.ChangeX / 2);
        }
        else if (this.DefensiveMode == DefensiveMode.Normal)
        {
          calculatedTargetX = TargetPlayer.Left + (TargetPlayer.ChangeX / 2) + (diffX/2);
        }
        else if (this.DefensiveMode == DefensiveMode.Soft)
        {
          calculatedTargetX = TargetPlayer.Left + (TargetPlayer.ChangeX / 2) + diffX;
        }

        base.MoveTowardsTarget(calculatedTargetY, calculatedTargetX);
      }
      base.Move();
    }

    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      if (collidedWithPlayer.HasBall && !Game.IsThrowing)
      {
        ParentGame.EndPlay("Tackled");
      }

      if (collidedWithPlayer is Offender)
      {
        base.MoveAroundPlayer(collisionOrientation);
        return;
      }

      base.CollisionMove(collidedWithPlayer, collisionOrientation);
    }
  }
}
