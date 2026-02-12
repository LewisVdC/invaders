using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invaders
{
    internal class laser
    {
        private int speed,
            x,
            y,
            direction,
            color,
            size;
        private Brush brush,
            gbrush;

        private int ox,
            oy;

        public laser(int startX, int startY, int lasersize, int laserspeed, Color lasercolor)
        {
            x = startX;
            y = startY;
            size = lasersize;
            brush = new SolidBrush(lasercolor);
            int alpha = 128; // 0 = fully transparent, 255 = fully opaque
            Color transparentColor = Color.FromArgb(alpha, lasercolor);
            gbrush = new SolidBrush(transparentColor);
            speed = laserspeed;
        }

        public void go(Graphics g, int angle, int ghosting) // declare ghosting here bc my god it does not work
        {
            Console.WriteLine("go!! angle:" + angle + "ghosting:" + ghosting);
            if (angle != 0)
            {
                float cx = Hitbox.X + Hitbox.Width / 2f;
                float cy = Hitbox.Y + Hitbox.Height / 2f;

                g.TranslateTransform(cx, cy);
                g.RotateTransform(angle + 90);
                g.TranslateTransform(-cx, -cy);

                ox = x;
                oy = y;
                float radians = angle * (float)Math.PI / 180f;

                y += (int)(speed * Math.Sin(radians));
                x += (int)(speed * Math.Cos(radians));
            }
            else
            {
                ox = x;
                oy = y;
                y -= speed;
            }

            g.FillRectangle(brush, x - size / 4, y - size / 2, size / 2, size * 2);

            if (ghosting == 1)
            {
                g.FillRectangle(gbrush, ox - size / 4, oy - size / 2, size / 2, size * 2);
            }
            Pen pen = new Pen(Color.Black);

            //Brush huh = new SolidBrush(Color.Black);
            //g.FillRectangle(huh, Hitbox);
            if (angle != 0)
            {
                g.ResetTransform();
            }
            g.DrawRectangle(pen, Hitbox);
        }

        public void tpplayer(int tx, int ty)
        {
            x = tx;
            y = ty;
        }

        public int getY()
        {
            return y;
        }

        public int getX()
        {
            return x;
        }

        public Rectangle Hitbox
        {
            get { return new Rectangle(x - size / 4, y - size / 2, size / 2, size * 2); }
        }

        public void goaway()
        {
            x += 10000;
            y += 10000;
        }
    }
}
