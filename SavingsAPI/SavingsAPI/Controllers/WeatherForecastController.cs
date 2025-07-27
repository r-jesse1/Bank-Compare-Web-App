using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SavingsAPI.Data;
using SavingsAPI.Models;
using SavingsAPI.Services;
using System.Globalization;
using System.Text.Json;
using System.Linq.Dynamic.Core;


namespace SavingsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SavingsRateController : ControllerBase
    {
        private readonly SavingsDbContext _dbContext;

        public SavingsRateController(SavingsDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        // GET: /savingsrate
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SavingsAcc>>> GetLatest(
            int pageNumber = 1, int pageSize = 10, string sortBy = "TotalRate desc", [FromQuery] List<string>? banks = null, string rateType = null)
        {
            Console.WriteLine(sortBy);
            if (banks != null)
            {
                Console.WriteLine($"Banks count: {banks.Count}");
                foreach (var bank in banks)
                {
                    Console.WriteLine($"Bank: {bank}");
                }
            }
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("pageNumber and pageSize must be greater than 0.");
            }

            var latestDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var query = _dbContext.SavingsAccounts
                .Where(a => a.Date == latestDate);

            if (banks != null && banks.Count > 0) 
            {
                query = query.Where(a => banks.Contains(a.Bank));
            }

            if (rateType == "intro")
            {
                query = query.Where(a => a.IntroRate > 0);
            }

            if (rateType == "bonus")
            {
                query = query.Where(a => a.BonusRate > 0);
            }

            if (rateType == "none")
            {
                query = query.Where(a => a.BonusRate == 0 && a.IntroRate == 0);
            }

            var totalRecords = await query.CountAsync();

            var accounts = await query
                .OrderBy(sortBy)  
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            var paginationMetadata = new
            {
                totalRecords,
                pageNumber,
                pageSize,
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
            };

            Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(paginationMetadata));

            return Ok(accounts);
        }
        //[HttpGet]
        //public async Task<ActionResult<SavingsAcc>> GetAccountAsync()
        //{
        //    string json = await ApiFetcher.GetJsonFromCBAAsync();
        //    string URL = "google.com";
        //    if (json == null)
        //        return BadRequest("problems getting bank json");

        //    var account = SavingsRateExtractor.ParseSavingsAccount(json, URL);

        //    if (account == null)
        //        return BadRequest("Unable to parse savings account data.");

        //    return Ok(account);
        //}
    }
}
