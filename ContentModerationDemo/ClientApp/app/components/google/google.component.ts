import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

import { ModerationComponent } from '../shared/moderation.component';
import { ModerationResponse } from '../../models/moderation-response';

@Component({
    selector: 'aws',
    templateUrl: '../shared/moderation.component.html'
})
export class GoogleComponent extends ModerationComponent {
    
    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        super(http,
            baseUrl + 'api/moderation/google',
            'Google Content Moderation',
            'Select a file to submit to Google Cloud Vision:');
    }    
}

