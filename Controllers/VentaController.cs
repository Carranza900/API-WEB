using Integrador.Services;
using Microsoft.AspNetCore.Mvc;
using Integrador.Models;
using System.Collections.Generic;


namespace Integrador.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentaController: ControllerBase
    {
        private readonly VentaService _VentaService;

        public VentaController(VentaService ventaService)
        {
            _VentaService = ventaService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Ventas>> GetVentas()
        {
            var ventas = _VentaService.GetVentas();
            if (ventas == null)
            {
                return NotFound();
            }
            
            return Ok(ventas);
        }

        [HttpPost]
        public ActionResult<int> PostVenta(Ventas venta)
        {
            _VentaService.CrearVenta(venta);
            return CreatedAtAction(nameof(GetVentas), new { id = venta.ID_Venta }, venta);
        }

    }
}
