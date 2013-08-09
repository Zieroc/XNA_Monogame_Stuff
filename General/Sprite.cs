using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PoorlyDrawnEngine.Graphics
{
    public class Sprite
    {
        #region Variables

        private Texture2D texture;      //The image or sprite sheet being used

        private int height;             //The height of a single image
        private int width;              //The width of a single image

        private int columns;            //The number of columns in a sprite sheet
        private int rows;               //The number of rows in a sprite sheet

        private int states;              //The number of animations to loop through
        private int currentState;       //The current animation state

        private bool animate;           //Is the sprite animating through states
        private bool looping;           //Is the animation looping back to start when final state is reached

        private float timer;            //Current time
        private float interval;         //Interval between state changes

        #endregion

        #region Constructors

        public Sprite(Texture2D texture)
            : this(texture, texture.Height, texture.Width, 1, false, false, 250f)
        {
        }

        public Sprite(Texture2D texture, int height, int width, int state)
            : this(texture, height, width, state, false, false, 250f)
        {
        }

        public Sprite(Texture2D texture, int height, int width, int state, bool animate, bool looping)
            : this(texture, height, width, state, animate, looping, 250f)
        {
        }

        public Sprite(Texture2D texture, int height, int width, int state, bool animate, bool looping, float interval)
        {
            this.texture = texture;
            this.height = height;
            this.width = width;
            this.states = state;
            this.animate = animate;
            this.looping = looping;

            //Calculate the number of rows and columns
            columns = texture.Width / width;
            rows = texture.Height / height;

            currentState = 0;
            timer = 0;
            this.interval = interval;
        }

        #endregion

        #region Properties

        public Texture2D Texture
        {
            set { texture = value; }
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public int CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        public bool Animate
        {
            set { animate = value; }
        }

        public bool Looping
        {
            set { looping = value; }
        }

        public bool FinishedPlaying
        {
            get
            {
                if (looping == false)
                {
                    if (currentState == states - 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            if (animate && currentState < states)
            {
                //Increase the timer by the number of milliseconds since update was last called
                timer += (float)gameTime.ElapsedGameTime.Milliseconds;

                //Check the timer is more than the chosen interval
                if (timer > interval)
                {
                    //Show the next frame
                    currentState++;
                    //Reset the timer
                    timer = 0f;
                }
            }

            if (currentState == states)
            {
                if (looping)
                {
                    currentState = 0;   //Animation should loop so set currentState back to 0
                }
                else
                {
                    currentState--;     //Animation has reached end so drop back one so the image can be drawn
                }
            }
        }

        #endregion

        #region Draw

        //Draw with no effects at given position and depth
        public void Draw(SpriteBatch spriteBatch, Vector2 position, float depth)
        {
            int imgX = width * (currentState % columns);
            int imgY = height * (currentState / columns);

            Rectangle sourceRect = new Rectangle(imgX, imgY, width, height);

            spriteBatch.Draw(texture, position, sourceRect, Color.White, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, depth);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float depth, Color colour)
        {
            int imgX = width * (currentState % columns);
            int imgY = height * (currentState / columns);

            Rectangle sourceRect = new Rectangle(imgX, imgY, width, height);

            spriteBatch.Draw(texture, position, sourceRect, colour, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, depth);
        }

        //Draw with option to flip sprite, at given position and depth
        public void Draw(SpriteBatch spriteBatch, Vector2 position, bool flip, float depth)
        {
            if (flip)
            {
                int imgX = width * (currentState % columns);
                int imgY = height * (currentState / columns);

                Rectangle sourceRect = new Rectangle(imgX, imgY, width, height);

                spriteBatch.Draw(texture, position, sourceRect, Color.White, 0, new Vector2(0, 0), 1.0f, SpriteEffects.FlipHorizontally, depth);
            }
            else
            {
                Draw(spriteBatch, position, depth);
            }
        }

        //Draw with option to flip sprite, at given position and depth and a given colour - will be used to change sprite transperancy
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color colour, bool flip, float depth)
        {
            int imgX = width * (currentState % columns);
            int imgY = height * (currentState / columns);

            Rectangle sourceRect = new Rectangle(imgX, imgY, width, height);

            if (flip)
            {
                spriteBatch.Draw(texture, position, sourceRect, colour, 0, new Vector2(0, 0), 1.0f, SpriteEffects.FlipHorizontally, depth);
            }
            else
            {
                spriteBatch.Draw(texture, position, sourceRect, colour, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, depth);
            }
        }

        #endregion
    }
}
