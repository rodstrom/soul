using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    public enum LightType
    {
        Point
    }

    public abstract class Light
    {
        protected float _initialPower;
        public bool dead = false;
        
        public Vector3 Position { get; set; }
        public Vector4 Color;

        [ContentSerializerIgnore]
        public float ActualPower { get; set; }

        /// <summary>
        /// The Power is the Initial Power of the Light
        /// </summary>
        public float Power
        {
            get { return _initialPower; }
            set
            {
                _initialPower = value;
                ActualPower = _initialPower;
            }
        }

        public int LightDecay { get; set; }

        [ContentSerializerIgnore]
        public LightType LightType { get; private set; }

        [ContentSerializer(Optional = true)]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets the spot direction. The axis of the cone.
        /// </summary>
        /// <value>The spot direction.</value>
        [ContentSerializer(Optional = true)]
        public Vector3 Direction { get; set; }

        protected Light(LightType lightType)
        {
            LightType = lightType;
        }

        public void EnableLight(bool enabled, float timeToEnable)
        {
            // If the light must be turned on
            IsEnabled = enabled;
        }

        /// <summary>
        /// Updates the Light.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public virtual void Update(GameTime gameTime)
        {
            if (!IsEnabled) return;
        }

        /// <summary>
        /// Copy all the base fields.
        /// </summary>
        /// <param name="light">The light.</param>
        protected void CopyBaseFields(Light light)
        {
            light.Color = this.Color;
            light.IsEnabled = this.IsEnabled;
            light.LightDecay = this.LightDecay;
            light.LightType = this.LightType;
            light.Position = this.Position;
            light.Power = this.Power;
        }

        public abstract Light DeepCopy();
    }
}