import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpParams } from '@angular/common/http';
import { mergeMap, groupBy, map, reduce } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ReportingService {

  constructor(private http : HttpClient) { }

  getReportingData(selection) {
    return this.http.get("http://localhost:16252/api/Reporting/getReportData?citySelection="+selection)
    .pipe(map(result => result));
  }
 
  downloadReport(selection, type) 
  {
    return this.http.get("http://localhost:16252/api/Reporting/downloadReport?citySelection="+selection+"&type="+type,{responseType : 'blob'});
  }
  
}


