﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  class DefenderMiddleLineman : Defender
  {
    public override void Initialize()
    {
      SpeedCap = 104;
      Intelligence = 8;
      TargetPlayer = Player.ControllablePlayer;
      base.Initialize();
    }

    public override void Move()
    {
      if (Player.ControllablePlayer.Left > Game.LineOfScrimage + 8)
      {
        Intelligence = 11; // Once the runner get past the line of scrimage, this defender doen't have to worry about the blockers.
      }

      if (TargetPlayer != Player.ControllablePlayer)
        TargetPlayer = Player.ControllablePlayer;

      if(Intelligence > Random.Next(0,15) || MovingAroundBlocker > 0)
      {
        base.MoveTowardsTarget(Player.ControllablePlayer.Left + 30, Player.ControllablePlayer.Top);
      }
      base.Move();
    }

    public override void CollisionMove(Player collidedWithPlayer, CollisionOrientation collisionOrientation)
    {
      if (collidedWithPlayer.HasBall && !IsThrowing)
      {
        if (Random.Next(0, 10) > 1) // Allow a missed tackle 10% of time.
          ParentGame.EndPlay("Tackled by Middle Lineman.");
      }

      if (collidedWithPlayer is Offender)
      {
        if (collisionOrientation == CollisionOrientation.ToLeft || collisionOrientation == CollisionOrientation.ToRight)
          ChangeX = ChangeX/2;
        if (collisionOrientation == CollisionOrientation.Above || collisionOrientation == CollisionOrientation.Below)
          ChangeY = ChangeY/2;

        base.MoveAroundPlayer(collisionOrientation);
        return;
      }

      base.CollisionMove(collidedWithPlayer, collisionOrientation);
    }
  }
}
