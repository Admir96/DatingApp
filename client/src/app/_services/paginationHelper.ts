import { HttpClient, HttpParams } from "@angular/common/http";
import { map } from "rxjs/operators";
import { PaginatedResult } from "../_models/Pagination";

export function getPaginatedResult<T> (url,params, http: HttpClient) {

    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();
   
    return http.get<T>(url, { observe: 'response', params }).pipe(
      map(response => {

        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') !== null) {
          paginatedResult.pagionation = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );
  }

  export function getPaginationHeaders(pageNumber: number, PageSize: number)
  {
      let params = new HttpParams();
      params = params.append('pageNumber', pageNumber.toString());
      params = params.append('pageSize', PageSize.toString());
      return params;
  
  }