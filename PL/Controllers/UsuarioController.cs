using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using System.Net.Mail;
using System.Web;
using static System.Net.WebRequestMethods;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Mail()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(ML.Usuario usuario)
        {
            ML.Result result = BL.Usuario.GetByEmail(usuario.Email);
            if (!result.Correct)  //Add
            {
                ML.Result resultAdd = BL.Usuario.Add(usuario);
                if (resultAdd.Correct)
                {
                    ViewBag.Message = "Usuario creado de forma exitosa, ya puede ingresar al sistema";
                }
                else
                {
                    ViewBag.Message = "Ha habido un error al crear el usuario, el correo ya ha sido utilizado para crear antes.";
                }
            }
            return PartialView("Modal");
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Email = email;
            usuario.Password = BL.Usuario.ComputeSHA256(password);
            ML.Result result = BL.Usuario.GetByEmail(usuario.Email);
            if (result.Correct)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "Usuario incorrecto, error en alguno de los campos: " + result.ErrorMessage;
                return PartialView("Modal");
            }
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpGet]
        public IActionResult NewPassword(string email)
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Email = email;
            return View(usuario);
        }

        [HttpPost]
        public ActionResult EnviarMail(string email)
        {
            //llamar al metodo
            string emailOrigen = "ivan16beto@gmail.com";
            string correo = $"<h5 style=\"color: darkblue; text-align: center; font-size: 25px;\">Cambio de la Contraseña</h5>"
                +
                $"<br />"+
                $"<h6 style=\"font-weight: bolder; text-align: center; font-size: 16px;\">Correo de recuperación de contraseña</h6>" +
                $"<br />" +
                $"<br />" +
                $"<div style=\"padding: 30px; font-size: 18px;\">" +
                $"<p>\r\n        Se te está proporcionando una página para que puedas cambiar tu contraseña de forma que la anterior no se usará más, cambiandola por la que\r\nescribas ahora. Por lo que te pedimos tengas de antemano anotada tu nueva contraseña antes de continuar.\r\n    </p>" +
                $"<p>Una vez listo para seguir, por favor haz click en el link de abajo llamado 'Cambio de contraseña' para dirigirte a cambiar tu contraseña y puedas iniciar sesión de nuevo.</p>" +
            $" <br />" +
                $"<center><a href=" + "http://192.168.0.150/Usuario/NewPassword?email=" + email +">Cambio de contraseña</a></center>" +
                $"<br />" +
                $"<br />" +
                $"<span>Agradecemos de antemando el uso del sistema para tus peliculas favoritas y tu dulceria de preferencia.</span>" +
                $"<h6>Atte. Equipo de Soporte Tecnico Movies-Project.</h6>" +
                $"<h6>2023</h6>" +
                $"</div>";
            MailMessage mailMessage = new MailMessage(emailOrigen, email, "Recuperar Contraseña",correo);
            mailMessage.IsBodyHtml = true;

            string url = "http://localhost:5214/Usuario/NewPassword/"+email + HttpUtility.UrlEncode(email);
            //string url = "http://192.168.0.104/Usuario/NewPassword/" + HttpUtility.UrlEncode(email);
            mailMessage.Body = mailMessage.Body.Replace("{Url}", url);
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new System.Net.NetworkCredential(emailOrigen, "nukgwtdkbjlrbaod");

            smtpClient.Send(mailMessage);
            smtpClient.Dispose();

            //ViewBag.Modal = "show";
            ViewBag.Mensaje = "Se ha enviado un correo de confirmación a tu correo electronico";
            return PartialView("Modal");
        }

        [HttpPost]
		public IActionResult ForgotPassword(string email)
		{
			ML.Usuario usuario = new ML.Usuario();
			usuario.Email = email;
			ML.Result resultEmail = BL.Usuario.GetByEmail(usuario.Email);
            if (resultEmail.Correct)
            {
				EnviarMail(usuario.Email);
				ViewBag.Message = "Se ha enviado un mail a tu correo con indicaciones para una nueva contraseña";
			}
            else
            {
                ViewBag.Message = "No existe tu usuario en el sistema, acceso de negado";
            }
			return PartialView("Modal");
		}

        [HttpPost]
        public IActionResult NewPassword(ML.Usuario usuario)
        {
            ML.Result result = BL.Usuario.GetByEmail(usuario.Email);
            if (result.Correct)
            {
                ML.Result updatePass = BL.Usuario.UpdatePassword(usuario);
                if (updatePass.Correct)
                {
                    ViewBag.Message = "Tu contraseña ha sido actualizada, puedes volver a iniciar sesión.";
                }
                else
                {
                    ViewBag.Message = "No se ha podido actualizar tu contraseña, ha ocurrido un error: " + updatePass.ErrorMessage;
                }
            }
            return PartialView("Modal");
        }
	}
}
