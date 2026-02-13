using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invaders
{
    internal class enemy
    {
        private int x,
            y,
            size,
            speed;
        private int ox,
            oy;
        private Brush brush,
            gbrush;
        private Pen pen,
            gpen;
        private double dx = 0;
        private double dy = 0;
        private int ghosting = 1;
        private int destinationposition;
        private int screenwidth;
        private int safe = 0;

        private int level = 1;
        private int spawnx,
            spawny;
        private int firstframe = 1;

        public enemy(
            int startX,
            int startY,
            int enemysize,
            int enemyspeed,
            Color playercolor,
            Color bordercolor
        )
        {
            x = startX;
            spawnx = startX;
            y = startY;
            spawny = startY;
            size = enemysize;
            brush = new SolidBrush(playercolor);
            int alpha = 128; // 0 = fully transparent, 255 = fully opaque
            Color transparentColor = Color.FromArgb(alpha, playercolor);
            gbrush = new SolidBrush(transparentColor);
            pen = new Pen(bordercolor);
            transparentColor = Color.FromArgb(alpha, bordercolor);
            gpen = new Pen(transparentColor);
            speed = enemyspeed;
        }

        public void move(int maxbound) // earthbound?
        {
            if (x < 0 || x > maxbound)
            {
                speed = -speed;
            }
            this.x += speed;
            ox = x;
            oy = y;
        }

        public void momentum(int maxbound)
        {
            screenwidth = maxbound; // this might genuinely be the worst way to store this info
            /*
            if (x < 0 || x > maxbound)
            {
                speed = -speed;
                dx = -dx;

            }
            ox = x; oy = y;
            dx += speed;

            //y += (int)dy / 2;
            x += (int)dx / 2;
            dx /= 1.1;
            //dy /= 1.1;
            */

            if (x < 0 || x > maxbound)
            {
                speed = -speed;
                dx = -dx;
            }
            ox = x;
            oy = y;

            if (x > (destinationposition - 5) && x < (destinationposition + 5))
            {
                // honk sho mimi
            }
            else if (x > destinationposition - 5)
            {
                dx -= speed;
            }
            else if (x < destinationposition + 5)
            {
                dx += speed;
            }

            x += (int)dx / 2;
            dx /= 1.1;
        }

        public void draw(Graphics g)
        {
            //g.DrawRectangle(gpen, destinationposition - 5,  100, 10, 50);
            if (firstframe == 0)
            {
                if (ghosting == 1)
                {
                    g.FillRectangle(gbrush, ox - size / 2, oy - size / 2, size, size);
                }

                if (ghosting == 1)
                {
                    g.DrawRectangle(gpen, ox - size / 2, oy - size / 2, size, size);
                }
            }
            firstframe = 0;
            g.FillRectangle(brush, x - size / 2, y - size / 2, size, size);
            g.DrawRectangle(pen, x - size / 2, y - size / 2, size, size);

            //Brush huh = new SolidBrush(Color.Black);
            //g.FillRectangle(huh, Hitbox);
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }

        public int getlevel()
        {
            return level;
        }

        public Rectangle Hitbox
        {
            get { return new Rectangle(x - size / 2, y - size / 2, size, size); }
        }

        /*public void dodgeold(List<int> uhoh, int playery)
        {
            // todo make this mf less greedy
            if (!uhoh.Contains(playery))
            {
                safe = 1;
                if (playery > y)
                {
                    for (int i = y; i >= playery && i <= 10000; i++) // sometimes this just crashes,,? // not anymore
                    {
                        if (uhoh.Contains(i))
                        {
                            safe = 0;
                        }
                    }
                }
                else
                {
                    for (int i = y; i <= playery && i >= 10000; i--)
                    {
                        if (uhoh.Contains(i))
                        {
                            safe = 0;
                        }
                    }
                }
                if (safe == 1)
                {
                    //Console.Write("doesnt contain, go to ");
                    //Console.WriteLine(playery);
                    destinationposition = playery;
                }
            }
            else if (!uhoh.Contains(y))
            {
                //Console.Write("does contain, changing to ");
                //Console.WriteLine(x);
                destinationposition = x;
                while (uhoh.Contains(destinationposition))
                {
                    if (x < screenwidth / 2)
                    {
                        //Console.Write("does contain, changing to ");
                        destinationposition--;
                        //Console.WriteLine(destinationposition);
                    }
                    else
                    {
                        //Console.Write("does contain, changing to ");
                        destinationposition++;
                        //Console.WriteLine(destinationposition);
                    }
                }
            }
        }*/
        // do nit use

        public void dodge(List<int> uhoh, int playerx)
        {
            // if theres no lasers just go for it
            if (uhoh.Count == 0)
            {
                destinationposition = playerx;
                return;
            }

            // check if player position is safe
            if (checkposition(playerx, uhoh))
            {
                destinationposition = playerx;
                return;
            }

            // find gap if not
            int safespot = findsafespot(uhoh);
            destinationposition = safespot;
        }

        private bool checkposition(int xposition, List<int> uhoh)
        {
            int safe = 50; // safety margin around bullets // this might be stupid?

            for (int checkx = -safe; checkx <= xposition + safe; checkx++)
            {
                if (uhoh.Contains(checkx))
                {
                    return false;
                }
            }
            return true;
        }

        private int findsafespot(List<int> uhoh)
        {
            if (uhoh.Count == 0)
                return x;

            uhoh = uhoh.OrderBy(d => d).ToList();

            int lagestgapsize = 0;
            int largestgapcenter = x; // default to not doing anything

            // check first gap
            if (uhoh.First() > 0)
            {
                int gapsize = uhoh.First();
                if (gapsize > lagestgapsize)
                {
                    lagestgapsize = gapsize;
                    largestgapcenter = uhoh.First() / 2;
                }
            }

            // check gaps between danger zones // kinda useless most of the time?
            for (int i = 0; i < uhoh.Count - 1; i++)
            {
                int gapstart = uhoh[i];
                int gapend = uhoh[i + 1];
                int gapsize = gapend - gapstart;

                if (gapsize > lagestgapsize)
                {
                    lagestgapsize = gapsize;
                    largestgapcenter = (gapstart + gapend) / 2;
                }
            }

            // check gap after last danger zone
            if (uhoh.Last() < screenwidth)
            {
                int gapsize = screenwidth - uhoh.Last();
                if (gapsize > lagestgapsize)
                {
                    lagestgapsize = gapsize;
                    largestgapcenter = uhoh.Last() + (gapsize / 2);
                }
            }

            // trap him
            return Math.Max(0, Math.Min(screenwidth, largestgapcenter));
        }

        public int getDestination()
        {
            return destinationposition;
        }

        public void levelup()
        {
            x = spawnx;
            y = spawny;
            level++;
            speed += level / 2;
        }

        public void hide()
        {
            x += 100000;
            y += 100000; // am i fr
        }

        public void unhide()
        {
            x = spawnx;
            y = spawny;
        }
    }
}
