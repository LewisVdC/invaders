using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace invaders
{
    public partial class Form1 : Form
    {
        // lowk to do add a secondary tick for slower movement

        // over the top cfg
        // ghosting in player.cs file
        int smooth = 1;
        int updown = 0;

        //end

        player me;
        enemy evil;
        laser shotconstruct;
        List<laser> shots = new List<laser>();
        List<bomb> bombs = new List<bomb>();
        List<int> danger = new List<int>();
        int left,
            right,
            down,
            up;
        int bullet = 0;
        int enemyalive = 1;
        Random bombchance;
        int bombtimer;
        bomb bombconstruct;
        int bombchancevalue;
        int explodepower = 10;
        int mealive = 1;
        int dont = 0;
        int spawn = 0;
        int delay = 0;
        int deathtimer = 0;
        int paused = 1;
        int poweruptimer = 0;
        Random powerupchance;
        int powerupchancevalue = 0;
        List<powerup> powerups = new List<powerup>();
        Random rndscreen;
        int rndscreenvalue;
        Random poweruptype;
        int poweruptypevalue;

        // set this to 0
        int powerupdebug = 0;

        public Form1()
        {
            InitializeComponent();
            // green is invincibility, red inf ammo, purple aimbot, turquoise idk yet
            // \r \n is linebreak
            gamestart.Text =
                "Press play to begin \r \n Powerup guide:"
                + "\r Green: Get a shield for 5 seconds "
                + "\r Red: Get infinite ammo for 5 seconds"
                + "\r Purple: Lasers are homing for 5 seconds"
                + "\r Light blue: idk twin";
            debuglabel.Text = "";

            me = new player(
                pictureBox1.Width / 2,
                pictureBox1.Height - 100,
                50,
                pictureBox1.Width,
                Color.Aqua,
                Color.Black
            );
            evil = new enemy(pictureBox1.Width / 2, 50, 50, 2, Color.Crimson, Color.Black);
            shotconstruct = new laser(me.getX(), me.getY(), 25, 25, Color.Goldenrod);
            bombconstruct = new bomb(
                evil.getX(),
                evil.getY(),
                25,
                25,
                explodepower,
                pictureBox1.Height,
                Color.Gray,
                Color.Black
            );
            gametimer.Stop();
            bombchance = new Random(42);
            bombchancevalue = bombchance.Next(100, 300);
            powerupchance = new Random(4242);
            powerupchancevalue = powerupchance.Next(500, 1000);
            rndscreen = new Random(424242);
            rndscreenvalue = rndscreen.Next(0, pictureBox1.Width);
            poweruptype = new Random(42424242);
            poweruptypevalue = poweruptype.Next(1, 4);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (powerupdebug == 1 && mealive == 1)
            {
                debuglabel.Text =
                    "aimbottimer: "
                    + me.getaimbottimer()
                    + "\ninfammotimer: "
                    + me.getinfammotimer()
                    + "\ninvincibilitytimer: "
                    + me.getinvincibilitytimer();
            }
            else
            {
                debuglabel.Text = "";
            }
            {
                powerupchancevalue = powerupchance.Next(500, 1000);
                rndscreenvalue = rndscreen.Next(0, pictureBox1.Width);
                poweruptypevalue = poweruptype.Next(1, 5);
                if (delay < 99)
                {
                    delay++;
                }
                else
                {
                    delay = 99;
                }
                poweruptimer++;
                if (poweruptimer >= powerupchancevalue / 1) // many powerups
                {
                    if (mealive == 1)
                    {
                        powerups.Add(new powerup(rndscreenvalue, me.getY() + 25, poweruptypevalue));
                    }
                    poweruptimer = 0;
                }
                bombtimer++;
                if (mealive == 0 || enemyalive == 0)
                {
                    deathtimer++;
                }

                pictureBox1.Invalidate();

                foreach (var shot in shots.ToList())
                {
                    if (enemyalive == 1)
                    {
                        if (shot.Hitbox.IntersectsWith(evil.Hitbox) == true)
                        {
                            enemyalive = 0;

                            //evil = null;
                            /*
                            (left, right, up, down) = (0,0,0,0);
                            MessageBox.Show("your winner");
                            */
                            evil.hide();
                            shot.goaway();
                        }
                    }
                }

                if (enemyalive == 0 && deathtimer > 150)
                {
                    shots.Clear();
                    bombtimer = 0;
                    evil.levelup();
                    enemyalive = 1;
                    deathtimer = 0;
                }
                if (mealive == 1)
                {
                    if (me.getinfammotimer() < 500)
                    {
                        delay = 99;
                    }
                }
                foreach (var bomb in bombs.ToList())
                {
                    dont = 1;
                    if (mealive == 1)
                    {
                        if (bomb.Hitbox.IntersectsWith(me.Hitbox) == true)
                        {
                            mealive = 0;
                            me = null;
                            //MessageBox.Show("your loser");
                            foreach (var bomb2 in bombs.ToList())
                            {
                                // this is bad
                                //bomb2.goaway();
                                // not anymore!
                            }
                        }
                    }
                }

                foreach (var powerup in powerups.ToList())
                {
                    if (mealive == 1)
                    {
                        if (powerup.Hitbox.IntersectsWith(me.otherhitbox) == true)
                        {
                            if (powerup.gettype() == 1)
                            {
                                // idk invincibility mayb
                                me.invincibility();
                            }
                            else if (powerup.gettype() == 2)
                            {
                                // infinit ammo
                                me.infiniteammo();
                            }
                            else if (powerup.gettype() == 3)
                            {
                                // shrug aimbot or something
                                me.aimbot();
                            }
                            else if (powerup.gettype() == 4)
                            {
                                // yea bro idk
                            }

                            powerups.Remove(powerup);
                        }
                    }
                }

                if (mealive == 0 && deathtimer > 150)
                {
                    bombs.Clear();
                    bombtimer = 0;
                    me = new player(
                        pictureBox1.Width / 2,
                        pictureBox1.Height - 100,
                        50,
                        pictureBox1.Width,
                        Color.Aqua,
                        Color.Black
                    );
                    mealive = 1;
                    deathtimer = 0;
                }
                dont = 0;
                if (enemyalive == 1)
                {
                    /*if (shot.Hitbox.IntersectsWith(evil.Hitbox) == true)
                    {
                        enemyalive = 0;
                        evil = null;
                    }*/
                }
                if (mealive == 1)
                {
                    if (smooth == 1)
                    {
                        if (left == 1)
                        {
                            me.momentum(-5, 0);
                        }
                        else if (right == 1)
                        {
                            me.momentum(5, 0);
                        }

                        if (up == 1 && updown == 1)
                        {
                            me.momentum(0, -5);
                        }
                        else if (down == 1 && updown == 1)
                        {
                            me.momentum(0, 5);
                        }
                        else
                        {
                            if (left == 1)
                            {
                                me.move(-10, 0);
                            }
                            else if (right == 1)
                            {
                                me.move(10, 0);
                            }
                            if (up == 1 && updown == 1)
                            {
                                me.move(0, -10);
                            }
                            else if (down == 1 && updown == 1)
                            {
                                me.move(0, 10);
                            }
                        }
                    }
                }

                if (enemyalive == 1)
                {
                    evil.momentum(pictureBox1.Width);

                    if (mealive == 1)
                    {
                        evil.dodge(danger, me.getX());
                    }
                }
                if (mealive == 1)
                {
                    me.momentum(0, 0);
                }

                if (bombtimer >= (bombchancevalue - (evil.getlevel() * 15)) && enemyalive == 1)
                {
                    spawn = 1;
                    bombchancevalue = bombchance.Next(100, 300);
                }
                if (spawn == 1 && dont == 0 && enemyalive == 1 && mealive == 1)
                {
                    bombs.Add(
                        new bomb(
                            evil.getX(),
                            evil.getY(),
                            25,
                            25,
                            explodepower + evil.getlevel(),
                            pictureBox1.Height,
                            Color.Gray,
                            Color.Black
                        )
                    );
                    bombtimer = 0;
                    spawn = 0;
                }
            }
        }

        private void pause_Click(object sender, EventArgs e)
        {
            gamestart.Text = "";
            if (paused == 0)
            {
                gametimer.Stop();
                paused = 1;
                pause.Text = "resume";
                gamestart.Text = "Game is paused";
            }
            else
            {
                gametimer.Start();
                paused = 0;
                pause.Text = "pause";
            }

            this.ActiveControl = null;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            /*if (bullet == 1)
            {
                shot.go(e.Graphics);
            }*/

            foreach (var shot in shots)
            {
                if (shot.getY() > 0)
                {
                    danger.Clear();
                    if (mealive == 1)
                    {
                        if (me.getaimbottimer() < 500 && enemyalive == 1)
                        {
                            // (int)(Math.Atan2(evil.getY() - me.getY(), evil.getX() - me.getX()) * 180.0 / Math.PI) // look towards something
                            shot.go(
                                e.Graphics,
                                (int)(
                                    Math.Atan2(evil.getY() - shot.getY(), evil.getX() - shot.getX())
                                    * 180.0
                                    / Math.PI
                                ),
                                0
                            // look towards something // uum
                            );
                        }
                        else
                        {
                            shot.go(e.Graphics, 0, 1);
                        }
                    }

                    for (int i = -50; i < 50; i++)
                    {
                        danger.Add(shot.getX() + i);
                    }
                }
            }
            foreach (var bomb in bombs)
            {
                bomb.go(e.Graphics);
            }
            foreach (var powerup in powerups)
            {
                powerup.draw(e.Graphics);
            }

            if (mealive == 1) // (int)(Math.Atan2(evil.getY() - me.getY(), evil.getX() - me.getX()) * 180.0 / Math.PI) // look towards something
            {
                if (me.getaimbottimer() < 500 && enemyalive == 1)
                {
                    me.draw(
                        e.Graphics,
                        delay,
                        (int)(
                            Math.Atan2(evil.getY() - me.getY(), evil.getX() - me.getX())
                            * 180.0
                            / Math.PI
                        )
                    );
                }
                else
                {
                    me.draw(e.Graphics, delay, 0);
                }
            }
            if (enemyalive == 1)
            {
                evil.draw(e.Graphics);
            }
            /*if (shot.getY() < 0)
            {
                bullet = 0;
                shot = null;
                shot = new laser(me.getX(), me.getY(), 25, 25, Color.Goldenrod);
            }*/
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Q:
                    left = 1;
                    break;
                case Keys.D:
                    right = 1;
                    break;
                case Keys.Z:
                    up = 1;
                    break;
                case Keys.S:
                    down = 1;
                    break;
                case Keys.Space:
                    /*if (bullet == 0) {
                    shot.tpplayer(me.getX(), me.getY());
                    }
                    bullet = 1;*/
                    if (mealive == 1 && delay >= 33)
                    {
                        delay -= 33;
                        shots.Add(new laser(me.getX(), me.getY() - 25, 25, 25, Color.Goldenrod));
                    }
                    //shots.Last().tpplayer(me.getX(), me.getY());
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Q:
                    left = 0;
                    break;
                case Keys.D:
                    right = 0;
                    break;
                case Keys.Z:
                    up = 0;
                    break;
                case Keys.S:
                    down = 0;
                    break;
            }
        }
    }
}
