using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    //every bubble in our game
    class Bubble
    {
        public Material BubbleMaterial { get; set; } //current color
        public Material OriginalBubbleMaterial { get; private set; } //cache the original color
        public GameObject GameObject;


        public Bubble(GameObject gameObject, Material material)
        {
            GameObject = gameObject;
            OriginalBubbleMaterial = BubbleMaterial = material;
        }
    }
}
