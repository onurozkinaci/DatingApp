import { Component } from '@angular/core';
import { Member } from '../../_models/member';
import { MembersService } from '../../_services/members.service';
import { MemberCardComponent } from "../member-card/member-card.component";
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-member-list',
    standalone: true,
    templateUrl: './member-list.component.html',
    styleUrl: './member-list.component.css',
    imports: [MemberCardComponent, CommonModule]
})
export class MemberListComponent {
   members$: Observable<Member[]> | undefined;
   
   constructor(private memberService:MembersService){}

   ngOnInit():void
   {
      this.members$ = this.memberService.getMembers();
   }
}
