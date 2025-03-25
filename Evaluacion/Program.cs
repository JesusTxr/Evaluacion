using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

class Producto
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public int CantidadStock { get; set; }
    public double PrecioUnitario { get; set; }
}

class Inventario
{
    private string _rutaArchivo;

    public Inventario(string rutaArchivo)
    {
        _rutaArchivo = rutaArchivo;
    }

    public void GuardarProducto(Producto producto)
    {
        List<Producto> productos = LeerProductos();
        productos.Add(producto);
        string json = JsonConvert.SerializeObject(productos, Formatting.Indented);
        File.WriteAllText(_rutaArchivo, json);
        Console.WriteLine("Producto guardado en el inventario.");
    }

    public List<Producto> LeerProductos()
    {
        if (File.Exists(_rutaArchivo))
        {
            string json = File.ReadAllText(_rutaArchivo);
            return JsonConvert.DeserializeObject<List<Producto>>(json) ?? new List<Producto>();
        }
        return new List<Producto>();
    }

    public void MostrarInventario()
    {
        List<Producto> productos = LeerProductos();
        Console.WriteLine("\nInventario actual:");
        foreach (var producto in productos)
        {
            Console.WriteLine($"Código: {producto.Codigo}, Nombre: {producto.Nombre}, Stock: {producto.CantidadStock}, Precio: {producto.PrecioUnitario:C}");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        string filePath = "inventario.json";
        Inventario inventario = new Inventario(filePath);

        RegistrarProductos(inventario);

        inventario.MostrarInventario();
    }

    static void RegistrarProductos(Inventario inventario)
    {
        Console.WriteLine("Ingrese los productos (escriba 'fin' en el código para terminar):");

        while (true)
        {
            Console.Write("Código del producto: ");
            string codigo = Console.ReadLine();
            if (codigo.ToLower() == "fin")
                break;

            Console.Write("Nombre del producto: ");
            string nombre = Console.ReadLine();

            Console.Write("Cantidad en stock: ");
            if (!int.TryParse(Console.ReadLine(), out int cantidadStock))
            {
                Console.WriteLine("Cantidad inválida, intente de nuevo.");
                continue;
            }

            Console.Write("Precio unitario: ");
            if (!double.TryParse(Console.ReadLine(), out double precioUnitario))
            {
                Console.WriteLine("Precio inválido, intente de nuevo.");
                continue;
            }

            Producto producto = new Producto
            {
                Codigo = codigo,
                Nombre = nombre,
                CantidadStock = cantidadStock,
                PrecioUnitario = precioUnitario
            };

            inventario.GuardarProducto(producto);
        }
    }
}