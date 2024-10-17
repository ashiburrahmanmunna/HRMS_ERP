using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace GTERP.EF
{
    public partial class gtrerp_allContext : DbContext
    {
        public gtrerp_allContext()
        {
        }

        public gtrerp_allContext(DbContextOptions<gtrerp_allContext> options)
            : base(options)
        {
        }

        public virtual DbSet<HrProcessedDataSal> HrProcessedDataSals { get; set; }
        public virtual DbSet<HrProcessedDataSalUpdate> HrProcessedDataSalUpdates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AI");

            modelBuilder.Entity<HrProcessedDataSal>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("HR_ProcessedDataSal");

                entity.Property(e => e.AEmpId).HasColumnName("aEmpID");

                entity.Property(e => e.AId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("aId");

                entity.Property(e => e.Ab)
                    .HasColumnType("money")
                    .HasColumnName("AB");

                entity.Property(e => e.AbL).HasColumnName("abL");

                entity.Property(e => e.AbLamt)
                    .HasColumnType("money")
                    .HasColumnName("abLAmt");

                entity.Property(e => e.AdjustedItded)
                    .HasColumnType("money")
                    .HasColumnName("AdjustedITDed");

                entity.Property(e => e.Adv)
                    .HasColumnType("money")
                    .HasColumnName("ADV");

                entity.Property(e => e.AdvAgainstExp).HasColumnType("money");

                entity.Property(e => e.AdvAgainstExpRefund).HasColumnType("money");

                entity.Property(e => e.AdvExploanBal).HasColumnType("money");

                entity.Property(e => e.AdvFacility).HasColumnType("money");

                entity.Property(e => e.AdvInTaxDed).HasColumnType("money");

                entity.Property(e => e.AdvWagesDed).HasColumnType("money");

                entity.Property(e => e.Amount).HasDefaultValueSql("((0))");

                entity.Property(e => e.Arrear).HasColumnType("money");

                entity.Property(e => e.ArrearBasic).HasColumnType("money");

                entity.Property(e => e.ArrearBonus).HasColumnType("money");

                entity.Property(e => e.ArrearElectricityExp).HasColumnType("money");

                entity.Property(e => e.ArrearElectricityExpBal).HasColumnType("money");

                entity.Property(e => e.ArrearElectricityExpOther).HasColumnType("money");

                entity.Property(e => e.ArrearElectricityExpOtherBal).HasColumnType("money");

                entity.Property(e => e.ArrearGasExp).HasColumnType("money");

                entity.Property(e => e.ArrearGasExpBal).HasColumnType("money");

                entity.Property(e => e.ArrearGasExpOther).HasColumnType("money");

                entity.Property(e => e.ArrearGasExpOtherBal).HasColumnType("money");

                entity.Property(e => e.ArrearHrexp)
                    .HasColumnType("money")
                    .HasColumnName("ArrearHRExp");

                entity.Property(e => e.ArrearHrexpBal)
                    .HasColumnType("money")
                    .HasColumnName("ArrearHRExpBal");

                entity.Property(e => e.ArrearHrexpOther)
                    .HasColumnType("money")
                    .HasColumnName("ArrearHRExpOther");

                entity.Property(e => e.ArrearHrexpOtherBal)
                    .HasColumnType("money")
                    .HasColumnName("ArrearHRExpOtherBal");

                entity.Property(e => e.ArrearInTaxDed).HasColumnType("money");

                entity.Property(e => e.ArrearOt)
                    .HasColumnType("money")
                    .HasColumnName("ArrearOT");

                entity.Property(e => e.AttAllow).HasColumnType("money");

                entity.Property(e => e.AttBonus).HasColumnType("money");

                entity.Property(e => e.Band)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.BandSl).HasColumnName("BandSL");

                entity.Property(e => e.Bank1).HasDefaultValueSql("((0))");

                entity.Property(e => e.BankAcNo)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.BankId).HasDefaultValueSql("((1))");

                entity.Property(e => e.BankPay).HasColumnType("money");

                entity.Property(e => e.BankPf)
                    .HasColumnType("money")
                    .HasColumnName("BankPF");

                entity.Property(e => e.BasicSalary).HasColumnType("money");

                entity.Property(e => e.Bcl)
                    .HasColumnType("money")
                    .HasColumnName("BCL");

                entity.Property(e => e.Bel)
                    .HasColumnType("money")
                    .HasColumnName("BEL");

                entity.Property(e => e.Bs)
                    .HasColumnType("money")
                    .HasColumnName("BS");

                entity.Property(e => e.Bsded)
                    .HasColumnType("money")
                    .HasColumnName("BSDed");

                entity.Property(e => e.Bsl)
                    .HasColumnType("money")
                    .HasColumnName("BSL");

                entity.Property(e => e.CanteenAllow).HasColumnType("money");

                entity.Property(e => e.Cash).HasDefaultValueSql("((0))");

                entity.Property(e => e.CashPay).HasColumnType("money");

                entity.Property(e => e.CashPf)
                    .HasColumnType("money")
                    .HasColumnName("CashPF");

                entity.Property(e => e.Cf)
                    .HasColumnName("CF")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ChargeAllow).HasColumnType("money");

                entity.Property(e => e.CheForum).HasColumnType("money");

                entity.Property(e => e.ChemicalAsso).HasColumnType("money");

                entity.Property(e => e.ChemicalForum).HasColumnType("money");

                entity.Property(e => e.Cl).HasColumnName("CL");

                entity.Property(e => e.Clh)
                    .HasColumnName("CLH")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ComId)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.ComPfCount).HasColumnType("money");

                entity.Property(e => e.ComPfRefund).HasColumnType("money");

                entity.Property(e => e.Conf)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ContainSub).HasColumnType("money");

                entity.Property(e => e.ConvAllow).HasColumnType("money");

                entity.Property(e => e.ConvAllowDed).HasColumnType("money");

                entity.Property(e => e.ConveyanceAllow).HasColumnType("money");

                entity.Property(e => e.CurrEl).HasColumnName("CurrEL");

                entity.Property(e => e.DapEmpClub).HasColumnType("money");

                entity.Property(e => e.Dd)
                    .HasColumnType("money")
                    .HasColumnName("DD");

                entity.Property(e => e.DearnessAllow).HasColumnType("money");

                entity.Property(e => e.DedIncBns).HasColumnType("money");

                entity.Property(e => e.DedIncBnsBal).HasColumnType("money");

                entity.Property(e => e.DedIncBonusOf).HasColumnType("money");

                entity.Property(e => e.DedIncBonusOfBal).HasColumnType("money");

                entity.Property(e => e.DiplomaassoDed).HasColumnType("money");

                entity.Property(e => e.Dishantenna).HasColumnType("money");

                entity.Property(e => e.DishantennaRefund).HasColumnType("money");

                entity.Property(e => e.Donation).HasColumnType("money");

                entity.Property(e => e.DtInput)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("dtInput");

                entity.Property(e => e.DtJoin)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("dtJoin");

                entity.Property(e => e.DtPayment)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("dtPayment")
                    .HasDefaultValueSql("('00:00')");

                entity.Property(e => e.DtPresentLast)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("dtPresentLast");

                entity.Property(e => e.DtSalary)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("dtSalary");

                entity.Property(e => e.EduAlloDed).HasColumnType("money");

                entity.Property(e => e.EduAllow).HasColumnType("money");

                entity.Property(e => e.El).HasColumnName("EL");

                entity.Property(e => e.Elamt)
                    .HasColumnType("money")
                    .HasColumnName("ELAmt");

                entity.Property(e => e.ElectricCharge).HasColumnType("money");

                entity.Property(e => e.ElectricChargeOther).HasColumnType("money");

                entity.Property(e => e.ElectricityExpRefund).HasColumnType("money");

                entity.Property(e => e.Elh).HasColumnName("ELH");

                entity.Property(e => e.EmpClubDed).HasColumnType("money");

                entity.Property(e => e.EmpCode)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.EmpId).HasColumnName("EmpID");

                entity.Property(e => e.EmpSts)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.EnggassoDed).HasColumnType("money");

                entity.Property(e => e.Epf)
                    .HasColumnType("money")
                    .HasColumnName("epf");

                entity.Property(e => e.ExOtamount)
                    .HasColumnType("money")
                    .HasColumnName("ExOTAmount");

                entity.Property(e => e.ExOthr).HasColumnName("ExOTHr");

                entity.Property(e => e.ExOtmin).HasColumnName("ExOTMin");

                entity.Property(e => e.ExchRate).HasColumnType("smallmoney");

                entity.Property(e => e.Fbonus)
                    .HasColumnType("money")
                    .HasColumnName("FBonus");

                entity.Property(e => e.Fcttl).HasColumnName("FCTtl");

                entity.Property(e => e.FesBonusDed).HasColumnType("money");

                entity.Property(e => e.FesBonusDedBal).HasColumnType("money");

                entity.Property(e => e.FestivalBonus).HasColumnType("money");

                entity.Property(e => e.FoodAllow).HasColumnType("money");

                entity.Property(e => e.GasAllow).HasColumnType("money");

                entity.Property(e => e.GasChargeOther).HasColumnType("money");

                entity.Property(e => e.GasExpRefund).HasColumnType("money");

                entity.Property(e => e.Gascharge).HasColumnType("money");

                entity.Property(e => e.Glid).HasColumnName("GLId");

                entity.Property(e => e.Gp)
                    .HasColumnType("money")
                    .HasColumnName("GP");

                entity.Property(e => e.Grade)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.GradeAmt).HasColumnType("money");

                entity.Property(e => e.GradeSal)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.GrossBank).HasColumnType("money");

                entity.Property(e => e.GrossCash).HasColumnType("money");

                entity.Property(e => e.Gs)
                    .HasColumnType("money")
                    .HasColumnName("GS");

                entity.Property(e => e.Gsdiff)
                    .HasColumnType("smallmoney")
                    .HasColumnName("GSDiff")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Gsfinal)
                    .HasColumnType("money")
                    .HasColumnName("GSFinal")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Gsmin)
                    .HasColumnType("smallmoney")
                    .HasColumnName("GSMin");

                entity.Property(e => e.Gsusd)
                    .HasColumnType("money")
                    .HasColumnName("GSUSD");

                entity.Property(e => e.HazScheme).HasColumnType("money");

                entity.Property(e => e.Hblid).HasColumnName("HBLId");

                entity.Property(e => e.Hblid2).HasColumnName("HBLId2");

                entity.Property(e => e.Hblid3).HasColumnName("HBLId3");

                entity.Property(e => e.HbloanBalance)
                    .HasColumnType("money")
                    .HasColumnName("HBLoanBalance");

                entity.Property(e => e.HbloanDed).HasColumnType("money");

                entity.Property(e => e.HbloanDedOther).HasColumnType("money");

                entity.Property(e => e.HbloanDedOther1).HasColumnType("money");

                entity.Property(e => e.HbloanDedOther2).HasColumnType("money");

                entity.Property(e => e.HbloanDedOtherBal).HasColumnType("money");

                entity.Property(e => e.HbloanDedOtherBal1).HasColumnType("money");

                entity.Property(e => e.HbloanDedOtherBal2).HasColumnType("money");

                entity.Property(e => e.HbloanRefund)
                    .HasColumnType("money")
                    .HasColumnName("HBLoanRefund");

                entity.Property(e => e.HdayBonus)
                    .HasColumnType("money")
                    .HasColumnName("HDayBonus");

                entity.Property(e => e.HdayP).HasColumnName("HDayP");

                entity.Property(e => e.HouseRent).HasColumnType("money");

                entity.Property(e => e.Hr)
                    .HasColumnType("money")
                    .HasColumnName("HR");

                entity.Property(e => e.HrExp).HasColumnType("money");

                entity.Property(e => e.Hrded)
                    .HasColumnType("money")
                    .HasColumnName("HRDed");

                entity.Property(e => e.HrexpOther)
                    .HasColumnType("money")
                    .HasColumnName("HRExpOther");

                entity.Property(e => e.HrexpRefund)
                    .HasColumnType("money")
                    .HasColumnName("HRExpRefund");

                entity.Property(e => e.Inc)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IncBonusDed).HasColumnType("money");

                entity.Property(e => e.IncenBns)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IncomeTax).HasColumnType("money");

                entity.Property(e => e.IncomeTaxRefund).HasColumnType("money");

                entity.Property(e => e.IsAllowGradeBns).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsAllowOt).HasColumnName("IsAllowOT");

                entity.Property(e => e.IsAllowPf).HasColumnName("IsAllowPF");

                entity.Property(e => e.IsNewJoin).HasColumnName("isNewJoin");

                entity.Property(e => e.IsOk).HasColumnName("IsOK");

                entity.Property(e => e.IsReleased)
                    .HasDefaultValueSql("((0))")
                    .HasComment("for past historical Data");

                entity.Property(e => e.Lid1).HasColumnName("LId1");

                entity.Property(e => e.Lid2).HasColumnName("LId2");

                entity.Property(e => e.Lid3).HasColumnName("LId3");

                entity.Property(e => e.Line)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.LineSl).HasColumnName("LineSL");

                entity.Property(e => e.Loan)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LoanBalance).HasColumnType("money");

                entity.Property(e => e.Lunch).HasDefaultValueSql("((0))");

                entity.Property(e => e.LunchAmt).HasDefaultValueSql("((0))");

                entity.Property(e => e.Lwp)
                    .HasColumnName("LWP")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Lwppay)
                    .HasColumnName("LWPPay")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Ma)
                    .HasColumnType("money")
                    .HasColumnName("MA");

                entity.Property(e => e.Maded)
                    .HasColumnType("money")
                    .HasColumnName("MADed");

                entity.Property(e => e.MadicalAllow).HasColumnType("money");

                entity.Property(e => e.MaterialLoanDed).HasColumnType("money");

                entity.Property(e => e.Mclid).HasColumnName("MCLId");

                entity.Property(e => e.McloanBalance)
                    .HasColumnType("money")
                    .HasColumnName("MCLoanBalance");

                entity.Property(e => e.McloanDed).HasColumnType("money");

                entity.Property(e => e.McloanDedOther).HasColumnType("money");

                entity.Property(e => e.McloanDedOtherBal)
                    .HasColumnType("money")
                    .HasColumnName("MCloanDedOtherBal");

                entity.Property(e => e.MedicalExp).HasColumnType("money");

                entity.Property(e => e.MedicalLoanDed).HasColumnType("money");

                entity.Property(e => e.MiscAddAllow).HasColumnType("money");

                entity.Property(e => e.MiscDed).HasColumnType("money");

                entity.Property(e => e.Ml).HasColumnName("ML");

                entity.Property(e => e.Mlpay)
                    .HasColumnType("money")
                    .HasColumnName("MLPay")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.MobileAllow).HasDefaultValueSql("((0))");

                entity.Property(e => e.MobileDeduction).HasDefaultValueSql("((0))");

                entity.Property(e => e.Moktab).HasColumnType("money");

                entity.Property(e => e.Month)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.NetFoodAllow).HasColumnType("money");

                entity.Property(e => e.NetOtamt)
                    .HasColumnType("money")
                    .HasColumnName("NetOTAmt");

                entity.Property(e => e.NetSalary).HasColumnType("money");

                entity.Property(e => e.NetSalaryB).HasColumnType("money");

                entity.Property(e => e.NetSalaryBuyer)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.NetSalaryPayable)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.NetSalaryPayableB).HasColumnType("money");

                entity.Property(e => e.Night).HasDefaultValueSql("((0))");

                entity.Property(e => e.NightAllow).HasColumnType("money");

                entity.Property(e => e.NightAmt).HasDefaultValueSql("((0))");

                entity.Property(e => e.NoticePayDed).HasColumnType("money");

                entity.Property(e => e.OffDayAllowAmt).HasColumnType("money");

                entity.Property(e => e.OffWlfareAsso).HasColumnType("money");

                entity.Property(e => e.OfficeclubDed).HasColumnType("money");

                entity.Property(e => e.Ot)
                    .HasColumnType("money")
                    .HasColumnName("OT");

                entity.Property(e => e.OtallowAmt)
                    .HasColumnType("money")
                    .HasColumnName("OTAllowAmt");

                entity.Property(e => e.OtamtBuyer)
                    .HasColumnType("money")
                    .HasColumnName("OTAmtBuyer")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Otdes)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("OTDes");

                entity.Property(e => e.OtherAddType)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.OtherAllow).HasColumnType("money");

                entity.Property(e => e.OtherDedType)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.OtherLoanBalance).HasColumnType("money");

                entity.Property(e => e.OtherloannDed).HasColumnType("money");

                entity.Property(e => e.OthersAddition).HasColumnType("money");

                entity.Property(e => e.OthersDeduct).HasColumnType("money");

                entity.Property(e => e.OthrTtl).HasColumnName("OTHrTtl");

                entity.Property(e => e.Othrbuyer)
                    .HasColumnName("OTHRBuyer")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.OtminTtl).HasColumnName("OTMinTtl");

                entity.Property(e => e.Otrate)
                    .HasColumnType("money")
                    .HasColumnName("OTRate");

                entity.Property(e => e.Otstamp)
                    .HasColumnType("money")
                    .HasColumnName("OTStamp");

                entity.Property(e => e.OwaSub).HasColumnType("money");

                entity.Property(e => e.PayMode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PaySlipNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PaySource)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Pbonus)
                    .HasColumnType("money")
                    .HasColumnName("PBonus");

                entity.Property(e => e.Pday).HasComment("Day of Payment");

                entity.Property(e => e.PersonalPay).HasColumnType("money");

                entity.Property(e => e.Pf)
                    .HasColumnType("money")
                    .HasColumnName("PF");

                entity.Property(e => e.PfAdd).HasColumnType("money");

                entity.Property(e => e.Pfcom)
                    .HasColumnType("money")
                    .HasColumnName("PFCom");

                entity.Property(e => e.Pflid).HasColumnName("PFLId");

                entity.Property(e => e.Pfllid).HasColumnName("PFLLId");

                entity.Property(e => e.Pfllid2).HasColumnName("PFLLId2");

                entity.Property(e => e.Pfllid3).HasColumnName("PFLLId3");

                entity.Property(e => e.PfloanBalance)
                    .HasColumnType("money")
                    .HasColumnName("PFLoanBalance");

                entity.Property(e => e.PfloanDedOther)
                    .HasColumnType("money")
                    .HasColumnName("PFloanDedOther");

                entity.Property(e => e.PfloanDedOther1)
                    .HasColumnType("money")
                    .HasColumnName("PFloanDedOther1");

                entity.Property(e => e.PfloanDedOther2)
                    .HasColumnType("money")
                    .HasColumnName("PFloanDedOther2");

                entity.Property(e => e.PfloanDedOtherBal)
                    .HasColumnType("money")
                    .HasColumnName("PFloanDedOtherBal");

                entity.Property(e => e.PfloanDedOtherBal1)
                    .HasColumnType("money")
                    .HasColumnName("PFloanDedOtherBal1");

                entity.Property(e => e.PfloanDedOtherBal2)
                    .HasColumnType("money")
                    .HasColumnName("PFloanDedOtherBal2");

                entity.Property(e => e.PfloanRefund)
                    .HasColumnType("money")
                    .HasColumnName("PFLoanRefund");

                entity.Property(e => e.PfloannDed).HasColumnType("money");

                entity.Property(e => e.Pfown)
                    .HasColumnType("money")
                    .HasColumnName("PFOwn");

                entity.Property(e => e.Pfprofit)
                    .HasColumnType("money")
                    .HasColumnName("PFProfit");

                entity.Property(e => e.Pp)
                    .HasColumnType("money")
                    .HasColumnName("PP");

                entity.Property(e => e.PrevEl).HasColumnName("PrevEL");

                entity.Property(e => e.ProssType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PurChange).HasColumnType("money");

                entity.Property(e => e.PurchaseAdv).HasColumnType("money");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RevenueStamp).HasColumnType("money");

                entity.Property(e => e.RiskAllow).HasColumnType("money");

                entity.Property(e => e.RiskDed).HasColumnType("money");

                entity.Property(e => e.Rvi)
                    .HasColumnType("money")
                    .HasColumnName("RVI")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SalAdvBal).HasColumnType("money");

                entity.Property(e => e.SalaryAdv).HasColumnType("money");

                entity.Property(e => e.ServiceBenefit).HasColumnType("money");

                entity.Property(e => e.ShiftAllowDed).HasColumnType("money");

                entity.Property(e => e.ShiftAllowDedBal).HasColumnType("money");

                entity.Property(e => e.ShiftAllowanceAmt).HasColumnType("smallmoney");

                entity.Property(e => e.ShiftAmt).HasColumnType("money");

                entity.Property(e => e.ShiftIncenAmt).HasColumnType("smallmoney");

                entity.Property(e => e.SiftAllow).HasColumnType("money");

                entity.Property(e => e.Sl).HasColumnName("SL");

                entity.Property(e => e.Slh)
                    .HasColumnName("SLH")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Spl).HasColumnName("SPL");

                entity.Property(e => e.Stamp).HasColumnType("smallmoney");

                entity.Property(e => e.Taexpense)
                    .HasColumnType("money")
                    .HasColumnName("TAExpense");

                entity.Property(e => e.TeliphoneCharge).HasColumnType("money");

                entity.Property(e => e.TiffinAllow).HasColumnType("money");

                entity.Property(e => e.TiffinAllowDed).HasColumnType("money");

                entity.Property(e => e.Tk1).HasColumnName("TK1");

                entity.Property(e => e.Tk10).HasColumnName("TK10");

                entity.Property(e => e.Tk100).HasColumnName("TK100");

                entity.Property(e => e.Tk1000).HasColumnName("TK1000");

                entity.Property(e => e.Tk2).HasColumnName("TK2");

                entity.Property(e => e.Tk20).HasColumnName("TK20");

                entity.Property(e => e.Tk5).HasColumnName("TK5");

                entity.Property(e => e.Tk50).HasColumnName("TK50");

                entity.Property(e => e.Tk500).HasColumnName("TK500");

                entity.Property(e => e.TotalAddition).HasColumnType("money");

                entity.Property(e => e.TotalDeduct)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TotalDeductBank).HasColumnType("money");

                entity.Property(e => e.TotalDeductCash).HasColumnType("money");

                entity.Property(e => e.TotalOt)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TotalOT");

                entity.Property(e => e.TotalOtb)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TotalOTB");

                entity.Property(e => e.TotalPayable)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TotalSalary).HasColumnType("money");

                entity.Property(e => e.TotaldeductB)
                    .HasColumnType("money")
                    .HasColumnName("totaldeductB");

                entity.Property(e => e.TotalpayableB)
                    .HasColumnType("money")
                    .HasColumnName("totalpayableB");

                entity.Property(e => e.Transportcharge).HasColumnType("money");

                entity.Property(e => e.Trn).HasColumnType("money");

                entity.Property(e => e.Trnd)
                    .HasColumnName("trnd")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TtlAbsent).HasDefaultValueSql("((0))");

                entity.Property(e => e.UniFormAdd).HasColumnType("money");

                entity.Property(e => e.UniFormDed).HasColumnType("money");

                entity.Property(e => e.UnionSubDed).HasColumnType("money");

                entity.Property(e => e.UnitName)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.VigilanceDutyAllow).HasColumnType("money");

                entity.Property(e => e.WId)
                    .HasColumnName("wId")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.WagesaAdv).HasColumnType("money");

                entity.Property(e => e.WagesaAdvBal).HasColumnType("money");

                entity.Property(e => e.WashingAllow).HasColumnType("money");

                entity.Property(e => e.WashingAllowDed).HasColumnType("money");

                entity.Property(e => e.WaterChargeOther).HasColumnType("money");

                entity.Property(e => e.WaterExpRefund).HasColumnType("money");

                entity.Property(e => e.Watercharge).HasColumnType("money");

                entity.Property(e => e.WdayP).HasColumnName("WDayP");

                entity.Property(e => e.WelfareLid).HasColumnName("WelfareLId");

                entity.Property(e => e.Wfl)
                    .HasColumnType("money")
                    .HasColumnName("WFL");

                entity.Property(e => e.WflloanBalance)
                    .HasColumnType("money")
                    .HasColumnName("WFLLoanBalance");

                entity.Property(e => e.WfloanLocal).HasColumnType("money");

                entity.Property(e => e.WfloanOther).HasColumnType("money");

                entity.Property(e => e.WfloanOtherBal).HasColumnType("money");

                entity.Property(e => e.Wfsub).HasColumnType("money");

                entity.Property(e => e.WhdayAbsent).HasColumnName("WHDayAbsent");

                entity.Property(e => e.WhdayOtamt)
                    .HasColumnType("smallmoney")
                    .HasColumnName("WHDayOTAmt");

                entity.Property(e => e.WhdayOthr).HasColumnName("WHDayOTHr");

                entity.Property(e => e.WhdayP).HasColumnName("WHDayP");

                entity.Property(e => e.WpfloanDed).HasColumnType("money");
            });

            modelBuilder.Entity<HrProcessedDataSalUpdate>(entity =>
            {
                entity.HasKey(e => e.Suid);

                entity.ToTable("HR_ProcessedDataSalUpdate");

                entity.Property(e => e.Suid).HasColumnName("suid");

                entity.Property(e => e.AEmpId).HasColumnName("aEmpID");

                entity.Property(e => e.AId).HasColumnName("aId");

                entity.Property(e => e.Ab)
                    .HasColumnType("money")
                    .HasColumnName("AB");

                entity.Property(e => e.AbL).HasColumnName("abL");

                entity.Property(e => e.AbLamt)
                    .HasColumnType("money")
                    .HasColumnName("abLAmt");

                entity.Property(e => e.AdjustedItded)
                    .HasColumnType("money")
                    .HasColumnName("AdjustedITDed");

                entity.Property(e => e.Adv)
                    .HasColumnType("money")
                    .HasColumnName("ADV");

                entity.Property(e => e.AdvAgainstExp).HasColumnType("money");

                entity.Property(e => e.AdvAgainstExpRefund).HasColumnType("money");

                entity.Property(e => e.AdvExploanBal).HasColumnType("money");

                entity.Property(e => e.AdvFacility).HasColumnType("money");

                entity.Property(e => e.AdvInTaxDed).HasColumnType("money");

                entity.Property(e => e.AdvWagesDed).HasColumnType("money");

                entity.Property(e => e.Arrear).HasColumnType("money");

                entity.Property(e => e.ArrearBasic).HasColumnType("money");

                entity.Property(e => e.ArrearBonus).HasColumnType("money");

                entity.Property(e => e.ArrearElectricityExp).HasColumnType("money");

                entity.Property(e => e.ArrearElectricityExpBal).HasColumnType("money");

                entity.Property(e => e.ArrearElectricityExpOther).HasColumnType("money");

                entity.Property(e => e.ArrearElectricityExpOtherBal).HasColumnType("money");

                entity.Property(e => e.ArrearGasExp).HasColumnType("money");

                entity.Property(e => e.ArrearGasExpBal).HasColumnType("money");

                entity.Property(e => e.ArrearGasExpOther).HasColumnType("money");

                entity.Property(e => e.ArrearGasExpOtherBal).HasColumnType("money");

                entity.Property(e => e.ArrearHrexp)
                    .HasColumnType("money")
                    .HasColumnName("ArrearHRExp");

                entity.Property(e => e.ArrearHrexpBal)
                    .HasColumnType("money")
                    .HasColumnName("ArrearHRExpBal");

                entity.Property(e => e.ArrearHrexpOther)
                    .HasColumnType("money")
                    .HasColumnName("ArrearHRExpOther");

                entity.Property(e => e.ArrearHrexpOtherBal)
                    .HasColumnType("money")
                    .HasColumnName("ArrearHRExpOtherBal");

                entity.Property(e => e.ArrearInTaxDed).HasColumnType("money");

                entity.Property(e => e.ArrearOt)
                    .HasColumnType("money")
                    .HasColumnName("ArrearOT");

                entity.Property(e => e.AttAllow).HasColumnType("money");

                entity.Property(e => e.AttBonus).HasColumnType("money");

                entity.Property(e => e.Band)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.BandSl).HasColumnName("BandSL");

                entity.Property(e => e.BankAcNo)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.BankPay).HasColumnType("money");

                entity.Property(e => e.BankPf)
                    .HasColumnType("money")
                    .HasColumnName("BankPF");

                entity.Property(e => e.BasicSalary).HasColumnType("money");

                entity.Property(e => e.Bcl)
                    .HasColumnType("money")
                    .HasColumnName("BCL");

                entity.Property(e => e.Bel)
                    .HasColumnType("money")
                    .HasColumnName("BEL");

                entity.Property(e => e.Bs)
                    .HasColumnType("money")
                    .HasColumnName("BS");

                entity.Property(e => e.Bsded)
                    .HasColumnType("money")
                    .HasColumnName("BSDed");

                entity.Property(e => e.Bsl)
                    .HasColumnType("money")
                    .HasColumnName("BSL");

                entity.Property(e => e.CanteenAllow).HasColumnType("money");

                entity.Property(e => e.CashPay).HasColumnType("money");

                entity.Property(e => e.CashPf)
                    .HasColumnType("money")
                    .HasColumnName("CashPF");

                entity.Property(e => e.Cf).HasColumnName("CF");

                entity.Property(e => e.ChargeAllow).HasColumnType("money");

                entity.Property(e => e.CheForum).HasColumnType("money");

                entity.Property(e => e.ChemicalAsso).HasColumnType("money");

                entity.Property(e => e.ChemicalForum).HasColumnType("money");

                entity.Property(e => e.Cl).HasColumnName("CL");

                entity.Property(e => e.Clh).HasColumnName("CLH");

                entity.Property(e => e.ComId)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.ComPfCount).HasColumnType("money");

                entity.Property(e => e.ComPfRefund).HasColumnType("money");

                entity.Property(e => e.Conf).HasColumnType("money");

                entity.Property(e => e.ContainSub).HasColumnType("money");

                entity.Property(e => e.ConvAllow).HasColumnType("money");

                entity.Property(e => e.ConvAllowDed).HasColumnType("money");

                entity.Property(e => e.ConveyanceAllow).HasColumnType("money");

                entity.Property(e => e.CurrEl).HasColumnName("CurrEL");

                entity.Property(e => e.DapEmpClub).HasColumnType("money");

                entity.Property(e => e.Dd)
                    .HasColumnType("money")
                    .HasColumnName("DD");

                entity.Property(e => e.DearnessAllow).HasColumnType("money");

                entity.Property(e => e.DedIncBns).HasColumnType("money");

                entity.Property(e => e.DedIncBnsBal).HasColumnType("money");

                entity.Property(e => e.DedIncBonusOf).HasColumnType("money");

                entity.Property(e => e.DedIncBonusOfBal).HasColumnType("money");

                entity.Property(e => e.DiplomaassoDed).HasColumnType("money");

                entity.Property(e => e.Dishantenna).HasColumnType("money");

                entity.Property(e => e.DishantennaRefund).HasColumnType("money");

                entity.Property(e => e.Donation).HasColumnType("money");

                entity.Property(e => e.DtInput)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("dtInput");

                entity.Property(e => e.DtJoin)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("dtJoin");

                entity.Property(e => e.DtPayment)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("dtPayment");

                entity.Property(e => e.DtPresentLast)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("dtPresentLast");

                entity.Property(e => e.DtSalary)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("dtSalary");

                entity.Property(e => e.EduAlloDed).HasColumnType("money");

                entity.Property(e => e.EduAllow).HasColumnType("money");

                entity.Property(e => e.El).HasColumnName("EL");

                entity.Property(e => e.Elamt)
                    .HasColumnType("money")
                    .HasColumnName("ELAmt");

                entity.Property(e => e.ElectricCharge).HasColumnType("money");

                entity.Property(e => e.ElectricChargeOther).HasColumnType("money");

                entity.Property(e => e.ElectricityExpRefund).HasColumnType("money");

                entity.Property(e => e.Elh).HasColumnName("ELH");

                entity.Property(e => e.EmpClubDed).HasColumnType("money");

                entity.Property(e => e.EmpCode)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.EmpId).HasColumnName("EmpID");

                entity.Property(e => e.EmpSts)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.EnggassoDed).HasColumnType("money");

                entity.Property(e => e.Epf)
                    .HasColumnType("money")
                    .HasColumnName("epf");

                entity.Property(e => e.ExOtamount)
                    .HasColumnType("money")
                    .HasColumnName("ExOTAmount");

                entity.Property(e => e.ExOthr).HasColumnName("ExOTHr");

                entity.Property(e => e.ExOtmin).HasColumnName("ExOTMin");

                entity.Property(e => e.ExchRate).HasColumnType("smallmoney");

                entity.Property(e => e.Fbonus)
                    .HasColumnType("money")
                    .HasColumnName("FBonus");

                entity.Property(e => e.Fcttl).HasColumnName("FCTtl");

                entity.Property(e => e.FesBonusDed).HasColumnType("money");

                entity.Property(e => e.FesBonusDedBal).HasColumnType("money");

                entity.Property(e => e.FestivalBonus).HasColumnType("money");

                entity.Property(e => e.FoodAllow).HasColumnType("money");

                entity.Property(e => e.GasAllow).HasColumnType("money");

                entity.Property(e => e.GasChargeOther).HasColumnType("money");

                entity.Property(e => e.GasExpRefund).HasColumnType("money");

                entity.Property(e => e.Gascharge).HasColumnType("money");

                entity.Property(e => e.Glid).HasColumnName("GLId");

                entity.Property(e => e.Gp)
                    .HasColumnType("money")
                    .HasColumnName("GP");

                entity.Property(e => e.Grade)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.GradeAmt).HasColumnType("money");

                entity.Property(e => e.GradeSal)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.GrossBank).HasColumnType("money");

                entity.Property(e => e.GrossCash).HasColumnType("money");

                entity.Property(e => e.Gs)
                    .HasColumnType("money")
                    .HasColumnName("GS");

                entity.Property(e => e.Gsdiff)
                    .HasColumnType("smallmoney")
                    .HasColumnName("GSDiff");

                entity.Property(e => e.Gsfinal)
                    .HasColumnType("money")
                    .HasColumnName("GSFinal");

                entity.Property(e => e.Gsmin)
                    .HasColumnType("smallmoney")
                    .HasColumnName("GSMin");

                entity.Property(e => e.Gsusd)
                    .HasColumnType("money")
                    .HasColumnName("GSUSD");

                entity.Property(e => e.HazScheme).HasColumnType("money");

                entity.Property(e => e.Hblid).HasColumnName("HBLId");

                entity.Property(e => e.Hblid2).HasColumnName("HBLId2");

                entity.Property(e => e.Hblid3).HasColumnName("HBLId3");

                entity.Property(e => e.HbloanBalance)
                    .HasColumnType("money")
                    .HasColumnName("HBLoanBalance");

                entity.Property(e => e.HbloanDed).HasColumnType("money");

                entity.Property(e => e.HbloanDedOther).HasColumnType("money");

                entity.Property(e => e.HbloanDedOther1).HasColumnType("money");

                entity.Property(e => e.HbloanDedOther2).HasColumnType("money");

                entity.Property(e => e.HbloanDedOtherBal).HasColumnType("money");

                entity.Property(e => e.HbloanDedOtherBal1).HasColumnType("money");

                entity.Property(e => e.HbloanDedOtherBal2).HasColumnType("money");

                entity.Property(e => e.HbloanRefund)
                    .HasColumnType("money")
                    .HasColumnName("HBLoanRefund");

                entity.Property(e => e.HdayBonus)
                    .HasColumnType("money")
                    .HasColumnName("HDayBonus");

                entity.Property(e => e.HdayP).HasColumnName("HDayP");

                entity.Property(e => e.HouseRent).HasColumnType("money");

                entity.Property(e => e.Hr)
                    .HasColumnType("money")
                    .HasColumnName("HR");

                entity.Property(e => e.HrExp).HasColumnType("money");

                entity.Property(e => e.Hrded)
                    .HasColumnType("money")
                    .HasColumnName("HRDed");

                entity.Property(e => e.HrexpOther)
                    .HasColumnType("money")
                    .HasColumnName("HRExpOther");

                entity.Property(e => e.HrexpRefund)
                    .HasColumnType("money")
                    .HasColumnName("HRExpRefund");

                entity.Property(e => e.Inc).HasColumnType("money");

                entity.Property(e => e.IncBonusDed).HasColumnType("money");

                entity.Property(e => e.IncenBns).HasColumnType("money");

                entity.Property(e => e.IncomeTax).HasColumnType("money");

                entity.Property(e => e.IncomeTaxRefund).HasColumnType("money");

                entity.Property(e => e.IsAllowOt).HasColumnName("IsAllowOT");

                entity.Property(e => e.IsAllowPf).HasColumnName("IsAllowPF");

                entity.Property(e => e.IsNewJoin).HasColumnName("isNewJoin");

                entity.Property(e => e.IsOk).HasColumnName("IsOK");

                entity.Property(e => e.Lid1).HasColumnName("LId1");

                entity.Property(e => e.Lid2).HasColumnName("LId2");

                entity.Property(e => e.Lid3).HasColumnName("LId3");

                entity.Property(e => e.Line)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.LineSl).HasColumnName("LineSL");

                entity.Property(e => e.Loan).HasColumnType("money");

                entity.Property(e => e.LoanBalance).HasColumnType("money");

                entity.Property(e => e.Lwp).HasColumnName("LWP");

                entity.Property(e => e.Lwppay).HasColumnName("LWPPay");

                entity.Property(e => e.Ma)
                    .HasColumnType("money")
                    .HasColumnName("MA");

                entity.Property(e => e.Maded)
                    .HasColumnType("money")
                    .HasColumnName("MADed");

                entity.Property(e => e.MadicalAllow).HasColumnType("money");

                entity.Property(e => e.MaterialLoanDed).HasColumnType("money");

                entity.Property(e => e.Mclid).HasColumnName("MCLId");

                entity.Property(e => e.McloanBalance)
                    .HasColumnType("money")
                    .HasColumnName("MCLoanBalance");

                entity.Property(e => e.McloanDed).HasColumnType("money");

                entity.Property(e => e.McloanDedOther).HasColumnType("money");

                entity.Property(e => e.McloanDedOtherBal)
                    .HasColumnType("money")
                    .HasColumnName("MCloanDedOtherBal");

                entity.Property(e => e.MedicalExp).HasColumnType("money");

                entity.Property(e => e.MedicalLoanDed).HasColumnType("money");

                entity.Property(e => e.MiscAddAllow).HasColumnType("money");

                entity.Property(e => e.MiscDed).HasColumnType("money");

                entity.Property(e => e.Ml).HasColumnName("ML");

                entity.Property(e => e.Mlpay)
                    .HasColumnType("money")
                    .HasColumnName("MLPay");

                entity.Property(e => e.Moktab).HasColumnType("money");

                entity.Property(e => e.Month)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.NetFoodAllow).HasColumnType("money");

                entity.Property(e => e.NetOtamt)
                    .HasColumnType("money")
                    .HasColumnName("NetOTAmt");

                entity.Property(e => e.NetSalary).HasColumnType("money");

                entity.Property(e => e.NetSalaryB).HasColumnType("money");

                entity.Property(e => e.NetSalaryBuyer).HasColumnType("money");

                entity.Property(e => e.NetSalaryPayable).HasColumnType("money");

                entity.Property(e => e.NetSalaryPayableB).HasColumnType("money");

                entity.Property(e => e.NightAllow).HasColumnType("money");

                entity.Property(e => e.NoticePayDed).HasColumnType("money");

                entity.Property(e => e.OffDayAllowAmt).HasColumnType("money");

                entity.Property(e => e.OffWlfareAsso).HasColumnType("money");

                entity.Property(e => e.OfficeclubDed).HasColumnType("money");

                entity.Property(e => e.Ot)
                    .HasColumnType("money")
                    .HasColumnName("OT");

                entity.Property(e => e.OtallowAmt)
                    .HasColumnType("money")
                    .HasColumnName("OTAllowAmt");

                entity.Property(e => e.OtamtBuyer)
                    .HasColumnType("money")
                    .HasColumnName("OTAmtBuyer");

                entity.Property(e => e.Otdes)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("OTDes");

                entity.Property(e => e.OtherAddType)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.OtherAllow).HasColumnType("money");

                entity.Property(e => e.OtherDedType)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.OtherLoanBalance).HasColumnType("money");

                entity.Property(e => e.OtherloannDed).HasColumnType("money");

                entity.Property(e => e.OthersAddition).HasColumnType("money");

                entity.Property(e => e.OthersDeduct).HasColumnType("money");

                entity.Property(e => e.OthrTtl).HasColumnName("OTHrTtl");

                entity.Property(e => e.Othrbuyer).HasColumnName("OTHRBuyer");

                entity.Property(e => e.OtminTtl).HasColumnName("OTMinTtl");

                entity.Property(e => e.Otrate)
                    .HasColumnType("money")
                    .HasColumnName("OTRate");

                entity.Property(e => e.Otstamp)
                    .HasColumnType("money")
                    .HasColumnName("OTStamp");

                entity.Property(e => e.OwaSub).HasColumnType("money");

                entity.Property(e => e.PayMode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PaySlipNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PaySource)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Pbonus)
                    .HasColumnType("money")
                    .HasColumnName("PBonus");

                entity.Property(e => e.PersonalPay).HasColumnType("money");

                entity.Property(e => e.Pf)
                    .HasColumnType("money")
                    .HasColumnName("PF");

                entity.Property(e => e.PfAdd).HasColumnType("money");

                entity.Property(e => e.Pfcom)
                    .HasColumnType("money")
                    .HasColumnName("PFCom");

                entity.Property(e => e.Pflid).HasColumnName("PFLId");

                entity.Property(e => e.Pfllid).HasColumnName("PFLLId");

                entity.Property(e => e.Pfllid2).HasColumnName("PFLLId2");

                entity.Property(e => e.Pfllid3).HasColumnName("PFLLId3");

                entity.Property(e => e.PfloanBalance)
                    .HasColumnType("money")
                    .HasColumnName("PFLoanBalance");

                entity.Property(e => e.PfloanDedOther)
                    .HasColumnType("money")
                    .HasColumnName("PFloanDedOther");

                entity.Property(e => e.PfloanDedOther1)
                    .HasColumnType("money")
                    .HasColumnName("PFloanDedOther1");

                entity.Property(e => e.PfloanDedOther2)
                    .HasColumnType("money")
                    .HasColumnName("PFloanDedOther2");

                entity.Property(e => e.PfloanDedOtherBal)
                    .HasColumnType("money")
                    .HasColumnName("PFloanDedOtherBal");

                entity.Property(e => e.PfloanDedOtherBal1)
                    .HasColumnType("money")
                    .HasColumnName("PFloanDedOtherBal1");

                entity.Property(e => e.PfloanDedOtherBal2)
                    .HasColumnType("money")
                    .HasColumnName("PFloanDedOtherBal2");

                entity.Property(e => e.PfloanRefund)
                    .HasColumnType("money")
                    .HasColumnName("PFLoanRefund");

                entity.Property(e => e.PfloannDed).HasColumnType("money");

                entity.Property(e => e.Pfown)
                    .HasColumnType("money")
                    .HasColumnName("PFOwn");

                entity.Property(e => e.Pfprofit)
                    .HasColumnType("money")
                    .HasColumnName("PFProfit");

                entity.Property(e => e.Pp)
                    .HasColumnType("money")
                    .HasColumnName("PP");

                entity.Property(e => e.PrevEl).HasColumnName("PrevEL");

                entity.Property(e => e.ProssType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PurChange).HasColumnType("money");

                entity.Property(e => e.PurchaseAdv).HasColumnType("money");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RevenueStamp).HasColumnType("money");

                entity.Property(e => e.RiskAllow).HasColumnType("money");

                entity.Property(e => e.RiskDed).HasColumnType("money");

                entity.Property(e => e.Rvi)
                    .HasColumnType("money")
                    .HasColumnName("RVI");

                entity.Property(e => e.SalAdvBal).HasColumnType("money");

                entity.Property(e => e.SalaryAdv).HasColumnType("money");

                entity.Property(e => e.ServiceBenefit).HasColumnType("money");

                entity.Property(e => e.ShiftAllowDed).HasColumnType("money");

                entity.Property(e => e.ShiftAllowDedBal).HasColumnType("money");

                entity.Property(e => e.ShiftAllowanceAmt).HasColumnType("smallmoney");

                entity.Property(e => e.ShiftAmt).HasColumnType("money");

                entity.Property(e => e.ShiftIncenAmt).HasColumnType("smallmoney");

                entity.Property(e => e.SiftAllow).HasColumnType("money");

                entity.Property(e => e.Sl).HasColumnName("SL");

                entity.Property(e => e.Slh).HasColumnName("SLH");

                entity.Property(e => e.Spl).HasColumnName("SPL");

                entity.Property(e => e.Stamp).HasColumnType("smallmoney");

                entity.Property(e => e.Taexpense)
                    .HasColumnType("money")
                    .HasColumnName("TAExpense");

                entity.Property(e => e.TeliphoneCharge).HasColumnType("money");

                entity.Property(e => e.TiffinAllow).HasColumnType("money");

                entity.Property(e => e.TiffinAllowDed).HasColumnType("money");

                entity.Property(e => e.Tk1).HasColumnName("TK1");

                entity.Property(e => e.Tk10).HasColumnName("TK10");

                entity.Property(e => e.Tk100).HasColumnName("TK100");

                entity.Property(e => e.Tk1000).HasColumnName("TK1000");

                entity.Property(e => e.Tk2).HasColumnName("TK2");

                entity.Property(e => e.Tk20).HasColumnName("TK20");

                entity.Property(e => e.Tk5).HasColumnName("TK5");

                entity.Property(e => e.Tk50).HasColumnName("TK50");

                entity.Property(e => e.Tk500).HasColumnName("TK500");

                entity.Property(e => e.TotalAddition).HasColumnType("money");

                entity.Property(e => e.TotalDeduct).HasColumnType("money");

                entity.Property(e => e.TotalDeductBank).HasColumnType("money");

                entity.Property(e => e.TotalDeductCash).HasColumnType("money");

                entity.Property(e => e.TotalOt)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TotalOT");

                entity.Property(e => e.TotalOtb)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TotalOTB");

                entity.Property(e => e.TotalPayable).HasColumnType("money");

                entity.Property(e => e.TotalSalary).HasColumnType("money");

                entity.Property(e => e.TotaldeductB)
                    .HasColumnType("money")
                    .HasColumnName("totaldeductB");

                entity.Property(e => e.TotalpayableB)
                    .HasColumnType("money")
                    .HasColumnName("totalpayableB");

                entity.Property(e => e.Transportcharge).HasColumnType("money");

                entity.Property(e => e.Trn).HasColumnType("money");

                entity.Property(e => e.Trnd).HasColumnName("trnd");

                entity.Property(e => e.UniFormAdd).HasColumnType("money");

                entity.Property(e => e.UniFormDed).HasColumnType("money");

                entity.Property(e => e.UnionSubDed).HasColumnType("money");

                entity.Property(e => e.UnitName)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.VigilanceDutyAllow).HasColumnType("money");

                entity.Property(e => e.WId).HasColumnName("wId");

                entity.Property(e => e.WagesaAdv).HasColumnType("money");

                entity.Property(e => e.WagesaAdvBal).HasColumnType("money");

                entity.Property(e => e.WashingAllow).HasColumnType("money");

                entity.Property(e => e.WashingAllowDed).HasColumnType("money");

                entity.Property(e => e.WaterChargeOther).HasColumnType("money");

                entity.Property(e => e.WaterExpRefund).HasColumnType("money");

                entity.Property(e => e.Watercharge).HasColumnType("money");

                entity.Property(e => e.WdayP).HasColumnName("WDayP");

                entity.Property(e => e.WelfareLid).HasColumnName("WelfareLId");

                entity.Property(e => e.Wfl)
                    .HasColumnType("money")
                    .HasColumnName("WFL");

                entity.Property(e => e.WflloanBalance)
                    .HasColumnType("money")
                    .HasColumnName("WFLLoanBalance");

                entity.Property(e => e.WfloanLocal).HasColumnType("money");

                entity.Property(e => e.WfloanOther).HasColumnType("money");

                entity.Property(e => e.WfloanOtherBal).HasColumnType("money");

                entity.Property(e => e.Wfsub).HasColumnType("money");

                entity.Property(e => e.WhdayAbsent).HasColumnName("WHDayAbsent");

                entity.Property(e => e.WhdayOtamt)
                    .HasColumnType("smallmoney")
                    .HasColumnName("WHDayOTAmt");

                entity.Property(e => e.WhdayOthr).HasColumnName("WHDayOTHr");

                entity.Property(e => e.WhdayP).HasColumnName("WHDayP");

                entity.Property(e => e.WpfloanDed).HasColumnType("money");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
