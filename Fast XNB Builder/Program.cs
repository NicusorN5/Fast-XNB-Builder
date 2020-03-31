using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MonoGame.Framework.Content.Pipeline.Builder;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Fast_XNB_Builder
{
    class Program
    {
        static string PathA = Environment.CurrentDirectory + "\\Source\\";
        static string PathB = Environment.CurrentDirectory + "\\Intermediate\\";
        static string PathC = Environment.CurrentDirectory + "\\Final\\";
        static bool _usingUI = false;
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Start(null);
            }
            else
            {
                for (int i = 0; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "/?":
                            Console.WriteLine("Fast XNB Builder is a quick and easy way to convert .X files to be compatible with Monogame.");
                            break;
                        case "/ui":
                            Start(null);
                            break;
                        case "/f":
                            Start(args[i + 1]);
                            break;
                        default: break;
                    }
                }
            }
            if(_usingUI)
            {
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
        }
        static void ConvertModelX(string path, string outputpath)
        {
            OpaqueDataDictionary keyValues = new OpaqueDataDictionary();
            keyValues.Add("ColorKeyColor", new Color(0, 0, 0, 0));
            keyValues.Add("ColorKeyEnabled", true);
            keyValues.Add("DefaultEffect", "BasicEffect");
            keyValues.Add("GenerateMipmaps", true);
            keyValues.Add("GenerateTangentFrames", false);
            keyValues.Add("PremultiplyTextureAlpha", true);
            keyValues.Add("PremultiplyVertexColors", true);
            keyValues.Add("ResizeTexturesToPowerOfTwo", false);
            keyValues.Add("RotationX", 0);
            keyValues.Add("RotationY", 0);
            keyValues.Add("RotationZ", 0);
            keyValues.Add("Scale", 1);
            keyValues.Add("SwapWindingOrder", false);
            keyValues.Add("TextureFormat", "Compressed");
            ContentBuildLogger logger = new Logger();
            PipelineManager pipeline = new PipelineManager(PathA, PathB, PathC)
            {
                RethrowExceptions = true,
                CompressContent = true,
                Logger = logger,
                Platform = TargetPlatform.Windows,
                Profile = GraphicsProfile.Reach
            };
            pipeline.BuildContent(path, outputpath, "XImporter", "ModelProcessor", keyValues);
        }
        static void ConvertTexture2D(string input, string output)
        {
            OpaqueDataDictionary keyValues = new OpaqueDataDictionary();
            keyValues.Add("Importer", "TextureImporter");
            keyValues.Add("Processor", "TextureProcessor");
            keyValues.Add("ColorKeyColor", new Color(255, 0, 255, 255));
            keyValues.Add("ColorKeyEnabled", true);
            keyValues.Add("GenerateMipmaps", false);
            keyValues.Add("PremultiplyAlpha", true);
            keyValues.Add("ResizeToPowerOfTwo", false);
            keyValues.Add("MakeSquare", false);
            keyValues.Add("TextureFormat", "Color");
            ContentBuildLogger logger = new Logger();
            PipelineManager manager = new PipelineManager(PathA, PathB, PathC)
            {
                RethrowExceptions = true,
                CompressContent = true,
                Logger = logger,
                Platform = TargetPlatform.Windows,
                Profile = GraphicsProfile.Reach
            };
            manager.BuildContent(input, output, "TextureImporter", "TextureProcessor", keyValues);
        }

        static void Start(string folder)
        {
            if (folder == null)
            {
                _usingUI = true;
                Console.WriteLine("XNB Converter > [OK] Running wizard.");
                FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
                if (folderBrowser.ShowDialog() == DialogResult.OK) folder = folderBrowser.SelectedPath;
                else
                {
                    Console.Write("XNB Converter > [OK] Operation cancelled.");
                    return;
                }
            }
            else Console.WriteLine("XNB Converter > [OK] Starting operation.");
            Console.WriteLine("XNB Converter > [INFO] Creating temporary directories.");
            Directory.CreateDirectory(PathA);
            Directory.CreateDirectory(PathB);
            Directory.CreateDirectory(PathC);
            Console.WriteLine("XNB Converter > [INFO] Finished creating temporary directories.");
            string[] files = Directory.EnumerateFiles(folder).ToArray();
            string[] copied_f = new string[files.Length];
            Console.WriteLine("XNB Converter > [INFO] Copying files...");
            for (int i = 0; i < files.Length; i++)
            {
                string a = PathA + Path.GetFileName(files[i]);
                try
                {
                    File.Copy(Path.GetFullPath(files[i]), a);
                }
                catch (IOException e)
                {
                    Console.WriteLine("XNB Converter > [ERROR] :" + e.Message + " (" + e.HResult + ")");
                    Console.WriteLine("File: " + files[i]);
                }
                finally
                {
                    Console.WriteLine("XNB Converter > [INFO] Copied: " + files[i] + " to " + a);
                    copied_f[i] = a;
                }
            }
            Console.WriteLine("XNB Converter > [INFO] Converting given files...");
            for (int i = 0; i < copied_f.Length; i++)
            {
                ConvertFile(copied_f[i]);
            }
            Console.WriteLine("XNB Converter > [OK] Done!");
            
        }
        static void ConvertFile(string file)
        {
            string[] ext = file.Split('.');
            string extension = ext[ext.Length - 1];
            switch (extension.ToLower())
            {
                case "x":
                    ConvertModelX(file, PathC + Path.GetFileNameWithoutExtension(file));
                    break;
                case "png":
                case "bmp":
                case "dds":
                    ConvertTexture2D(file, PathC + Path.GetFileNameWithoutExtension(file));
                    break;
                default:
                    Console.WriteLine("XNB Converter > [INFO] XNB Converter: Unsuported format found:" + extension);
                    break;
            }
        }

    }
}
