using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invaders
{
    internal class player
    {
        private int x,
            y,
            size,
            width;
        private int ox,
            oy;
        private Brush brush,
            gbrush;
        private Pen pen,
            gpen;
        private double dx = 0;
        private double dy = 0;
        private int ghosting = 1;
        private int invincibilitytimer = 300;
        private int infiniteammotimer = 499;
        private int aimbottimer = 500;
        private int firstframe = 1;
        private int explosivetimer = 500;

        public player(
            int startX,
            int startY,
            int playerSize,
            int bound,
            Color playercolor,
            Color bordercolor
        )
        {
            x = startX;
            y = startY;
            size = playerSize;
            brush = new SolidBrush(playercolor);
            width = bound;
            int alpha = 128; // 0 = fully transparent, 255 = fully opaque
            Color transparentColor = Color.FromArgb(alpha, playercolor);
            gbrush = new SolidBrush(transparentColor);
            pen = new Pen(bordercolor);
            transparentColor = Color.FromArgb(alpha, bordercolor);
            gpen = new Pen(transparentColor);
        }

        public void move(int xmove, int ymove)
        {
            x += xmove;
            y += ymove;
        }

        public void momentum(int px, int py)
        {
            invincibilitytimer++;
            aimbottimer++;
            explosivetimer++;

            if (x < 0 + size / 2)
            {
                dx = -dx;
                dx += 1.5 * dx;
                if (dx < 25)
                {
                    dx = -25;
                }
            }

            if (x > width - size / 2)
            {
                dx = -dx;
                dx -= 1.5 * -dx;
                if (dx > 25)
                {
                    dx = 25;
                }
            }

            ox = x;
            oy = y;
            dx += px;
            dy += py;

            y += (int)dy / 2;
            x += (int)dx / 2;
            dx /= 1.1;
            dy /= 1.1;
        }

        public void draw(Graphics g, int delay, int angle)
        {
            /*using (Brush delaybrush = new SolidBrush(Color.FromArgb(255 - delay, 255 - delay, 255 - delay)))
            {
                g.FillRectangle(delaybrush, width - 100 , 1000 - delay*4, 80, delay*4);
            }
            */

            Brush one = new SolidBrush(Color.Red);
            Brush two = new SolidBrush(Color.Yellow);
            Brush three = new SolidBrush(Color.Green);

            if (firstframe == 0)
            {
                // Stage 1: Red bar (0-33 ticks)
                int redHeight = Math.Min(delay, 33) * 4;
                g.FillRectangle(one, width - 100, 800 - redHeight, 80, redHeight);

                // Stage 2: Yellow bar (33-66 ticks)
                if (delay > 33)
                {
                    int yellowHeight = Math.Min(delay - 33, 33) * 4;
                    g.FillRectangle(two, width - 100, 800 - 132 - yellowHeight, 80, yellowHeight);
                }

                // Stage 3: Green bar (66+ ticks)
                if (delay > 66)
                {
                    int greenHeight = (delay - 66) * 4;
                    g.FillRectangle(three, width - 100, 800 - 264 - greenHeight, 80, greenHeight);
                }
                if (infiniteammotimer < 500)
                {
                    Brush infammo = new SolidBrush(Color.Gold);
                    g.FillRectangle(infammo, width - 100, 200, 80, 600);
                    infiniteammotimer++;
                }

                float cx = otherhitbox.X + otherhitbox.Width / 2f;
                float cy = otherhitbox.Y + otherhitbox.Height / 2f;

                g.TranslateTransform(cx, cy);
                g.RotateTransform(angle);
                g.TranslateTransform(-cx, -cy);

                if (ghosting == 1)
                {
                    g.FillRectangle(gbrush, otherhitbox);
                }

                if (ghosting == 1)
                {
                    g.DrawRectangle(gpen, otherhitbox);
                }
            }
            firstframe = 0;
            g.FillRectangle(brush, otherhitbox);
            g.DrawRectangle(pen, otherhitbox);

            g.ResetTransform();

            //g.FillRectangle(brush, Hitbox);

            Pen shield = new Pen(Color.FromArgb(100, 0, 0, 255), 10);
            if (invincibilitytimer < 500)
            {
                DrawEllipseCentered(
                    shield,
                    x,
                    y,
                    size + 150 - invincibilitytimer / 5,
                    size + 150 - invincibilitytimer / 5,
                    g
                );
            }
        }

        private void DrawEllipseCentered(
            Pen pen,
            int centerx,
            int centery,
            int width,
            int height,
            Graphics g
        )
        {
            g.DrawEllipse(pen, centerx - width / 2, centery - height / 2, width, height);
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }

        public int getinfammotimer()
        {
            return infiniteammotimer;
        }

        public int getaimbottimer()
        {
            return aimbottimer;
        }

        public int getinvincibilitytimer()
        {
            return invincibilitytimer;
        }

        public int getexplosivetimer()
        {
            return explosivetimer;
        }

        public Rectangle Hitbox
        {
            get
            {
                if (invincibilitytimer > 500)
                {
                    return new Rectangle(x - size / 2, y - size / 2, size, size);
                }
                else
                {
                    return new Rectangle(0, 0, 0, 0);
                }
            }
        }

        public Rectangle otherhitbox
        {
            get
            {

                return new Rectangle(x - size / 2, y - size / 2, size, size);
            }
        }

        public void invincibility()
        {
            invincibilitytimer = 0;
        }

        public void infiniteammo()
        {
            infiniteammotimer = 0;
        }

        public void aimbot()
        {
            aimbottimer = 0;
        }

        public void explode()
        {
            explosivetimer = 0;
        }
    }
}
