using System.Text.Json;
using ClosedXML.Excel;
public class TaskService
{
    private List<TaskItem> tasks = new List<TaskItem>();
    private int nextId = 1;
    private string filePath = "tasks.json";

    public TaskService()
    {
        LoadTasks();
    }

    public void CompleteTask(int id)
    {
        var task = tasks.FirstOrDefault(t => t.Id == id);

        if (task != null)
        {
            task.IsCompleted = true;
            SaveTasks();
        }
        else
        {
            Console.WriteLine("Tarea no encontrada.");
        }
    }

    public void ShowTasks()
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("No hay tareas para mostrar.");
            return;
        }

        Console.WriteLine("======= Mis Tareas =======");
        Console.WriteLine("|".PadRight(5) + "ID".PadRight(5) + "Título".PadRight(30) + "Descripción".PadRight(50) + "Estado");

        foreach (var task in tasks)
        {
            string status = task.IsCompleted ? "✔️" : "❌";

            Console.WriteLine("|".PadRight(5) +
                task.Id.ToString().PadRight(5) +
                task.title.PadRight(30) +
                task.description.PadRight(50) + 
                status
            );
        }

        Console.WriteLine("=========================");
    }

    public void DeleteTaskCompleted()
    {
        tasks.RemoveAll(task => task.IsCompleted);
        SaveTasks();
    }
    public void AddTask()
    {
        Console.Write("Título de la tarea: ");
        string title = Console.ReadLine();
        Console.Write("Descripción de la tarea: ");
        string description = Console.ReadLine();
        Console.WriteLine("Seleccione la prioridad: ");


        foreach (var prioridad in prioridades)
        {
            Console.WriteLine($"{prioridad.id}) {prioridad.title}");
        }

        string priorityId = Console.ReadLine();
        var _priority = prioridades.FirstOrDefault(p => p.id == Convert.ToInt32(priorityId));
        var task = new TaskItem
        {
            Id = nextId++,
            title = title,
            description = description,
            dateCreated = DateTime.Now,
            dateUpdated = DateTime.Now,
            IsCompleted = false,
            priority = _priority.title
        };

        tasks.Add(task);
        SaveTasks();
    }
    public void DeleteTask(int id)
    {
        TaskItem task = getTask(id);

        if (task != null)
        {
            tasks.Remove(task);
            SaveTasks();
        }
        else
        {
            Console.WriteLine("Tarea no encontrada.");
        }
    }

    public void ExportToExcel(string ruta)
    {

        //Validamos que exista la ruta
        if (!Directory.Exists(ruta))
        {
            Console.WriteLine("No existe la ruta proporcionada, favor de seleccionar un directorio existente");
            return;
        }
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Tareas");

        // Encabezados
        worksheet.Cell(1, 1).Value = "ID";
        worksheet.Cell(1, 2).Value = "Título";
        worksheet.Cell(1, 3).Value = "Descripción";
        worksheet.Cell(1, 4).Value = "Completada";
        worksheet.Cell(1, 5).Value = "Prioridad";
        worksheet.Cell(1, 6).Value = "Fecha Creacion";
        worksheet.Cell(1, 7).Value = "Fecha Actualizacion";


        int row = 2;

        foreach (var task in tasks)
        {
            worksheet.Cell(row, 1).Value = task.Id;
            worksheet.Cell(row, 2).Value = task.title;
            worksheet.Cell(row, 3).Value = task.description;
            worksheet.Cell(row, 4).Value = task.IsCompleted ? "Sí" : "No";
            worksheet.Cell(row, 5).Value = task.priority;
            worksheet.Cell(row, 6).Value = task.dateCreated;
            worksheet.Cell(row, 7).Value = task.dateUpdated;

            row++;
        }

        string rutaDescarga = ruta + "/tareas.xlsx";

        workbook.SaveAs(rutaDescarga);

        Console.WriteLine("Exportado correctamente en " + rutaDescarga);
    }
    public void ShowMenu()
    {
        Console.WriteLine("\n===== TASK MANAGER =====");
        Console.WriteLine("1. Agregar tarea");
        Console.WriteLine("2. Ver tareas");
        Console.WriteLine("3. Completar tarea");
        Console.WriteLine("4. Eliminar tarea");
        Console.WriteLine("5. Exportar a Excel");
        Console.WriteLine("6. Modificar Tarea");
        Console.WriteLine("7. Limpiar tareas completadas");
        Console.WriteLine("0. Salir");
        Console.Write("Elige una opción: ");
    }

    public bool isTaskNotCompleted(int taskUpdated)
    {
        TaskItem task = getTask(taskUpdated);
        if (task == null)
        {
            Console.WriteLine("No se ha encontrado la tarea a modificar.");
            return false;
        }

        if (task.IsCompleted)
        {
            Console.WriteLine("No es posible modificarse debodo a que es una actividad ya completada.");
            return false;
        }

        return true;
    }

    public int getCountTaskCompleted()
    {
        return  tasks.Count(task => task.IsCompleted);
    }

    public int getCountTask()
    {
        return tasks.Count();
    }

    public int getCountTaskNotCompleted()
    {
        return  tasks.Count(task => task.IsCompleted == false);
    }
    public void updateTask(int taskId)
    {
        TaskItem task = getTask(taskId);
        Console.WriteLine("Ingrese el nuevo titulo (en caso de no requerir modificar no escribir nada):");
        string newTitle = Console.ReadLine();
        if (newTitle != null && newTitle != "")
        {
            task.title = newTitle;
        }

        Console.WriteLine("Ingrese la nueva descripcion (en caso de no requerir modificar no escribir nada):");
        string newDescription = Console.ReadLine();
        if (newDescription != null && newDescription != "")
        {
            task.description = newDescription;
        }

        SaveTasks();

        Console.WriteLine("Datos actualizados exitosamente.");

    }

    private TaskItem getTask(int taskId)
    {
        TaskItem task = tasks.FirstOrDefault(task => task.Id == taskId);
        return task;
    }

    private void SaveTasks()
    {
        var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(filePath, json);
    }
    private static readonly List<PriorityTask> prioridades = new()
{
    new PriorityTask { id = 1, title = "Alta" },
    new PriorityTask { id = 2, title = "Media" },
    new PriorityTask { id = 3, title = "Baja" }
};
    private void LoadTasks()
    {
        if (!File.Exists(filePath))
            return;

        string json = File.ReadAllText(filePath);
        tasks = JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
        if (tasks.Count > 0)
        {
            nextId = tasks.Max(t => t.Id) + 1;
        }

    }
}