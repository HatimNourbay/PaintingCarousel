using Microsoft.Expression.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace PaintingCarousel
{
    public class PathToCarousel
    {
        PathListBox TransformedCarousel;


        private double _receivedReference;

        public double ReceivedReference
        {
            get { return _receivedReference; }

            set
            {
                if (_receivedReference != value)
                {
                    _receivedReference = value;
                    TransformedCarousel.LayoutPaths[0].Start = value;
                }

            }
        }


        public PathToCarousel(PathListBox InitPath)
        {
            TransformedCarousel = InitPath;
        }


        public void Next()
        {

        }

        public void Previous()
        {

        }



        public void AnimateScene(int i)
        {
            OriginAnimation = new Point3DAnimation();
            OriginAnimation.From = this.OriginRect;
            OriginAnimation.To = new Point3D(DictionaryValuesRectangle.XFixedFromIndex[this.IndexOfRectangle + i], 0,
                DictionaryValuesRectangle.ZFixedFromIndex[this.IndexOfRectangle + i]);
            OriginAnimation.Duration = new Duration(TimeSpan.FromSeconds(DurationOfAnimation));
            OriginAnimation.EasingFunction = easingFunction;

            NormalAnimation = new Vector3DAnimation();
            NormalAnimation.From = this.NormalRect;
            NormalAnimation.To = new Vector3D(DictionaryValuesRectangle.XNormalFixedFromIndex[this.IndexOfRectangle + i], 0, 1);
            NormalAnimation.Duration = new Duration(TimeSpan.FromSeconds(DurationOfAnimation));
            NormalAnimation.EasingFunction = easingFunction;

            //Checking if the animation is completed for the property release
            //needs to be placed BEFORE the beginning of animation, else never reached
            OriginAnimation.Completed += OriginAnimation_Completed;
            NormalAnimation.Completed += NormalAnimation_Completed;

            this.BeginAnimation(RectangleForCarousel3D.OriginProperty, OriginAnimation);
            this.BeginAnimation(RectangleForCarousel3D.NormalProperty, NormalAnimation);

            this.IndexOfRectangle = this.IndexOfRectangle + i;

            //Apply the new values to the rectangle 
            this.OriginRect.X = DictionaryValuesRectangle.XFixedFromIndex[this.IndexOfRectangle];
            this.OriginRect.Y = 0;
            this.OriginRect.Z = DictionaryValuesRectangle.ZFixedFromIndex[this.IndexOfRectangle];

            this.Origin = this.OriginRect;

            this.NormalRect.X = DictionaryValuesRectangle.XNormalFixedFromIndex[this.IndexOfRectangle];
            this.NormalRect.Y = 0;
            this.NormalRect.Z = 1;
            this.XPosition = DictionaryValuesRectangle.XFixedFromIndex[this.IndexOfRectangle];
            this.ZPosition = DictionaryValuesRectangle.ZFixedFromIndex[this.IndexOfRectangle];
            this.RotationX = DictionaryValuesRectangle.XNormalFixedFromIndex[this.IndexOfRectangle];

        }

        /// <summary>
        /// The Normal Animation needs to be released, as the Property binded to it are locked
        /// To ensure the next frames will move the rectangle after the animation
        /// We create a nul animation
        /// </summary>
        void NormalAnimation_Completed(object sender, EventArgs e)
        {
            TransformedCarousel.BeginAnimation(PathListBox.LayoutPathsProperty, null);
        }


        /// <summary>
        /// The Origin Animation needs to be released, as the Property binded to it are locked
        /// To ensure the next frames will move the rectangle after the animation
        /// We create a null animation
        /// </summary>
        void OriginAnimation_Completed(object sender, EventArgs e)
        {
            this.BeginAnimation(OriginProperty, null);
        }
    }
}
