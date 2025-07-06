using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SavingsAPI.Data;
using SavingsAPI.Models;
using SavingsAPI.Services;
using System.Text.Json;

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
        public async Task<ActionResult<IEnumerable<SavingsAcc>>> GetLatest()
        {
            var latestDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var latestAccounts = await _dbContext.SavingsAccounts
                .Where(a => a.Date == latestDate)
                .ToListAsync();

            return Ok(latestAccounts);
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
