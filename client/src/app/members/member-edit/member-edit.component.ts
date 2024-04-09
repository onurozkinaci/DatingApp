import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { Member } from '../../_models/member';
import { User } from '../../_models/user';
import { AccountService } from '../../_services/account.service';
import { MembersService } from '../../_services/members.service';
import { take } from 'rxjs';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-member-edit',
  standalone: true,
  imports: [CommonModule, FormsModule, NgbNavModule],
  templateUrl: './member-edit.component.html',
  styleUrl: './member-edit.component.css'
})
export class MemberEditComponent  implements OnInit {
   @ViewChild('editForm') editForm:NgForm|undefined; //html'de tanimlanan componente erismek icin
   @HostListener('window:beforeunload', ['$event']) unloadNotification($event:any){
      //** */=>If the form has any change the browser will not let the user to leave where he/she are(editprofile page);
      if(this.editForm?.dirty){
         $event.returnValue = true;
      }
   }

   member:Member|undefined;
   user:User|null = null;

   active:any = 1; //for ngbnav(tabs) usage

   constructor(private accountService:AccountService, private memberService:MembersService, private toastr:ToastrService){
      this.accountService.currentUser$.pipe(take(1)).subscribe({
         next: user => this.user = user
      })
   }

   ngOnInit():void{
      this.loadMember();
   }

   loadMember(){
      if(!this.user)return;
      this.memberService.getMember(this.user.username).subscribe({
          next: member => this.member = member
      })
   }

   updateMember(){
      this.memberService.updateMember(this.editForm?.value).subscribe({
         next: _ => {
            this.toastr.success('Profile updated successfully.');
            this.editForm?.reset(this.member); //**to reset the form after its updated.
         }
      });
   }
}
