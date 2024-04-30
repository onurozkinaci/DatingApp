import { Component, Input, OnInit } from '@angular/core';
import { Member } from '../../_models/member';
import { CommonModule } from '@angular/common';
import { FileUploadModule, FileUploader } from 'ng2-file-upload';
import { environment } from '../../../environments/environment';
import { User } from '../../_models/user';
import { AccountService } from '../../_services/account.service';
import { take } from 'rxjs';

@Component({
  selector: 'app-photo-editor',
  standalone: true,
  imports: [CommonModule, FileUploadModule],
  templateUrl: './photo-editor.component.html',
  styleUrl: './photo-editor.component.css'
})
export class PhotoEditorComponent implements OnInit {
  @Input() member:Member|undefined;
  uploader: FileUploader|undefined;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  user: User | undefined;

  //=>The AccountService is injected since we need to get the loggedin user info here;
  constructor(private accountService:AccountService){
    this.accountService.currentUser$.pipe(take(1)).subscribe({
        next: user => {
          if(user) this.user = user
        }
    });
  }

  ngOnInit(): void {
    this.initializeUploader();
  }

  fileOverBase(e:any){
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader(){
     this.uploader = new FileUploader({
        url: this.baseUrl + 'users/add-photo',
        authToken : 'Bearer ' + this.user?.token, //**since we dont send user http interceptor here for this request.
        isHTML5: true,
        allowedFileType: ['image'], //all image types will be allowed(e.g. JPEG,PNG,etc.)
        removeAfterUpload: true,
        autoUpload: false,
        maxFileSize: 10*1024*1024
     });
     this.uploader.onAfterAddingFile = (file) => {
        file.withCredentials = false //CORS conf would be needed without it.
     }
     //=>if file upload is successful;
     this.uploader.onSuccessItem = (item, response, status,headers) => {
       if(response){
          const photo = JSON.parse(response);
          this.member?.photos.push(photo); //to add the new photo that we get from the API
       }
     }
  }
  
}
