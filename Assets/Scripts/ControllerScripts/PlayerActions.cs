using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

//Player Action Set where default and custom bindings are defined.
public class PlayerActions : PlayerActionSet
{
    //Player actions
    public PlayerAction Transform;      //Grow/shrink character
    public PlayerAction Interact;       //Pickup/drop an item
    public PlayerAction Join;           //Join the game
    public PlayerAction Left;
    public PlayerAction Right;
    public PlayerAction Up;
    public PlayerAction Down;
    public PlayerTwoAxisAction Move;

    //Developer actions
    public PlayerAction ResetLevel;         //Reset the current level
    public PlayerAction ResetCheckpoint;    //Return players to last checkpoint
    public PlayerAction AdvanceLevel;       //Advance to the next level
    public PlayerAction BackLevel;          //Regress to the previous level

    public PlayerActions()
    {
        //Player Actions
        Transform   = CreatePlayerAction("Transform");
        Interact    = CreatePlayerAction("Interact");
        Join        = CreatePlayerAction("Join");
        Left        = CreatePlayerAction("Left");
        Right       = CreatePlayerAction("Right");
        Up          = CreatePlayerAction("Up");
        Down        = CreatePlayerAction("Down");
        Move        = CreateTwoAxisPlayerAction(Left, Right, Down, Up);

        //Developer Actions
        ResetLevel      = CreatePlayerAction("ResetLevel");
        ResetCheckpoint = CreatePlayerAction("ResetCheckpoint");
        AdvanceLevel    = CreatePlayerAction("AdvanceLevel");
        BackLevel       = CreatePlayerAction("BackLevel");
    }

    //Define Player actions for keyboard. DO NOT CHANGE THESE.
    public static PlayerActions CreateWithKeyboardBindings()
    {
        var actions = new PlayerActions();

        //Player Actions
        actions.Transform.AddDefaultBinding(Key.Z);
        actions.Interact.AddDefaultBinding(Key.X);
        actions.Join.AddDefaultBinding(Key.C);

        actions.Left.AddDefaultBinding(Key.LeftArrow);
        actions.Right.AddDefaultBinding(Key.RightArrow);
        actions.Up.AddDefaultBinding(Key.UpArrow);
        actions.Down.AddDefaultBinding(Key.DownArrow);

        //Developer Actions
        actions.ResetLevel.AddDefaultBinding(Key.I);
        actions.ResetCheckpoint.AddDefaultBinding(Key.K);
        actions.AdvanceLevel.AddDefaultBinding(Key.J);
        actions.BackLevel.AddDefaultBinding(Key.L);

        return actions;
    }

    //Define Player actions for controller. DO NOT CHANGE THESE.
    public static PlayerActions CreateWithControllerBindings()
    {
        var actions = new PlayerActions();

        //Player Actions
        actions.Transform.AddDefaultBinding(InputControlType.Action1);
        actions.Interact.AddDefaultBinding(InputControlType.Action2);
        actions.Join.AddDefaultBinding(InputControlType.Start);

        actions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
        actions.Right.AddDefaultBinding(InputControlType.LeftStickRight);
        actions.Up.AddDefaultBinding(InputControlType.LeftStickUp);
        actions.Down.AddDefaultBinding(InputControlType.LeftStickDown);

        //Developer Actions
        actions.ResetLevel.AddDefaultBinding(InputControlType.DPadUp);
        actions.ResetCheckpoint.AddDefaultBinding(InputControlType.DPadDown);
        actions.AdvanceLevel.AddDefaultBinding(InputControlType.DPadLeft);
        actions.BackLevel.AddDefaultBinding(InputControlType.DPadRight);

        return actions;
    }
}
