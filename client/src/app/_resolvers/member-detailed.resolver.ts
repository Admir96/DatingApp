import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable, observable } from "rxjs";
import { Member } from "../_models/member";
import { MembersService } from "../_services/members.service";


@Injectable({
providedIn: 'root'
})


export class MemberDetailedResolver implements Resolve<Member>{

  constructor(private memberService:MembersService){

  }

    resolve(route: ActivatedRouteSnapshot): Observable<Member> {
        return this.memberService.GetMember(route.paramMap.get('username'));
    }

}