using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using SnakeGame;
using StbImageSharp;
using System.Transactions;
using SnakeGame.Drawing;
using static OpenTK.Graphics.OpenGL.GL;
using System.Runtime.CompilerServices;

namespace WinFormsApp1.Drawing
{
    internal class Octagon
    {
        public List<Vector3> vertices = new List<Vector3>()
        {
            new Vector3(1f,0f,0f),
            new Vector3(1f,0.5f,0f),
            new Vector3(0.5f,1f,0f),
            new Vector3(0f,1f,0f),
            new Vector3(-0.5f,1f,0f),
            new Vector3(-1f,0.5f,0f),
            new Vector3(-1f,0f,0f),
            new Vector3(-1f,-0.5f,0f),
            new Vector3(-0.5f,-1f,0f),
            new Vector3(0f,-1f,0f),
            new Vector3(0.5f,-1f,0f),
            new Vector3(1f,-0.5f,0f),

            new Vector3(1f,0f,2f),
            new Vector3(1f,0.5f,2f),
            new Vector3(0.5f,1f,2f),
            new Vector3(0f,1f,2f),
            new Vector3(-0.5f,1f,2f),
            new Vector3(-1f,0.5f,2f),
            new Vector3(-1f,0f,2f),
            new Vector3(-1f,-0.5f,2f),
            new Vector3(-0.5f,-1f,2f),
            new Vector3(0f,-1f,2f),
            new Vector3(0.5f,-1f,2f),
            new Vector3(1f,-0.5f,2f),

            new Vector3(0f,0f,0f),
            new Vector3(0f,0f,2f),

        };
        public List<uint> indicesTops = new List<uint>();
        public List<uint> indicesWalls = new List<uint>();
        public Octagon()
        {
            for (uint x = 0; x < 11; x++)
            {
                indicesTops.Add(x);
                indicesTops.Add(x + 1);
                indicesTops.Add(26);

                indicesTops.Add(x+12);
                indicesTops.Add(x + 13);
                indicesTops.Add(25);
            }
            indicesTops.Add(11);
            indicesTops.Add(0);
            indicesTops.Add(26);

            indicesTops.Add(23);
            indicesTops.Add(12);
            indicesTops.Add(25);

            for (uint x = 0; x < 11; x++)
            {
                indicesWalls.Add(12 + x);
                indicesWalls.Add(x + 12 + 1);
                indicesWalls.Add(x + 1);

                indicesWalls.Add(x + 1);
                indicesWalls.Add(x);
                indicesWalls.Add(x + 12);
            }
            indicesWalls.Add(23);
            indicesWalls.Add(12);
            indicesWalls.Add(0);

            indicesWalls.Add(0);
            indicesWalls.Add(11);
            indicesWalls.Add(23);
        }
    }

    internal class OctagonWalls {
        VAO vao;
        IBO ibo;
        VBO vbo;
        Texture texture;
        Texture texture1;

        public Octagon octagon = new Octagon();

        List<Vector2> texCoord = new List<Vector2>()
        {
            new Vector2(1f, 0.5f),
            new Vector2(1f, 0.75f),
            new Vector2(0.75f, 1f),

            new Vector2(0.5f, 1f),
            new Vector2(0.25f, 1f),
            new Vector2(0f, 0.75f),

            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.25f),
            new Vector2(0.25f, 0f),

            new Vector2(0.5f, 0f),
            new Vector2(0.75f, 0f),
            new Vector2(1f, 0.25f),

            new Vector2(1f, 0.5f),
            new Vector2(1f, 0.75f),
            new Vector2(0.75f, 1f),

            new Vector2(0.5f, 1f),
            new Vector2(0.25f, 1f),
            new Vector2(0f, 0.75f),

            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.25f),
            new Vector2(0.25f, 0f),

            new Vector2(0.5f, 0f),
            new Vector2(0.75f, 0f),
            new Vector2(1f, 0.25f),

            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),

             new Vector2(1f, 0.5f),
            new Vector2(1f, 0.75f),
            new Vector2(0.75f, 1f),

            new Vector2(0.5f, 1f),
            new Vector2(0.25f, 1f),
            new Vector2(0f, 0.75f),

            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.25f),
            new Vector2(0.25f, 0f),

            new Vector2(0.5f, 0f),
            new Vector2(0.75f, 0f),
            new Vector2(1f, 0.25f),

            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),


        };
        public void Load()
        {
            vao = new VAO();
            VBO vbo = new VBO(octagon.vertices);
            vao.LinkToVAO(0, 3, vbo);
            VBO uvVBO = new VBO(texCoord);
            vao.LinkToVAO(1, 2, uvVBO);

            ibo = new IBO(octagon.indicesWalls);

            GL.Enable(EnableCap.DepthTest);

            texture = new Texture("../../../Textures/walls.png");
        }

        public void Bind()
        {
            vao.Bind();
            texture.Bind();
            ibo.Bind();
        }
        public void Delete()
        {
            vao.Unbind();
            vbo.Unbind();
            texture.Unbind();
        }
    }

    internal class FoodWalls
    {
        VAO vao;
        IBO ibo;
        VBO vbo;
        Texture texture;
        Texture texture1;

        public Octagon octagon = new Octagon();

        List<Vector2> texCoord = new List<Vector2>()
        {
            new Vector2(1f, 0.5f),
            new Vector2(1f, 0.75f),
            new Vector2(0.75f, 1f),

            new Vector2(0.5f, 1f),
            new Vector2(0.25f, 1f),
            new Vector2(0f, 0.75f),

            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.25f),
            new Vector2(0.25f, 0f),

            new Vector2(0.5f, 0f),
            new Vector2(0.75f, 0f),
            new Vector2(1f, 0.25f),

            new Vector2(1f, 0.5f),
            new Vector2(1f, 0.75f),
            new Vector2(0.75f, 1f),

            new Vector2(0.5f, 1f),
            new Vector2(0.25f, 1f),
            new Vector2(0f, 0.75f),

            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.25f),
            new Vector2(0.25f, 0f),

            new Vector2(0.5f, 0f),
            new Vector2(0.75f, 0f),
            new Vector2(1f, 0.25f),

            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),

             new Vector2(1f, 0.5f),
            new Vector2(1f, 0.75f),
            new Vector2(0.75f, 1f),

            new Vector2(0.5f, 1f),
            new Vector2(0.25f, 1f),
            new Vector2(0f, 0.75f),

            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.25f),
            new Vector2(0.25f, 0f),

            new Vector2(0.5f, 0f),
            new Vector2(0.75f, 0f),
            new Vector2(1f, 0.25f),

            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),


        };
        public void Load()
        {
            octagon.vertices.Clear();
            List<Vector3> newVert = new List<Vector3>()
            {
            new Vector3(1f,0f,0.5f),
            new Vector3(1f,0.5f,0.5f),
            new Vector3(0.5f,1f,0.5f),
            new Vector3(0f,1f,0.5f),
            new Vector3(-0.5f,1f,0.5f),
            new Vector3(-1f,0.5f,0.5f),
            new Vector3(-1f,0f,0.5f),
            new Vector3(-1f,-0.5f,0.5f),
            new Vector3(-0.5f,-1f,0.5f),
            new Vector3(0f,-1f,0.5f),
            new Vector3(0.5f,-1f,0.5f),
            new Vector3(1f,-0.5f,0.5f),

            new Vector3(1f,0f,1.5f),
            new Vector3(1f,0.5f,1.5f),
            new Vector3(0.5f,1f,1.5f),
            new Vector3(0f,1f,1.5f),
            new Vector3(-0.5f,1f,1.5f),
            new Vector3(-1f,0.5f,1.5f),
            new Vector3(-1f,0f,1.5f),
            new Vector3(-1f,-0.5f,1.5f),
            new Vector3(-0.5f,-1f,1.5f),
            new Vector3(0f,-1f,1.5f),
            new Vector3(0.5f,-1f,1.5f),
            new Vector3(1f,-0.5f,1.5f),

            new Vector3(0f,0f,0.5f),
            new Vector3(0f,0f,1.5f),

            };
            octagon.vertices.AddRange(newVert);
            vao = new VAO();
            VBO vbo = new VBO(octagon.vertices);
            vao.LinkToVAO(0, 3, vbo);
            VBO uvVBO = new VBO(texCoord);
            vao.LinkToVAO(1, 2, uvVBO);

            ibo = new IBO(octagon.indicesWalls);

            GL.Enable(EnableCap.DepthTest);

            texture = new Texture("../../../Textures/foodWalls.png");
        }

        public void Bind()
        {
            vao.Bind();
            texture.Bind();
            ibo.Bind();
        }
        public void Delete()
        {
            vao.Unbind();
            vbo.Unbind();
            texture.Unbind();
        }
    }

    internal class Tower
    {
        VAO vao;
        IBO ibo;
        VBO vbo;
        Texture texture;

        public Octagon octagon = new Octagon();
        List<Vector2> texCoord = new List<Vector2>()
        {
            new Vector2(1f, 0.5f),
            new Vector2(1f, 0.75f),
            new Vector2(0.75f, 1f),

            new Vector2(0.5f, 1f),
            new Vector2(0.25f, 1f),
            new Vector2(0f, 0.75f),

            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.25f),
            new Vector2(0.25f, 0f),

            new Vector2(0.5f, 0f),
            new Vector2(0.75f, 0f),
            new Vector2(1f, 0.25f),

            new Vector2(1f, 0.5f),
            new Vector2(1f, 0.75f),
            new Vector2(0.75f, 1f),

            new Vector2(0.5f, 1f),
            new Vector2(0.25f, 1f),
            new Vector2(0f, 0.75f),

            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.25f),
            new Vector2(0.25f, 0f),

            new Vector2(0.5f, 0f),
            new Vector2(0.75f, 0f),
            new Vector2(1f, 0.25f),

            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),

             new Vector2(1f, 0.5f),
            new Vector2(1f, 0.75f),
            new Vector2(0.75f, 1f),

            new Vector2(0.5f, 1f),
            new Vector2(0.25f, 1f),
            new Vector2(0f, 0.75f),

            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.25f),
            new Vector2(0.25f, 0f),

            new Vector2(0.5f, 0f),
            new Vector2(0.75f, 0f),
            new Vector2(1f, 0.25f),

            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),


        };
        public void Load()
        {
            vao = new VAO();
            VBO vbo = new VBO(octagon.vertices);
            vao.LinkToVAO(0, 3, vbo);
            VBO uvVBO = new VBO(texCoord);
            vao.LinkToVAO(1, 2, uvVBO);

            ibo = new IBO(octagon.indicesTops);

            GL.Enable(EnableCap.DepthTest);

            texture = new Texture("../../../Textures/tower.png");
        }

        public void Bind()
        {
            vao.Bind();
            texture.Bind();
            ibo.Bind();
        }
        public void Delete()
        {
            vao.Unbind();
            vbo.Unbind();
            texture.Unbind();
        }
    }
    internal class Food
    {
        VAO vao;
        IBO ibo;
        VBO vbo;
        Texture texture;

        public Octagon octagon = new Octagon();
        List<Vector2> texCoord = new List<Vector2>()
        {
            new Vector2(1f, 0.5f),
            new Vector2(1f, 0.75f),
            new Vector2(0.75f, 1f),

            new Vector2(0.5f, 1f),
            new Vector2(0.25f, 1f),
            new Vector2(0f, 0.75f),

            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.25f),
            new Vector2(0.25f, 0f),

            new Vector2(0.5f, 0f),
            new Vector2(0.75f, 0f),
            new Vector2(1f, 0.25f),

            new Vector2(1f, 0.5f),
            new Vector2(1f, 0.75f),
            new Vector2(0.75f, 1f),

            new Vector2(0.5f, 1f),
            new Vector2(0.25f, 1f),
            new Vector2(0f, 0.75f),

            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.25f),
            new Vector2(0.25f, 0f),

            new Vector2(0.5f, 0f),
            new Vector2(0.75f, 0f),
            new Vector2(1f, 0.25f),

            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),

             new Vector2(1f, 0.5f),
            new Vector2(1f, 0.75f),
            new Vector2(0.75f, 1f),

            new Vector2(0.5f, 1f),
            new Vector2(0.25f, 1f),
            new Vector2(0f, 0.75f),

            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.25f),
            new Vector2(0.25f, 0f),

            new Vector2(0.5f, 0f),
            new Vector2(0.75f, 0f),
            new Vector2(1f, 0.25f),

            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),


        };
        public void Load()
        {
            octagon.vertices.Clear();
            List<Vector3> newVert = new List<Vector3>()
            {
            new Vector3(1f,0f,0.5f),
            new Vector3(1f,0.5f,0.5f),
            new Vector3(0.5f,1f,0.5f),
            new Vector3(0f,1f,0.5f),
            new Vector3(-0.5f,1f,0.5f),
            new Vector3(-1f,0.5f,0.5f),
            new Vector3(-1f,0f,0.5f),
            new Vector3(-1f,-0.5f,0.5f),
            new Vector3(-0.5f,-1f,0.5f),
            new Vector3(0f,-1f,0.5f),
            new Vector3(0.5f,-1f,0.5f),
            new Vector3(1f,-0.5f,0.5f),

            new Vector3(1f,0f,1.5f),
            new Vector3(1f,0.5f,1.5f),
            new Vector3(0.5f,1f,1.5f),
            new Vector3(0f,1f,1.5f),
            new Vector3(-0.5f,1f,1.5f),
            new Vector3(-1f,0.5f,1.5f),
            new Vector3(-1f,0f,1.5f),
            new Vector3(-1f,-0.5f,1.5f),
            new Vector3(-0.5f,-1f,1.5f),
            new Vector3(0f,-1f,1.5f),
            new Vector3(0.5f,-1f,1.5f),
            new Vector3(1f,-0.5f,1.5f),

            new Vector3(0f,0f,0.5f),
            new Vector3(0f,0f,1.5f),

            };
            octagon.vertices.AddRange(newVert);

            vao = new VAO();
            VBO vbo = new VBO(octagon.vertices);
            vao.LinkToVAO(0, 3, vbo);
            VBO uvVBO = new VBO(texCoord);
            vao.LinkToVAO(1, 2, uvVBO);

            ibo = new IBO(octagon.indicesTops);

            GL.Enable(EnableCap.DepthTest);

            texture = new Texture("../../../Textures/food.png");
        }

        public void Bind()
        {
            vao.Bind();
            texture.Bind();
            ibo.Bind();
        }
        public void Delete()
        {
            vao.Unbind();
            vbo.Unbind();
            texture.Unbind();
        }
    }
    internal class Head
    {
        VAO vao;
        IBO ibo;
        VBO vbo;
        Texture texture;

        public Octagon octagon = new Octagon();
        List<Vector2> texCoord = new List<Vector2>()
        {
             new Vector2(1f, 0.5f),
            new Vector2(1f, 0.75f),
            new Vector2(0.75f, 1f),

            new Vector2(0.5f, 1f),
            new Vector2(0.25f, 1f),
            new Vector2(0f, 0.75f),

            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.25f),
            new Vector2(0.25f, 0f),

            new Vector2(0.5f, 0f),
            new Vector2(0.75f, 0f),
            new Vector2(1f, 0.25f),

            new Vector2(1f, 0.5f),
            new Vector2(1f, 0.75f),
            new Vector2(0.75f, 1f),

            new Vector2(0.5f, 1f),
            new Vector2(0.25f, 1f),
            new Vector2(0f, 0.75f),

            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.25f),
            new Vector2(0.25f, 0f),

            new Vector2(0.5f, 0f),
            new Vector2(0.75f, 0f),
            new Vector2(1f, 0.25f),

            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),

             new Vector2(1f, 0.5f),
            new Vector2(1f, 0.75f),
            new Vector2(0.75f, 1f),

            new Vector2(0.5f, 1f),
            new Vector2(0.25f, 1f),
            new Vector2(0f, 0.75f),

            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.25f),
            new Vector2(0.25f, 0f),

            new Vector2(0.5f, 0f),
            new Vector2(0.75f, 0f),
            new Vector2(1f, 0.25f),

            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),


        };
        public void Load()
        {
            vao = new VAO();
            VBO vbo = new VBO(octagon.vertices);
            vao.LinkToVAO(0, 3, vbo);
            VBO uvVBO = new VBO(texCoord);
            vao.LinkToVAO(1, 2, uvVBO);

            ibo = new IBO(octagon.indicesTops);

            GL.Enable(EnableCap.DepthTest);

            texture = new Texture("../../../Textures/head.png");
        }

        public void Bind()
        {
            vao.Bind();
            texture.Bind();
            ibo.Bind();
        }
        public void Delete()
        {
            vao.Unbind();
            vbo.Unbind();
            texture.Unbind();
        }
    }
    public class Background
    {
        List<Vector3> vertices = new List<Vector3>() {
           new Vector3(-15f,15f,0f),
            new Vector3(15f,15f,0f),
            new Vector3(15f,-15f,0f),
            new Vector3(-15f,-15f,0f),
            };

        List<Vector2> texCoord = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
    };

        public List<uint> indices = new List<uint>()
            {
                0, 1, 2,
                2, 3, 0,
            };

        VAO vao;
        IBO ibo;
        VBO vbo;
        Texture texture;
        public void Load()
        {
            vao = new VAO();
            VBO vbo = new VBO(vertices);
            vao.LinkToVAO(0, 3, vbo);
            VBO uvVBO = new VBO(texCoord);
            vao.LinkToVAO(1, 2, uvVBO);

            ibo = new IBO(indices);

            GL.Enable(EnableCap.DepthTest);

            
        }
        public void Bind(int i = 0)
        {
            string tmpStr = i.ToString();
            texture = new Texture("../../../Textures/vikings/"+ i.ToString()+".gif");
            vao.Bind();
            texture.Bind();
            ibo.Bind();
        }
        public void Delete()
        {
            vao.Unbind();
            vbo.Unbind();
            texture.Unbind();
        }
    }
}
