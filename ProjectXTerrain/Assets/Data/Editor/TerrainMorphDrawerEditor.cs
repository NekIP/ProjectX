using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Data.Editor
{
    public static class TerrainMorphDrawerEditor
    {
        public static void DrawCircleOnMouse(float radius)
        {
            var ray = Camera.current.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit))
            {
                return;
            }
        }
    }
}
