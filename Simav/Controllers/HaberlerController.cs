using App.Common.Filters;
using Microsoft.AspNetCore.Mvc;
using Simav.Common;
using Simav.Core;
using Simav.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simav.Controllers
{
    public class HaberlerController : Controller
    {
        private readonly IService<Haberler> _service;
        public HaberlerController(IService<Haberler> service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            ViewBag.Baslik = "Haber Listesi";
            var haberListesi = _service.GetAll();
            return View(haberListesi);
        }
        [HttpGet]
        [AutFilter]
        public IActionResult YeniHaber()
        {
            ViewBag.Baslik = "Yeni Haber";
            return View();
        }
        [AutFilter]
        [HttpPost]
        public IActionResult YeniHaber(Haberler haber)
        {
            haber.DegistirenKulId = SessionInfo.GirisYapanKullaniciId;
            haber.DegistirmeTarihi = DateTime.Now;
            haber.Durum = (byte)Enums.HaberDurumu.OnayBekliyor;
            haber.KaydedenKulId = SessionInfo.GirisYapanKullaniciId;
            haber.KayıtTarihi = DateTime.Now;
            _service.Save(haber);
            return RedirectToAction("Haberler", "Index");
        }
        [AutFilter]
        [HttpGet]
        public IActionResult HaberDetaylar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var haber = _service.GetById(id.Value);
            if (haber == null)
            {
                return NotFound();//Bulunamadı
            }
            return View(haber);
        }
        [AutFilter]
        [HttpGet]
        public IActionResult HaberGuncelle(int? id)
        {
            ViewBag.Baslik = "Haber Düzenle";
            if (id == null)
            {
                return NotFound();
            }
            var haber = _service.GetById(id.Value);
            if (haber == null)
            {
                return NotFound();//Bulunamadı
            }
            return View(haber);
        }
        [HttpPost]
        public IActionResult HaberGuncelle(Haberler haber)
        {
            if (ModelState.IsValid)
            {
                haber.DegistirenKulId = SessionInfo.GirisYapanKullaniciId;
                haber.DegistirmeTarihi = DateTime.Now;
                _service.Update(haber);
                return RedirectToAction("Index");
            }
            return View(haber);
        }
    }
}
