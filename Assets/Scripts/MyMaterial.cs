using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// A helper class to cache our color name along with the current material
    /// </summary>
    class MyMaterial : Material
    {
        
        public string ColorName { get; set; }

        public MyMaterial(Material baseMaterial)
            : base(baseMaterial)
        { }

        public MyMaterial(Material baseMaterial, string colorName)
            : base(baseMaterial)
        {
            ColorName = colorName;
        }

        /// <summary>
        /// helper method to get a random color
        /// </summary>
        /// <returns></returns>
        public static MyMaterial GetRandomMaterial()
        {
            int x = UnityEngine.Random.Range(0, 5);
            if (x == 0)
                return new MyMaterial(Resources.Load("Materials/redMaterial") as Material, "red");
            else if (x == 1)
                return new MyMaterial(Resources.Load("Materials/greenMaterial") as Material, "green");
            else if (x == 2)
                return new MyMaterial(Resources.Load("Materials/blueMaterial") as Material, "blue");
            else if (x == 3)
                return new MyMaterial(Resources.Load("Materials/yellowMaterial") as Material, "yellow");
            else if (x == 4)
                return new MyMaterial(Resources.Load("Materials/purpleMaterial") as Material, "purple");
            else
                return new MyMaterial(Resources.Load("Materials/redMaterial") as Material, "red");
        }
    }
}
