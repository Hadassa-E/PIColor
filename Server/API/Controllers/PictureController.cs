using BLL;
using Microsoft.AspNetCore.Http;
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PictureController : ControllerBase
    {
        IprocessPictureBLL p;
        public PictureController(IprocessPictureBLL _p)
        {
            p = _p;
        }
        [HttpPost("GetPixelPicture")]
        public ActionResult<string> GetPixelPicture(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("No image file was upload");
            try { 
            using (var stream = new MemoryStream())
            {
                image.CopyTo(stream);
                var bitmap = new Bitmap(stream);
                int[,] mat = p.ProcessPicture(bitmap);
                string json = JsonConvert.SerializeObject(mat);
                return Ok(json);
            }
            }
            catch (Exception ex)
            {
                return BadRequest("the image file was upload");
            }
        }
    }
}
