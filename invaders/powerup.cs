using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invaders
{
    internal class powerup
    {
        private int x,
            y,
            type;

        public powerup(int _x, int _y, int _type)
        {
            x = _x;
            y = _y;
            type = _type;
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }

        public int gettype()
        {
            return type;
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

        public void draw(Graphics e)
        {
            if (type == 1)
            {
                // idk invincibility mayb
                FillEllipseCentered(new SolidBrush(Color.LimeGreen), x - 25, y - 25, 50, 50, e);
            }
            if (type == 2)
            {
                // infinit ammo
                FillEllipseCentered(new SolidBrush(Color.Red), x - 25, y - 25, 50, 50, e);
            }
            if (type == 3)
            {
                // shrug aimbot or something
                FillEllipseCentered(new SolidBrush(Color.Purple), x - 25, y - 25, 50, 50, e);
            }
            if (type == 4)
            {
                // yea bro idk
                FillEllipseCentered(new SolidBrush(Color.Turquoise), x - 25, y - 25, 50, 50, e);
            }
        }

        public Rectangle Hitbox
        {
            get { return new Rectangle(x - 25, y - 25, 50, 50); }
        }
    }
}
