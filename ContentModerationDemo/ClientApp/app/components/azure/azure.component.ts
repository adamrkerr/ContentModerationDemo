import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

import { ModerationResponse } from '../../models/moderation-response';

@Component({
    selector: 'azure',
    templateUrl: './azure.component.html'
})
export class AzureComponent {
    public response: ModerationResponse;

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string) {
        
    }

    public fileChange(event:any) {
        let fileList: FileList = event.target.files;
        if (fileList.length > 0) {
            let file: File = fileList[0];
            let formData: FormData = new FormData();
            formData.append('uploadFile', file, file.name);
            let headers = new Headers();
            this.http.post(this.baseUrl + 'api/moderation/azure', formData)
                .subscribe(
                result => { this.response = result.json() as ModerationResponse; },
                error => console.log(error)
                );
        }
    }
}

