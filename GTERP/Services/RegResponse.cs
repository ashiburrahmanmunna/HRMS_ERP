using GTERP.Services;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace GTERP.Services
{
    public class RegResponse
    {

        public RegResponse(string json)
        {
            if (json != null)
            {
                JObject jObject = JObject.Parse(json);
                string errors = jObject["Errors"].ToString();
                Succeeded = (bool)jObject["Succeeded"];

                var e = JsonConvert.DeserializeObject<List<IdentityError>>(errors);

                Errors = e;
                //Description = (string)jObject["description"];
            }

        }

        public bool Succeeded { get; set; }
        public List<IdentityError> Errors { get; set; }
        //public string Description { get; set; }




    }

    public class GetResponse
    {

        public GetResponse(string json)
        {

            if (json != null)
            {
                JObject jObject = JObject.Parse(json);

                string data = jObject["MyUsers"].ToString();
                string data2 = jObject["Companies"].ToString();

                MyResponse Response = JsonConvert.DeserializeObject<MyResponse>(json);
                MyUsers = Response.MyUsers;
                Companies = Response.Companies;

            }
        }
        public ICollection<MyUser> MyUsers { get; set; }
        public ICollection<Company> Companies { get; set; }

    }

    public class GetSoftwareResponse
    {

        public GetSoftwareResponse(string json)
        {

            if (json != null)
            {
                JObject jObject = JObject.Parse(json);

                string softwares = jObject["Softwares"].ToString();
                string softwareVersions = jObject["SoftwareVersions"].ToString();

                GetSoftwareVersions Response = JsonConvert.DeserializeObject<GetSoftwareVersions>(json);
                Softwares = Response.Softwares;
                SoftwareVersions = Response.SoftwareVersions;
            }
        }
        public ICollection<GTSoftware> Softwares { get; set; }
        public ICollection<GTSoftwareVersion> SoftwareVersions { get; set; }

    }

}
public class RegError
{
    public string Code { get; set; }
    public string Description { get; set; }
}
public class MyUser
{
    public string UserID { get; set; }
    public string UserName { get; set; }

}
public class MyUserMenuPermission
{
    public string useridPermission { get; set; }
    public string UserName { get; set; }

}
public class MyResponse
{
    public ICollection<MyUser> MyUsers { get; set; }
    public ICollection<Company> Companies { get; set; }

}

public class GTSoftware
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Details { get; set; }
}

public partial class GTSoftwareVersion
{
    public int Id { get; set; }
    public string VersionName { get; set; }
}

public class GetSoftwareVersions
{
    public ICollection<GTSoftware> Softwares { get; set; }
    public ICollection<GTSoftwareVersion> SoftwareVersions { get; set; }
}