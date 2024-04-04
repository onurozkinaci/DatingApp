import { Component, OnInit} from '@angular/core';
import { Member } from '../../_models/member';
import { MembersService } from '../../_services/members.service';
import { ActivatedRoute } from '@angular/router';
import { NgbNavModule, NgbCarouselModule } from '@ng-bootstrap/ng-bootstrap';
import { CommonModule } from '@angular/common';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [CommonModule, NgbNavModule, GalleryModule, NgbCarouselModule],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css'
})
export class MemberDetailComponent implements OnInit{
   active:any = 1; //for ngbnav(tabs) usage
   member: Member | undefined;
   images: GalleryItem[] = []; //=>to use the images with GalleryModule

   //*=>ActivatedRoute inject edilerek app.routes'taki route tanimlarina erisilebilecek ve MemberDetailComponent'in cagrildigi username parametresine erisilebilecek;
   constructor(private memberService:MembersService, private route:ActivatedRoute){}

   ngOnInit(): void {
     this.loadMember();
   }
   
   loadMember()
   {
     const username = this.route.snapshot.paramMap.get('username');
     if(!username) return;
     this.memberService.getMember(username).subscribe({
        next: member => {
          this.member = member,
          this.getImages() //=>the photos of the member will be added to imageGallery when the members are loaded.
        }
     });
   }

   getImages(){
    if(!this.member) return;
     for(const photo of this.member?.photos){
        this.images.push(new ImageItem({src: photo.url, thumb:photo.url}));
        this.images.push(new ImageItem({src: photo.url, thumb:photo.url}));
     }
   }
}
