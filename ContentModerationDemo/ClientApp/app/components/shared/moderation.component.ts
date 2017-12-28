import { Http } from '@angular/http';

import { ModerationResponse } from '../../models/moderation-response';

export abstract class ModerationComponent {
    public response: ModerationResponse;

    protected constructor(private http: Http,
        private endpointUrl: string,
        public title: string,
        public description: string) {

    }

    public fileChange(event: any) {
        let fileList: FileList = event.target.files;
        if (fileList.length > 0) {
            let file: File = fileList[0];
            let formData: FormData = new FormData();
            formData.append('uploadFile', file, file.name);
            let headers = new Headers();
            this.http.post(this.endpointUrl, formData)
                .subscribe(
                result => { this.response = result.json() as ModerationResponse; },
                error => console.log(error)
                );
        }
    }
}

