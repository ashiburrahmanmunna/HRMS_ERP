using GTERP.Interfaces.HR;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Repository.HR
{
    public class ProductionRepository : IProductionRepository
    {
        private readonly GTRDBContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public ProductionRepository(GTRDBContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public void CreateProduction(string GridDataList)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var pcName = _httpContext.HttpContext.Session.GetString("pcname");
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            var dateadded = DateTime.Now.ToString("dd-MMM-yyyy");
            var jobject = new JObject();
            var d = JObject.Parse(GridDataList);
            string objct = d["GridDataList"].ToString();
            var model = JsonConvert.DeserializeObject<List<ProdGrid>>(objct);
            foreach (var item in model)
            {
                var f = _context.HR_Production.Where(x => x.ComId == comid && x.EmpId == item.EmpId && x.dtInput == item.dtPunchDate && x.StyleId == item.StyleId).ToList();
                if (f.Count() > 0)
                {
                    _context.HR_Production.RemoveRange(f);
                    _context.SaveChanges();
                }
                var style = _context.Cat_Style.Where(x => x.ComId==comid).ToList();
                var prob = new HR_Production();
                
                var production = _context.Cat_Style.Where(x => x.StyleId == item.StyleId &&
                      x.StyleDate == item.dtPunchDate && x.ComId == comid).Select(x => x.Rate)
                       .FirstOrDefault();

                var fixedRate = _context.Cat_Style.Where(x => x.StyleId == item.StyleId && x.IsFixedRate==true
                       && x.ComId == comid)
                       .FirstOrDefault();

                prob.ComId = comid;
                prob.EmpId = item.EmpId;
                prob.UserId = userid;
                prob.dtInput = item.dtPunchDate;
                prob.StyleId = Convert.ToInt16(item.StyleId);
                prob.Quantity = item.Quantity;
                prob.Remarks = item.Remarks;
                prob.PcName = pcName;
                
                if(production>0)
                {
                    prob.Amount = prob.Quantity * production;
                }
                else if(fixedRate.IsFixedRate==true)
                {
                    prob.Amount = prob.Quantity * fixedRate.Rate;
                }
                prob.UpdateByUserId = userid;
                prob.DateAdded = DateTime.Parse(dateadded);
                prob.DateUpdated = DateTime.Parse(dateadded);
                _context.HR_Production.Add(prob);
                _context.SaveChanges();
            }
        }

        public List<ProdGrid> GetProduction(string DtFrom, string DtTo, string criteria, string value)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");

            var quary = $"EXEC HR_prcGetProduction '1','{comid}','{criteria}','{value}','{DtFrom}','{DtTo}'";

            SqlParameter[] sqlParameter = new SqlParameter[6];
            sqlParameter[0] = new SqlParameter("@Id", "1");
            sqlParameter[1] = new SqlParameter("@ComId", comid);
            sqlParameter[2] = new SqlParameter("@OptCriteria", criteria);
            sqlParameter[3] = new SqlParameter("@value", value);
            sqlParameter[4] = new SqlParameter("@dtfrom", DtFrom);
            sqlParameter[5] = new SqlParameter("@dtto", DtTo);

            var listOfProd = Helper.ExecProcMapTList<ProdGrid>("HR_prcGetProduction", sqlParameter);
            return listOfProd;
        }

        public void UpdateProduction(string GridDataList)
        {
            var comid = _httpContext.HttpContext.Session.GetString("comid");
            var pcName = _httpContext.HttpContext.Session.GetString("pcname");
            var userid = _httpContext.HttpContext.Session.GetString("userid");
            if (GridDataList != null)
            {
                var jobject = new JObject();
                var d = JObject.Parse(GridDataList);
                string objct = d["GridDataList"].ToString();
                var model = JsonConvert.DeserializeObject<List<ProdGrid>>(objct);

                using (var tr = _context.Database.BeginTransaction())
                {
                    foreach (var aGridData in model)
                    {

                        try
                        {
                            #region update HR_Production working


                            var fixAtt = new HR_Production();
                            var processData = new HR_ProcessedData();

                            fixAtt.ComId = comid;
                            fixAtt.EmpId = aGridData.EmpId;
                            fixAtt.dtInput = Convert.ToDateTime(aGridData.dtPunchDate);

                            fixAtt.StyleId = Convert.ToInt16(aGridData.StyleId);// stId;
                                                                                //fixAtt.StyleName = aGridData.StyleName;// stId;

                            fixAtt.Quantity = aGridData.Quantity;
                            fixAtt.Remarks = aGridData.Remarks;
                            fixAtt.PcName = pcName;

                            _context.HR_Production.Add(fixAtt);
                            _context.SaveChanges();
                            #endregion



                            #region Method 1 to update ProcessData
                            var prosData = _context.HR_ProcessedData.Where(x => x.EmpId == aGridData.EmpId && x.DtPunchDate.Date == aGridData.dtPunchDate.Date && x.ComId == comid).FirstOrDefault();
                            if (prosData != null)
                            {
                                prosData.DtPunchDate = aGridData.dtPunchDate;
                                prosData.Remarks = aGridData.Remarks;

                                _context.Entry(prosData).State = EntityState.Modified;
                                _context.SaveChanges();
                            }
                            #endregion




                        }
                        catch (SqlException ex)
                        {

                            Console.WriteLine(ex.Message);
                            tr.Rollback();

                        }
                    }
                    tr.Commit();
                }


            }
        }
    }
}
