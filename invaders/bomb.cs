using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace invaders
{
    internal class bomb
    {
        private int x,
            y,
            size,
            explosivepower,
            speed,
            height;
        private int exploded = 0;
        private int explodedtimer = 0;
        private Brush brush;
        private Pen pen;

        public bomb(
            int startX,
            int startY,
            int bombsSize,
            int bombspeed,
            int power,
            int screenheight,
            Color bombcolor,
            Color bordercolor
        )
        {
            x = startX;
            y = startY;
            size = bombsSize;
            speed = bombspeed;
            height = screenheight;
            explosivepower = power;
            brush = new SolidBrush(bombcolor);
            pen = new Pen(bordercolor);
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

        private void FillEllipseCentered(
            Brush brush,
            int centerx,
            int centery,
            int width,
            int height,
            Graphics g
        )
        {
            g.FillEllipse(brush, centerx - width / 2, centery - height / 2, width, height);
        }

        public void go(Graphics g)
        {
            if (y >= height - 100)
            {
                speed = 0;
                y = height - 100;
                explode(g);
                exploded = 1;
                explodedtimer++;
            }
            if (exploded == 0)
            {
                y += speed;
                FillEllipseCentered(brush, x - size / 2, y - size / 2, size, size, g);
                DrawEllipseCentered(pen, x - size / 2, y - size / 2, size, size, g);
            }

            //g.FillRectangle(brush, Hitbox);
        }

        public void explode(Graphics g)
        {
            if (explodedtimer < 100)
            {
                Brush explodebrushouter = new SolidBrush(Color.Red);
                Brush explodebrushmid = new SolidBrush(Color.OrangeRed);
                Brush explodebrushinner = new SolidBrush(Color.Yellow);
                FillEllipseCentered(
                    explodebrushouter,
                    x - size / 2,
                    y - size / 2,
                    size * explosivepower,
                    size * explosivepower,
                    g
                );
                FillEllipseCentered(
                    explodebrushmid,
                    x - size / 2,
                    y - size / 2,
                    (int)(size * explosivepower / 1.5),
                    (int)(size * explosivepower / 1.5),
                    g
                );
                FillEllipseCentered(
                    explodebrushinner,
                    x - size / 2,
                    y - size / 2,
                    size * explosivepower / 3,
                    size * explosivepower / 3,
                    g
                );
            }
        }

        public Rectangle Hitbox
        {
            get
            {
                if (explodedtimer < 100)
                {
                    return new Rectangle(
                        x - (size * explosivepower / 2) - (size / 2),
                        y - (size),
                        size * explosivepower,
                        size
                    );
                }
                else
                {
                    return new Rectangle(0, 0, 0, 0);
                }
            }
        }

        public void goaway()
        { // this sucks what if you ahve a 16k monitor
            x += 10000;
            y += 10000;
        }
    }
}
