import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ReportingService } from './reporting.service';
import { mergeMap, groupBy, map, reduce } from 'rxjs/operators';
import { of } from 'rxjs';
import { Chart } from 'chart.js';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Ziel INF 354 Activity 6 Reporting';

  chart: Object = [];
  cities: Object=null;
  options = [
    {id: 1 , text: "Online Carsales"},
    {id: 2 , text: "Onsite Carsales"},
    {id: 3 , text: "All Sales"}
  ]
  selection:number = 3;

  constructor(private reporting : ReportingService) {}

  onSubmit() {
    this.reporting.getReportingData(this.selection).subscribe(response => 
  {
      console.log(response);

      let keys = response["Provinces"].map(d=>d.ProvinceName);
      let values = response["Provinces"].map(d=>d.AveragePrice);

      this.cities = response["Cities"];

      this.chart = new Chart('canvas', {
        type: 'bar',
        data: {
          labels: keys,
          datasets: [
            {
              data: values,
              borderColor: "#32CD32",
              fill: false,
              backgroundColor : [
                "0000ff",
                "#FF4500",
                "#800080"
              ],
              borderWidth: 1
            }
            
          ],
          
        },
        options: {
          legend: {
            display : false
          },
          title: {
            display : true,
            text: "Average Car Price per Province"
          },
          scales: {
            xAxes: [{
              display: true,
              barPercentage: 0.75
            }],
            yAxes: [{
              display: true,
              ticks: {
                min: 0,
                max: 750000,
                stepSize : 500000
              }
            }],
          }
        }
    })
  });
 }
 downloadRequest(type) 
 {
  this.reporting.downloadReport(this.selection, type).subscribe(x => 
  {
  var fileType = type==1?"application/pdf":"application/msword";
  var fileName = type==1?"report.pdf":"report.doc";
  var newBlob = new Blob([x], {type: fileType});

    if (window.navigator && window.navigator.msSaveOrOpenBlob) 
    {
      window.navigator.msSaveOrOpenBlob(newBlob);
      return;
    }

    const data = window.URL.createObjectURL(newBlob);

    var link = document.createElement('a');
    link.href = data;
    link.download = fileName;

    link.dispatchEvent(new MouseEvent('click',{bubbles: true, cancelable: true, view: window}));

    setTimeout(function () {
      window.URL.revokeObjectURL(data);
      link.remove();
    }, 100);
  })
 }

}
