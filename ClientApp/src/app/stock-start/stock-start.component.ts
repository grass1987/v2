import { Component, Inject, OnInit } from '@angular/core';
import { trigger, state, transition, animate, style } from '@angular/animations';
import { HttpClient } from '@angular/common/http';
import { FormBuilder, FormGroup,Validators } from '@angular/forms';
import { EChartOption } from 'echarts';


@Component({
  selector: 'app-stock-start',
  templateUrl: './stock-start.component.html',
  styleUrls: ['./stock-start.component.css']
})
export class StockStartComponent implements OnInit {
  public searchBase : periodWeekProgress;
  public holdingCountList :holdingCount[];
  public allCompanyList:allCompanyHoldingCount[];
  public loadingSearchProgress=false;
  public _http: HttpClient;
  public _baseUrl;


  public allChart :EChartOption[];
  public tempXAxis:string[];
  public tempSeries:number[];

  public periodWeekes = [1,2,3,4,5,6,7,8,20];
  public searchStockForm: FormGroup;

  public testholdingdayList:onedayHolding[]; 
  
  public nowPage:number;
  public nowStock:string;
  public nowWeek:number;
  public nowEndOrNot:number;
  public stockDetail:string;
  public companyDetail:string[];


  

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private fb: FormBuilder) {
    this._http = http;
    this._baseUrl = baseUrl;
    this.tempSeries=[];
    this.tempXAxis=[];
    this.allChart=[];
    this.nowPage=0;
    this.nowStock='';
    this.nowWeek=0;
    this.nowEndOrNot=1;
    this.stockDetail='';
    
    this.searchStockForm = this.fb.group({
      stockNum: ['700'],
      periodWeek:[20],
      whichPage:[0],

    });
    

   }
   /*
   chartOption: EChartOption = {
    xAxis: {
      type: 'category',
      data: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun']
    },
    yAxis: {
      type: 'value'
    },
    series: [{
      data: [820, 932, 901, 934, 1290, 1330, 1320],
      type: 'line'
    }]
  }
*/
sendPost(formGroup){
  this.loadingSearchProgress = true;
  //this._http.post<onestock>(this._baseUrl + 'api/BasePro/aaa', formGroup.value).subscribe(revStock => {
    this._http.post<onestock>("http://localhost:80/" + 'api/BasePro/aaa', formGroup.value).subscribe(revStock => {
    
    if(revStock.whichpage==-1)
    {
      alert("找不到相關數據");
    }else
    {
      console.log(revStock);
      //this.testholdingdayList=ansList;
      this.allCompanyList=revStock.allCompanyHoldingCount;
      this.nowPage=revStock.whichpage;
      this.nowStock=revStock.stockID;
      this.nowWeek=revStock.periodWeek;
      this.nowEndOrNot=revStock.endOrNot;
      this.stockDetail=revStock.stockID+" "+revStock.stockName;
      for(let unitCompany of this.allCompanyList)
      {
        this.tempSeries=[];
        this.tempXAxis=[];
        this.companyDetail.push(unitCompany.companyid+" "+unitCompany.companyname+" 變化總數: "+unitCompany.allDayDiff);
        for(let unitCount of unitCompany.allDayHoldingCount)
        {
          this.tempXAxis.push(unitCount.holdingDate.substring(0,10));
          this.tempSeries.push(unitCount.holdingCount);
        }
        this.allChart.push({
          xAxis: { type: 'category',    data: this.tempXAxis },
          yAxis: { type: 'value',min:Math.min.apply(null,this.tempSeries) },
          tooltip : { trigger: 'axis' },
          series: [{ data: this.tempSeries,  type: 'line' }]
        })
      }
    }
    this.loadingSearchProgress=false;
    //alert("nowPage="+this.nowPage+"\n nowEndOrNot= "+this.nowEndOrNot);

  }, error => console.error(error));


}
  searchNewStockSubmit(formGroup) {
    this.allChart=[];
    this.companyDetail=[];
    this.nowEndOrNot=1;
    this.nowPage=0;
    this.stockDetail='';
    this.searchStockForm.patchValue({
      whichPage:0
    })
    
    this.sendPost(formGroup);

  }
  searchMoreCompany() {
    //this.allChart=[];
    
    this.searchStockForm = this.fb.group({
      stockNum: [this.nowStock],
      periodWeek:[this.nowWeek],
      whichPage:[this.nowPage+1],
      

    });
    this.sendPost(this.searchStockForm);

  }
  
  ngOnInit() {
  }

  genChart(orgList: allCompanyHoldingCount[] ){
    //this.tempXAxis=new Array();
    //this.tempSeries=new Array();
    orgList[0].allDayHoldingCount.forEach(
      function (value) {
        //console.log(value.holdingCount);
        this.tempXAxis.push(value.holdingDate);
        this.tempSeries.push(value.holdingCount);
        this.chartOption.xAxis=this.tempXAxis;
        this.chartOption.series=this.tempSeries;
    }
    )
  }
  

}
interface onestock{
  stockID:string;
  stockName:string;
  whichpage:number;
  periodWeek:number;
  endOrNot:number;
  allCompanyHoldingCount:allCompanyHoldingCount[];

}


interface allCompanyHoldingCount{
  allDayHoldingCount:onedayHolding[];
  allDayDiff:number;
  companyid:string;
  companyname:string;
  
}
interface onedayHolding{
  holdingDate:string;
  holdingCount:number;
  holdingDiff:number;

}


interface periodWeekProgress {
  stockNum: string;
  periodWeek: number;
  whichPage:number;
}
interface holdingCount {
  stockNum: string;
  periodWeek: number;


}