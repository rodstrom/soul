using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soul
{
    public class PointLight : Light
    {
        public PointLight()
            : base (LightType.Point)
        {
            
        }

        public override Light DeepCopy()
        {
            var newLight = new PointLight();
            CopyBaseFields(newLight);

            return newLight;
        }
    }
}
