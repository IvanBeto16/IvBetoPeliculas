using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Cine
    {
        public static ML.Result Add(ML.Cine cine)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.IvbetoCineContext context = new DL.IvbetoCineContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"CineAdd '{cine.Nombre}','{cine.Direccion}',{cine.Zona.IdZona},{cine.Ventas}");
                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        
        public static ML.Result Update(ML.Cine cine)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.IvbetoCineContext context = new DL.IvbetoCineContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"CineUpdate {cine.IdCine},'{cine.Nombre}','{cine.Direccion}',{cine.Zona.IdZona},{cine.Ventas}");
                    if(query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public static ML.Result Delete(int idCine)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.IvbetoCineContext context = new DL.IvbetoCineContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"CineDelete {idCine}");
                    if(query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            using (DL.IvbetoCineContext context = new DL.IvbetoCineContext())
            {
                var query = context.Cines.FromSqlRaw($"CineGetAll").ToList();
                if(query != null)
                {
                    result.Objects = new List<object>();
                    foreach(var obj in query)
                    {
                        ML.Cine cine = new ML.Cine();
                        cine.Zona = new ML.Zona();

                        cine.IdCine = obj.IdCine;
                        cine.Nombre = obj.Nombre;
                        cine.Direccion = obj.Direccion;
                        cine.Ventas = int.Parse(obj.Ventas.ToString());
                        cine.Zona.IdZona = int.Parse(obj.IdZona.ToString());
                        cine.Zona.Nombre = obj.NombreZona;

                        result.Objects.Add(cine);
                    }
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se encontraron registros en la tabla";
                }
                return result;
            }
        }

        public static ML.Result GetById(int idCine)
        {
            ML.Result result = new ML.Result();
            using (DL.IvbetoCineContext context = new DL.IvbetoCineContext())
            {
                var query = context.Cines.FromSqlRaw($"CineGetById {idCine}");
                if( query != null)
                {
                    result.Object = new object();
                    foreach(var item in query)
                    {
                        ML.Cine cine = new ML.Cine();
                        cine.Zona = new ML.Zona();

                        cine.IdCine = item.IdCine;
                        cine.Nombre = item.Nombre;
                        cine.Direccion = item.Direccion;
                        cine.Ventas = int.Parse(item.Ventas.ToString());
                        cine.Zona.IdZona = int.Parse(item.IdZona.ToString());
                        cine.Zona.Nombre = item.NombreZona;

                        result.Object = cine;
                    }
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se encontró el elemento seleccionado";
                }
                return result;
            }
        }
    }
}
