using mcvhoconline.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mcvhoconline.Controllers
{
    [SessionAuthorizeAdmin]
    public class AdminController : Controller
    {
        private ShopEntities5 _ShopContext;
        public AdminController()
        {
            _ShopContext = new ShopEntities5();
        }


        // GET: Admin
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/DanhSachSanPham
        [HttpGet]
        public ActionResult List()
        {
            var loais = _ShopContext.products.ToList();

            string paramCategory = HttpContext.Request.QueryString["category"];
            string paramName = HttpContext.Request.QueryString["name"];

            var sps = _ShopContext.products.Include("category").ToList();
            if (!string.IsNullOrEmpty(paramCategory))
            {
                //sps = _ShopContext.products.Include("Loai").ToList().Where(product => product.== paramLoai).ToList();
            }
            if (!string.IsNullOrEmpty(paramName))
            {
                //sps = sps.ToList().Where(sp => sp.TenSP.Contains(paramTen)).ToList();
            }
            //var viewModel = new SP_View_Model { Loais = loais, SanPhams = sps };
            return View(paramName);
        }


        [HttpPost]
        public ActionResult CreateAction(product prod, HttpPostedFileBase UrlImage)
        {
            try
            {
                if (UrlImage != null && UrlImage.ContentLength > 0)
                {
                    var imgPath = Server.MapPath("~/Content/Product/Image");
                    if (!Directory.Exists(imgPath))
                    {
                        Directory.CreateDirectory(imgPath);
                    }
                    var fileName = Path.Combine(imgPath, Path.GetFileName(UrlImage.FileName));

                    UrlImage.SaveAs(fileName);
                    prod.imageUrl = UrlImage.FileName;
                }
                _ShopContext.products.Add(prod);
                _ShopContext.SaveChanges();
                return RedirectToAction("/products");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return RedirectToAction("/add-product");
        }


        [HttpPost]
        public ActionResult EditAction(product prod, HttpPostedFileBase UrlImage)
        {
            try
            {
                string queryProdId = HttpContext.Request.QueryString["MaSP"];

                if (queryProdId.Length > 0)
                {
                    var sanPham = _ShopContext.products.ToList().Where(product => product.id == queryProdId)?.First();
                    if (sanPham == null)
                    {
                        return Redirect("/Admin/list-products");
                    }

                    if (UrlImage != null && UrlImage.ContentLength > 0)
                    {
                        var imgPath = Server.MapPath("~/Content/Product/Image");
                        if (!Directory.Exists(imgPath))
                        {
                            Directory.CreateDirectory(imgPath);
                        }
                        var fileName = Path.Combine(imgPath, Path.GetFileName(UrlImage.FileName));

                        UrlImage.SaveAs(fileName);
                        sanPham.imageUrl = UrlImage.FileName;
                    }

                    sanPham.name = prod.name;
                    sanPham.description = prod.description;
                    sanPham.price = prod.price;
                    
                    _ShopContext.SaveChanges();
                    return RedirectToAction("/Admin/list-proucts");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return RedirectToAction("/Admin/add-product");
        }

        // GET: Admin/SuaSanPham
        [HttpGet]
        public ActionResult Edit()
        {
            //var loais = _ShopContext.Loais.ToList();

            string queryProdId = HttpContext.Request.QueryString["id"].ToString();
            var prod = _ShopContext.products.ToList().Where(product => product.id == queryProdId)?.First();
            if (prod == null)
            {
                return Redirect("/Admin/DanhSachSanPham");
            }
            //var viewModel = new Sua_SP_View_DTO { Loais = loais, SanPham = sanPham };
            return View(prod);
        }

        // GET: Admin/ThemSanPham
        [HttpGet]
        public ActionResult Create()
        {
            //var loais = _ShopContext.Loais.ToList();

            return View();
        }


        // DELETE: Admin/Delete
        [HttpGet]
        public ActionResult Delete()
        {
            string queryProdId = HttpContext.Request.QueryString["id"].ToString();
            var prod = _ShopContext.products.ToList().Where(product => product.id == queryProdId)?.First();
            if(prod == null)
            {
                return Redirect("/Admin/list-products");
            }
            var imgPath = HttpContext.Server.MapPath("~/App_Data/UploadedFiles/");
            var filePathToDelete = Path.Combine(imgPath, Path.GetFileName(prod.imageUrl));
            _ShopContext.products.Remove(prod);
            _ShopContext.SaveChanges();
            try
            {
                // Check if the file exists before attempting to delete
                if (System.IO.File.Exists(filePathToDelete))
                {
                    System.IO.File.Delete(filePathToDelete);
                    ViewBag.Message = "File deleted successfully.";
                }
                else
                {
                    Console.WriteLine("File does not exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file: {ex.Message}");
            }
            return Redirect("/Admin/list-products");
        }
    }
}