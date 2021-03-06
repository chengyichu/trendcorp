﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrendCorp.Models;

namespace TrendCorp.Controllers
{
    public class ProductController : Controller
    {
        private TrendsysEntities db = new TrendsysEntities();

        // GET: PaPrs
        public ActionResult Index()
        {
            return View(db.PaPrs.Take(50).ToList());
        }

        private string GetProductPicturePath(PaPr product)
        {
            var fileExtension = "." + System.Configuration.ConfigurationManager.AppSettings["PicExtend"];
            var fileName = !string.IsNullOrEmpty(product.PicFileName) ? product.PicFileName : product.PartNo;
            var path1 = System.Configuration.ConfigurationManager.AppSettings["PicPath1"] + fileName.Substring(0,2) + "-/"+ fileName.Substring(0, 3) + "/" + fileName + fileExtension;
            var path2 = System.Configuration.ConfigurationManager.AppSettings["PicPath2"] + fileName.Substring(0, 2) + "-/" + fileName.Substring(0, 3) + "/" + fileName + fileExtension;
            var path3 = System.Configuration.ConfigurationManager.AppSettings["PicPath3"] + fileName.Substring(0, 2) + "-/" + fileName.Substring(0, 3) + "/" + fileName + fileExtension;

            if (System.IO.File.Exists(HttpContext.Server.MapPath(path1))) {
                return path1;
            }
            else if (System.IO.File.Exists(HttpContext.Server.MapPath(path2))) {
                return path2;
            }
            else if (System.IO.File.Exists(HttpContext.Server.MapPath(path3))) {
                return path3;
            }
            else
            {
                return null;
            }
            
        }
        public JsonResult GetProduct(string barcode)
        {
            var product = db.PaPrs.FirstOrDefault(i => i.BarCode == barcode);
            
            if (product == null)
            {
                return Json(new { status = false, message = "Can't find the product." }, JsonRequestBehavior.AllowGet);
            }

            var picturePath = GetProductPicturePath(product);
            var productVm = new ProductVm
            {
                ImagePath = GetProductPicturePath(product),
                Product = product
            };
            return Json(productVm, JsonRequestBehavior.AllowGet);
        }


        // GET: PaPrs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaPr paPr = db.PaPrs.Find(id);
            if (paPr == null)
            {
                return HttpNotFound();
            }
            return View(paPr);
        }

        // GET: PaPrs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaPrs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,BarCode,PartNo,Descr,DescrCN,PicFileName,CPartNo,Species,ISize,ColorKind,Plating,Material,IShape,MaterialExp,StoneCode,StoneSize,StoneShape,StoneQtys,WasteRate,HumanExp,CostZm,CostPg,CostDd,LevelZm,IUnit,CurrencySymbol,UnitPrice,Weight,WeightMemo,INote,EditMemo,ModelNo,CreateDate,CheckPriceDt,SilverWeight,EditDateDd,EditDate,CreateEmployeeCode,EditEmployeeCode,MadeSupply,SilverLoss,SettingCost,PolishingCost,MountingCost,PlatingCost,StoneCost,TotalProngs,LoppingCost,CostST,CostPA,CostDM,CostXS,CostNS,CostSH,CostOT,CostRemark,CostCrDate,CostEdDate,CostCrEmpCode,CostEdEmpCode,CostPrice,WeightCS,WeightPA,WeightST,CreatePriceDt,UnitPrice08,NoMemo,CheckDate,CheckEmployeeCode,Checked,WorkPriceZM,WorkPricePG,WorkPriceCheckDt,QuNoZm,QuNoPg,SetNo1,SetNo2,SetNo3,NeedNewPic")] PaPr paPr)
        {
            if (ModelState.IsValid)
            {
                db.PaPrs.Add(paPr);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(paPr);
        }

        // GET: PaPrs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaPr paPr = db.PaPrs.Find(id);
            if (paPr == null)
            {
                return HttpNotFound();
            }
            return View(paPr);
        }

        // POST: PaPrs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,BarCode,PartNo,Descr,DescrCN,PicFileName,CPartNo,Species,ISize,ColorKind,Plating,Material,IShape,MaterialExp,StoneCode,StoneSize,StoneShape,StoneQtys,WasteRate,HumanExp,CostZm,CostPg,CostDd,LevelZm,IUnit,CurrencySymbol,UnitPrice,Weight,WeightMemo,INote,EditMemo,ModelNo,CreateDate,CheckPriceDt,SilverWeight,EditDateDd,EditDate,CreateEmployeeCode,EditEmployeeCode,MadeSupply,SilverLoss,SettingCost,PolishingCost,MountingCost,PlatingCost,StoneCost,TotalProngs,LoppingCost,CostST,CostPA,CostDM,CostXS,CostNS,CostSH,CostOT,CostRemark,CostCrDate,CostEdDate,CostCrEmpCode,CostEdEmpCode,CostPrice,WeightCS,WeightPA,WeightST,CreatePriceDt,UnitPrice08,NoMemo,CheckDate,CheckEmployeeCode,Checked,WorkPriceZM,WorkPricePG,WorkPriceCheckDt,QuNoZm,QuNoPg,SetNo1,SetNo2,SetNo3,NeedNewPic")] PaPr paPr)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paPr).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(paPr);
        }

        // GET: PaPrs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaPr paPr = db.PaPrs.Find(id);
            if (paPr == null)
            {
                return HttpNotFound();
            }
            return View(paPr);
        }

        // POST: PaPrs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PaPr paPr = db.PaPrs.Find(id);
            db.PaPrs.Remove(paPr);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
