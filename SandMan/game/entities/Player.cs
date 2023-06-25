using Box2DSharp.Collision.Collider;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SandMan.blocks;
using SandMan.game.world;
using SandMan.rendering;

namespace SandMan.game.entities;

public class Player : Entity
{
    public Player(BlockWorld blockWorld, Vector2 position) : base(blockWorld, position)
    {
        
    }

    public float idle_timer = 0;
    
    public override void Update()
    {
        base.Update();
        
        float dist = float.MaxValue;
        Vector2 closest = new Vector2(position.X, position.Y);
        bool found = false;
        for (int y = 0; y < 4096; y++)
        {
            int X = (int)position.X;
            int Y = y;
            if (world.GetBlock(X, Y - 1).solid && world.GetBlock(X, Y).solid && !world.GetBlock(X, Y + 1).solid && !world.GetBlock(X, Y + 2).solid)
            {
                Vector2 testPos = new Vector2(X, Y);
                float d = Vector2.Distance(closest, testPos);
                if (d < dist)
                {
                    dist = d;
                    closest = testPos;
                    found = true;
                }
            }
        }

        if (found)
        {
            position.Y = closest.Y + 40;
        }
        
        Game.INSTANCE.camera.position = Vector2.Lerp(Game.INSTANCE.camera.position, draw_position, Game.INSTANCE.Delta * 8.0f);

        bool IsKeyDown(Keys key) => Game.INSTANCE.IsKeyDown(key);

        float delta = Game.INSTANCE.Delta;

        float speed = 16 * 16;

        idle_timer -= delta;
        
        if (IsKeyDown(Keys.A))
        {
            if (motion.X > -4)
            {
                motion.X -= delta * 8;
            }
            idle_timer = 1.0f;
        } else
        if (IsKeyDown(Keys.D))
        {
            if (motion.X < 4)
            {
                motion.X += delta * 8;
            }
            idle_timer = 1.0f;
        }
        else
        {
            motion.X *= 0.95f;
        }

        

        Vector2i mousePos = Game.INSTANCE.camera.GetMousePositionInWorld();
        bool mouse_press = Game.INSTANCE.IsMouseButtonPressed(MouseButton.Button1);
        
        //BlockWorld.SetBlock(mousePos.X, mousePos.Y, BlockRegistry.air);
        if (mouse_press)
        {
            int leftYBoost = 0, rightYBoost = 0;
            if (world.GetBlock(mousePos.X - 15, mousePos.Y + 5).id == 1) leftYBoost = 5;
            if (world.GetBlock(mousePos.X + 15, mousePos.Y + 5).id == 1) rightYBoost = 5;
            world.CreateChunkEntity(mousePos + new Vector2i(0, -7), 16, mousePos + new Vector2(0, 16), new Vector2(-20, 35), true);
            world.CreateChunkEntity(mousePos + new Vector2i(-5, -3), 16, mousePos + new Vector2(0, 16), new Vector2(20, 35), true);
            world.CreateChunkEntity(mousePos + new Vector2i(5, -3), 16, mousePos + new Vector2(0, 16), new Vector2(0, 35), true);
            world.CreateChunkEntity(mousePos + new Vector2i(-15, leftYBoost), 16, mousePos + new Vector2(-15, 16), new Vector2(-25,35), true);
            world.CreateChunkEntity(mousePos + new Vector2i(15, rightYBoost), 16, mousePos + new Vector2(15, 16), new Vector2(25, 35), true);
        }

        position.X += motion.X * delta * 60.0f;
    }

    public Vector2[] connections =
    {
        new(18, 18),
        new(101 - 18, 18),
        new(3, 50),
        new(101 - 3, 50)
    };
    public Vector2[] connections_draw =
    {
        new(18, 18),
        new(101 - 18, 18),
        new(3, 50),
        new(101 - 3, 50)
    };

    public Vector2[] leg_midpoints =
    {
        new(0, 0),
        new(0, 0),
        new(0, 0),
        new(0, 0)
    };
    
    public Vector2[] leg_bottom =
    {
        new(0, 0),
        new(0, 0),
        new(0, 0),
        new(0, 0)
    };

    public Vector2[] leg_attachment =
    {
        new Vector2(0, 0),
        new Vector2(0, 0),
        new Vector2(0, 0),
        new Vector2(0, 0)
    };

    public Vector2[] leg_move_point =
    {
        new(),
        new(),
        new(),
        new()
    };

    public float move_timer = 0;
    public int move_leg = 0;
    public Vector2 last_head_pos;
    public Vector2 draw_position;
    
    public override void Render()
    {
        base.Render();

        Vector2 head_size = new Vector2(101, 101);

        Vector2 head_pos = new Vector2(0, 0);
        
        last_head_pos = Vector2.Lerp(last_head_pos, head_pos, Game.INSTANCE.Delta * 4);
        draw_position = Vector2.Lerp(draw_position, position, Game.INSTANCE.Delta * 4);
        
        for (int i = 0; i < connections.Length; i++)
        {
            Vector2 pos = draw_position - (head_size / 2.0f) + connections[i];

            Vector2 pivot_point = pos + (pos - draw_position) * 0.75f;
            
            
            leg_midpoints[i].X = leg_attachment[i].X;

            float step_height = MathHelper.Clamp(MathF.Abs(leg_attachment[i].X - leg_move_point[i].X) - 40, 0, 40) * 2.0f;
            
            
            
            leg_midpoints[i].Y = MathHelper.Lerp(leg_midpoints[i].Y, leg_attachment[i].Y + 80 + step_height, Game.INSTANCE.Delta * 6);
            float mid_dist = Vector2.Distance(leg_attachment[i], pos);
            connections_draw[i].Y = connections[i].Y + MathHelper.Clamp((160 - mid_dist), -40, 80);
            //connections_draw[i].X = connections[i].X - (160 - mid_dist);
            
            Vector2 draw_pos = draw_position - (head_size / 2.0f) + connections_draw[i];
            head_pos += draw_pos / 4.0f;
            
            leg_bottom[i] = leg_attachment[i];
            
            Vector2 midpoint = leg_midpoints[i];

            midpoint = draw_pos + Vector2.Normalize(midpoint - draw_pos) * 80 + new Vector2(0, -20);


            float rotation = MathHelper.RadiansToDegrees(MathF.Atan2(draw_pos.Y - midpoint.Y, draw_pos.X - midpoint.X)) - 90;
            float rotation_bottom = MathHelper.RadiansToDegrees(MathF.Atan2(midpoint.Y - leg_attachment[i].Y, midpoint.X - leg_attachment[i].X)) - 90;
            
            Game.DrawTexture(TextureRegistry.PLAYER_LEG_CONNECTION, Vector2.Lerp(pos, draw_pos, 0.75f), new Vector2(64, 64), 0, true);
            Game.DrawTexture(TextureRegistry.PLAYER_LEG_UP, draw_pos, new Vector2(30, 191), rotation, true);
            
            Game.DrawTexture(TextureRegistry.PLAYER_LEG_DOWN, midpoint, new Vector2(128, 128), rotation_bottom, true);
            
            
            float motion_x = pivot_point.X + MathF.Sign(pivot_point.X - pos.X) * 40;

            bool flag = false;
            if (i == 0 || i == 2)
            {
                if (leg_attachment[i].X < motion_x - 40 && motion.X > 0 || leg_attachment[i].X > motion_x + 20 && motion.X < 0)
                {
                    flag = true;
                }
            }
            else
            {
                if (leg_attachment[i].X > motion_x + 40 && motion.X < 0 || leg_attachment[i].X < motion_x - 20 && motion.X > 0)
                {
                    flag = true;
                }
            }

            if (leg_attachment[i].X == 0)
            {
                leg_attachment[i].X = motion_x + MathF.Sign(motion.X) * 40;
                leg_attachment[i].Y = pivot_point.Y;
                
                flag = true;
            }
            
            if (flag && move_timer <= 0 || idle_timer <= 0)
            {
                move_timer = 0.1f;
                move_leg++;
                move_leg %= 4;
                if (idle_timer > 0)
                {
                    leg_move_point[i].X = (motion_x + pivot_point.X) / 2.0f + MathF.Sign(motion.X) * 120;
                }
                else
                {
                    leg_move_point[i].X = (motion_x + pivot_point.X) / 2.0f;
                }
                leg_move_point[i].Y = pivot_point.Y;
                
                float dist = float.MaxValue;
                Vector2 closest = new Vector2(leg_move_point[i].X, leg_move_point[i].Y);
                bool found = false;
                for (int y = 0; y < 4096; y++)
                {
                    int X = (int)leg_move_point[i].X;
                    int Y = y;
                    if (world.GetBlock(X, Y - 1).solid && world.GetBlock(X, Y).solid && !world.GetBlock(X, Y + 1).solid && !world.GetBlock(X, Y + 2).solid)
                    {
                        Vector2 testPos = new Vector2(X, Y);
                        float d = Vector2.Distance(closest, testPos);
                        if (d < dist)
                        {
                            dist = d;
                            closest = testPos;
                            found = true;
                        }
                    }
                }

                if (found)
                {
                    leg_move_point[i].Y = closest.Y;
                }
            }

            leg_attachment[i] = Vector2.Lerp(leg_attachment[i], leg_move_point[i], Game.INSTANCE.Delta * 4.0f);
        }

        if (move_timer > 0)
        {
            move_timer -= Game.INSTANCE.Delta;
        }

        var mouse_pos = Game.INSTANCE.camera.GetMousePositionInWorld();
        
        Game.DrawTexture(TextureRegistry.PLAYER_HEAD, head_pos, new Vector2(101, 101), 0, true);

        float cannon_rot = MathHelper.RadiansToDegrees(MathF.Atan2(mouse_pos.Y - head_pos.Y, mouse_pos.X - head_pos.X)) + 90;
        Game.DrawTexture(TextureRegistry.PLAYER_CANNON, head_pos, new Vector2(101, 101), cannon_rot, true);

    }
}