﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility {
    public class EnumService {

        public enum InputType
        {
            LeftStickX = 0,
            LeftStickY = 1,
            RightStickX = 2,
            RightStickY = 3,
            LB = 4,
            LT = 5,
            RB = 6,
            RT = 7,
            DUp = 8,
            DDown = 9,
            DLeft = 10,
            DRight = 11,
            X = 12,
            O = 13,
            Triangle = 14,
            Square = 15,
            LeftStickButton = 16,
            RightStickButton = 17,
            Command = 18,
            Size //must be last
        }

        public enum BondProperty
        {
            Trust,
            Irritation,
            Neglect,
            Cooperation,
            Size //must be last
        }

        public enum ItemName
        {
            NONE = -1,
            Bow,
        }

        public enum OffMeshLinkMoveMethod
        {
            Teleport,
            NormalSpeed,
            Parabola
        }

        public enum GameState
        {
            Win_SwitchActivated,
            Win_EnemiesEliminated,
            Lose_AccaliaDead,
            Lose_PlayerDead,
            InGame,
            InTutorial,
            InPauseMenu,
        }

        public enum CPNeighborType
        {
            JumpTo,
            ClimbTo,
            Dispatch
        }
    }
}
