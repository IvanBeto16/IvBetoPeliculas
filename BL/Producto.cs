using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Producto
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            using (DL.IvbetoCineContext context = new DL.IvbetoCineContext())
            {
                var query = context.Productos.FromSqlRaw($"ProductoGetAll");
                if(query != null)
                {
                    result.Objects = new List<object>();
                    foreach( var item in query)
                    {
                        ML.Producto producto = new ML.Producto();
                        producto.IdProducto = item.IdProducto;
                        producto.Nombre = item.Nombre;
                        producto.Descripcion = item.Descripcion;
                        producto.Precio = Convert.ToDecimal(item.Precio.ToString());
                        producto.Imagen = item.Imagen;

                        result.Objects.Add(producto);
                    }
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                }
                return result;
            }
        }

        public static ML.Result GetById(int idProducto)
        {
            ML.Result result = new ML.Result();
            using (DL.IvbetoCineContext context = new DL.IvbetoCineContext())
            {
                var query = context.Productos.FromSqlRaw($"ProductoGetById {idProducto}");
                if(query != null)
                {
                    result.Object = new object();
                    foreach( var item in query)
                    {
                        ML.Producto producto = new ML.Producto();
                        producto.IdProducto = item.IdProducto;
                        producto.Nombre = item.Nombre;
                        producto.Descripcion = item.Descripcion;
                        producto.Precio = Decimal.Parse(item.Precio.ToString());
                        producto.Imagen = item.Imagen;

                        result.Object = producto;
                    }
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                }
                return result;
            }
        }
    }
}
