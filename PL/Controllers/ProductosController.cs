using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using ML;
using Org.BouncyCastle.Asn1.Pkcs;

namespace PL.Controllers
{
    public class ProductosController : Controller
    {
        public IActionResult Dulceria()
        {
            ML.Producto producto = new ML.Producto();
            producto.Productos = new List<object>();
            ML.Result result = BL.Producto.GetAll();
            if (result.Correct)
            {
                producto.Productos = result.Objects;
            }
            else
            {
                ViewBag.Message = "No hay productos registrados en Dulceria";
            }

            return View(producto);
        }

        public ML.Venta GetCarrito(ML.Venta carrito)
        {
            var sesionVenta = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Compra"));
            foreach (var obj in sesionVenta)
            {
                ML.Producto product = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Producto>(obj.ToString());
                carrito.Carrito.Add(product);
            }
            return carrito;
        }

        //Metodo para la vista del carrito
        public IActionResult Carrito()
        {
            ML.Venta carrito = new ML.Venta();
            carrito.Carrito = new List<object>();

            //Comprobacion si no existe una sesion antes
            if (HttpContext.Session.GetString("Compra") == null)
            {
                return View(carrito);
            }
            else
            {
                GetCarrito(carrito);
                return View(carrito);
            }

        }

        public IActionResult AgregarCarrito(int idProducto)
        {
            bool exist = false;
            ML.Venta carshop = new ML.Venta();
            carshop.Carrito = new List<object>();
            ML.Result result = BL.Producto.GetById(idProducto);
            //Bloque de la sesiones, se comprueba si no existe una sesion
            if (HttpContext.Session.GetString("Compra") == null)
            {
                if (result.Correct)
                {
                    ML.Producto product = (ML.Producto)result.Object;
                    product.Cantidad = 1;
                    carshop.Total = product.Precio;
                    //Se añade al carrito
                    carshop.Carrito.Add(product);
                    //Se serializa el carrito
                    HttpContext.Session.SetString("Compra", Newtonsoft.Json.JsonConvert.SerializeObject(carshop.Carrito));
                }
            }
            else
            {
                ML.Producto product = (ML.Producto)result.Object;
                GetCarrito(carshop); //Se recupera el carrito
                foreach (ML.Producto item in carshop.Carrito)
                {
                    //Si ya existe el producto en el carrito
                    if (product.IdProducto == item.IdProducto)
                    {
                        item.Cantidad += 1;
                        exist = true;
                        break;
                    }
                    else
                    {
                        //No existe en el carrito
                        exist = false;
                    }
                }
                if (exist)
                {
                    HttpContext.Session.SetString("Compra", Newtonsoft.Json.JsonConvert.SerializeObject(carshop.Carrito));
                }
                else
                {
                    product.Cantidad = 1;
                    carshop.Carrito.Add(product);
                    HttpContext.Session.SetString("Compra", Newtonsoft.Json.JsonConvert.SerializeObject(carshop.Carrito));
                }
            }
            return RedirectToAction("Carrito");
        }

        public IActionResult Clear(List<object> ventas)
        {
            if (HttpContext.Session.GetString("Compra") != null)
            {
                HttpContext.Session.Remove("Compra");
            }
            return RedirectToAction("Carrito");
        }

        public IActionResult Delete(int indice, int idProducto)
        {
            ML.Venta carrito = new ML.Venta();
            carrito.Carrito = new List<object>();
            ML.Result result = BL.Producto.GetById(idProducto);
            if (HttpContext.Session.GetString("Compra") != null)
            {
                ML.Producto producto = (ML.Producto)result.Object;
                GetCarrito(carrito);
                foreach (ML.Producto item in carrito.Carrito)
                {
                    if (producto.IdProducto == item.IdProducto)
                    {
                        carrito.Carrito.Remove(item);
                        HttpContext.Session.SetString("Compra", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));
                        break;
                    }
                }
            }
            return RedirectToAction("Carrito");
        }

        public IActionResult CreaPDF()
        {
            ML.Venta venta = new ML.Venta();
            venta.Carrito = new List<object>();
            GetCarrito(venta);

            string tempPath = Path.GetTempFileName() + ".pdf";
            using (PdfDocument documentopdf = new PdfDocument(new PdfWriter(tempPath)))
            {
                using (Document documento = new Document(documentopdf))
                {
                    //Se crea la tabla para la lista de productos comprados
                    documento.Add(new Paragraph("Recibo de Compra " + DateTime.Today.ToString()).SetBackgroundColor(ColorConstants.BLUE).SetBorderRadius(new BorderRadius(5)));
                    iText.Layout.Element.Table tabla = new iText.Layout.Element.Table(5);
                    tabla.SetWidth(UnitValue.CreatePercentValue(100));

                    documento.Add(new Paragraph("Esta es su lista de productos adquiridos en la compra actual: "));

                    //Se añaden los nombres de las columnas (encabezados)
                    tabla.AddHeaderCell("ID Producto");
                    tabla.AddHeaderCell("Nombre del Producto");
                    tabla.AddHeaderCell("Precio Unitario");
                    tabla.AddHeaderCell("Cantidad añadida");
                    tabla.AddHeaderCell("Imagen");

                    foreach (ML.Producto producto in venta.Carrito)
                    {
                        tabla.AddCell(producto.IdProducto.ToString());
                        tabla.AddCell(producto.Nombre);
                        tabla.AddCell(producto.Precio.ToString());
                        tabla.AddCell(producto.Cantidad.ToString());
                        //byte[] imageBytes = Convert.FromBase64String(producto.Imagen);
                        ImageData data = ImageDataFactory.Create(producto.Imagen);
                        Image imagen = new Image(data);
                        tabla.AddCell(imagen.SetWidth(25).SetHeight(25));
                    }

                    //Se agrega la tabla al documento
                    documento.Add(tabla);
                    documento.Add(new Paragraph("El total a pagar por la compra fue de: " + venta.Total));
                    //documento.Add(new Paragraph("Los datos de la forma de pago [Pago con Tarjeta] son: "));
                    //documento.Add(new Paragraph(venta.numeroTarjeta.ToString()));
                    //documento.Add(new Paragraph(venta.fechaCaducidad.ToString()));
                    //documento.Add(new Paragraph(venta.CVV.ToString()));

                }
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(tempPath);

            //Se elimina el archivo temporal
            System.IO.File.Delete(tempPath);
            HttpContext.Session.Remove("Compra");

            return new FileStreamResult(new MemoryStream(fileBytes), "application/pdf")
            {
                FileDownloadName = "ReciboCompra.pdf"
            };
        }
    }
}

