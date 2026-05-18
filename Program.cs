class Program
{
    static void Main(string[] args)
    {

        Console.WriteLine("Arrancando Sistema de Gestión de Tareas...");
        TaskService service = new TaskService();
        string option;
        do
        {

            service.ShowMenu();

            option = Console.ReadLine();

            Console.WriteLine();

            switch (option)
            {
                case "1":
                    Console.Clear();
                    service.AddTask();
                    Console.WriteLine("");
                    service.ShowTasks();
                    break;

                case "2":
                    Console.Clear();
                    service.ShowTasks();
                    break;

                case "3":

                    int taskForCompleted = service.getCountTaskNotCompleted();
                    if(taskForCompleted == 0)
                    {
                        Console.WriteLine("No existen tareas por completar.");
                        break;
                    }
                    Console.Clear();
                    Console.Write("ID de la tarea a completar: ");
                    int idComplete = int.Parse(Console.ReadLine());
                    service.CompleteTask(idComplete);
                    Console.WriteLine("");
                    service.ShowTasks();

                    break;

                case "4":
                    int tasks = service.getCountTask();
                    if (tasks == 0 )
                    {
                        Console.WriteLine("No existen tareas para eliminar.");
                        break;
                    }
                    Console.Clear();
                    Console.Write("ID de la tarea a eliminar: ");
                    int idDelete = int.Parse(Console.ReadLine());
                    service.DeleteTask(idDelete);
                    Console.WriteLine("");
                    service.ShowTasks();
                    break;

                case "5":
                    Console.Clear();
                    string ruta = FolderNavigator.SelectFolder();

                    if (ruta == null)
                    {
                        Console.WriteLine("Exportación cancelada");
                        break;
                    }


                    service.ExportToExcel(ruta);
                    Console.WriteLine("");
                    break;
                case "6":


                    if(service.getCountTaskNotCompleted()  == 0 )
                    {
                        Console.WriteLine("No es posible esta acción, debido a que no existen tareas por completar.");
                        break;
                    }

                    Console.Clear();
                    service.ShowTasks();
                    int taskUpdated;
                    do
                    {
                        Console.WriteLine("Ingrese el número de tarea a modificar (recuerda que solo puedes modificar tareas no completadas):");
                        bool isValid = int.TryParse(Console.ReadLine(), out taskUpdated);
                        if (!isValid)
                        {
                            Console.WriteLine("Favor de ingresar un número valido");
                        }
                    } while (taskUpdated <= 0);

                    bool taskValid = service.isTaskNotCompleted(taskUpdated);
                    if (taskValid)
                    {
                        service.updateTask(taskUpdated);
                    }
                    break;
                case "7":
                    bool inBucle = true;
                    do
                    {
                        Console.WriteLine("Estas seguro de eliminar todas las tareas completadas (s/n)");
                        string confirmation = Console.ReadLine();
                        if (confirmation.ToUpper() == "S")
                        {
                            service.DeleteTaskCompleted();
                            Console.WriteLine("Datos eliminados...");
                            inBucle = false;
                        }
                        else if (confirmation.ToUpper() == "N")
                        {
                            inBucle = false;
                        }
                        else
                        {
                            Console.WriteLine("Selecciona una opción valida.");
                        }

                    } while (inBucle);

                    break;
                case "0":
                    Console.WriteLine("Saliendo...");
                    break;

                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }

        } while (option != "0");
    }
}