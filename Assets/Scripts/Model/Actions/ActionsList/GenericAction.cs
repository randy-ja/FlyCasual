﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActionsList
{

    public class GenericAction
    {
        protected GameManagerScript Game;

        public string Name;
        public string EffectName;
        public string ImageUrl;

        public GenericAction() {
            Game = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        }

        public virtual void ActionEffect()
        {

        }

        public virtual bool IsActionEffectAvailable()
        {
            return true;
        }

        public virtual void ActionTake()
        {

        }

    }

}
