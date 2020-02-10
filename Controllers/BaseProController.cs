using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using v2.Model;
using v2.Data;
using Microsoft.EntityFrameworkCore;


namespace v2.Controllers
{
    [Route("api/[controller]")]
    public class BaseProController : Controller
    {
        private readonly allcompanyContext uContext;
        private stockRecord todayStockRecord;

        public BaseProController(allcompanyContext context,stockRecord record)
        {
            uContext = context;
            //todayStockRecord=ustockRecord;
            if(record.AllStock==null)
                record.AllStock=new List<oneStock>{};

            todayStockRecord=record;

        }

        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            //var result = new ResultModel();
            
            //result.Data=uContext.Stockcompany.Where(x=>x.Id==30);
            var result= uContext.Companydata.Where(x=>x.Id<10);
            
            Console.Write("RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR");
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }
        [HttpGet("[action]")]
        [Route("aaa")]
        //public async Task<ActionResult<oneStock>> company([FromBody] searchStock searchCondition)
        //List<oneDayHoldingCount>
        public async Task<ActionResult<oneStock>> company([FromBody] searchStock searchCondition)
        //public IEnumerable<WeatherForecast> comapny()
        {
            //var result = new ResultModel();
            
            //result.Data= uContext.Companydata.Where(x=>x.Id<10);
            int period=0;
            oneStock findData=null;
            if(searchCondition.stockNum==null)
            {
                findData=new oneStock();
                findData.whichpage=-1;
                return findData;
            }
            if(searchCondition.periodWeek>0&&searchCondition.periodWeek<=20)
                period=searchCondition.periodWeek;
            else
            {
                findData=new oneStock();
                findData.whichpage=-1;
                return findData;
            }
            string stockNumForStr=searchCondition.stockNum.ToString().PadLeft(5,'0');
            //todayStockRecord.AllStock.Exists();
            //oneStock existsOrNot=null;
            oneStock L3=null;
            //L3.allCompanyHoldingCount=new List<oneCompanyAllDayHolding>{};
            for(int a=0;a<todayStockRecord.AllStock.Count();a++)
            {
                if(todayStockRecord.AllStock[a].periodWeek==searchCondition.periodWeek&&todayStockRecord.AllStock[a].stockID==stockNumForStr)
                {
                    L3=todayStockRecord.AllStock[a];
                    break;
                }
            }
            if(L3==null)
            {

                var readidAndName=await uContext.Stockcompany.Where(x=>x.SNo.Equals(stockNumForStr)).Select(x=>new {x.Id,x.SName,x.SNo}).FirstOrDefaultAsync();
                if(readidAndName==null)
                {
                    findData=new oneStock();
                    findData.whichpage=-1;
                    return findData;
                }

                //short testNum=1543;
                DateTime beforeWeek = DateTime.Today.AddDays(-(searchCondition.periodWeek*5));
                //var result= uContext.Companydata.Where(x=>x.Id>2240&&x.Id<2340).Select(commmm=>commmm.Id);
                
                List<middleOutput> holdingdata2 =await uContext.Holdingdaily
                                            //.Where(x=>x.Keepdata.Date>beforeWeek && x.Stockid==readidAndName.Id)
                                            .Where(x=>x.Keepdata>beforeWeek && x.Stockid==readidAndName.Id)
                                            .OrderBy(x=>x.Keepdata)
                                            .OrderBy(x=>x.Companyid)
                                            .Join(uContext.Companydata,x=>x.Companyid,y=>y.Id,
                                            (x,y)=>
                                                new middleOutput(){keepdata=x.Keepdata,
                                                    companyid=y.Id,
                                                    holdingcount=x.Holdingcount?? 0,
                                                    companyname=y.Companyname,
                                                    govcompanyid=y.Govcompanyid,
                                                    stockid=stockNumForStr})
                                            
                                            .ToListAsync();
                if(holdingdata2.Count()==0||holdingdata2==null)
                {
                    findData=new oneStock();
                    findData.whichpage=-1;
                    return findData;
                }

                //
                //oneStock     L3
                //oneCompanyAllDayHolding      L2
                //oneDayHoldingCount      L1

                oneDayHoldingCount L1=new oneDayHoldingCount();
                oneCompanyAllDayHolding L2=new oneCompanyAllDayHolding();
                L2.allDayHoldingCount=new List<oneDayHoldingCount>{};
                //oneStock L3=new oneStock();
                L3=new oneStock();
                L3.allCompanyHoldingCount=new List<oneCompanyAllDayHolding>{};
                
                //List<oneDayHoldingCount> oneCompanyHold=new List<oneDayHoldingCount>{};
                //oneDayHoldingCount ODHC =new oneDayHoldingCount();
                
                var tempcompanyid=holdingdata2[0].companyid;
                var tempcompanyname=holdingdata2[0].companyname;
                decimal tempSumDiffCount=0;
                int innerLoop=0;
                
                Boolean newCompany=true;
                oneStock sample=new oneStock();
                for(int i=0;i<holdingdata2.Count;i++)
                {
                    if(newCompany)
                    {
                        tempcompanyname=holdingdata2[i].companyname;
                        tempcompanyid=holdingdata2[i].companyid;
                        tempSumDiffCount=0;
                        innerLoop=1;

                        
                        L1=new oneDayHoldingCount();
                        L1.holdingDate=holdingdata2[i].keepdata;
                        L1.holdingCount=holdingdata2[i].holdingcount;
                        L1.holdingDiff=0;
                        L2=new oneCompanyAllDayHolding();
                        L2.allDayHoldingCount=new List<oneDayHoldingCount>{};
                        L2.companyid=holdingdata2[i].govcompanyid;
                        L2.companyname=holdingdata2[i].companyname;
                        L2.allDayHoldingCount.Add(L1);

                        newCompany=false;
                        continue;
                    }else
                    {
                            if(tempcompanyid!=holdingdata2[i].companyid)
                            {
                                L2.allDayDiff=tempSumDiffCount;
                                L3.allCompanyHoldingCount.Add(L2);
                                newCompany=true;
                                i--;

                                continue;
                            }else
                            {
                                L1=new oneDayHoldingCount();
                                L1.holdingDate=holdingdata2[i].keepdata;
                                L1.holdingCount=holdingdata2[i].holdingcount;
                                L1.holdingDiff=L1.holdingCount - L2.allDayHoldingCount[innerLoop-1].holdingCount;
                                tempSumDiffCount+=System.Math.Abs(L1.holdingDiff);
                                L2.allDayHoldingCount.Add(L1);
                                innerLoop++;
                            }
                    }
                }
                L3.allCompanyHoldingCount.Sort();

                L3.whichpage=0;
                L3.periodWeek=searchCondition.periodWeek;
                L3.stockID=readidAndName.SNo;
                L3.stockName=readidAndName.SName;
                if(todayStockRecord.AllStock.Count()<100)
                    todayStockRecord.AllStock.Add(L3);
            }
            int temppage=L3.allCompanyHoldingCount.Count()-searchCondition.whichPage*5;
            L3.whichpage=searchCondition.whichPage;
            var newL3=new oneStock();
            newL3=(oneStock)L3.Clone();
            if(temppage>=5)
                //L3.allCompanyHoldingCount=L3.allCompanyHoldingCount.GetRange(searchCondition.whichPage*5,5);
            {
                newL3.allCompanyHoldingCount=L3.allCompanyHoldingCount.GetRange(searchCondition.whichPage*5,5);
                newL3.endOrNot=0;
            }
            else if(temppage<5&&temppage>0)
            {
                newL3.allCompanyHoldingCount=L3.allCompanyHoldingCount.GetRange(searchCondition.whichPage*5,temppage);
                newL3.endOrNot=1;
            }
            else
            {
                newL3.allCompanyHoldingCount=L3.allCompanyHoldingCount.GetRange(0,0);
                newL3.endOrNot=1;
            }


            return newL3;
            //return newL3;
            
        }

        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }
    }
}
