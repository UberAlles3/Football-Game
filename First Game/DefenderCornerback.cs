﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  class DefenderCornerbackTop : DefenderCornerback { }
  class DefenderCornerbackBottom : DefenderCornerback { }
  class DefenderCornerback : Defender
  {
    private bool InCoverage = true;

    public override void Initialize()
    {
      SpeedCap = 130;
      Intelligence = 10;
      InCoverage = true;
      base.Initialize();
    }

    public override void Move()
    {
      if (Game.ControllablePlayer.Left > Game.LineOfScrimage && TargetPlayer != Game.ControllablePlayer)
      {
        TargetPlayer = Game.ControllablePlayer;
        ChangeX += 20;
        base.MoveTowardsTarget(TargetPlayer.Top, TargetPlayer.Left + 160 + (TargetPlayer.ChangeX / 2));
        InCoverage = false;
      }

      ChangeX += 4;
      if (this.Intelligence > Player.Random.Next(0,15) || MovingAroundBlocker > 0)
      {
        if (InCoverage)
        {
          if (TargetPlayer.Top < Game.FieldCenterY)
            base.MoveTowardsTarget(TargetPlayer.Top + 40, TargetPlayer.Left + Player.Random.Next(-100, 100) + (TargetPlayer.ChangeX / 3));
          else
            base.MoveTowardsTarget(TargetPlayer.Top - 40, TargetPlayer.Left + Player.Random.Next(-100, 100) + (TargetPlayer.ChangeX / 3));
        }
        else
        {
          base.MoveTowardsTarget(TargetPlayer.Top, TargetPlayer.Left + 20 + (TargetPlayer.ChangeX / 2));
        }
      }
      base.Move();
    }

    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      if (collidedWithPlayer is BallAsPlayer)
      {
        if (((BallAsPlayer)collidedWithPlayer).BallIsCatchable == false)
          return;
        
        Game.IsThrowing = false;
        if (Game.Random.Next(0, 10) > 4)
          ParentGame.EndPlay("Cornerback dropped");
        else
        {
          HasBall = true;
          PicBox.BackColor = System.Drawing.Color.Yellow;
          ParentGame.EndPlay("Intercepted");
        }
      }

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
