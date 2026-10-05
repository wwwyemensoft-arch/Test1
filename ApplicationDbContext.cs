//namespace UltimateArchiveAPI
//{
//    public class ApplicationDbContext
//    {

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using System.ComponentModel.DataAnnotations;
//using UltimateArchiveAPIs.Dtos;    ???
using UltimateArchiveAPI.Models;
//using static UltimateArchiveAPIs.Controllers.Additionals123TryXYR_7XX;
//using static UltimateArchiveAPIs.Controllers.FinalUpdateDRTabs;   ???

namespace UltimateArchiveAPI
{
    public class ApplicationDbContext : DbContext
    {
        // The Connection String will be in appsettings.json File
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }



        //
        public DbSet<Dboption> Dboptions { get; set; }
        //
        //
        public DbSet<User> Users { get; set; }
        //
        // جدول تبويب صلاحيات الوثائق
        public DbSet<ImgAccessCodeInfoU> ImgAccessCodeInfoU { get; set; }

        //
        public DbSet<AccessCodeInfoU> AccessCodeInfoU { get; set; }

    }
}
