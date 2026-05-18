using System;
using System.IO;

public class FolderNavigator
{
    public static string SelectFolder(string startPath = null)
    {
        string currentPath = startPath ?? Directory.GetCurrentDirectory();

        while (true)
        {
            Console.WriteLine("\n============================");
            Console.WriteLine("Ruta actual: " + currentPath);
            Console.WriteLine("============================\n");

            string[] dirs;

            try
            {
                dirs = Directory.GetDirectories(currentPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error accediendo a la carpeta: " + ex.Message);
                return null;
            }

            Console.WriteLine("0. .. (Subir nivel)");

            for (int i = 0; i < dirs.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {Path.GetFileName(dirs[i])}");
            }

            Console.WriteLine("\nSelecciona una opción:");
            Console.WriteLine("- Número para entrar a carpeta");
            Console.WriteLine("- 's' para seleccionar esta carpeta");
            Console.WriteLine("- 'q' para cancelar");

            string input = Console.ReadLine();

            if (input == "q")
                return null;

            if (input == "s")
                return currentPath;

            if (input == "0")
            {
                var parent = Directory.GetParent(currentPath);
                if (parent != null)
                    currentPath = parent.FullName;

                continue;
            }

            if (int.TryParse(input, out int index))
            {
                index--; // ajustar porque 0 es subir nivel

                if (index >= 0 && index < dirs.Length)
                {
                    currentPath = dirs[index];
                }
                else
                {
                    Console.WriteLine("Opción inválida");
                }
            }
        }
    }
}