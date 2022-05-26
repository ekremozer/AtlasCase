import { Component } from '@angular/core';
import * as environment from './environment.json';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  public apiUrl: string = '';
  title = 'AtlasCaseUI';
  constructor() {

  }
  
  ngOnInit() {

  }

  getApiUrl() {
    this.apiUrl = JSON.parse(JSON.stringify(environment)).default.apiUrl;
    console.log(this.apiUrl);
  }
}
