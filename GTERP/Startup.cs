#region Using Derective
using AutoMapper;
using GTERP.BLL;
using GTERP.EF;
using GTERP.Interfaces;
using GTERP.Interfaces.Accounts;
using GTERP.Interfaces.API;
using GTERP.Interfaces.Budget;
using GTERP.Interfaces.Commercial;
using GTERP.Interfaces.ControllerFolder;
using GTERP.Interfaces.HR;
using GTERP.Interfaces.HR_Report;
using GTERP.Interfaces.HRrecruitment;
using GTERP.Interfaces.HRVariables;
using GTERP.Interfaces.Inventory;
using GTERP.Interfaces.Letter;
using GTERP.Interfaces.Payroll;
using GTERP.Interfaces.Payroll_Report;
using GTERP.Interfaces.PF;
using GTERP.Interfaces.Tax;
using GTERP.Interfaces.Tax_Report;
using GTERP.Interfaces.Technicals;
using GTERP.Models;
using GTERP.Models.Common;
using GTERP.Models.Email;
using GTERP.Repository;
using GTERP.Repository.Accounts;
using GTERP.Repository.API;
using GTERP.Repository.Budget;
using GTERP.Repository.Commercial;
using GTERP.Repository.ControllerFolder;
using GTERP.Repository.HR;
using GTERP.Repository.HR_Report;
using GTERP.Repository.HrRecruitment;
using GTERP.Repository.HRVariables;
using GTERP.Repository.Inventory;
using GTERP.Repository.Letter;
using GTERP.Repository.Payroll;
using GTERP.Repository.Payroll_Report;
using GTERP.Repository.PF;
using GTERP.Repository.Tax;
using GTERP.Repository.Tax_Report;
using GTERP.Repository.Technicals;
using GTERP.Services;
using GTERP.ViewModels;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
//using AlanJuden.MvcReportViewer;
#endregion

namespace GTERP
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddDbContext<GTRDBContext>(options =>
            //options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthPolicyProvider>();

            //services.AddSingleton<IAuthorizationHandler, OverridableAuthorize>();
            //services.AddSingleton<Attribute, OverridableAuthorize>();
            //            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            //{
            //    if (!optionsBuilder.IsConfigured)
            //    {
            //        optionsBuilder.UseSqlServer(@"Data Source=MNILAY-ENVY\\SQLEXPRESS;Initial Catalog=TutorialsTeam;Integrated Security=True");
            //    }
            //}





            //services.AddHttpContextAccessor();

            services.AddDbContext<GTERP.Data.ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<GTRDBContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"))/*.EnableSensitiveDataLogging()*/
                    );
            // this Context For HR_RawData_Upload and HR_RawData
            services.AddDbContext<GSRawDataDBContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("GS_RawData")));
            //gs raw data
            services.AddDbContext<GSDeviceDataDBContext>(options =>
              options.UseSqlServer(
                  Configuration.GetConnectionString("GS_DeviceData")));
            //add for salary update--kamrul
            services.AddDbContext<gtrerp_allContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDbContext<GTRDBContext>(options => options.UseSqlServer(
            //    this.Configuration.GetConnectionString("DefaultConnection"),
            //    sqlServerOptions => sqlServerOptions.CommandTimeout(300))
            //);
            services.Configure<FormOptions>(x =>
            {
                x.BufferBody = false;
                x.KeyLengthLimit = 2048; // 2 KiB
                x.ValueLengthLimit = 4194304; // 32 MiB
                x.ValueCountLimit = 2048;// 1024
                x.MultipartHeadersCountLimit = 32; // 16
                x.MultipartHeadersLengthLimit = 32768; // 16384
                x.MultipartBoundaryLengthLimit = 256; // 128
                x.MultipartBodyLengthLimit = 134217728; // 128 MiB
                x.ValueCountLimit = 10000;
            });


            services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;

            })
                .AddEntityFrameworkStores<GTERP.Data.ApplicationDbContext>();
            //services.ConfigureApplicationCookie(op => {
            // //   op.Cookie.Expiration = TimeSpan.FromSeconds(5); 
            //    op.ExpireTimeSpan= TimeSpan.FromSeconds(5);
            //    op.LoginPath = "/Account/LogIn1";
            //});
            services.AddControllersWithViews();
            services.AddSingleton<IConfiguration>(Configuration);
            // IMvcBuilder mvcBuilder

            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            //services.AddScoped<CommercialRepository>();
            services.AddScoped<WebHelper>();
            services.AddScoped<AttendanceRepository>();
            services.AddScoped<HrRepository>();
            services.AddScoped<PayrollRepository>();
            services.AddScoped<clsConnectionNew>();
            services.AddScoped<clsProcedure>();
            services.AddScoped<POSRepository>();
            services.AddScoped<ProductSerialtemp>();
            services.AddScoped<Dashboard>();
            services.AddScoped<DailyAttendanceSum>();
            services.AddScoped<MonthlyAttendance>();
            services.AddScoped<DailyAttendance>();
            services.AddScoped<SalaryDetails>();
            services.AddScoped<EmployeeDetails>();
            services.AddScoped<TransactionLogRepository>();
            services.AddScoped<PermissionLevel>();
            services.AddScoped<RawDataViewRepository>();
            //kamrul
            services.AddScoped<SalaryAdditionRepository>();
            services.AddAutoMapper(typeof(Startup));

            //services.AddTransient<UserLogingInfo>();
            //services.AddScoped<UserLog>();
            //services.AddTransient<AuthorizationFilterContext>();
            //services.AddTransient<ActionContext>();

            //   Repository Service Register 

            #region HRVariable

            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDesignationRepository, DesignationRepository>();
            services.AddScoped<ISectionRepository, SectionRepository>();
            services.AddScoped<IGradeRepository, GradeRepository>();
            services.AddScoped<IBloodGroupRepository, BloodGroupRepository>();
            services.AddScoped<IReligionRepository, ReligionRepository>();
            services.AddScoped<ILineRepository, LineRepository>();
            services.AddScoped<IShiftRepository, ShiftRepository>();
            services.AddScoped<ISkillRepository, SkillRepository>();
            services.AddScoped<IFloorRepository, FloorRepository>();
            services.AddScoped<ICat_UnitRepository, Cat_UnitRepository>();
            services.AddScoped<IBankRepository, BankRepository>();
            services.AddScoped<IBankBranchRepository, BankBranchRepository>();
            services.AddScoped<IBuildingTypeRepository, BuildingTypeRepository>();
            services.AddScoped<IEmpTypeRepository, EmpTypeRepository>();
            services.AddScoped<IBusStopRepository, BusStopRepository>();
            services.AddScoped<IMeetingRepository, MeetingRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IDistrictRepository, DistrictRepository>();
            services.AddScoped<IPoliceStationRepository, PoliceStationRepository>();
            services.AddScoped<IPostOfficeRepository, PostOfficeRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IStyleRepository, StyleRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ISummerWinterAllowanceRepository, SummerWinterAllowanceRepository>();
            services.AddScoped<IComputationSettingRepository, ITComputationSettingRepository>();
            services.AddScoped<IGasChargeSettingRepository, GasChargeRepository>();
            services.AddScoped<ITaxBankRepository, TaxBankRepository>();
            services.AddScoped<IIncenBandRepository, IncenBandRepository>();
            services.AddScoped<IInsureGradeRepository, InsureGradeRepository>();
            services.AddScoped<IExchangeRateRepository, ExchageRateRepository>();
            services.AddScoped<IElectricChargeSettingRepository, ElectricChargeSettingRepository>();
            services.AddScoped<IDiagnosisRepository, DiagnosisRepository>();
            services.AddScoped<ISignatoryRepository, SignatoryRepository>();
            services.AddScoped<IWareHouseRepository, WareHouseRepository>();
            services.AddScoped<IHRExpSettingRepository, HRExpSettingRepository>();
            services.AddScoped<IHRSettingRepository, HRSettingRepository>();
            services.AddScoped<IHRReportRepository, HRReportRepository>();
            services.AddScoped<IHRCustomReportRepository, HRCustomReportRepository>();
            services.AddScoped<IStrengthRepository, StrengthRepository>();
            services.AddScoped<ICatVariableRepository, CatVariableRepository>();
            services.AddScoped<ICatAttStatusRepository, CatAttStatusRepository>();
            services.AddScoped<IGenderRepository, GenderRepository>();
            services.AddScoped<ISubSectionRepository, SubSectionRepository>();
            services.AddScoped<ISizeRepository, SizeRepository>();
            services.AddScoped<IColorRepository, ColorRepository>();
            services.AddScoped<IHROvertimeSettingRepository, HROvertimeSettingRepository>();
            services.AddScoped<ITaxAmountSettingRepository, TaxAmountSettingRepository>();
            services.AddScoped<IHR_ApprovalSettingRepository, HR_ApprovalSettingRepository>();
            services.AddScoped<IIncomeTaxRepository, IncomeTaxRepository>();
            services.AddScoped<ISalaryMonthRepository, SalaryMonthRepository>();
            services.AddScoped<ICat_AttBonusSetting, AttnBonusSetting>();
            services.AddScoped<ICat_Stamp, Cat_StampRep>();
            #endregion 

            #region Company API

            services.AddScoped<ICompanyRepository, CompanyRepository>();

            #endregion

            #region EasyHR API

            services.AddScoped<IEasyHRRepository, EasyHRRepository>();

            #endregion

            #region HR Report

            services.AddScoped<IAllHRReportRepository, AllHRReportRepository>();

            #endregion

            #region Payroll Report
            services.AddScoped<IPayrollReportRepository, PayrollReportRepository>();
            #endregion

            #region Letter
            services.AddScoped<IAbsentLetterRepository, AbsentLetterRepository>();
            #endregion

            #region HR
            services.AddScoped<IHRRepository, HRRepository>();
            services.AddScoped<IBusinessAllowanceRepository, BusinessAllowanceRepository>();
            services.AddScoped<IEmployeeBehaviourRepository, EmployeeBehaviourRepository>();
            services.AddScoped<IEmpShiftInputRepository, EmpShiftInputRepository>();
            services.AddScoped<IIncrementAllRepository, IncrementAllRepository>();
            services.AddScoped<IEmployeeArrearBill, EmployeeArrearBillRepository>();
            services.AddScoped<IEmpShiftInputRepository, EmpShiftInputRepository>();
            services.AddScoped<ILeaveBalanceRepository, LeaveBalanceRepository>();

            services.AddScoped<ILoanHaltRepository, LoanHaltRepository>();
            services.AddScoped<ILoanHouseBuilding, LoanHouseBuildingRepository>();
            services.AddScoped<IEmpInfoRepository, EmpInfoRepository>();
            services.AddScoped<IFixAttRepository, FixAttRepository>();
            services.AddScoped<ITotalNightRepository, TotalNightRepository>();
            services.AddScoped<ITotalFCRepository, TotalFCRepository>();
            services.AddScoped<IEmpReleaseRepository, EmployeeReleasedRepository>();
            services.AddScoped<IIdCardRepository, IdCardRepository>();
            services.AddScoped<IHolidaySetupRepository, HolidaySetupRepository>();
            services.AddScoped<IAttendanceProcessRepository, AttendanceProcessRepository>();
            services.AddScoped<IRawDataViewRepository, RawDataViewRepository>();
            services.AddScoped<IRecreationRepository, RecreationRepository>();
            services.AddScoped<ILoanReturnRepository, LoanReturnRepository>();
            services.AddScoped<IHREmpInfoRepository, HREmpInfoListRepository>();
            services.AddScoped<ILeaveEntryRepository, LeaveEntryRepository>();
            services.AddScoped<IProcessLockRepository, ProcessLockRepository>();
            services.AddScoped<IOTAndNightShiftRepository, OTAndNightShiftRepository>();
            services.AddScoped<IEmployeeSalaryRepository, EmployeeSalaryRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<ISummerWinterAllowRepository, SummerWinterAllowRepository>();
            services.AddScoped<IProductionRepository, ProductionRepository>();
            services.AddScoped<ISupplimentRepository, SupplimentRepository>();
            services.AddScoped<ITotalFCRepository, TotalFCRepository>();
            services.AddScoped<ITotalNightRepository, TotalNightRepository>();
            services.AddScoped<ILoanMotorCycleRepository, LoanMotorCycleRepository>();
            services.AddScoped<ILoanOtherRepository, LoanOtherRepository>();
            services.AddScoped<ILoanPFRepository, LoanPFRepository>();
            services.AddScoped<ILoanWelfareRepository, LoanWelfareRepository>();
            services.AddScoped<ILeaveEncashmentRepository, LeaveEncashmentRepository>();
            services.AddScoped<IIncrementRepository, IncrementRepository>();
            services.AddScoped<ITransferRepository, TransferRepository>();
            services.AddScoped<IRawDataImportRepository, RawDataImportRepository>();
            #endregion

            #region Payroll
            services.AddScoped<IEmpWiseSalaryLedgerRepository, EmpWiseSalaryLedgerRepository>();
            services.AddScoped<IPFContributionRepository, PFContributionRepository>();
            services.AddScoped<ISalaryCheckRepository, SalaryCheckRepository>();
            services.AddScoped<ISalarySettlementRepository, SalarySettlementRepository>();
            services.AddScoped<IWFLedgerRepository, WFLedgerRepository>();
            services.AddScoped<IPFLedgerRepository, PFLedgerRepository>();
            services.AddScoped<IGratuityLedgerRepository, GratuityLedgerRepository>();
            services.AddScoped<ISalaryAdditionRepository, SalaryAdditionRepository>();
            services.AddScoped<ISalaryDeductionRepository, SalaryDeductionRepository>();
            services.AddScoped<IExcelUploadRepository, ExcelUploadRepository>();
            services.AddScoped<IMLExcelUploadRepository, MLExcelUploadRepository>();
            services.AddScoped<IPELExcelFileUploadRepository, PELExcelFileUploadRepository>();
            services.AddScoped<IPFEmpOpBalRepository, PFEmpOpBalRepository>();
            #endregion

            #region Accounts
            services.AddScoped<IBankStatementBLRepository, BankStatementBLRepository>();
            services.AddScoped<IFiscalYearRepository, FiscalYearRepository>();
            services.AddScoped<IVoucherTypeRepository, VoucherTypeRepository>();
            services.AddScoped<IVoucherNoPrefixRepository, VoucherNoPrefixRepository>();
            services.AddScoped<IGovtScheduleEquityRepository, GovtScheduleEquityRepository>();
            services.AddScoped<IGovtScheduleForeignRepository, GovtScheduleForeignRepository>();
            services.AddScoped<IAccountProcessRepository, AccountProcessRepository>();
            services.AddScoped<IPFProcessRepository, PFProcessRepository>();
            services.AddScoped<IGovtScheduleJapanLoanRepository, GovtScheduleJapanLoanRepository>();
            services.AddScoped<IAccBudgetRepository, AccBudgetRepository>();
            services.AddScoped<IAccountChartRepository, AccountChartRepository>();
            services.AddScoped<IBankReconcilRepository, BankReconcilRepository>();
            services.AddScoped<IVoucherCheckSubNoRepository, VoucherCheckSubNoRepository>();
            services.AddScoped<IBudgetRepository, BudgetRepository>();
            services.AddScoped<IAccountDashboardRepository, AccountDashboardRepository>();
            services.AddScoped<IAccPostVoucherRepository, AccPostVoucherRepository>();
            services.AddScoped<IShowVoucherRepository, ShowVoucherRepository>();
            services.AddScoped<ISummaryReportRepository, SummaryReportRepository>();
            services.AddScoped<IGeneralReportRepository, GeneralReportRepository>();
            services.AddScoped<IBudgetReleaseRepository, BudgetReleaseRepository>();
            services.AddScoped<IPrdUnitRepository, PrdUnitRepository>();
            services.AddScoped<ICostAllocationRepository, CostAllocationRepository>();
            services.AddScoped<IBillManagementRepository, BillManagementRepository>();
            services.AddScoped<IBankClearingRepository, BankClearingRepository>();
            services.AddScoped<IVoucherTranGroupRepository, VoucherTranGroupRepository>();
            services.AddScoped<IShareHoldingRepository, ShareHoldingRepository>();
            services.AddScoped<INoteDescriptionRepository, NoteDescriptionRepository>();
            services.AddScoped<IAcc_BudgetRepository, Acc_BudgetRepository>();
            services.AddScoped<IAccVoucherRepository, AccVoucherRepository>();
            #endregion

            #region Commercial
            services.AddScoped<IBankAccountNoRepository, BankAccountNoRepository>();
            services.AddScoped<IBuyerGroupsRepository, BuyerGroupsRepository>();
            services.AddScoped<IBuyerInformationsRepository, BuyerInformationsRepository>();
            services.AddScoped<IDestinationsRepository, DestinationsRepository>();
            services.AddScoped<ILienBanksRepository, LienBanksRepository>();
            services.AddScoped<IOpeningBanksRepository, OpeningBanksRepository>();
            services.AddScoped<IPortOfDischargesRepository, PortOfDischargesRepository>();
            services.AddScoped<IPortOfLoadingsRepository, PortOfLoadingsRepository>();
            services.AddScoped<ISisterConcernCompanyRepository, SisterConcernCompanyRepository>();
            services.AddScoped<ICOM_ProformaInvoiceRepository, COM_ProformaInvoiceRepository>();
            #endregion


            #region Inventory
            services.AddScoped<IPurchaseRequisitionRepository, PurchaseRequisitionRepository>();
            services.AddScoped<IStoreRequisitionRepository, StoreRequisitionRepository>();
            services.AddScoped<IIssuesRepository, IssuesRepository>();
            services.AddScoped<IGoodsReceiveRepository, GoodsReceiveRepository>();
            services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
            services.AddScoped<IPostDocumentRepository, PostDocumentRepository>();
            #endregion

            #region Controller Folder
            services.AddScoped<IAddToCartRepository, AddToCartRepository>();
            services.AddScoped<IAssetsRepository, AssetsRepository>();
            services.AddScoped<ICompanyDetailsRepository, CompanyDetailsRepository>();
            services.AddScoped<ICurrenciesRepository, CurrenciesRepository>();
            services.AddScoped<ICompaniesRepository, CompaniesRepository>();
            services.AddScoped<ICompanyPermissionsRepository, CompanyPermissionsRepository>();
            services.AddScoped<IModuleGroupsRepository, ModuleGroupsRepository>();
            services.AddScoped<IModuleMenusRepository, ModuleMenusRepository>();
            services.AddScoped<IModulesRepository, ModulesRepository>();
            services.AddScoped<IUnitMastersRepository, UnitMastersRepository>();
            services.AddScoped<IReportPermissionsRepository, ReportPermissionsRepository>();
            services.AddScoped<ISystemAdminRepository, SystemAdminRepository>();
            services.AddScoped<IUserPermissionRepository, UserPermissionRepository>();
            services.AddScoped<IReportTypesRepository, ReportTypesRepository>();
            #endregion

            #region HRrecruitment
            services.AddScoped<IPostRepository, PostRepository>();

            #endregion
            #region Tax
            services.AddScoped<ITaxRepository, TaxRepository>();
            services.AddScoped<IClientCompany, ClientCompany>();
            services.AddScoped<IClientContractPayment, ClientContractPayment>();
            services.AddScoped<IClientTaxInfo, ClientTaxInfo>();
            services.AddScoped<IClientContactPayment, ClientContactPayment>();
            services.AddScoped<IDocumentInfo, DocumentInfo>();
            services.AddScoped<IClientTaxCosting, ClientTaxCosting>();
            services.AddScoped<IClientTaxPayment, ClientTaxPayment>();

            services.AddScoped<ITaxReportRepository, TaxReportRepository>();

            #endregion



            services.AddScoped<IPFWithdrawnRepository, PFWithdrawnRepository>();
            services.AddScoped<ITechnicalRepository, TechnicalRepository>();
            services.AddScoped<IBudgetProductionTargetRepository, BudgetProductionTargetRepository>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            //services.AddDetection();///stop by fahad
            //services.AddDetectionCore().AddBrowser();//stop by fahad
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(240);//You can set Time   
                //options.Cookie.Path = "/Account/LogIn1";
            });

            services.Configure<FormOptions>(options =>
            {
                //options.MultipartBodyLengthLimit = 52428800;
                options.ValueCountLimit = 6000;  //// baper beta saddam

            });

            services.AddMvc();
            services.Configure<SMTPConfigModel>(Configuration.GetSection("SMTPConfig"));

            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
            services.AddControllers().AddJsonOptions(options =>
            {

                // Use the default property (Pascal) casing.
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            services.AddAntiforgery(options =>
            {
                // Set Cookie properties using CookieBuilder properties�.
                options.FormFieldName = "GTR_ANTIFORZERY";
                options.HeaderName = "X-CSRF-TOKEN-GTR_ANTIFORZERY";
                options.SuppressXFrameOptionsHeader = false;
            });

            var connectionString = Configuration.GetConnectionString("HangfireConnection");
            services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(connectionString, new Hangfire.SqlServer.SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true

            }));
            services.AddHangfireServer();



            string conString = Configuration.GetConnectionString("DefaultConnection");
            AppData.AppData.dbdaperpconstring = conString;

            services.AddControllersWithViews();
            IMvcBuilder builder = services.AddRazorPages();
            builder.AddRazorRuntimeCompilation();
            //#if DEBUG
            //            if (Env.IsDevelopment())
            //                        {

            //                        }
            //            #endif



        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();


            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHangfireDashboard();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
