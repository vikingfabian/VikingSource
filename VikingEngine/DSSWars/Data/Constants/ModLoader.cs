using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Data.Constants
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using VikingEngine.DataStream;
    using VikingEngine.DebugExtensions;

    public class ModLoader
    {
        public ModLoader(string csFilePath)
        {
            // Read the modded C# file content
            string modCode = File.ReadAllText(Engine.LoadContent.Content.RootDirectory+ DataStream.FilePath.Dir + csFilePath);

            // Create a syntax tree from the mod code
            var syntaxTree = CSharpSyntaxTree.ParseText(modCode);

            // Define the references required to compile the code
            var references = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .Select(a => MetadataReference.CreateFromFile(a.Location))
                .Cast<MetadataReference>()
                .ToList();

            // Create a CSharp compilation
            var compilation = CSharpCompilation.Create("ModdedAssembly")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddSyntaxTrees(syntaxTree)
                .AddReferences(references);

            // Emit the compiled code to a memory stream
            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                string error = "Load mod constants error";
                //List<string> errors = new List<string>();
                // Handle compilation errors
                foreach (var diagnostic in result.Diagnostics)
                {
                    error += Environment.NewLine + diagnostic.ToString();
                }

                new BlueScreen(error);
                return;
            }

            // Load the compiled assembly
            ms.Seek(0, SeekOrigin.Begin);
            var assembly = Assembly.Load(ms.ToArray());

            // Get the modded constants class
            var moddedType = assembly.GetType("ModConst");
            if (moddedType != null)
            {
                // Automate searching for all constants in GameConstants
                ApplyModdedConstants(moddedType);
            }

            DssVar.UpdateConstants();
            //var food = DssConst.MaxFood;
        }

        public static void ApplyModdedConstants(Type moddedType)
        {
            // Get all fields from GameConstants
            var gameConstantsType = typeof(DssConst);
            var gameFields = gameConstantsType.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var gameField in gameFields)
            {
                // Try to find a corresponding field in ModdedConstants
                var moddedField = moddedType.GetField(gameField.Name);
                if (moddedField != null && moddedField.FieldType == gameField.FieldType)
                {
                    // Copy the value from ModdedConstants to GameConstants
                    var moddedValue = moddedField.GetValue(null);
                    gameField.SetValue(null, moddedValue);  // Set value in GameConstants
                    Console.WriteLine($"{gameField.Name} updated to {moddedValue}");
                }
            }
        }
    }

    //public static class GameConstants
    //{
    //    public static float MaxFood = 500;
    //    public static float MinFood = -MaxFood;
    //    public static float DefaultHealth = 100;
    //}
}
