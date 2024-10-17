using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GTERP.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;




namespace GTCommercial.Controllers
{
    //[OverridableAuthorize]
    public class CitiesController : Controller
    {
        private static GTRDBContext db;
        public CitiesController(GTRDBContext context)
        {
            db = context;
        }
        private int TOTAL_ROWS = countdata();
        private static  List<CityViewModel> citymodellist = new List<CityViewModel>();
        //private static readonly List<CityViewModel> _data = CreateData();

        private static int countdata()
        {
            //using (GTRDBContext db = new GTRDBContext())
            //{
                int TOTAL_ROWS = db.Cities.Count();  //5000;// db.Cities.Count();
                return TOTAL_ROWS;
            //}
          

        }

        //private static List<CityViewModel> CreateData()
        //{
        //    List<City> empList = new List<City>();
        //    using (GTRDBContext db = new GTRDBContext())
        //    {
        //        //db.Configuration.ProxyCreationEnabled = false;
        //        db.Configuration.LazyLoadingEnabled = false;


        //        empList = db.Cities.ToList<City>(); //Include(g => g.vStateCity)
        //        int totalrows = empList.Count;
        //        if (!string.IsNullOrEmpty(searchValue))//filter
        //        {
        //            empList = empList.
        //                Where(x => x.CityId.ToString().Contains(searchValue.ToLower()) || x.CityName.ToLower().Contains(searchValue.ToLower()) || x.CityCode.ToLower().Contains(searchValue.ToLower()))
        //                .ToList<City>();
        //        }
        //        int totalrowsafterfiltering = empList.Count;
        //        //sorting
        //        //empList = empList.OrderBy(sortColumnName + " " + sortDirection).ToList<City>();

        //        //paging
        //        empList = empList.Skip(start).Take(length)
        //        .ToList<City>();

        //        return empList;

        //    }


        //}


        //public class DataItem
        //{
        //    public int CityId { get; set; }
        //    public string CityCode { get; set; }
        //    public int StateId { get; set; }
        //    public string CityName { get; set; }

        //}


        //public class DataTableData
        //{
        //    public int draw { get; set; }
        //    public int recordsTotal { get; set; }
        //    public int recordsFiltered { get; set; }
        //    public List<DataItem> data { get; set; }
        //}



        // GET: Cities
        //[OutputCache(Duration = int.MaxValue)]
        //[OutputCache(Duration = int.MaxValue, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        //public ActionResult Index(string search,int? i)
        //public ActionResult Index()

        //{
        //    var cities = db.Cities.Include(c => c.vStateCity);
        //    //if (search != null)
        //    //{
        //    //    return View(cities.Where(x => x.CityName.StartsWith(search)).ToList().ToPagedList(i ?? 1, 50));
        //    //}
        //    //else
        //    //{
        //    //    return View(cities.ToList().ToPagedList(i ?? 1, 50));
        //    //}

        //    return View(cities.ToList());

        //}
        public ActionResult Index()
        {
            citymodellist.Clear();
            //List<DataItem> list = new List<DataItem>();
            ////db.Configuration.ProxyCreationEnabled = false;
            ////db.Configuration.LazyLoadingEnabled = false;
            //var x  = db.Cities.Take(2000).ToList();
            //TOTAL_ROWS = db.Cities.Take(2000).ToList().Count;
            ////List<DataItem>  _data = new List<DataItem>;

            //foreach (var i in x)
            //{
            //    DataItem item = new DataItem();
            //    item.CityId = i.CityId;
            //    item.CityCode = i.CityCode.ToString();
            //    item.StateId = i.StateId;
            //    item.CityName = i.CityName.ToString();

            //    list.Add(item);
            //}


            //_data = list;
            return View();
        }

        //private int SortString(string s1, string s2, string sortDirection)
        //{
        //    return sortDirection == "asc" ? s1.CompareTo(s2) : s2.CompareTo(s1);
        //}
        //private int SortInteger(string s1, string s2, string sortDirection)
        //{
        //    int i1 = int.Parse(s1);
        //    int i2 = int.Parse(s2);
        //    return sortDirection == "asc" ? i1.CompareTo(i2) : i2.CompareTo(i1);
        //}
        //private int SortDateTime(string s1, string s2, string sortDirection)
        //{
        //    DateTime d1 = DateTime.Parse(s1);
        //    DateTime d2 = DateTime.Parse(s2);
        //    return sortDirection == "asc" ? d1.CompareTo(d2) : d2.CompareTo(d1);
        //}
        //private List<DataItem> FilterData(ref int recordFiltered, int start, int length, string search, int sortColumn, string sortDirection)
        //{
        //    List<DataItem> list = new List<DataItem>();
        //    if (search == null)
        //    {
        //        list = _data;
        //    }
        //    else
        //    {
        //        //db.Configuration.ProxyCreationEnabled = false;
        //        //db.Configuration.LazyLoadingEnabled = false;

        //        // simulate search
        //        foreach (DataItem dataItem in _data)
        //        {
        //            if (dataItem.CityId.ToString().Contains(search.ToUpper()) ||
        //                dataItem.CityCode.ToUpper().Contains(search.ToUpper()) ||
        //                dataItem.StateId.ToString().Contains(search.ToUpper()) ||
        //                dataItem.CityName.ToString().Contains(search.ToUpper()))
        //            {
        //                list.Add(dataItem);
        //            }
        //        }
        //    }

        //    // simulate sort
        //    if (sortColumn == 0)
        //    {// sort Name
        //        list.Sort((x, y) => SortInteger((x.CityId.ToString()), y.CityId.ToString(), sortDirection));
        //    }
        //    else if (sortColumn == 1)
        //    {// sort Age
        //        list.Sort((x, y) => SortString(x.CityCode, y.CityCode, sortDirection));
        //    }
        //    else if (sortColumn == 2)
        //    {// sort Age
        //        list.Sort((x, y) => SortInteger(x.StateId.ToString(), y.StateId.ToString(), sortDirection));
        //    }
        //    else if (sortColumn == 3)
        //    {// sort Age
        //        list.Sort((x, y) => SortString(x.CityName, y.CityName, sortDirection));

        //    }

        //    //else if (sortColumn == 2)
        //    //{   // sort DoB
        //    //    list.Sort((x, y) => SortDateTime(x.CityCode, y.CityCode, sortDirection));
        //    //}

        //    recordFiltered = list.Count;

        //    // get just one page of data
        //    list = list.GetRange(start, Math.Min(length, list.Count - start));

        //    return list;
        //}
        //public ActionResult AjaxGetJsonData(int draw, int start, int length)
        //{
        //    string search = Request.QueryString["search[value]"];
        //    int sortColumn = -1;
        //    string sortDirection = "asc";
        //    if (length == -1)
        //    {
        //        length = TOTAL_ROWS;
        //    }

        //    // note: we only sort one column at a time
        //    if (Request.QueryString["order[0][column]"] != null)
        //    {
        //        sortColumn = int.Parse(Request.QueryString["order[0][column]"]);
        //    }
        //    if (Request.QueryString["order[0][dir]"] != null)
        //    {
        //        sortDirection = Request.QueryString["order[0][dir]"];
        //    }

        //    DataTableData dataTableData = new DataTableData();
        //    dataTableData.draw = draw;
        //    dataTableData.recordsTotal = TOTAL_ROWS;
        //    int recordsFiltered = 0;
        //    dataTableData.data = FilterData(ref recordsFiltered, start, length, search, sortColumn, sortDirection);
        //    dataTableData.recordsFiltered = recordsFiltered;

        //    return Json(dataTableData, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public ActionResult GetList()
        {
            //Server Side Parameter
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            List<City> empList = new List<City>();
            using (GTRDBContext db = new GTRDBContext())
            {
                //db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;


                empList = db.Cities.ToList<City>(); //Include(g => g.vStateCity)
                int totalrows = empList.Count;
                if (!string.IsNullOrEmpty(searchValue))//filter
                {
                    empList = empList.
                        Where(x => x.CityId.ToString().Contains(searchValue.ToLower()) || x.CityName.ToLower().Contains(searchValue.ToLower()) || x.CityCode.ToLower().Contains(searchValue.ToLower()))
                        .ToList<City>();
                }
                int totalrowsafterfiltering = empList.Count;
                //sorting
                //empList = empList.OrderBy(sortColumnName + " " + sortDirection).ToList<City>();

                //paging
                empList = empList.Skip(start).Take(length)
                    //   .Select(x => new { x.CityId, x.CityCode, x.CityName, x.StateId, x.vStateCity.StateName,}).ToList();
                    //    .ToList<City>();
                .ToList<City>();

                //var dataset = db.Cities.Skip(start).Take(length)
                //                 .Select(x => new { x.CityId, x.CityName, x.CityCode , x.StateId , x.vStateCity.StateName})
                // .ToList() /// To get data from database
                // .Select(y => new State()
                // {

                //     StateName = y.StateName,

                // });


                return Json(new { data = empList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }


        }


        [HttpPost]
        public ActionResult GetListViewModel()
        {
            //db.Configuration.LazyLoadingEnabled = false;

            //var result = db.Countries
            //.Include(a => a.vStateCountry.Select(f => f.StateName))
            //.Where(a=> a.CountryId == 18);


            //Server Side Parameter
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];


            List<City> empList = new List<City>();




             //.Include(p => p.ProgramFoods.Select(f => f.Food))


            using (GTRDBContext db = new GTRDBContext())
            {
                //db.Configuration.ProxyCreationEnabled = false;


                empList = db.Cities.Take(TOTAL_ROWS).ToList<City>(); //Include(g => g.vStateCityViewModel)

                if (citymodellist.Count == 0)
                {
                    foreach (var item in empList)
                    {
                        CityViewModel cityview = new CityViewModel();
                        cityview.CityViewId = item.CityId;
                        cityview.CityCode = item.CityCode;
                        cityview.CityName = item.CityName;
                        cityview.CountryName = item.vStateCity.vStateCountry.CountryName;
                        cityview.StateName = item.vStateCity.StateName;

                        citymodellist.Add(cityview);

                    }
                }
                //db.Configuration.LazyLoadingEnabled = false;


                List<CityViewModel> citymodellista = new List<CityViewModel>();
                citymodellista = citymodellist;
                int totalrows = citymodellist.Count;
                if (!string.IsNullOrEmpty(searchValue))//filter
                {
                    citymodellista = citymodellista.Where(x => x.CityViewId.ToString().Contains(searchValue.ToLower()) || x.CityCode.ToLower().Contains(searchValue.ToLower()) || x.CityName.ToLower().Contains(searchValue.ToLower()) || x.StateName.ToLower().Contains(searchValue.ToLower()) || x.CountryName.ToLower().Contains(searchValue.ToLower()))
                        .ToList<CityViewModel>();
                }
                int totalrowsafterfiltering = empList.Count;
                //sorting
                //empList = empList.OrderBy(sortColumnName + " " + sortDirection).ToList<CityViewModel>();

                //paging
                citymodellista = citymodellista.Skip(start).Take(length)
                //   .Select(x => new { x.CityViewModelId, x.CityViewModelCode, x.CityViewModelName, x.StateId, x.vStateCityViewModel.StateName,}).ToList();
                //    .ToList<CityViewModel>();
                .ToList<CityViewModel>();

                //var dataset = db.Cities.Skip(start).Take(length)
                //                 .Select(x => new { x.CityViewModelId, x.CityViewModelName, x.CityViewModelCode , x.StateId , x.vStateCityViewModel.StateName})
                // .ToList() /// To get data from database
                // .Select(y => new State()
                // {

                //     StateName = y.StateName,

                // });


                return Json(new { data = citymodellista, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }


        }

        //public ActionResult getlist() {
        //    db.Configuration.ProxyCreationEnabled = false;
        //    db.Configuration.LazyLoadingEnabled = false;
        //    var citylist = db.Cities.Take(10000).ToList();
        //    return Json(new { data = citylist }, JsonRequestBehavior.AllowGet);


        //}


        [HttpPost]
        public ActionResult LoadData(DTParameters param)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            int Count;
            var Result = RetrieveData(param, out Count);
            DTResult<City> result = new DTResult<City>
            {
                draw = param.Draw,
                data = Result,
                recordsFiltered = Count,
                recordsTotal = Count
            };
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.DateFormatString = "dd/MM/yyy";
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string json = JsonConvert.SerializeObject(result, jsSettings);
            return Content(json, "application/json");
        }

        private List<City> RetrieveData(DTParameters Param, out int Count)
        {
            using (var Context = new GTRDBContext())
            {

                Context.Configuration.ProxyCreationEnabled = false;
                Context.Configuration.LazyLoadingEnabled = false;

                Count = Context.Cities.Count();
                var BookData = Context.Cities.Select(s => new City() { CityId = s.CityId, CityCode = s.CityCode, StateId = s.StateId, CityName = s.CityName}).AsQueryable();
                //Global Text Search
                var GlobalSearchFilteredData = BookData.ToGlobalSearchInAllColumn(Param);
                //Search Text in Specific or Individual
                var IndividualColSearchFilteredData = GlobalSearchFilteredData.ToIndividualColumnSearch(Param);
                //Sorting order
                var SortedFilteredData = IndividualColSearchFilteredData.ToSorting(Param);
                //Apply Pagination (Taking N number by page size)
                var SortedData = SortedFilteredData.ToPagination(Param).ToList();
                return SortedData;
            }
        }

        // GET: Cities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            City city = db.Cities.Find(id);
            if (city == null)
            {
                return NotFound();
            }
            return View(city);
        }

        // GET: Cities/Create
        public ActionResult Create()
        {
            ViewBag.StateId = new SelectList(db.States, "StateId", "StateName");
            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(/*Include =*/ "CityId,CityCode,StateId,CityName")] City city)
        {
            if (ModelState.IsValid)
            {
                db.Cities.Add(city);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StateId = new SelectList(db.States, "StateId", "StateName", city.StateId);
            return View(city);
        }

        // GET: Cities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            City city = db.Cities.Find(id);
            if (city == null)
            {
                return NotFound();
            }
            ViewBag.StateId = new SelectList(db.States, "StateId", "StateName", city.StateId);
            return View(city);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(/*Include =*/ "CityId,CityCode,StateId,CityName")] City city)
        {
            if (ModelState.IsValid)
            {
                db.Entry(city).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StateId = new SelectList(db.States, "StateId", "StateName", city.StateId);
            return View(city);
        }

        // GET: Cities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            City city = db.Cities.Find(id);
            if (city == null)
            {
                return NotFound();
            }
            return View(city);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            City city = db.Cities.Find(id);
            db.Cities.Remove(city);
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
