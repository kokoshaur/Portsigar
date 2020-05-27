using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using TestPort.Logical;

namespace TestPort.Controllers
{
    public class FlexController : Controller
    {
        [HttpGet]
        public JsonResult GetNextNum(int data)
        {
            return Json(Saver.Show());
        }
        [HttpGet]
        public JsonResult IsOpen(int data)
        {
            return Json(Saver.isOpen());
        }
        [HttpGet]
        public JsonResult GetSubj(int data)
        {
            return Json(Saver.GetSubj());
        }
        [HttpGet]
        public JsonResult WifiIsReady(int data)
        {
            return Json(Saver.wiFiAsync());
        }
        [HttpGet]
        public JsonResult IsFirstStart(int data)
        {
            return Json(Saver.isFirst());
        }
        [HttpPost]
        public Object RefreshWifi()
        {
            var syncIOFeature = HttpContext.Features.Get<IHttpBodyControlFeature>();
            syncIOFeature.AllowSynchronousIO = true;
            StreamReader sr = new StreamReader(Request.Body);
            string data = sr.ReadToEnd();
            string[] words = data.Split(new char[] {'#'});

            return Json(Saver.CreateWifi(words[0], words[1]));
        }
        [HttpPost]
        public Object RefreshSigar()
        {
            var syncIOFeature = HttpContext.Features.Get<IHttpBodyControlFeature>();
            syncIOFeature.AllowSynchronousIO = true;
            StreamReader sr = new StreamReader(Request.Body);
            string data = sr.ReadToEnd();

            return Json(Saver.Refresh(Convert.ToInt32(data)));
        }
    }
}
