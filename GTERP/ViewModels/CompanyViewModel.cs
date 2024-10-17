namespace GTERP.ViewModels
{
    public class CompanyViewModel
    {




        public int ComId { get; set; }


        public string CompanySecretCode { get; set; }


        public string AppKey { get; set; }



        public string CompanyCode { get; set; }


        public string CompanyName { get; set; }


        public string CompanyNameBangla { get; set; }


        public string CompanyShortName { get; set; }


        public string PrimaryAddress { get; set; }



        public string CompanyAddressBangla { get; set; }



        public string SecoundaryAddress { get; set; }




        public string comPhone { get; set; }



        public string comPhone2 { get; set; }



        public string comFax { get; set; }



        public string comEmail { get; set; }




        public string comWeb { get; set; }



        public int BusinessTypeId { get; set; }




        public int? BaseComId { get; set; }




        public int CountryId { get; set; }





        public string ContPerson { get; set; }


        public string ContDesig { get; set; }













        public byte[] ComImageHeader { get; set; }



        public string HeaderImagePath { get; set; }


        public string HeaderFileExtension { get; set; }





        //[ValidateFile(ErrorMessage = "Please select a PNG image smaller than 1MB")]

        public byte[] ComLogo { get; set; }



        public string LogoImagePath { get; set; }


        public string LogoFileExtension { get; set; }
        public string Addvertise { get; set; }


        public string MD_Name { get; set; }




        public string MD_Phone { get; set; }

        public string MD_Email { get; set; }


        public string HR_Name { get; set; }




        public string HR_Phone { get; set; }


        public string HR_Email { get; set; }



        public string AcountantName { get; set; }



        public string AcountantPhone { get; set; }


        public string AcountantEmail { get; set; }


        public int YearOfEst { get; set; }



        public int NoE { get; set; }

    }


}
