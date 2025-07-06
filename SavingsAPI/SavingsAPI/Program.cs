

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SavingsAPI.Data;
using SavingsAPI.URLs;
using System;

namespace SavingsAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<SavingsDbContext>(options =>
    options.UseSqlite("Data Source=savings.db"));

            builder.Services.AddControllers();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder
                        .AllowAnyOrigin()   
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });



            builder.Services.AddOpenApi();

            var app = builder.Build();
            app.UseCors("AllowAll");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();

            //using (var connection = new SqliteConnection("Data Source=savings.db"))
            //{
            //    connection.Open();

            //    var command = connection.CreateCommand();
            //    command.CommandText =
            //    @"
            //        CREATE TABLE user (
            //            id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
            //            name TEXT NOT NULL
            //        );

            //        INSERT INTO user
            //        VALUES (1, 'Brice'),
            //               (2, 'Alexander'),
            //               (3, 'Nate');
            //    ";
            //    command.ExecuteNonQuery();

            //    Console.Write("Name: ");
            //    var name = Console.ReadLine();

            //    #region snippet_Parameter
            //    command.CommandText =
            //    @"
            //        INSERT INTO user (name)
            //        VALUES ($name)
            //    ";
            //    command.Parameters.AddWithValue("$name", name);
            //    #endregion
            //    command.ExecuteNonQuery();

            //    command.CommandText =
            //    @"
            //        SELECT last_insert_rowid()
            //    ";
            //    var newId = (long)command.ExecuteScalar();

            //    Console.WriteLine($"Your new user ID is {newId}.");
            //}

            //Console.Write("User ID: ");
            //var id = int.Parse(Console.ReadLine());

            //#region snippet_HelloWorld
            //using (var connection = new SqliteConnection("Data Source=savings.db"))
            //{
            //    connection.Open();

            //    var command = connection.CreateCommand();
            //    command.CommandText =
            //    @"
            //        SELECT name
            //        FROM user
            //        WHERE id = $id
            //    ";
            //    command.Parameters.AddWithValue("$id", id);

            //    using (var reader = command.ExecuteReader())
            //    {
            //        while (reader.Read())
            //        {
            //            var name = reader.GetString(0);

            //            Console.WriteLine($"Hello, {name}!");
            //        }
            //    }
            //}
            //#endregion






            // GET FRESH DATA 

            //using (var scope = app.Services.CreateScope())
            //{
            //    var dbContext = scope.ServiceProvider.GetRequiredService<SavingsDbContext>();

            //    //for (int i = 0; i < 3;  i++)
            //    //{
            //    //    await ApiFetcher.FetchAndSaveProductAsync(ProductURLs.All[i], dbContext);
            //    //}

            //    foreach (var url in ProductURLs.All)
            //    {
            //        await ApiFetcher.FetchAndSaveProductAsync(url, dbContext);
            //    }
            //}
























            //        var urls = new List<string>
            //{
            //    "https://api.cdr-api.amp.com.au/cds-au/v1",
            //    "https://pub.cdr-sme.amp.com.au/api/cds-au/v1",
            //    "https://api.anz/cds-au/v1",
            //    "https://cdr.apix.anz/cds-au/v1",
            //    "https://api.cdr.adelaidebank.com.au/cds-au/v1",
            //    "https://public.cdr.alex.com.au/cds-au/v1",
            //    "https://public.cdr.arabbank.com.au/cds-au/v1",
            //    "https://aussie.openportal.com.au/cds-au/v1",
            //    "https://public.open.australianmilitarybank.com.au/cds-au/v1",
            //    "https://internetbanking.australianmutual.bank/openbanking/cds-au/v1",
            //    "https://open-banking.australianunity.com.au/cds-au/v1",
            //    "https://api.auswidebank.com.au/openbanking/cds-au/v1",
            //    "https://public.cdr-api.bcu.com.au/cds-au/v1",
            //    "https://api.cds.boqspecialist.com.au/cds-au/v1",
            //    "https://public.cdr.bankaust.com.au/cds-au/v1",
            //    "https://public.cdr.bankfirst.com.au/cds-au/v1",
            //    "https://api-gateway.bankofchina.com.au/cds-au/v1",
            //    "https://digital-api.bankofmelbourne.com.au/cds-au/v1",
            //    "https://api.cds.boq.com.au/cds-au/v1",
            //    "https://openbank.api.banksyd.com.au/cds-au/v1",
            //    "https://api.bankofus.com.au/OpenBanking/cds-au/v1",
            //    "https://digital-api.banksa.com.au/cds-au/v1",
            //    "https://ib.bankvic.com.au/openbanking/cds-au/v1",
            //    "https://open-api.bankwest.com.au/bwpublic/cds-au/v1",
            //    "https://api.cdr.bendigobank.com.au/cds-au/v1",
            //    "https://public.cdr.api.beyondbank.com.au/cds-au/v1",
            //    "https://public.cdr.prd.borderbank.com.au/cds-au/v1",
            //    "https://public.cdr-api.bhccu.com.au/cds-au/v1",
            //    "https://cdr.commbiz.api.commbank.com.au/cbzpublic/cds-au/v1",
            //    "https://openbanking.cairnsbank.com.au/OpenBanking/cds-au/v1",
            //    "https://api.openbanking.cardservicesdirect.com.au/cds-au/v1",
            //    "https://secure.cmcu.com.au/openbanking/cds-au/v1",
            //    "https://ib.cwcu.com.au/openbanking/cds-au/v1",
            //    "https://openbanking.api.citi.com.au/cds-au/v1",
            //    "https://public.cdr-api.coastline.com.au/cds-au/v1",
            //    "https://api.openbanking.secure.coles.com.au/cds-au/v1",
            //    "https://api.commbank.com.au/public/cds-au/v1",
            //    "https://netbank.communityfirst.com.au/cf-OpenBanking/cds-au/v1",
            //    "https://openbanking.api.creditunionsa.com.au/cds-au/v1",
            //    "https://api.cds.ddhgraham.com.au/cds-au/v1",
            //    "https://product.defencebank.com.au/cds-au/v1",
            //    "https://public.cdr-api.dnister.com.au/cds-au/v1",
            //    "https://ebranch.easystreet.com.au/es-OpenBanking/cds-au/v1",
            //    "https://public.cdr.familyfirst.com.au/cds-au/v1",
            //    "https://public.cdr-api.fscu.com.au/cds-au/v1",
            //    "https://ob.tmbl.com.au/fmbank/cds-au/v1",
            //    "https://internetbanking.firstoption.com.au/OpenBanking/cds-au/v1",
            //    "https://ibank.gcmutualbank.com.au/OpenBanking/cds-au/v1",
            //    "https://public.cdr-api.gatewaybank.com.au/cds-au/v1",
            //    "https://online.geelongbank.com.au/OpenBanking/cds-au/v1",
            //    "https://prd.bnk.com.au/cds-au/v1",
            //    "https://api.open-banking.greatsouthernbank.com.au/cds-au/v1",
            //    "https://od1.open-banking.business.greatsouthernbank.com.au/api/cds-au/v1",
            //    "https://public.cdr.greater.com.au/cds-au/v1",
            //    "https://public.ob.hsbc.com.au/cds-au/v1",
            //    "https://public.ob.business.hsbc.com.au/cds-au/v1",
            //    "https://ob.tmbl.com.au/hpbank/cds-au/v1",
            //    "https://api.cdr.heartlandbank.com.au/cds-au/v1",
            //    "https://product.api.heritage.com.au/cds-au/v1",
            //    "https://ob.tmbl.com.au/hiver/cds-au/v1",
            //    "https://onlinebanking.horizonbank.com.au/openbanking/cds-au/v1",
            //    "https://ibankob.humebank.com.au/OpenBanking/cds-au/v1",
            //    "https://openbank.openbanking.imb.com.au/cds-au/v1",
            //    "https://id.ob.ing.com.au/cds-au/v1",
            //    "https://onlineteller.cu.com.au/OpenBanking/cds-au/v1",
            //    "https://public.open.judo.bank/cds-au/v1",
            //    "https://api.openbanking.cards.koganmoney.com.au/cds-au/v1",
            //    "https://internetbanking.lcu.com.au/OpenBanking/cds-au/v1",
            //    "https://services.liberty.com.au/api/data-holder-public/cds-au/v1",
            //    "https://public.openbank.mebank.com.au/cds-au/v1",
            //    "https://api.cds.mebank.com.au/cds-au/v1",
            //    "https://openbanking.movebank.com.au/OpenBanking/cds-au/v1",
            //    "https://api.macquariebank.io/cds-au/v1",
            //    "https://openbanking.themutual.com.au/OpenBanking/cds-au/v1",
            //    "https://public.cdr.mystate.com.au/cds-au/v1",
            //    "https://openbank.api.nab.com.au/cds-au/v1",
            //    "https://openbank.newcastlepermanent.com.au/cds-au/v1",
            //    "https://secure.nicu.com.au/OpenBanking/cds-au/v1",
            //    "https://online.orangecu.com.au/openbanking/cds-au/v1",
            //    "https://public.cdr-api.pnbank.com.au/cds-au/v1",
            //    "https://api.paypal.com/v1/identity/cds-au/v1",
            //    "https://ob-public.peopleschoice.com.au/cds-au/v1",
            //    "https://public.cdr.prd.policebank.com.au/cds-au/v1",
            //    "https://api.policecu.com.au/OpenBanking/cds-au/v1",
            //    "https://banking.qbank.com.au/openbanking/cds-au/v1",
            //    "https://api.openbanking.qantasmoney.com/cds-au/v1",
            //    "https://public.cdr.qudosbank.com.au/cds-au/v1",
            //    "https://public.cdr-api.queenslandcountry.bank/cds-au/v1",
            //    "https://cdrbank.racq.com.au/cds-au/v1",
            //    "https://digital-api.westpac.com.au/rams/cds-au/v1",
            //    "https://public.open.rslmoney.com.au/cds-au/v1",
            //    "https://openbanking.api.rabobank.com.au/public/cds-au/v1",
            //    "https://public-data.cdr.regaustbank.io/cds-au/v1",
            //    "https://ibanking.reliancebank.com.au/rel-openbanking/cds-au/v1",
            //    "https://online.swsbank.com.au/openbanking/cds-au/v1",
            //    "https://cdr.sccu.com.au/openbanking/cds-au/v1",
            //    "https://digital-api.stgeorge.com.au/cds-au/v1",
            //    "https://public.cdr-api.summerland.com.au/cds-au/v1",
            //    "https://id-ob.suncorpbank.com.au/cds-au/v1",
            //    "https://banking.transportmutual.com.au/OpenBanking/cds-au/v1",
            //    "https://ob.tmbl.com.au/tmbank/cds-au/v1",
            //    "https://public.cdr.onlinebanking.capricornian.com.au/cds-au/v1",
            //    "https://onlinebanking.themaccu.com.au/OpenBanking/cds-au/v1",
            //    "https://public.cdr.thriday.com.au/cds-au/v1",
            //    "https://prd.tcu.com.au/cds-au/v1",
            //    "https://public.cdr.tyro.com/cds-au/v1",
            //    "https://public.cdr-api.86400.com.au/cds-au/v1",
            //    "https://ob.tmbl.com.au/unibank/cds-au/v1",
            //    "https://ibanking.unitybank.com.au/OpenBanking/cds-au/v1",
            //    "https://public.api.cdr.unloan.com.au/cds-au/v1",
            //    "https://api.up.com.au/cds-au/v1",
            //    "https://api.cds.virginmoney.com.au/cds-au/v1",
            //    "https://openbanking.wcu.com.au/OpenBanking/cds-au/v1",
            //    "https://digital-api.westpac.com.au/cds-au/v1",
            //    "https://au-cdrbanking-pub.wise.com/cds-au/v1",
            //    "https://online.woolworthsteambank.com.au/OpenBanking/cds-au/v1",
            //    "https://onlinebanking.wawcu.com.au/OpenBanking/cds-au/v1",
            //    "https://secure.gmcu.com.au/OpenBanking/cds-au/v1",
            //    "https://cdr.in1bank.com.au/cds-au/v1"
            //};

            //            var savingsProducts = await ProductFetcher.GetSavingsProductsFromApisAsync(urls);

            //            foreach (var product in savingsProducts)
            //            {
            //                //Console.WriteLine($"{product.BrandName} - {product.Name} - {product.ProductId}");
            //                Console.WriteLine(product.ProductId?.ToString() ?? "null");

            //            }
            //            Console.WriteLine(savingsProducts.Count);






















        }
    }
}
