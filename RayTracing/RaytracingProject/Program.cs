using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Windows.Input;
using OpenTK.Mathematics;
using StbImageSharp;
using System.Transactions;
using System.Globalization;
using OpenTK.Graphics.OpenGL4;


namespace RaytracingProject
{
    public class View : GameWindow{
        int Program;
        int FragShader;
        int VertShader;
        int width, height;

		int VAO;
		int VBO;
		int EBO;

        public List<Vector3> vertices = new List<Vector3>()
        {
            new Vector3(-1f,1f,-1f),
            new Vector3(1f,1f,-1f),
            new Vector3(1f,-1f,-1f),
            new Vector3(-1f,-1f,-1f),
        };


        uint[] indices =
        {
            0, 1, 2,
            2, 3, 0
        };

        public View(int width, int height, string title): base (GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title ,Vsync = VSyncMode.On}){
            this.width = width;
            this.height = height;
            this.CenterWindow(new Vector2i(width, height));
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            this.width = e.Width;
            this.height = e.Height;
        }

        void LoadShader(String filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            string shaderSource = "";

            using (StreamReader reader = new StreamReader(filename))
            {
                shaderSource = reader.ReadToEnd();
            }

            GL.ShaderSource(address, shaderSource);
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }
        
        void InitShaders(){
            Program = GL.CreateProgram();
			LoadShader("../../../Shaders/shader.vert", ShaderType.VertexShader, Program,
						out VertShader);		
			LoadShader("../../../Shaders/shader.frag", ShaderType.FragmentShader, Program,
						out FragShader);
			GL.LinkProgram(Program);

			int status = 0;
			GL.GetProgram(Program, GetProgramParameterName.LinkStatus, out status);
			Console.WriteLine(GL.GetProgramInfoLog(Program));
        }



        protected override void OnLoad()
		{
			base.OnLoad();

			VAO = GL.GenVertexArray();
			GL.BindVertexArray(VAO);
			VBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
			GL.BufferData(BufferTarget.ArrayBuffer,  vertices.Count * Vector3.SizeInBytes,  vertices.ToArray(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib( VAO, 0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			EBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

			InitShaders();

		}
        
        protected override void OnRenderFrame(FrameEventArgs args)
		{
			GL.ClearColor(0.1f, 0.3f, 0.8f, 0.5f);
			GL.Clear(ClearBufferMask.ColorBufferBit);


			GL.UseProgram(Program);
			GL.BindVertexArray(VAO);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);

			GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

			Context.SwapBuffers();
			base.OnRenderFrame(args);
		}

		protected override void OnUpdateFrame(FrameEventArgs args)
		{
			base.OnUpdateFrame(args);
		}
        
    }
    class Program
    {
        [STAThread]
        static void Main()
        {
            using (View view = new View(800, 800, "Raytracing Happened"))
			{
				view.Run();
			}
		}
    }
}
