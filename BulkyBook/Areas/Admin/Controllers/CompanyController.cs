using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IWebHostEnvironment _hostEnvironment;

        public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            //_hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Company company = new Company();
            //{
            //    Product=new Product(),
            //    CategoryList = _unitOfWork.Category.GetAll().Select(i=> new SelectListItem { 
            //        Text = i.Name,
            //        Value = i.Id.ToString()
            //    }),
            //    CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
            //    {
            //        Text = i.Name,
            //        Value = i.Id.ToString()
            //    })
            //};
            if (id == null)
            {
                //this is for create
                return View(company);
            }
            //this is for edit
            company = _unitOfWork.Company.Get(id.GetValueOrDefault());
            if (company == null)
            {
                return NotFound();
            }
            return View(company);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                //string webRootPath = _hostEnvironment.WebRootPath;
                //var files = HttpContext.Request.Form.Files;
                //if (files.Count > 0)
                //{
                //    string fileName = Guid.NewGuid().ToString();
                //    var uploads = Path.Combine(webRootPath, @"images\products");
                //    var extenstion = Path.GetExtension(files[0].FileName);

                //    if (productVM.Product.ImageUrl != null)
                //    {
                //        //this is an edit and we need to remove old image
                //        var imagePath = Path.Combine(webRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                //        if (System.IO.File.Exists(imagePath))
                //        {
                //            System.IO.File.Delete(imagePath);
                //        }
                //    }
                //    using(var filesStreams = new FileStream(Path.Combine(uploads,fileName+extenstion),FileMode.Create))
                //    {
                //        files[0].CopyTo(filesStreams);
                //    }
                //    productVM.Product.ImageUrl = @"\images\products\" + fileName + extenstion;
                //}
                //else
                //{
                //    //update when they do not change the image
                //    if(productVM.Product.Id != 0)
                //    {
                //        Product objFromDb = _unitOfWork.Product.Get(productVM.Product.Id);
                //        productVM.Product.ImageUrl = objFromDb.ImageUrl;
                //    }
                //}


                if (company.Id == 0)
                {
                    _unitOfWork.Company.Add(company);

                }
                else
                {
                    _unitOfWork.Company.Update(company);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            //else
            //{
            //    productVM.CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
            //    {
            //        Text = i.Name,
            //        Value = i.Id.ToString()
            //    });
            //    productVM.CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
            //    {
            //        Text = i.Name,
            //        Value = i.Id.ToString()
            //    });
            //    if (productVM.Product.Id != 0)
            //    {
            //        productVM.Product = _unitOfWork.Product.Get(productVM.Product.Id);
            //    }
            //}
            return View(company);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Company.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Company.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            //string webRootPath = _hostEnvironment.WebRootPath;
            //var imagePath = Path.Combine(webRootPath, objFromDb.ImageUrl.TrimStart('\\'));
            //if (System.IO.File.Exists(imagePath))
            //{
            //    System.IO.File.Delete(imagePath);
            //}
            _unitOfWork.Company.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion
    }
}