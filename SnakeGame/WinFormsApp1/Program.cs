using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Windows.Input;
using OpenTK.Mathematics;
using SnakeGame;
using StbImageSharp;
using System.Transactions;
using SnakeGame.Drawing;
using WinFormsApp1;
using WinFormsApp1.Drawing;
using System.Globalization;

namespace SnakeGame
{
    
public class Game : GameWindow
    {
        float tmp = (float)(Math.PI/2);
        VAO vao;
        IBO ibo;
        Shader shader;
        Texture texture;
        Tower tower;
        Head head;
        int gifNumber = 0;
        OctagonWalls walls;
        Background background;
        Food food;
        FoodWalls foodWalls;
        float move = 0.2f;
        float totalmove = 0f;
        int HasToRotate;
        bool HasToGrow = false;
        bool HasGrown = true;
        GameLogic game_logic = new GameLogic();

        int width, height;
        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title ,Vsync = VSyncMode.On}) {
            this.width = width;
            this.height = height;

            this.CenterWindow(new Vector2i(width, height));
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
            {
                Close();
            }
        }

        /*
        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.3f, 0.3f, 1f, 1f);
            vao = new VAO();
            VBO vbo = new VBO(vertices);
            vao.LinkToVAO(0, 3, vbo);
            VBO uvVBO = new VBO(KnightCoords);
            vao.LinkToVAO(1, 2, uvVBO);

            ibo = new IBO(indices);

            shader = new Shader("../../../../shader.vert", "../../../../shader.frag");

            texture = new Texture("../../../Textures/knight.png");

            GL.Enable(EnableCap.DepthTest);
        }
         protected override void OnUnload()
        {
            base.OnUnload();
            vao.Delete();
            ibo.Delete();
            texture.Delete();
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            yRot += 0.01f;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            shader.Use();
            vao.Bind();
            texture.Bind();
            ibo.Bind();


            Matrix4 view = Matrix4.Identity;
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(90.0f), width/height, 0.1f, 100.0f);

            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);

            for (float i = 0f;i < 19f; i += 1f)
            {
                Matrix4 model = Matrix4.Identity;
                Matrix4 translation = Matrix4.CreateTranslation(-9f+i, -9f + i, -10f);
                model *= translation;
                shader.SetMatrix4("model", model);
                GL.DrawElements(PrimitiveType.Triangles, indices.Count, DrawElementsType.UnsignedInt, 0);
            }
            SwapBuffers();

            base.OnRenderFrame(e);
        }
        */
        protected override void OnLoad() { 
            base.OnLoad(); 
            tower = new Tower();
            tower.Load();
            background = new Background();
            background.Load();
            food = new Food();
            food.Load();
            head = new Head();  
            head.Load();
            walls = new OctagonWalls();
            walls.Load();
            game_logic.SetGame();
            game_logic.SpawnFood();
            foodWalls = new FoodWalls();
            foodWalls.Load();
            shader = new Shader("../../../../shader.vert", "../../../../shader.frag");
        }
        protected override void OnUnload()
        {
            base.OnUnload();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {

            if (game_logic.CollidedWithTail())
                Close();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            shader.Use();

            Matrix4 model;
            Matrix4 translation;
            Matrix4 view = Matrix4.Identity;
            view *= Matrix4.CreateRotationX(-0.5f * tmp); 
            view *= Matrix4.CreateTranslation(0f, 18f, -10f);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(63.0f), width / height, 0.1f, 100.0f);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);

            background.Bind(gifNumber/4);
            gifNumber++;
            if (gifNumber == 99*4)
                gifNumber = 0;
            model = Matrix4.Identity;
            translation = Matrix4.CreateTranslation(-1f, 0f, -21f);
            model *= translation;
            shader.SetMatrix4("model", model);
            GL.DrawElements(PrimitiveType.Triangles, background.indices.Count, DrawElementsType.UnsignedInt, 0);

            food.Bind();
            model = Matrix4.Identity;
            translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.FoodCoord.Item1) * 2f, -9f + (float)(game_logic.FoodCoord.Item2) * 2f, -20f);
            model *= translation;
            shader.SetMatrix4("model", model);
            GL.DrawElements(PrimitiveType.Triangles, food.octagon.indicesTops.Count, DrawElementsType.UnsignedInt, 0);

            foodWalls.Bind();
            model = Matrix4.Identity;
            translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.FoodCoord.Item1) * 2f, -9f + (float)(game_logic.FoodCoord.Item2) * 2f, -20f);
            model *= translation;
            shader.SetMatrix4("model", model);
            GL.DrawElements(PrimitiveType.Triangles, food.octagon.indicesWalls.Count, DrawElementsType.UnsignedInt, 0);


            if (KeyboardState.IsKeyPressed(OpenTK.Windowing.GraphicsLibraryFramework.Keys.A))
            {
                game_logic.IsWaitingForInput = false;
                HasToRotate = C.LEFT;
            } else if (KeyboardState.IsKeyPressed(OpenTK.Windowing.GraphicsLibraryFramework.Keys.D))
            {
                game_logic.IsWaitingForInput = false;
                HasToRotate = C.RIGHT;
            } else if (KeyboardState.IsKeyPressed(OpenTK.Windowing.GraphicsLibraryFramework.Keys.W))
            {
                game_logic.IsWaitingForInput= false;
                HasToRotate = C.NO;
            }

                if (game_logic.IsWaitingForInput)
            {
                game_logic.IsWaitingForInput = false;
                HasToRotate = C.NO;
                head.Bind();
                model = Matrix4.Identity;
                model = Matrix4.CreateRotationZ((float)(game_logic.HeadDir-11)*tmp);
                translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.HeadCoord.Item1)*2f, -9f+(float)(game_logic.HeadCoord.Item2) * 2f, -20f);
                model *= translation;
                shader.SetMatrix4("model", model);
                GL.DrawElements(PrimitiveType.Triangles, tower.octagon.indicesTops.Count, DrawElementsType.UnsignedInt, 0);

                tower.Bind();
                for (int i = 0; i < game_logic.tailCoords.Count(); i++)
                {
                    model = Matrix4.Identity;
                    model = Matrix4.CreateRotationZ((float)(game_logic.dirs[i] - 11) * tmp);
                    translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.tailCoords[i].Item1) * 2f, -9f + (float)(game_logic.tailCoords[i].Item2) * 2f, -20f);
                    model *= translation;
                    shader.SetMatrix4("model", model);
                    GL.DrawElements(PrimitiveType.Triangles, tower.octagon.indicesTops.Count, DrawElementsType.UnsignedInt, 0);
                }

                walls.Bind();
                for (int i = 0; i < game_logic.tailCoords.Count(); i++)
                {
                    model = Matrix4.Identity;
                    model = Matrix4.CreateRotationZ((float)(game_logic.dirs[i] - 11) * tmp);
                    translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.tailCoords[i].Item1) * 2f, -9f + (float)(game_logic.tailCoords[i].Item2) * 2f, -20f);
                    model *= translation;
                    shader.SetMatrix4("model", model);
                    GL.DrawElements(PrimitiveType.Triangles, walls.octagon.indicesWalls.Count, DrawElementsType.UnsignedInt, 0);
                }
                model = Matrix4.Identity;
                model = Matrix4.CreateRotationZ((float)(game_logic.HeadDir - 11) * tmp);
                translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.HeadCoord.Item1) * 2f, -9f + (float)(game_logic.HeadCoord.Item2) * 2f, -20f);
                model *= translation;
                shader.SetMatrix4("model", model);
                GL.DrawElements(PrimitiveType.Triangles, walls.octagon.indicesWalls.Count, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                if (HasToRotate == C.RIGHT)
                {
                    
                    
                    tower.Bind();
                    for (int i = 0; i < game_logic.tailCoords.Count(); i++)
                    {
                        model = Matrix4.Identity;
                        model = Matrix4.CreateRotationZ((float)(game_logic.dirs[i] - 11) * tmp);
                        translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.tailCoords[i].Item1) * 2f, -9f + (float)(game_logic.tailCoords[i].Item2) * 2f, -20f);
                        model *= translation;
                        shader.SetMatrix4("model", model);
                        GL.DrawElements(PrimitiveType.Triangles, tower.octagon.indicesTops.Count, DrawElementsType.UnsignedInt, 0);
                    }

                    walls.Bind();
                    for (int i = 0; i < game_logic.tailCoords.Count(); i++)
                    {
                        model = Matrix4.Identity;
                        model = Matrix4.CreateRotationZ((float)(game_logic.dirs[i] - 11) * tmp);
                        translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.tailCoords[i].Item1) * 2f, -9f + (float)(game_logic.tailCoords[i].Item2) * 2f, -20f);
                        model *= translation;
                        shader.SetMatrix4("model", model);
                        GL.DrawElements(PrimitiveType.Triangles, walls.octagon.indicesWalls.Count, DrawElementsType.UnsignedInt, 0);
                    }
                    model = Matrix4.Identity;
                    model = Matrix4.CreateRotationZ((float)(game_logic.HeadDir - 11) * tmp);
                    translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.HeadCoord.Item1) * 2f, -9f + (float)(game_logic.HeadCoord.Item2) * 2f, -20f);
                    model *= translation;
                    shader.SetMatrix4("model", model);
                    GL.DrawElements(PrimitiveType.Triangles, walls.octagon.indicesWalls.Count, DrawElementsType.UnsignedInt, 0);

                    if (HasToGrow)
                    {
                        game_logic.tailCoords.Add(game_logic.lastPos);
                        game_logic.dirs.Add(game_logic.dirs[game_logic.dirs.Count - 1]);
                        HasGrown = true;
                        game_logic.SpawnFood();
                    }

                    game_logic.HeadDir += 1;
                    if (game_logic.HeadDir == 15)
                        game_logic.HeadDir = C.UP;

                    head.Bind();
                    model = Matrix4.Identity;
                    model = Matrix4.CreateRotationZ((float)(game_logic.HeadDir - 11) * tmp);
                    translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.HeadCoord.Item1) * 2f, -9f + (float)(game_logic.HeadCoord.Item2) * 2f, -20f);
                    model *= translation;
                    shader.SetMatrix4("model", model);
                    GL.DrawElements(PrimitiveType.Triangles, tower.octagon.indicesTops.Count, DrawElementsType.UnsignedInt, 0);
                    HasToRotate = C.NO;
                }
                else if (HasToRotate == C.LEFT)
                {
                    

                    tower.Bind();
                    for (int i = 0; i < game_logic.tailCoords.Count(); i++)
                    {
                        model = Matrix4.Identity;
                        model = Matrix4.CreateRotationZ((float)(game_logic.dirs[i] - 11) * tmp);
                        translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.tailCoords[i].Item1) * 2f, -9f + (float)(game_logic.tailCoords[i].Item2) * 2f, -20f);
                        model *= translation;
                        shader.SetMatrix4("model", model);
                        GL.DrawElements(PrimitiveType.Triangles, tower.octagon.indicesTops.Count, DrawElementsType.UnsignedInt, 0);
                    }

                    walls.Bind();
                    for (int i = 0; i < game_logic.tailCoords.Count(); i++)
                    {
                        model = Matrix4.Identity;
                        model = Matrix4.CreateRotationZ((float)(game_logic.dirs[i] - 11) * tmp);
                        translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.tailCoords[i].Item1) * 2f, -9f + (float)(game_logic.tailCoords[i].Item2) * 2f, -20f);
                        model *= translation;
                        shader.SetMatrix4("model", model);
                        GL.DrawElements(PrimitiveType.Triangles, walls.octagon.indicesWalls.Count, DrawElementsType.UnsignedInt, 0);
                    }
                    model = Matrix4.Identity;
                    model = Matrix4.CreateRotationZ((float)(game_logic.HeadDir - 11) * tmp);
                    translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.HeadCoord.Item1) * 2f, -9f + (float)(game_logic.HeadCoord.Item2) * 2f, -20f);
                    model *= translation;
                    shader.SetMatrix4("model", model);
                    GL.DrawElements(PrimitiveType.Triangles, walls.octagon.indicesWalls.Count, DrawElementsType.UnsignedInt, 0);
                    if (HasToGrow)
                    {
                        game_logic.tailCoords.Add(game_logic.lastPos);
                        HasGrown = true;
                        game_logic.SpawnFood();
                    }
                    game_logic.HeadDir -= 1;
                    if (game_logic.HeadDir == 10)
                        game_logic.HeadDir = C.LEFT;

                    if (HasToGrow)
                    {
                        game_logic.tailCoords.Add(game_logic.lastPos);
                        game_logic.dirs.Add(game_logic.dirs[game_logic.dirs.Count - 1]);
                        HasGrown = true;
                        game_logic.SpawnFood();
                    }

                    head.Bind();
                    model = Matrix4.Identity;
                    model = Matrix4.CreateRotationZ((float)(game_logic.HeadDir - 11) * tmp);
                    translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.HeadCoord.Item1) * 2f, -9f + (float)(game_logic.HeadCoord.Item2) * 2f, -20f);
                    model *= translation;
                    shader.SetMatrix4("model", model);
                    GL.DrawElements(PrimitiveType.Triangles, tower.octagon.indicesTops.Count, DrawElementsType.UnsignedInt, 0);
                    HasToRotate = C.NO;
                } else if (HasToRotate == C.NO)
                {
                    totalmove += move;
                    game_logic.IsWaitingForInput = false;
                    tower.Bind();
                    for (int i = 0; i < game_logic.tailCoords.Count(); i++)
                    {
                        model = Matrix4.Identity;
                        model = Matrix4.CreateRotationZ((float)(game_logic.dirs[i] - 11) * tmp);
                        if (game_logic.dirs[i] == C.UP)
                            translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.tailCoords[i].Item1) * 2f, -9f + (float)(game_logic.tailCoords[i].Item2) * 2f + totalmove, -20f);
                        else if (game_logic.dirs[i] == C.RIGHT)
                            translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.tailCoords[i].Item1) * 2f + totalmove, -9f + (float)(game_logic.tailCoords[i].Item2) * 2f, -20f);
                        else if (game_logic.dirs[i] == C.LEFT)
                            translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.tailCoords[i].Item1) * 2f - totalmove, -9f + (float)(game_logic.tailCoords[i].Item2) * 2f, -20f);
                        else if (game_logic.dirs[i] == C.DOWN)
                            translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.tailCoords[i].Item1) * 2f, -9f + (float)(game_logic.tailCoords[i].Item2) * 2f-totalmove, -20f);
                        model *= translation;
                        shader.SetMatrix4("model", model);
                        GL.DrawElements(PrimitiveType.Triangles, tower.octagon.indicesTops.Count, DrawElementsType.UnsignedInt, 0);
                    }

                    walls.Bind();
                    for (int i = 0; i < game_logic.tailCoords.Count(); i++)
                    {
                        model = Matrix4.Identity;
                        model = Matrix4.CreateRotationZ((float)(game_logic.dirs[i] - 11) * tmp);
                        if (game_logic.dirs[i] == C.UP)
                            translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.tailCoords[i].Item1) * 2f, -9f + (float)(game_logic.tailCoords[i].Item2) * 2f + totalmove, -20f);
                        else if (game_logic.dirs[i] == C.RIGHT)
                            translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.tailCoords[i].Item1) * 2f + totalmove, -9f + (float)(game_logic.tailCoords[i].Item2) * 2f, -20f);
                        else if (game_logic.dirs[i] == C.LEFT)
                            translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.tailCoords[i].Item1) * 2f - totalmove, -9f + (float)(game_logic.tailCoords[i].Item2) * 2f, -20f);
                        else if (game_logic.dirs[i] == C.DOWN)
                            translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.tailCoords[i].Item1) * 2f, -9f + (float)(game_logic.tailCoords[i].Item2) * 2f - totalmove, -20f);
                        model *= translation;
                        shader.SetMatrix4("model", model);
                        GL.DrawElements(PrimitiveType.Triangles, walls.octagon.indicesWalls.Count, DrawElementsType.UnsignedInt, 0);
                    }
                    model = Matrix4.Identity;
                    model = Matrix4.CreateRotationZ((float)(game_logic.HeadDir - 11) * tmp);
                    if (game_logic.HeadDir == C.UP)
                        translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.HeadCoord.Item1) * 2f, -9f + (float)(game_logic.HeadCoord.Item2) * 2f + totalmove, -20f);
                    else if (game_logic.HeadDir == C.RIGHT)
                        translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.HeadCoord.Item1) * 2f + totalmove, -9f + (float)(game_logic.HeadCoord.Item2) * 2f, -20f);
                    else if (game_logic.HeadDir == C.LEFT)
                        translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.HeadCoord.Item1) * 2f - totalmove, -9f + (float)(game_logic.HeadCoord.Item2) * 2f, -20f);
                    else if (game_logic.HeadDir == C.DOWN)
                        translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.HeadCoord.Item1) * 2f, -9f + (float)(game_logic.HeadCoord.Item2) * 2f - totalmove, -20f);
                    model *= translation;
                    shader.SetMatrix4("model", model);
                    GL.DrawElements(PrimitiveType.Triangles, walls.octagon.indicesWalls.Count, DrawElementsType.UnsignedInt, 0);

                    head.Bind();
                    model = Matrix4.Identity;
                    model = Matrix4.CreateRotationZ((float)(game_logic.HeadDir - 11) * tmp);
                    if (game_logic.HeadDir == C.UP)
                        translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.HeadCoord.Item1) * 2f, -9f + (float)(game_logic.HeadCoord.Item2) * 2f + totalmove, -20f);
                    else if (game_logic.HeadDir == C.RIGHT)
                        translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.HeadCoord.Item1) * 2f + totalmove, -9f + (float)(game_logic.HeadCoord.Item2) * 2f, -20f);
                    else if (game_logic.HeadDir == C.LEFT)
                        translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.HeadCoord.Item1) * 2f - totalmove, -9f + (float)(game_logic.HeadCoord.Item2) * 2f, -20f);
                    else if (game_logic.HeadDir == C.DOWN)
                        translation = Matrix4.CreateTranslation(-9f + (float)(game_logic.HeadCoord.Item1) * 2f, -9f + (float)(game_logic.HeadCoord.Item2) * 2f - totalmove, -20f);
                    model *= translation;
                    shader.SetMatrix4("model", model);
                    GL.DrawElements(PrimitiveType.Triangles, head.octagon.indicesTops.Count, DrawElementsType.UnsignedInt, 0);
                    if (HasToGrow)
                    {
                        game_logic.tailCoords.Add(game_logic.lastPos);
                        game_logic.dirs.Add(game_logic.dirs[game_logic.dirs.Count - 1]);
                        HasGrown = true;
                        game_logic.SpawnFood();
                    }
                    if (totalmove >= 2f)
                    {
                        

                        game_logic.lastPos = game_logic.tailCoords[game_logic.tailCoords.Count - 1];
                        game_logic.tailCoords.Insert(0, game_logic.HeadCoord);
                        game_logic.tailCoords.RemoveAt(game_logic.dirs.Count());

                        game_logic.dirs.Insert(0, game_logic.HeadDir);
                        game_logic.dirs.RemoveAt(game_logic.dirs.Count() - 1);

                        if (game_logic.HeadDir == C.UP)
                            game_logic.HeadCoord.Item2++;
                        else if (game_logic.HeadDir == C.RIGHT)
                            game_logic.HeadCoord.Item1++;
                        else if (game_logic.HeadDir == C.LEFT)
                            game_logic.HeadCoord.Item1--;
                        else if (game_logic.HeadDir == C.DOWN)
                            game_logic.HeadCoord.Item2--;
                        totalmove = 0;

                        if (game_logic.CollidedWithFood())
                        {
                            HasToGrow = true;
                            HasGrown = false;
                            game_logic.IsWaitingForInput = false;
                        }
                        else
                        {
                            game_logic.IsWaitingForInput = true;
                        }
                    }
                }
            }


            if (HasGrown)
                HasToGrow = false;
            
            SwapBuffers();

            base.OnRenderFrame(e);
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
            this.width = e.Width;
            this.height = e.Height; 
        }


        
    }
}
namespace WinFormsApp1
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            
            using (Game game = new Game(1024, 1024, "GameHappened"))
            {
                game.Run();
            }
        }
    }
}