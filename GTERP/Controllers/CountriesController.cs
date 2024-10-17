using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using GTCommercial.Models;

namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class CountriesController : Controller
    {
        private GTRDBContext db = new GTRDBContext();

        // GET: Countries
        public ActionResult Index()
        {
            //string a = "ক্যামব্রিয়ান কলেজে একাদশ শ্রেণিতে ভর্তি চলছে শিক্ষা আমাদের অঙ্গীকার, সাফল্য আমাদের প্রেরণ া ভর্তি ফি: ৩০০০/ - মাসিক বেতন: ৩০০০ - ৬০০০/ - 01762688180";
            //int z = (a.Length);

            //string asdf = "ক্যামব্রিয়ান কলেজে একাদশ শ্রেণিতে ভর্তি চলছে শিক্ষা আমাদের অঙ্গীকার, সাফল্য আমাদের প্রেরণ া ভর্তি ফি: ৩০০০/ - মাসিক বেতন: ৩০০০ - ৬০০০/ - 01762688180";



            //double x =CalculateSmsLength(asdf);
            //double y =  (asdf.Length) * 2.865;
            //fahad();
            return View(db.Countries.ToList());
        }

        public int CalculateSmsLength(string text)
        {



            if (IsEnglishText(text))
            {
                return text.Length <= 160 ? 1 : Convert.ToInt32(Math.Ceiling(Convert.ToDouble(text.Length) / 153));
            }

            return text.Length <= 70 ? 1 : Convert.ToInt32(Math.Ceiling(Convert.ToDouble(text.Length) / 67));
        }

        public bool IsEnglishText(string text)
        {
            return Regex.IsMatch(text, @"^[\u0000-\u007F]+$");
        }

        public void fahad() {


            int[] a = new int[] { 10, 2, 3, 1, 5, 3, 1, 4, -4 - 3 - 2 };
            int[] b = a.Skip(1).ToArray();
            int ii = 0;
            int sum = 0;
            int firstvalue = a[0];
            int runninglength = 0;
            Random random = new Random();

            //foreach (int row in a)
            //{
            //    Console.WriteLine(row);
            //}
            // Loop over array of integers.

            for (int x = 0; x < a.Length; x++)
            {
                runninglength++;
                if (x != 0)
                {
                    sum = 0;
                    ii = 0;
                    for (int i = x; i < a.Length; i++)
                    {

                        if (i != 0)
                        {
                            if (ii < 3)
                            {
                                int takevalueforsum = random.Next(0, b.Length);
                                sum = sum + takevalueforsum;// a[i];
                                ii++;
                                //Console.WriteLine("Orginal Value : " + a[i].ToString());
                                Console.WriteLine("sum : " + sum.ToString());
                                Console.WriteLine("Firstvalue : " + firstvalue.ToString());

                            }

                            if (ii == 3)
                            {
                                if (firstvalue == sum)
                                {

                                    Console.WriteLine("true");

                                }
                                else
                                {

                                    Console.WriteLine("sum : " + sum.ToString());
                                    Console.WriteLine("false");
                                    i = x;
                                    break;
                                }


                            }
                        }
                    }
                }
            }





            //if (a.Length != 0)
            //{
            //    Console.WriteLine('test');
            //    i++;

            //}
        }

        // GET: Countries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Country country = db.Countries.Find(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // GET: Countries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*Include =*/ "CountryCode,CountryName,CurrencyName,CountryShortName,CultureInfo,CurrencySymbol,CurrencyShortName,flagclass,DialCode")] Country country)
        {
            if (ModelState.IsValid)
            {
                db.Countries.Add(country);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(country);
        }

        // GET: Countries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Country country = db.Countries.Find(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(/*Include =*/ "CountryCode,CountryName,CurrencyName,CountryShortName,CultureInfo,CurrencySymbol,CurrencyShortName,flagclass,DialCode")] Country country)
        {
            if (ModelState.IsValid)
            {
                db.Entry(country).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(country);
        }

        // GET: Countries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Country country = db.Countries.Find(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Country country = db.Countries.Find(id);
            db.Countries.Remove(country);
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
