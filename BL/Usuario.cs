using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Usuario
    {
        public static string ComputeSHA256(string s)
		{
			string hash = String.Empty;

			// Initialize a SHA256 hash object
			using (SHA256 sha256 = SHA256.Create())
			{
				// Compute the hash of the given string
				byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(s));

				// Convert the byte array to string format
				foreach (byte b in hashValue)
				{
					hash += $"{b:X2}";
				}
			}

			return hash;
		}
		public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.IvbetoCineContext context = new DL.IvbetoCineContext())
                {
                    usuario.Password = ComputeSHA256(usuario.Password);
                    var query = context.Database.ExecuteSqlRaw($"UsuarioAdd '{usuario.Username}','{usuario.Nombre}','{usuario.Email}','{usuario.Password}'");
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

        public static ML.Result GetByEmail(string email)
        {
            ML.Result result = new ML.Result();
            try
            {
				using (DL.IvbetoCineContext context = new DL.IvbetoCineContext())
				{
                    //usuario.Password = ComputeSHA256(usuario.Password);
                    var query = context.Usuarios.FromSqlRaw($"UsuarioGetByEmail '{email}'").AsEnumerable().SingleOrDefault();
                    result.Object = new object();
                    if(query != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();
                        usuario.Nombre = query.Nombre;
                        usuario.Username = query.Username;
                        usuario.Email = query.Email;
                        usuario.Password = query.Password;

                        result.Object = usuario;
                        result.Correct = true;
                    }
                    else
                    {
                        result.ErrorMessage = "No existe ese usuario en el sistema";
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

        public static ML.Result UpdatePassword(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.IvbetoCineContext context = new DL.IvbetoCineContext())
                {
                    usuario.Password = ComputeSHA256(usuario.Password);
                    var query = context.Database.ExecuteSqlRaw($"UsuarioUpdatePassword '{usuario.Email}','{usuario.Password}'");
                    if(query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.ErrorMessage = "No se ha podido actualizar tu contraseña, ha ocurrido un error";
                        result.Correct = false;
                    }
                }
            }catch(Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }
    }
}
